using Core;

public class SceneRoleEvent : Event
{
	public const string CullingChange = "CullingChange";

	public const string ActiveChange = "ActiveChange";

	public static string RenderChange = "RenderChange";

	public const string LayerChange = "LayerChange";

	public const string PositionChange = "PositionChange";

	public const string RotationChange = "RotationChange";

	public const string LocalScaleChange = "LocalScaleChange";

    public const string AirChange = "AirChange";

    public const string UseGravityChange = "UseGravityChange";

    public const string UseFloorDragChange = "UseFloorDragChange";

    public const string SceneRoleControllTypeChange = "SceneRoleControllTypeChange";

    public const string SceneRoleStateChange = "SceneRoleStateChange";

	public SceneRoleEvent(string type, object data = null) : base(type, data)
	{

	}
}