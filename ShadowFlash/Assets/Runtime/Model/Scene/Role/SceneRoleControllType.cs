/// <summary>
/// 角色控制状态
/// </summary>
public enum SceneRoleControllType
{
    Free,           // 无外力控制
    ForceSelf,      // 自力控制(自己使用技能的表现)
    ForceExternal,  // 外力控制(受到外力影响时，例如敌人攻击)
}
