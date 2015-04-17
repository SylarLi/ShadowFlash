public class GameConst
{
	/// <summary>
	/// 模拟摄像机俯视绕X轴角度
	/// </summary>
	public const float CameraAngle = 45;

	/// <summary>
	/// 重力常数
	/// </summary>
	public const float Gravity = -9.8f;

    /// <summary>
    /// 地面一致高度
    /// </summary>
    public const float FloorHeight = 0;

    /// <summary>
    /// 在地面时的刚体linearDrag
    /// </summary>
    public const float FloorLinearDrag = 5f;

    /// <summary>
    /// 在地面主动移动时的受力
    /// </summary>
    public const float FloorRunForce = 5f;

    /// <summary>
    /// 起跳时的向上速度
    /// </summary>
    public const float JumpSpeed = 3f;
}