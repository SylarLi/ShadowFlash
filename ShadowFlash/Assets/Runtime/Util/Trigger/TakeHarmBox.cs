using UnityEngine;

namespace Trigger
{
	[RequireComponent(typeof(Collider2D))]
	public class TakeHarmBox : MonoBehaviour, ITakeHarmBox
	{
	    private long mRoleId;

	    public long roleId
	    {
			get { return mRoleId; }
			set { mRoleId = value; }
	    }

	    /// <summary>
	    /// 触发时受到伤害区块的表现
	    /// </summary>
	    public virtual void Trigger(ITriggerBox input)
	    {
	        Debug.Log("I'm hurt!!!");
	    }
	}
}
