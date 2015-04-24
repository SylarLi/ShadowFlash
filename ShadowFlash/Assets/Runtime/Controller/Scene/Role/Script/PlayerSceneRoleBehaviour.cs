using Core;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneRoleBehaviour : SceneRoleBehaviour<PlayerSceneRole>
{
    public override void Listen(PlayerSceneRole value)
    {
        base.Listen(value);
    }

    protected override void UnListen()
    {
        base.UnListen();
    }

    protected override void UpdateFloorDrag()
    {
        
    }

    public void ListenKeyboard(KeyBoardListener listener)
    {
        listener.RegistKeyPress(KeyCode.W, DirectionKeyPress);
        listener.RegistKeyPress(KeyCode.S, DirectionKeyPress);
        listener.RegistKeyPress(KeyCode.A, DirectionKeyPress);
        listener.RegistKeyPress(KeyCode.D, DirectionKeyPress);
        listener.RegistKeyDown(KeyCode.K, JumpKey);
    }

    private void DirectionKeyPress(KeyCode key)
    {
        if (sceneRole.controllType == SceneRoleControllType.Free)
        {
            if (physicsProxy.GetVelocity().z >= 0)
            {
                Vector2 horizontalVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * GameConst.MoveSpeed;
                physicsProxy.SetHorizontalVelocity(horizontalVelocity);
            }
        }
    }

    private void JumpKey(KeyCode key)
    {
        if (sceneRole.controllType == SceneRoleControllType.Free)
        {
            if (!sceneRole.air)
            {
                physicsProxy.AddImpulseVelocity(new Vector3(0, 0, GameConst.JumpSpeed));
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}