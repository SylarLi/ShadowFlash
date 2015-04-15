public class PlayerSceneRole : SceneRole
{
	private bool _self;

    public PlayerSceneRole(IRole role) : base(role)
	{

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