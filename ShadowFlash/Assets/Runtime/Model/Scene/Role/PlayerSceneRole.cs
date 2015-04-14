public class PlayerSceneRole : SceneRole
{
	private bool _self;

	public PlayerSceneRole(long id) : base(id)
	{

	}

	public override SceneRoleType type
	{
		get
		{
			return SceneRoleType.Player;
		}
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