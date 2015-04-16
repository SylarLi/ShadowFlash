using System.Collections.Generic;
using UnityEngine;

public class KeyBoardListener : MonoBehaviour
{
    private Dictionary<KeyCode, System.Action> keyDowns;

    private Dictionary<KeyCode, System.Action> keyUps;

    private Dictionary<KeyCode, System.Action> keyPresses;

    public void RegistKeyDown(KeyCode key, System.Action action)
    {
        keyDowns[key] = action;
    }

    public void RegistKeyUp(KeyCode key, System.Action action)
    {
        keyUps[key] = action;
    }

    public void RegistKeyPress(KeyCode key, System.Action action)
    {
        keyPresses[key] = action;
    }

    private void Awake()
    {
        keyDowns = new Dictionary<KeyCode, System.Action>();
        keyUps = new Dictionary<KeyCode, System.Action>();
        keyPresses = new Dictionary<KeyCode, System.Action>();
    }

    private void Update()
    {
        foreach (KeyValuePair<KeyCode, System.Action> pair in keyDowns)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                pair.Value();
            }
        }
        foreach (KeyValuePair<KeyCode, System.Action> pair in keyUps)
        {
            if (Input.GetKeyUp(pair.Key))
            {
                pair.Value();
            }
        }
        foreach (KeyValuePair<KeyCode, System.Action> pair in keyPresses)
        {
            if (Input.GetKey(pair.Key))
            {
                pair.Value();
            }
        }
    }
}
