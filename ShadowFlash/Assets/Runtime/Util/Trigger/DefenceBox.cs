using UnityEngine;

namespace Trigger
{
	[RequireComponent(typeof(Collider2D))]
	public class DefenceBox : TakeHarmBox, IDefenceBox
	{
	    /// <summary>
	    /// 触发时防御区块的表现
	    /// </summary>
	    public override void Trigger(ITriggerBox input)
	    {
	        Debug.Log("Block!!!");
	    }
	}
}
