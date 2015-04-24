public class PlayerSceneRole : SceneRole
{
    #region SelfProperty

    private bool _self;

    #endregion

    public PlayerSceneRole(IRole role) : base(role)
	{
        _self = false;
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
}