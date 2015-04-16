using Core;
using UnityEngine;

public class SceneRoleView<T1, T2> : ISceneRoleView where T1 : class, ISceneRole where T2 : SceneRoleBehaviour<T1>
{
	protected LoadProxy loadProxy;

	protected T1 sceneRole;

	protected int entityId;

	protected GameObject entity;

	protected T2 behaviour;

	public SceneRoleView(LoadProxy loadProxy, T1 sceneRole)
	{
		this.loadProxy = loadProxy;
        this.sceneRole = sceneRole;
		this.entityId = 0;
		TriggerEntityChange();
		Listen();
	}

	protected virtual void Listen()
	{
		sceneRole.role.AddEventListener(RoleEvent.EntityIdChange, EntityIdChangeHandler);
        sceneRole.role.AddEventListener(RoleEvent.NameChange, NameChangeHandler);
		sceneRole.AddEventListener(SceneRoleEvent.CullingChange, CullingChangeHandler);
        sceneRole.AddEventListener(SceneRoleEvent.ActiveChange, ActiveChangeHandler);
		sceneRole.AddEventListener(SceneRoleEvent.LayerChange, LayerChangeHandler);
        sceneRole.AddEventListener(SceneRoleEvent.PositionChange, PositionChangeHandler);
        sceneRole.AddEventListener(SceneRoleEvent.RotationChange, RotationChangeHandler);
        sceneRole.AddEventListener(SceneRoleEvent.LocalScaleChange, LocalScaleChangeHandler);
	}
	
	protected virtual void Unlisten()
	{
        sceneRole.role.RemoveEventListener(RoleEvent.EntityIdChange, EntityIdChangeHandler);
        sceneRole.role.RemoveEventListener(RoleEvent.NameChange, NameChangeHandler);
		sceneRole.RemoveEventListener(SceneRoleEvent.CullingChange, CullingChangeHandler);
        sceneRole.RemoveEventListener(SceneRoleEvent.ActiveChange, ActiveChangeHandler);
		sceneRole.RemoveEventListener(SceneRoleEvent.LayerChange, LayerChangeHandler);
        sceneRole.RemoveEventListener(SceneRoleEvent.PositionChange, PositionChangeHandler);
        sceneRole.RemoveEventListener(SceneRoleEvent.RotationChange, RotationChangeHandler);
        sceneRole.RemoveEventListener(SceneRoleEvent.LocalScaleChange, LocalScaleChangeHandler);
	}
	
	protected virtual void TriggerEntityChange()
	{
		EntityIdChangeHandler(null);
	}

	protected virtual void TriggerPropertyChange()
	{
		CullingChangeHandler(null);
        ActiveChangeHandler(null);
		LayerChangeHandler(null);
        NameChangeHandler(null);
        PositionChangeHandler(null);
        RotationChangeHandler(null);
        LocalScaleChangeHandler(null);
	}

	protected virtual void AttachComponent()
	{
		behaviour = entity.AddComponent<T2>();
		behaviour.Link(sceneRole);
	}

	protected virtual void DettachComponent()
	{
		GameUtil.Destroy(behaviour);
		behaviour = null;
	}
	
	private void EntityIdChangeHandler(IEvent obj)
	{
		int loadEntityId = sceneRole.role.GetInt(Role.entityId);
		if (!sceneRole.culling && entityId != loadEntityId)
		{
			loadProxy.LoadEntity(loadEntityId, (GameObject go) =>
            {
				if (entityId == -1)
				{
					loadProxy.RecycleEntity(loadEntityId, go);
				}
				else
				{
					RecycleEntity();
					entityId = loadEntityId;
					entity = go;
					AttachComponent();
					TriggerPropertyChange();
				}
			});
		}
	}

	private void CullingChangeHandler(IEvent obj)
	{
		if (sceneRole.culling)
		{
			if (entity != null)
			{
				GameUtil.SetLayer(entity, GameLayer.Culling);
			}
		}
		else
		{
			if (entity == null)
			{
                TriggerEntityChange();
			}
            else
            {
                LayerChangeHandler(null);
            }
		}
	}

    private void ActiveChangeHandler(IEvent obj)
    {
        if (entity != null)
        {
            entity.SetActive(sceneRole.active);
        }
    }

	private void LayerChangeHandler(IEvent obj)
	{
		if (!sceneRole.culling && entity != null)
		{
			GameUtil.SetLayer(entity, sceneRole.layer);
		}
	}

    private void NameChangeHandler(IEvent obj)
    {
        if (entity != null)
        {
            entity.name = sceneRole.role.GetString(Role.name);
        }
    }

    private void PositionChangeHandler(IEvent obj)
    {
        if (entity != null)
        {
            entity.transform.position = GameUtil.Position3DZ22DY(sceneRole.position);
        }
    }

    private void RotationChangeHandler(IEvent obj)
    {
        if (entity != null)
        {
            entity.transform.rotation = Quaternion.Euler(sceneRole.rotation);
        }
    }

    private void LocalScaleChangeHandler(IEvent obj)
    {
        if (entity != null)
        {
            entity.transform.localScale = sceneRole.localScale;
        }
    }

	private void RecycleEntity()
	{
		if (entityId > 0 && entity != null)
		{
			loadProxy.RecycleEntity(entityId, entity);
		}
		entityId = 0;
	}

	public void Dispose()
	{
		Unlisten();
		DettachComponent();
		RecycleEntity();
		entityId = -1;
		entity = null;
		sceneRole = null;
		loadProxy = null;
	}
}