using Core;

public class SceneEvent : Event
{
	public const string SceneChange = "SceneChange";

	public const string MapChange = "MapChange";

	public const string StateChange = "StateChange";

	public const string AddRole = "AddRole";

	public const string RemoveRole = "RemoveRole";

	public SceneEvent(string type, object data = null) : base(type, data)
	{

	}
}

