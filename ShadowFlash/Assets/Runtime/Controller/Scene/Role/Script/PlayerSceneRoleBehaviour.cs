using Core;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneRoleBehaviour : SceneRoleBehaviour<PlayerSceneRole>
{
    private Dictionary<KeyCode, int> keyboardForces;

    public override void Listen(PlayerSceneRole value)
    {
        base.Listen(value);
        sceneRole.AddEventListener(PlayerSceneRoleEvent.DirectionKeyLockChange, DirectionKeyLockChangeHandler);
    }

    protected override void UnListen()
    {
        base.UnListen();
        sceneRole.RemoveEventListener(PlayerSceneRoleEvent.DirectionKeyLockChange, DirectionKeyLockChangeHandler);
    }

    private void DirectionKeyLockChangeHandler(IEvent e)
    {
        KeyCode[] keys = new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
        foreach (KeyCode key in keys)
        {
            if (keyboardForces.ContainsKey(key))
            {
                physicsProxy.RemoveForce(keyboardForces[key]);
                keyboardForces.Remove(key);
            }
        }
    }

    public void ListenKeyboard(KeyBoardListener listener)
    {
        listener.RegistKeyDown(KeyCode.W, ForceDirectionKeyDown);
        listener.RegistKeyDown(KeyCode.S, ForceDirectionKeyDown);
        listener.RegistKeyDown(KeyCode.A, ForceDirectionKeyDown);
        listener.RegistKeyDown(KeyCode.D, ForceDirectionKeyDown);

        listener.RegistKeyUp(KeyCode.W, ForceDirectionKeyUp);
        listener.RegistKeyUp(KeyCode.S, ForceDirectionKeyUp);
        listener.RegistKeyUp(KeyCode.A, ForceDirectionKeyUp);
        listener.RegistKeyUp(KeyCode.D, ForceDirectionKeyUp);

        listener.RegistKeyDown(KeyCode.K, TriggerJumpKeyDown);
    }

    private void ForceDirectionKeyDown(KeyCode key)
    {
        if (!sceneRole.directionKeyLock)
        {
            Direction direction = GameUtil.KeyCodeToDirection(key);
            keyboardForces[key] = physicsProxy.ForceFoward(direction, GameConst.FloorRunForce);
        }
    }

    private void ForceDirectionKeyUp(KeyCode key)
    {
        Direction direction = GameUtil.KeyCodeToDirection(key);
        if (keyboardForces.ContainsKey(key))
        {
            physicsProxy.RemoveForce(keyboardForces[key]);
            keyboardForces.Remove(key);
        }
    }

    private void TriggerJumpKeyDown(KeyCode key)
    {
        if (Mathf.Approximately(sceneRole.position.z, 0))
        {
            physicsProxy.AddImpulseVelocity(new Vector3(0, 0, GameConst.JumpSpeed));
        }
    }

    protected override void Awake()
    {
        base.Awake();
        keyboardForces = new Dictionary<KeyCode, int>();
    }

    protected override void Update()
    {
        base.Update();
        if (sceneRole.self)
        {
            sceneRole.directionKeyLock = physicsProxy.GetVelocity().z < 0;
        }
        Debug.Log(Time.time + " " + Input.GetAxis("Horizontal"));
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        keyboardForces = null;
    }
}