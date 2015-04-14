using Core;
using UnityEngine;

public class SceneRoleBehaviour<T> : MonoBehaviour where T : ISceneRole
{
	protected ISceneRole role;

	protected MeshRenderer[] renderers;
	
	protected Animator animator;
	
	protected Rigidbody2D rigidbody2D;

	public void Bind(T role)
	{
		this.role = role;
		Listen();
	}

	protected virtual void Listen()
	{
		role.AddEventListener(SceneRoleEvent.RenderChange, RenderChangeHandler);
		role.AddEventListener(SceneRoleEvent.ActiveChange, ActiveChangeHandler);
		role.AddEventListener(SceneRoleEvent.PositionChange, PositionChangeHandler);
		role.AddEventListener(SceneRoleEvent.RotationChange, RotationChangeHandler);
		role.AddEventListener(SceneRoleEvent.LocalScaleChange, LocalScaleChangeHandler);
		role.AddEventListener(SceneRoleEvent.AnimatorEnabledChange, AnimatorEnabledChangeHandler);
		role.AddEventListener(SceneRoleEvent.AnimatorSpeedChange, AnimatorSpeedChangeHandler);
	}
	
	protected virtual void Unlisten()
	{
		role.RemoveEventListener(SceneRoleEvent.RenderChange, RenderChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.ActiveChange, ActiveChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.PositionChange, PositionChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.RotationChange, RotationChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.LocalScaleChange, LocalScaleChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.AnimatorEnabledChange, AnimatorEnabledChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.AnimatorSpeedChange, AnimatorSpeedChangeHandler);
	}

	private void ActiveChangeHandler(IEvent obj)
	{
		gameObject.SetActive(role.active);
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

	private void Update()
	{
		// Todo: 2D坐标3D化
		// role.SyncPosition(transform.position);
		role.SyncRotation(transform.rotation.eulerAngles);
		role.SyncLocalScale(transform.localScale);
	}
	
	private void FixedUpdate()
	{

	}

	private void Awake()
	{
		renderers = GetComponentsInChildren<MeshRenderer>();
		animator = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void OnDestroy()
	{
		Unlisten();
		renderers = null;
		animator = null;
		rigidbody2D = null;
		role = null;
	}
}