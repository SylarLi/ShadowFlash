using Core;
using UnityEngine;

public class SceneRoleView<T1, T2> : ISceneRoleView where T1 : class, ISceneRole where T2 : SceneRoleBehaviour<T1>
{
	protected LoadProxy loadProxy;

	protected T1 role;

	protected int entityId;

	protected GameObject entity;

	protected T2 behaviour;

	public SceneRoleView(LoadProxy loadProxy, T1 role)
	{
		this.loadProxy = loadProxy;
		this.role = role;
		this.entityId = 0;
		TriggerEntityChange();
		Listen();
	}

	protected virtual void Listen()
	{
		role.AddEventListener(SceneRoleEvent.EntityIdChange, EntityIdChangeHandler);
		role.AddEventListener(SceneRoleEvent.CullingChange, CullingChangeHandler);
		role.AddEventListener(SceneRoleEvent.LayerChange, LayerChangeHandler);
	}
	
	protected virtual void Unlisten()
	{
		role.RemoveEventListener(SceneRoleEvent.EntityIdChange, EntityIdChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.CullingChange, CullingChangeHandler);
		role.RemoveEventListener(SceneRoleEvent.LayerChange, LayerChangeHandler);
	}
	
	protected virtual void TriggerEntityChange()
	{
		EntityIdChangeHandler(null);
	}

	protected virtual void TriggerPropertyChange()
	{
		CullingChangeHandler(null);
		LayerChangeHandler(null);
	}

	protected virtual void AttachComponent()
	{
		behaviour = entity.AddComponent<T2>();
		behaviour.Bind(role);
	}

	protected virtual void DettachComponent()
	{
		GameUtil.Destroy(behaviour);
		behaviour = null;
	}
	
	private void EntityIdChangeHandler(IEvent obj)
	{
		int loadEntityId = role.entityId;
		if (!role.culling && entityId != loadEntityId)
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
		if (role.culling)
		{
			if (entity != null)
			{
				GameUtil.SetLayer(entity, GameLayer.Culling);
			}
		}
		else
		{
			if (entity != null)
			{
				TriggerPropertyChange();
			}
			else
			{
				TriggerEntityChange();
			}
		}
	}

	private void LayerChangeHandler(IEvent obj)
	{
		if (!role.culling && entity != null)
		{
			GameUtil.SetLayer(entity, role.layer);
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
		role = null;
		loadProxy = null;
	}
}