using Core;
using System;
using UnityEngine;

public class SceneRoleBehaviour<T> : MonoBehaviour where T : ISceneRole
{
	protected ISceneRole sceneRole;
	
    protected SceneRolePhysics physicsProxy;

    public void Link(T sceneRole)
	{
        this.sceneRole = sceneRole;
	}

    protected virtual void Awake()
    {
        physicsProxy = new SceneRolePhysics(GetComponent<Rigidbody2D>());
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
	}

    protected virtual void FixedUpdate()
	{
         physicsProxy.FixedUpdate();
	}

    protected virtual void OnDestroy()
    {
        physicsProxy.OnDestroy();
        physicsProxy = null;
        sceneRole = null;
    }
}