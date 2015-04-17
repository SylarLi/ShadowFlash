public class PlayerSceneRole : SceneRole
{
    #region SelfProperty

    private bool _self;

    private bool _directionKeyLock;

    #endregion

    public PlayerSceneRole(IRole role) : base(role)
	{
        _self = false;
        _directionKeyLock = false;
	}

	public bool self
	{
		get
		{
			return _self;
		}
		set
		{
			if (_self != value)
			{
				_self = value;
				DispatchEvent(new PlayerSceneRoleEvent(PlayerSceneRoleEvent.SelfChange));
			}
		}
	}

    /// <summary>
    /// 是否锁定方向控制
    /// </summary>
    public bool directionKeyLock
    {
        get
        {
            return _directionKeyLock;
        }
        set
        {
            if (_directionKeyLock != value)
            {
                _directionKeyLock = value;
                DispatchEvent(new PlayerSceneRoleEvent(PlayerSceneRoleEvent.DirectionKeyLockChange));
            }
        }
    }
}