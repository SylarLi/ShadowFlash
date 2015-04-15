using Core;
using UnityEngine;
using System.Collections.Generic;

public class SceneController : IController
{
	private SceneModel sceneModel;

	private LoadProxy loadProxy;

	private Dictionary<ISceneRole, ISceneRoleView> sceneRoles;

	public SceneController()
	{
		sceneRoles = new Dictionary<ISceneRole, ISceneRoleView>();
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
            Test();
		});
	}

    private void Test()
    {
        Role role = new Role();
        role.SetLong(Role.id, 1);
        role.SetInt(Role.entityId, 100001);
        role.SetEnum<RoleType>(Role.type, RoleType.Player);
        role.SetString(Role.name, "Player");
        PlayerSceneRole sceneRole = new PlayerSceneRole(role);
        sceneRole.active = true;
        sceneRole.self = true;
        sceneModel.AddSceneRole(sceneRole);
    }

	private void AddRoleHandler(IEvent obj)
	{
		AddSceneRole(obj.data as ISceneRole);
	}

	private void RemoveRoleHandler(IEvent obj)
	{
		RemoveSceneRole(obj.data as ISceneRole);
	}

	private void AddSceneRole(ISceneRole role)
	{
		if (!sceneRoles.ContainsKey(role))
		{
			ISceneRoleView view = null;
			switch (role.type)
			{
				case RoleType.Player:
				{
					view = new PlayerSceneRoleView(loadProxy, role as PlayerSceneRole);
					break;
				}
                case RoleType.Monster:
				{
					view = new MonsterSceneRoleView(loadProxy, role as MonsterSceneRole);
					break;
				}
                case RoleType.Npc:
				{
					view = new NpcSceneRoleView(loadProxy, role as NpcSceneRole);
					break;
				}
				default:
				{
					throw new System.NotSupportedException(role.type + " is not supported");
				}
			}
			sceneRoles.Add(role, view);
		}
	}

	private void RemoveSceneRole(ISceneRole role)
	{
		if (sceneRoles.ContainsKey(role))
		{
			sceneRoles[role].Dispose();
			sceneRoles.Remove(role);
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

