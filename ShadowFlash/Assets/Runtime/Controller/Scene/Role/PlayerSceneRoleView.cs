using Core;
using UnityEngine;

public class PlayerSceneRoleView : SceneRoleView<PlayerSceneRole, PlayerSceneRoleBehaviour>
{
    public PlayerSceneRoleView(LoadProxy loadProxy, PlayerSceneRole sceneRole) : base(loadProxy, sceneRole)
	{

	}

	protected override void Listen()
	{
		base.Listen();
		sceneRole.AddEventListener(PlayerSceneRoleEvent.SelfChange, SelfChangeHandler);
	}

	protected override void Unlisten()
	{
		base.Unlisten();
		sceneRole.AddEventListener(PlayerSceneRoleEvent.SelfChange, SelfChangeHandler);
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
			if (sceneRole.self)
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