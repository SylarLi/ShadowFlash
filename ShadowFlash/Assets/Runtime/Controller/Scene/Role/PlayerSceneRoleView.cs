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
            KeyBoardListener listener = entity.GetComponent<KeyBoardListener>();
            if (sceneRole.self)
            {
                if (listener == null)
                {
                    listener = entity.AddComponent<KeyBoardListener>();
                }
            }
            else
            {
                if (listener != null)
                {
                    GameUtil.Destroy(listener);
                }
            }
		}
	}
}