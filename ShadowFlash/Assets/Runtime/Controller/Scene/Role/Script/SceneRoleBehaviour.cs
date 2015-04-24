using Core;
using System;
using UnityEngine;

public class SceneRoleBehaviour<T> : MonoBehaviour where T : ISceneRole
{
	protected T sceneRole;
	
    protected PhysicsProxy physicsProxy;

    protected int gravity;

    public virtual void Listen(T value)
	{
        sceneRole = value;
        sceneRole.AddEventListener(SceneRoleEvent.AirChange, AirChangeHandler);
        sceneRole.AddEventListener(SceneRoleEvent.UseGravityChange, UseGravityChangeHandler);
        sceneRole.AddEventListener(SceneRoleEvent.UseFloorDragChange, UseFloorDragChangeHandler);
        sceneRole.AddEventListener(SceneRoleEvent.SceneRoleControllTypeChange, ControllTypeChangeHandler);
	}

    protected virtual void UnListen()
    {
        sceneRole.RemoveEventListener(SceneRoleEvent.AirChange, AirChangeHandler);
        sceneRole.RemoveEventListener(SceneRoleEvent.UseGravityChange, UseGravityChangeHandler);
        sceneRole.RemoveEventListener(SceneRoleEvent.UseFloorDragChange, UseFloorDragChangeHandler);
        sceneRole.RemoveEventListener(SceneRoleEvent.SceneRoleControllTypeChange, ControllTypeChangeHandler);
    }

    private void UseGravityChangeHandler(IEvent e)
    {
        UpdateGravity();
    }

    private void UseFloorDragChangeHandler(IEvent e)
    {
        UpdateFloorDrag();
    }

    private void AirChangeHandler(IEvent e)
    {
        UpdateGravity();
        UpdateFloorDrag();
        UpdateAir();
    }

    private void ControllTypeChangeHandler(IEvent e)
    {
        
    }

    protected virtual void UpdateGravity()
    {
        if (sceneRole.air && sceneRole.useGravity)
        {
            if (gravity == 0)
            {
                gravity = physicsProxy.NatureFoward(new Vector3(0, 0, -1), GetComponent<Rigidbody2D>().mass * GameConst.Gravity);
            }
        }
        else
        {
            if (gravity > 0)
            {
                physicsProxy.RemoveNature(gravity);
                gravity = 0;
            }
        }
    }

    protected virtual void UpdateFloorDrag()
    {
        physicsProxy.SetHorizontalDragEnable(sceneRole.useFloorDrag && !sceneRole.air);
    }

    protected virtual void UpdateAir()
    {
        if (!sceneRole.air)
        {
            if (sceneRole.controllType == SceneRoleControllType.Free)
            {
                physicsProxy.SetHorizontalVelocity(Vector2.zero);
            }
            physicsProxy.SetVerticalVelocity(0);
            physicsProxy.SetVerticalPosition(GameConst.FloorHeight);
        }
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
        Vector3 position3D = physicsProxy.Get3DPosition();
        Vector3 nextPosition3D = physicsProxy.GetNext3DPosition();
        sceneRole.air = position3D.z >= GameConst.FloorHeight && nextPosition3D.z > 0;
        sceneRole.SyncPosition(position3D);
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