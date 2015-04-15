using Core;

public class RoleEvent : Event
{
    public const string NameChange = "NameChange";

	public const string EntityIdChange = "EntityIdChange";

    public RoleEvent(string type, object data = null) : base(type, data)
	{

	}
}
