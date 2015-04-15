using UnityEngine;
using Core;
using System;
using System.Collections;

public class LoadProxy : MonoBehaviour
{
    private const string CharacterPath = "Character/";

    private const string EffectPath = "Effect/";

	public void LoadEntity(int entityId, Action<GameObject> callBack = null)
	{
		if (entityId < 100000)
		{
			StartCoroutine(LoadSceneCoroutine(entityId, callBack));
		}
		else if (entityId < 200000)
		{
            StartCoroutine(LoadGameObjectCoroutine(CharacterPath + entityId, callBack));
		}
		else if (entityId < 300000)
		{
            StartCoroutine(LoadGameObjectCoroutine(EffectPath + entityId, callBack));
		}
		else
		{
			throw new InvalidOperationException("Current load entityId is invalide, scene id : [0, 99999], character id : [100000, 199999], effect id : [200000, 299999]");
		}
	}

	public void RecycleEntity(int entityId, GameObject go)
	{
		UnityEngine.Object.Destroy(go);
	}

	private IEnumerator LoadSceneCoroutine(int index, Action<GameObject> callBack = null)
	{
		yield return Resources.UnloadUnusedAssets();
		yield return Application.LoadLevelAsync(index);
		if (callBack != null)
		{
			callBack(null);
		}
	}

	private IEnumerator LoadGameObjectCoroutine(string path, Action<GameObject> callBack = null)
	{
		GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(path)) as GameObject;
		go.SetActive(false);
		GameObject.DontDestroyOnLoad(go);
		yield return new WaitForEndOfFrame();
		if (callBack != null)
		{
			callBack(go);
		}
	}
}