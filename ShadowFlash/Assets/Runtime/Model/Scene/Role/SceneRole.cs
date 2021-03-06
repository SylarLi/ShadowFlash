using Core;
using UnityEngine;

public class SceneRole : EventDispatcher, ISceneRole
{
    private IRole _role;

	private bool _active;

	private bool _culling;

	private int _layer;

	private Vector3 _position; 

	private Vector3 _rotation;

	private Vector3 _localScale;

    private bool _air;

    private bool _useGravity;

    private bool _useFloorDrag;

    private SceneRoleControllType _controllType;

    private SceneRoleState _state;

	public SceneRole(IRole role)
	{
        _role = role;
		_active = false;
		_culling = false;
		_layer = GameLayer.Default;
		_position = Vector3.zero;
		_rotation = Vector3.zero;
		_localScale = Vector3.one;
        _air = false;
        _useGravity = true;
        _useFloorDrag = true;
        _controllType = SceneRoleControllType.Free;
        _state = SceneRoleState.Idle;
	}

	#region ISceneRole implementation

	public IRole role
    {
        get
        {
            return _role;
        }
    }

    public long id
    {
        get
        {
            return _role.GetLong(Role.id);
        }
    }

    public RoleType type
    {
        get
        {
            return _role.GetEnum<RoleType>(Role.type);
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

    public bool air
    {
        get
        {
            return _air;
        }
        set
        {
            if (_air != value)
            {
                _air = value;
                DispatchEvent(new SceneRoleEvent(SceneRoleEvent.AirChange));
            }
        }
    }

    public bool useGravity
    {
        get
        {
            return _useGravity;
        }
        set
        {
            if (_useGravity != value)
            {
                _useGravity = value;
                DispatchEvent(new SceneRoleEvent(SceneRoleEvent.UseGravityChange));
            }
        }
    }

    public bool useFloorDrag
    {
        get
        {
            return _useFloorDrag;
        }
        set
        {
            if (_useFloorDrag != value)
            {
                _useFloorDrag = value;
                DispatchEvent(new SceneRoleEvent(SceneRoleEvent.UseFloorDragChange));
            }
        }
    }

    public SceneRoleControllType controllType
    {
        get
        {
            return _controllType;
        }
        set
        {
            if (_controllType != value)
            {
                _controllType = value;
                DispatchEvent(new SceneRoleEvent(SceneRoleEvent.SceneRoleControllTypeChange));
            }
        }
    }

    public SceneRoleState state
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state != value)
            {
                _state = value;
                DispatchEvent(new SceneRoleEvent(SceneRoleEvent.SceneRoleStateChange));
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