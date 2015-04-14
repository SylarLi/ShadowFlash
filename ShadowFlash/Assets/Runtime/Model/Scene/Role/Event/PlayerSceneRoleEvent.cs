public class PlayerSceneRoleEvent : SceneRoleEvent
{
	public const string SelfChange = "SelfChange";

	public PlayerSceneRoleEvent(string type, object data = null) : base(type, data)
	{
	}
}