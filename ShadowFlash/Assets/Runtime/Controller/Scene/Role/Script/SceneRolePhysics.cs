using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理场景角色位置、物理等的代理
/// </summary>
public class SceneRolePhysics : IMonoProxy
{
    private Rigidbody2D rigidbody2D;

    private GameObject ghostv;

    private Rigidbody2D ghostvRigidbody2D;

    private GameObject ghosth;

    private Rigidbody2D ghosthRigidbody2D;

    private Dictionary<Func<Vector3>, float> forces;

    private Dictionary<int, Func<Vector3>> persists;

    private Direction moveDirection;

    public SceneRolePhysics(Rigidbody2D rigidbody2D)
    {
        this.rigidbody2D = rigidbody2D;
        this.forces = new Dictionary<Func<Vector3>, float>();
        this.persists = new Dictionary<int, Func<Vector3>>();
        this.moveDirection = Direction.None;
    }

    /// <summary>
    /// 假想3D坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 Get3DPosition()
    {
        Vector3 position = ghosth.transform.position;
        position.z = ghostv.transform.position.y;
        return position;
    }

    /// <summary>
    /// 实际2D坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 Get2DPosition()
    {
        return GameUtil.Position3DZ22DY(Get3DPosition());
    }

    /// <summary>
    /// 固定时间外力，朝向固定点
    /// </summary>
    /// <param name="point"></param>
    /// <param name="duration"></param>
    /// <param name="force"></param>
    public void ForceTo(Vector3 point, int duration, float force)
    {
        forces.Add(() => (Get3DPosition() - point).normalized * force, duration);
    }

    /// <summary>
    /// 固定时间外力，朝向固定方向
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="duration"></param>
    /// <param name="force"></param>
    public void ForceFoward(Vector3 direction, int duration, float force)
    {
        forces.Add(() => direction.normalized * force, duration);
    }

    /// <summary>
    /// 固定时间外力，朝向固定方向
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="duration"></param>
    /// <param name="force"></param>
    public void ForceFoward(Direction direction, int duration, float force)
    {
        ForceFoward(GameUtil.DirectionToVector3(direction), duration, force);
    }

    /// <summary>
    /// 清空固定时间外力
    /// </summary>
    public void ClearForce()
    {
        forces.Clear();
    }

    /// <summary>
    /// 不定时间外力，朝向固定点
    /// </summary>
    /// <param name="to"></param>
    /// <param name="force"></param>
    /// <returns></returns>
    public int PersistTo(Vector3 point, float force)
    {
        int key = UnityEngine.Random.Range(1, int.MaxValue);
        persists.Add(key, () => (Get3DPosition() - point).normalized * force);
        return key;
    }

    /// <summary>
    /// 不定时间外力，朝向固定方向
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    /// <returns></returns>
    public int PersistFoward(Vector3 direction, float force)
    {
        int key = UnityEngine.Random.Range(1, int.MaxValue);
        persists.Add(key, () => direction.normalized * force);
        return key;
    }

    /// <summary>
    /// 不定时间外力，朝指定方向
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    /// <returns></returns>
    public int PersistFoward(Direction direction, float force)
    {
        return PersistFoward(GameUtil.DirectionToVector3(direction), force);
    }

    /// <summary>
    /// 移除指定的不定时间外力
    /// </summary>
    /// <param name="key"></param>
    public bool RemovePersist(int key)
    {
        bool result = false;
        if (persists.ContainsKey(key))
        {
            persists.Remove(key);
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 清空不定时间外力
    /// </summary>
    public void ClearPersist()
    {
        persists.Clear();
    }

    /// <summary>
    /// 脉冲力
    /// </summary>
    /// <param name="impulse"></param>
    public void AddImpulse(Vector3 impulse)
    {
        // 水平力
        ghosthRigidbody2D.AddForce(new Vector2(impulse.x, impulse.y), ForceMode2D.Impulse);
        // 垂直力
        ghostvRigidbody2D.AddForce(new Vector2(0, GameUtil.Position3DZ22DY(impulse.z)), ForceMode2D.Impulse);
    }

    public void AddVelocity(Vector3 velocity)
    {
        // 水平速度
        ghosthRigidbody2D.velocity += new Vector2(velocity.x, velocity.y);
        // 垂直速度
        ghostvRigidbody2D.velocity += new Vector2(0, GameUtil.Position3DZ22DY(velocity.z));
    }

    public void Awake()
    {
        // 控制水平方向移动和受力
        ghostv = new GameObject("ghostv_" + rigidbody2D.gameObject.name);
        ghostv.SetActive(false);
        GameUtil.SetLayer(ghostv, GameLayer.Culling);
        ghostvRigidbody2D = ghostv.AddComponent<Rigidbody2D>();
        GameUtil.CopyRigidbody2DProperty(rigidbody2D, ghostvRigidbody2D);

        // 控制垂直方向移动和受力
        ghosth = new GameObject("ghosth_" + rigidbody2D.gameObject.name);
        ghosth.SetActive(false);
        GameUtil.SetLayer(ghosth, GameLayer.Culling);
        ghosthRigidbody2D = ghosth.AddComponent<Rigidbody2D>();
        GameUtil.CopyRigidbody2DProperty(rigidbody2D, ghosthRigidbody2D);
    }

    public void OnEnable()
    {
        ghostv.SetActive(true);
        ghosth.SetActive(true);
    }

    public void OnDisable()
    {
        ghostv.SetActive(false);
        ghosth.SetActive(false);
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        // 重力
        Vector3 vp = ghostv.transform.position;
        if (vp.y >= GameConst.FloorHeight && vp.y + ghostvRigidbody2D.velocity.y * Time.deltaTime > GameConst.FloorHeight)
        {
            // 如果在浮空，施加重力
            ghostvRigidbody2D.AddForce(new Vector2(0, ghostvRigidbody2D.mass * GameConst.Gravity), ForceMode2D.Force);
            // 设置水平阻力位0
            ghosthRigidbody2D.drag = 0;
        }
        else
        {
            // 如果落地
            vp.y = GameConst.FloorHeight;
            ghostv.transform.position = vp;
            // 垂直速度归0
            ghostvRigidbody2D.velocity = Vector2.zero;
            // 回复水平阻力
            ghosthRigidbody2D.drag = GameConst.FloorLinearDrag;
        }
        // 固定时间外力
        Dictionary<Func<Vector3>, float>.KeyCollection keys = forces.Keys;
        foreach (Func<Vector3> key in keys)
        {
            if ((forces[key] -= Time.fixedDeltaTime) <= 0)
            {
                forces.Remove(key);
            }
            else
            {
                Vector3 force = key();
                ghosthRigidbody2D.AddForce(new Vector2(force.x, force.y), ForceMode2D.Force);
                ghostvRigidbody2D.AddForce(new Vector2(0, force.z), ForceMode2D.Force);
            }
        }
        // 不定时间外力
        foreach (Func<Vector3> persist in persists.Values)
        {
            Vector3 force = persist();
            ghosthRigidbody2D.AddForce(new Vector2(force.x, force.y), ForceMode2D.Force);
            ghostvRigidbody2D.AddForce(new Vector2(0, force.z), ForceMode2D.Force);
        }
    }

    public void OnDestroy()
    {
        GameUtil.Destroy(ghostv);
        GameUtil.Destroy(ghosth);
        ghostv = null;
        ghosth = null;
        ghosthRigidbody2D = null;
        ghostvRigidbody2D = null;
        rigidbody2D = null;
        forces = null;
    }
}
