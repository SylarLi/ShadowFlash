namespace Trigger
{
	public interface ITriggerBox
	{
	    /// <summary>
	    /// 关联角色id
	    /// </summary>
	    long roleId { get; set; }

	    void Trigger(ITriggerBox input);
	}
}
