using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理物理等的代理
/// </summary>
public class PhysicsProxy : IMonoProxy
{
    private Rigidbody2D rigidbody2D;

    private GameObject ghostv;

    private Rigidbody2D ghostvRigidbody2D;

    private GameObject ghosth;

    private Rigidbody2D ghosthRigidbody2D;

    private Dictionary<int, ForceInfo> forces;

    private Dictionary<int, ForceInfo> natures;

    public PhysicsProxy(Rigidbody2D rigidbody2D)
    {
        this.rigidbody2D = rigidbody2D;
        this.forces = new Dictionary<int, ForceInfo>();
        this.natures = new Dictionary<int, ForceInfo>();
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
    /// 获得下一帧的预测3D坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNext3DPosition()
    {
        return Get3DPosition() + GetVelocity() * Time.deltaTime;
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
    /// 设置垂直刚体坐标
    /// </summary>
    /// <param name="y"></param>
    public void SetVerticalPosition(float y)
    {
        Vector3 vp = ghostv.transform.position;
        vp.y = y;
        ghostv.transform.position = vp;
    }

    /// <summary>
    /// 当前刚体速度
    /// </summary>
    /// <returns></returns>
    public Vector3 GetVelocity()
    {
        Vector3 v = ghosthRigidbody2D.velocity;
        v.z = ghostvRigidbody2D.velocity.y;
        return v;
    }

    /// <summary>
    /// 设置水平方向刚体速度
    /// </summary>
    /// <param name="velocity"></param>
    /// <returns></returns>
    public void SetHorizontalVelocity(Vector2 velocity)
    {
        ghosthRigidbody2D.velocity = velocity;
    }

    /// <summary>
    /// 设置垂直方向刚体速度
    /// </summary>
    /// <param name="velocity"></param>
    public void SetVerticalVelocity(float velocity)
    {
        ghostvRigidbody2D.velocity = new Vector2(0, velocity);
    }

    /// <summary>
    /// 是否开启水平方向的阻力
    /// </summary>
    /// <param name="enable"></param>
    public void SetHorizontalDragEnable(bool enable)
    {
        ghosthRigidbody2D.drag = enable ? GameConst.FloorLinearDrag : 0;
    }

    /// <summary>
    /// 自然力，朝向固定方向
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="duration"></param>
    /// <param name="force"></param>
    public int NatureFoward(Vector3 direction, float force, int duration = int.MaxValue)
    {
        int key = UnityEngine.Random.Range(0, int.MaxValue);
        ForceInfo forceInfo = new ForceInfo(() => direction.normalized * force, duration);
        natures.Add(key, forceInfo);
        return key;
    }

    /// <summary>
    /// 自然力，朝向固定方向
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="duration"></param>
    /// <param name="force"></param>
    public int NatureFoward(Direction direction, float force, int duration = int.MaxValue)
    {
        return ForceFoward(GameUtil.DirectionToVector3(direction), force, duration);
    }

    /// <summary>
    /// 移除指定的自然力
    /// </summary>
    /// <param name="key"></param>
    public bool RemoveNature(int key)
    {
        bool result = false;
        if (natures.ContainsKey(key))
        {
            natures.Remove(key);
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 清空自然力
    /// </summary>
    public void ClearNature()
    {
        natures.Clear();
    }

    /// <summary>
    /// 外力，朝向固定点
    /// </summary>
    /// <param name="point"></param>
    /// <param name="duration"></param>
    /// <param name="force"></param>
    public int ForceTo(Vector3 point, float force, int duration = int.MaxValue)
    {
        int key = UnityEngine.Random.Range(0, int.MaxValue);
        ForceInfo forceInfo = new ForceInfo(() => (Get3DPosition() - point).normalized * force, duration);
        forces.Add(key, forceInfo);
        return key;
    }

    /// <summary>
    /// 外力，朝向固定方向
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="duration"></param>
    /// <param name="force"></param>
    public int ForceFoward(Vector3 direction, float force, int duration = int.MaxValue)
    {
        int key = UnityEngine.Random.Range(0, int.MaxValue);
        ForceInfo forceInfo = new ForceInfo(() => direction.normalized * force, duration);
        forces.Add(key, forceInfo);
        return key;
    }

    /// <summary>
    /// 外力，朝向固定方向
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="duration"></param>
    /// <param name="force"></param>
    public int ForceFoward(Direction direction, float force, int duration = int.MaxValue)
    {
        return ForceFoward(GameUtil.DirectionToVector3(direction), force, duration);
    }

    /// <summary>
    /// 移除指定的外力
    /// </summary>
    /// <param name="key"></param>
    public bool RemoveForce(int key)
    {
        bool result = false;
        if (forces.ContainsKey(key))
        {
            forces.Remove(key);
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 清空外力
    /// </summary>
    public void ClearForce()
    {
        forces.Clear();
    }

    /// <summary>
    /// 脉冲外力
    /// </summary>
    /// <param name="impulse"></param>
    public void AddImpulseForce(Vector3 impulse)
    {
        // 水平力
        ghosthRigidbody2D.AddForce(new Vector2(impulse.x, impulse.y), ForceMode2D.Impulse);
        // 垂直力
        ghostvRigidbody2D.AddForce(new Vector2(0, GameUtil.Position3DZ22DY(impulse.z)), ForceMode2D.Impulse);
    }

    /// <summary>
    /// 脉冲速度
    /// </summary>
    /// <param name="velocity"></param>
    /// <returns></returns>
    public void AddImpulseVelocity(Vector3 velocity)
    {
        // 水平速度
        ghosthRigidbody2D.velocity += new Vector2(velocity.x, velocity.y);
        // 垂直速度
        ghostvRigidbody2D.velocity += new Vector2(0, GameUtil.Position3DZ22DY(velocity.z));
    }

    /// <summary>
    /// 自然力，朝向固定点
    /// </summary>
    /// <param name="point"></param>
    /// <param name="duration"></param>
    /// <param name="force"></param>
    public int NatureTo(Vector3 point, float force, int duration = int.MaxValue)
    {
        int key = UnityEngine.Random.Range(0, int.MaxValue);
        ForceInfo forceInfo = new ForceInfo(() => (Get3DPosition() - point).normalized * force, duration);
        natures.Add(key, forceInfo);
        return key;
    }

    public void Awake()
    {
        // 控制垂直方向受力和速度
        ghostv = new GameObject("ghostv_" + rigidbody2D.gameObject.name);
        ghostv.SetActive(false);
        GameUtil.SetLayer(ghostv, GameLayer.Culling);
        ghostvRigidbody2D = ghostv.AddComponent<Rigidbody2D>();
        GameUtil.CopyRigidbody2DProperty(rigidbody2D, ghostvRigidbody2D);

        // 控制水平方向受力和速度
        ghosth = new GameObject("ghosth_" + rigidbody2D.gameObject.name);
        ghosth.SetActive(false);
        GameUtil.SetLayer(ghosth, GameLayer.Culling);
        ghosthRigidbody2D = ghosth.AddComponent<Rigidbody2D>();
        GameUtil.CopyRigidbody2DProperty(rigidbody2D, ghosthRigidbody2D);
        SetHorizontalDragEnable(true);
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
        // 外力
        foreach (int key in forces.Keys)
        {
            if ((forces[key].duration -= Time.fixedDeltaTime) <= 0)
            {
                RemoveForce(key);
            }
            else
            {
                Vector3 force = forces[key].force();
                ghosthRigidbody2D.AddForce(new Vector2(force.x, force.y), ForceMode2D.Force);
                ghostvRigidbody2D.AddForce(new Vector2(0, force.z), ForceMode2D.Force);
            }
        }
        // 自然力
        foreach (int key in natures.Keys)
        {
            if ((natures[key].duration -= Time.fixedDeltaTime) <= 0)
            {
                RemoveNature(key);
            }
            else
            {
                Vector3 nature = natures[key].force();
                ghosthRigidbody2D.AddForce(new Vector2(nature.x, nature.y), ForceMode2D.Force);
                ghostvRigidbody2D.AddForce(new Vector2(0, nature.z), ForceMode2D.Force);
            }
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

    private class ForceInfo
    {
        public Func<Vector3> force;

        public float duration;

        public ForceInfo(Func<Vector3> force, float duration)
        {
            this.force = force;
            this.duration = duration;
        }
    }
}
