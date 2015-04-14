public class MonsterSceneRole : SceneRole
{
	public MonsterSceneRole(long id) : base(id)
	{
	}

	public override SceneRoleType type
	{
		get
		{
			return SceneRoleType.Monster;
		}
	}
}