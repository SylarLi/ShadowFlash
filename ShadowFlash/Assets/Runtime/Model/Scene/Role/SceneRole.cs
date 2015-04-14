using Core;
using UnityEngine;

public class SceneRole : EventDispatcher, ISceneRole
{
	private long _id;

	private int _entityId;

	private bool _active;

	private bool _culling;

	private int _layer;

	private bool _render;

	private Vector3 _position; 

	private Vector3 _rotation;

	private Vector3 _localScale;

	private bool _animatorEnabled;
	
	private float _animatorSpeed;

	public SceneRole(long id)
	{
		_id = id;
		_entityId = 0;
		_active = false;
		_culling = false;
		_render = true;
		_layer = GameLayer.Default;
		_position = Vector3.zero;
		_rotation = Vector3.zero;
		_localScale = Vector3.one;
	}

	#region ISceneRole implementation

	public long id
	{
		get
		{
			return _id;
		}
	}

	public virtual SceneRoleType type
	{
		get
		{
			return SceneRoleType.None;
		}
	}

	public int entityId
	{
		get
		{
			return _entityId;
		}
		set
		{
			if (_entityId != value)
			{
				_entityId = value;
				DispatchEvent(new SceneRoleEvent(SceneRoleEvent.EntityIdChange));
			}
		}
	}

	public bool active
	{
		get
		{
			return _active;
		}
		set
		{
			if (_active != value)
			{
				_active = value;
				DispatchEvent(new SceneRoleEvent(SceneRoleEvent.ActiveChange));
			}
		}
	}

	public bool culling
	{
		get
		{
			return _culling;
		}
		set
		{
			if (_culling != value)
			{
				_culling = value;
				DispatchEvent(new SceneRoleEvent(SceneRoleEvent.CullingChange));
			}
		}
	}

	public bool render
	{
		get
		{
			return _render;
		}
		set
		{
			if (_render != value)
			{
				_render = value;
				DispatchEvent(new SceneRoleEvent(SceneRoleEvent.RenderChange));
			}
		}
	}

	public int layer
	{
		get
		{
			return _layer;
		}
		set
		{
			if (_layer != value)
			{
				_layer = value;
				DispatchEvent(new SceneRoleEvent(SceneRoleEvent.LayerChange));
			}
		}
	}

	/// <summary>
	/// 数据为3D，表现为2D
	/// </summary>
	/// <value>The position.</value>
	public Vector3 position
	{
		get
		{
			return _position;
		}
		set
		{
			if (_position != value)
			{
				_position = value;
				DispatchEvent(new SceneRoleEvent(SceneRoleEvent.PositionChange));
			}
		}
	}

	public Vector3 rotation
	{
		get
		{
			return _rotation;
		}
		set
		{
			if (_rotation != value)
			{
				_rotation = value;
				DispatchEvent(new SceneRoleEvent(SceneRoleEvent.RotationChange));
			}
		}
	}

	public Vector3 localScale
	{
		get
		{
			return _localScale;
		}
		set
		{
			if (_localScale != value)
			{
				_localScale = value;
				DispatchEvent(new SceneRoleEvent(SceneRoleEvent.LocalScaleChange));
			}
		}
	}

	public bool animatorEnabled
	{
		get
		{
			return _animatorEnabled;
		}
		set
		{
			if (_animatorEnabled != value)
			{
				_animatorEnabled = value;
				DispatchEvent(new SceneRoleEvent(SceneRoleEvent.AnimatorEnabledChange));
			}
		}
	}
	
	public float animatorSpeed
	{
		get
		{
			return _animatorSpeed;
		}
		set
		{
			if (!Mathf.Approximately(_animatorSpeed, value))
			{
				_animatorSpeed = value;
				DispatchEvent(new SceneRoleEvent(SceneRoleEvent.AnimatorSpeedChange));
			}
		}
	}

	/// <summary>
	/// view同步坐标
	/// </summary>
	/// <param name="value">Value.</param>
	public void SyncPosition(Vector3 value)
	{
		_position = value;
	}
	
	/// <summary>
	/// view同步旋转
	/// </summary>
	/// <param name="value">Value.</param>
	public void SyncRotation(Vector3 value)
	{
		_rotation = value;
	}
	
	/// <summary>
	/// view同步缩放
	/// </summary>
	/// <param name="value">Value.</param>
	public void SyncLocalScale(Vector3 value)
	{
		_localScale = value;
	}
    
	#endregion
}