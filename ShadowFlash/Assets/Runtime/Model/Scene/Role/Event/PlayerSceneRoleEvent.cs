public class PlayerSceneRoleEvent : SceneRoleEvent
{
	public const string SelfChange = "SelfChange";

    public const string DirectionKeyLockChange = "DirectionKeyLockChange";

	public PlayerSceneRoleEvent(string type, object data = null) : base(type, data)
	{

	}
}