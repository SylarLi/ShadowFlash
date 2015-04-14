using Core;
using UnityEngine;

public class PlayerSceneRoleView : SceneRoleView<PlayerSceneRole, PlayerSceneRoleBehaviour>
{
	public PlayerSceneRoleView(LoadProxy loadProxy, PlayerSceneRole role) : base(loadProxy, role)
	{

	}

	protected override void Listen()
	{
		base.Listen();
		role.AddEventListener(PlayerSceneRoleEvent.SelfChange, SelfChangeHandler);
	}

	protected override void Unlisten()
	{
		base.Unlisten();
		role.AddEventListener(PlayerSceneRoleEvent.SelfChange, SelfChangeHandler);
	}

	protected override void TriggerPropertyChange()
	{
		base.TriggerPropertyChange();
		SelfChangeHandler(null);
	}

	private void SelfChangeHandler(IEvent obj)
	{
		if (entity != null)
		{
			if (role.self)
			{
				// Append Controll Proxy
			}
			else
			{
				// Remove Controll Proxy
			}
		}
	}
}