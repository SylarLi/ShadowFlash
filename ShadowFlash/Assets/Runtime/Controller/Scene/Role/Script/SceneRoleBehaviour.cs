using Core;
using UnityEngine;

public class SceneRoleBehaviour<T> : MonoBehaviour where T : ISceneRole
{
    public float floorLinearDrag = GameConst.FloorLinearDrag;

	protected ISceneRole role;

	protected MeshRenderer[] renderers;
	
	protected Animator animator;
	
	protected Rigidbody2D rigidbody2D;

    protected GameObject ghostv;

    protected Rigidbody2D ghostvRigidbody2D;

    protected GameObject ghosth;

    protected Rigidbody2D ghosthRigidbody2D;

	public void Bind(T role)
	{
		this.role = role;
		Listen();
	}

	protected virtual void Listen()
	{
		role.AddEventListener(SceneRoleEvent.RenderChange, RenderChangeHandler);
		role.AddEventListener(SceneRoleEvent.PositionChange, PositionChangeHandler);
		role.AddEventListener(SceneRoleEvent.RotationChange, RotationChangeHandler);
		role.AddEventListener(SceneRoleEvent.LocalScaleChange, LocalScaleChangeHandler);
		role.AddEventListener(SceneRoleEvent.AnimatorEnabledChange, AnimatorEnabledChangeHandler);
		role.AddEventListener(SceneRoleEvent.AnimatorSpeedChange, AnimatorSpeedChangeHandler);
	}
	
	protected virtual void Unlisten()
	{
		role.RemoveEventListener(SceneRoleEvent.RenderChange, RenderChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.PositionChange, PositionChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.RotationChange, RotationChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.LocalScaleChange, LocalScaleChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.AnimatorEnabledChange, AnimatorEnabledChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.AnimatorSpeedChange, AnimatorSpeedChangeHandler);
	}
	
	private void RenderChangeHandler(IEvent obj)
	{
		if (renderers != null)
		{
			foreach (MeshRenderer renderer in renderers)
			{
				renderer.enabled = role.render;
			}
		}
	}
	
	private void PositionChangeHandler(IEvent obj)
	{
		transform.position = GameUtil.Position3DZ22DY(role.position);
	}
	
	private void RotationChangeHandler(IEvent obj)
	{
		transform.rotation = Quaternion.Euler(role.rotation);
	}
	
	private void LocalScaleChangeHandler(IEvent obj)
	{
		transform.localScale = role.localScale;
	}
	
	private void AnimatorEnabledChangeHandler(IEvent obj)
	{
		if (animator != null)
		{
			animator.enabled = role.animatorEnabled;
		}
	}
	
	private void AnimatorSpeedChangeHandler(IEvent obj)
	{
		if (animator != null)
		{
			animator.speed = role.animatorSpeed;
		}
	}

    protected void AddVelocity(Vector3 velocity)
    {
        // 水平速度
        ghosthRigidbody2D.velocity += new Vector2(velocity.x, velocity.y);
        // 垂直速度
        ghostvRigidbody2D.velocity += new Vector2(0, GameUtil.Position3DZ22DY(velocity.z));
    }

    protected void AddImpulse(Vector3 impulse)
    {
        // 水平力
        ghosthRigidbody2D.AddForce(new Vector2(impulse.x, impulse.y), ForceMode2D.Impulse);
        // 垂直力
        ghostvRigidbody2D.AddForce(new Vector2(0, GameUtil.Position3DZ22DY(impulse.z)), ForceMode2D.Impulse);
    }

    protected virtual void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // 控制水平方向移动和受力
        ghostv = new GameObject("ghostv_" + gameObject.name);
        GameUtil.SetLayer(ghostv, GameLayer.Culling);
        ghostvRigidbody2D = ghostv.AddComponent<Rigidbody2D>();
        GameUtil.CopyRigidbody2DProperty(rigidbody2D, ghostvRigidbody2D);

        // 控制垂直方向移动和受力
        ghosth = new GameObject("ghosth_" + gameObject.name);
        GameUtil.SetLayer(ghosth, GameLayer.Culling);
        ghosthRigidbody2D = ghosth.AddComponent<Rigidbody2D>();
        GameUtil.CopyRigidbody2DProperty(rigidbody2D, ghosthRigidbody2D);
    }

    protected virtual void OnEnable()
    {
        RenderChangeHandler(null);
        PositionChangeHandler(null);
        RotationChangeHandler(null);
        LocalScaleChangeHandler(null);
        AnimatorEnabledChangeHandler(null);
        AnimatorSpeedChangeHandler(null);
    }

    protected virtual void Update()
	{
        Vector3 position = ghosth.transform.position;
        position.z = ghostv.transform.position.y;
        transform.position = GameUtil.Position3DZ22DY(position);

        role.SyncPosition(position);
		role.SyncRotation(transform.rotation.eulerAngles);
		role.SyncLocalScale(transform.localScale);
	}

    protected virtual void FixedUpdate()
	{
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
            ghosthRigidbody2D.drag = floorLinearDrag;
        }
	}

    protected virtual void OnDestroy()
    {
        Unlisten();
        renderers = null;
        animator = null;
        rigidbody2D = null;
        role = null;
    }
}