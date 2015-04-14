public class NpcSceneRole : SceneRole
{
	public NpcSceneRole(long id) : base(id)
	{
	}

	public override SceneRoleType type
	{
		get
		{
			return SceneRoleType.Npc;
		}
	}
}