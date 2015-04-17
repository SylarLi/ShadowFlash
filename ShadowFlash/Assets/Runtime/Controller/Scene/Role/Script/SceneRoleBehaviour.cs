using Core;
using System;
using UnityEngine;

public class SceneRoleBehaviour<T> : MonoBehaviour where T : ISceneRole
{
	protected T sceneRole;
	
    protected PhysicsProxy physicsProxy;

    public virtual void Listen(T value)
	{
        sceneRole = value;
        sceneRole.AddEventListener(SceneRoleEvent.SceneRoleControllTypeChange, ControllTypeChangeHandler);
        sceneRole.AddEventListener(SceneRoleEvent.AirChange, AirChangeHandler);
	}

    protected virtual void UnListen()
    {
        sceneRole.RemoveEventListener(SceneRoleEvent.SceneRoleControllTypeChange, ControllTypeChangeHandler);
        sceneRole.RemoveEventListener(SceneRoleEvent.AirChange, AirChangeHandler);
    }

    private void ControllTypeChangeHandler(IEvent e)
    {
        
    }

    private void AirChangeHandler(IEvent e)
    {

    }

    protected virtual void Awake()
    {
        physicsProxy = new PhysicsProxy(GetComponent<Rigidbody2D>());
        physicsProxy.Awake();
    }

    protected virtual void OnEnable()
    {
        physicsProxy.OnEnable();
    }

    protected virtual void OnDisable()
    {
        physicsProxy.OnDisable();
    }

    protected virtual void Update()
	{
        physicsProxy.Update();
        transform.position = physicsProxy.Get2DPosition();
        sceneRole.SyncPosition(physicsProxy.Get3DPosition());
		sceneRole.SyncRotation(transform.rotation.eulerAngles);
		sceneRole.SyncLocalScale(transform.localScale);
        sceneRole.air = !Mathf.Approximately(sceneRole.position.z, GameConst.FloorHeight);
	}

    protected virtual void FixedUpdate()
	{
         physicsProxy.FixedUpdate();
	}

    protected virtual void OnDestroy()
    {
        UnListen();
        physicsProxy.OnDestroy();
        physicsProxy = null;
        sceneRole = default(T);
    }
}