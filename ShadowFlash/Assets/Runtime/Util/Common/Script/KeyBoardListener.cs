using System.Collections.Generic;
using UnityEngine;

public class KeyBoardListener : MonoBehaviour
{
    private Dictionary<KeyCode, System.Action<KeyCode>> keyDowns;

    private Dictionary<KeyCode, System.Action<KeyCode>> keyUps;

    private Dictionary<KeyCode, System.Action<KeyCode>> keyPresses;

    public void RegistKeyDown(KeyCode key, System.Action<KeyCode> action)
    {
        keyDowns[key] = action;
    }

    public void RegistKeyUp(KeyCode key, System.Action<KeyCode> action)
    {
        keyUps[key] = action;
    }

    public void RegistKeyPress(KeyCode key, System.Action<KeyCode> action)
    {
        keyPresses[key] = action;
    }

    private void Awake()
    {
        keyDowns = new Dictionary<KeyCode, System.Action<KeyCode>>();
        keyUps = new Dictionary<KeyCode, System.Action<KeyCode>>();
        keyPresses = new Dictionary<KeyCode, System.Action<KeyCode>>();
    }

    private void Update()
    {
        foreach (KeyValuePair<KeyCode, System.Action<KeyCode>> pair in keyDowns)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                pair.Value(pair.Key);
            }
        }
        foreach (KeyValuePair<KeyCode, System.Action<KeyCode>> pair in keyUps)
        {
            if (Input.GetKeyUp(pair.Key))
            {
                pair.Value(pair.Key);
            }
        }
        foreach (KeyValuePair<KeyCode, System.Action<KeyCode>> pair in keyPresses)
        {
            if (Input.GetKey(pair.Key))
            {
                pair.Value(pair.Key);
            }
        }
    }

    private void OnDestroy()
    {
        keyDowns.Clear();
        keyUps.Clear();
        keyPresses.Clear();
        keyDowns = null;
        keyUps = null;
        keyPresses = null;
    }
}
