using Core;
using UnityEngine;
using System.Collections.Generic;

public class SceneController : IController
{
	private SceneModel sceneModel;

	private LoadProxy loadProxy;

	private Dictionary<ISceneRole, ISceneRoleView> roles;

	public SceneController()
	{
		roles = new Dictionary<ISceneRole, ISceneRoleView>();
		loadProxy = GameObject.Find("LoadProxy").GetComponent<LoadProxy>();
	}

	private void ListenModel()
	{
		sceneModel.AddEventListener(SceneEvent.MapChange, MapChangeHandler);
		sceneModel.AddEventListener(SceneEvent.SceneChange, SceneChangeHandler);
		sceneModel.AddEventListener(SceneEvent.AddRole, AddRoleHandler);
		sceneModel.AddEventListener(SceneEvent.RemoveRole, RemoveRoleHandler);
	}

	private void MapChangeHandler(IEvent obj)
	{
		// 暂时相等，以后取配置表
		sceneModel.sceneId = sceneModel.mapId;
	}

	private void SceneChangeHandler(IEvent obj)
	{
		sceneModel.state = SceneState.Loading;
		loadProxy.LoadEntity(sceneModel.sceneId, (GameObject go) => 
        { 
			sceneModel.state = SceneState.Loaded; 
			Debug.Log("Scene load complete: " + Application.loadedLevelName);
		});
	}

	private void AddRoleHandler(IEvent obj)
	{
		AddRole(obj.data as ISceneRole);
	}

	private void RemoveRoleHandler(IEvent obj)
	{
		RemoveRole(obj.data as ISceneRole);
	}

	private void AddRole(ISceneRole role)
	{
		if (!roles.ContainsKey(role))
		{
			ISceneRoleView view = null;
			switch (role.type)
			{
				case SceneRoleType.Player:
				{
					view = new PlayerSceneRoleView(loadProxy, role as PlayerSceneRole);
					break;
				}
				case SceneRoleType.Monster:
				{
					view = new MonsterSceneRoleView(loadProxy, role as MonsterSceneRole);
					break;
				}
				case SceneRoleType.Npc:
				{
					view = new NpcSceneRoleView(loadProxy, role as NpcSceneRole);
					break;
				}
				default:
				{
					throw new System.NotSupportedException(role.type + " is not supported");
				}
			}
			roles.Add(role, view);
		}
	}

	private void RemoveRole(ISceneRole role)
	{
		if (roles.ContainsKey(role))
		{
			roles[role].Dispose();
			roles.Remove(role);
		}
	}

	#region IController implementation

	public void InjectModel(IModel[] models)
	{
		sceneModel = models [0] as SceneModel;
		ListenModel();
	}

	public ModelType type
	{
		get
		{
			return ModelType.Scene;
		}
	}

	public ModelType[] dependTypes
	{
		get
		{
			return null;
		}
	}

	#endregion
}

