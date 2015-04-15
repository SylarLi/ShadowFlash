using Core;
using System.Collections.Generic;

public class SceneModel : EventDispatcher, IModel
{
	private int _mapId;

	private int _sceneId;

	private SceneState _state;

	private Dictionary<long, ISceneRole> _sceneRoles;

	public SceneModel()
	{
		_mapId = 0;
		_sceneId = 0;
		_state = SceneState.Loaded;
        _sceneRoles = new Dictionary<long, ISceneRole>();
	}

	public int mapId
	{
		get
		{
			return _mapId;
		}
		set
		{
			if (_mapId != value)
			{
				_mapId = value;
				DispatchEvent(new SceneEvent(SceneEvent.MapChange));
			}
		}
	}

	public int sceneId
	{
		get
		{
			return _sceneId;
		}
		set
		{
			if (_state == SceneState.Loaded)
			{
				if (_sceneId != value)
				{
					_sceneId = value;
					DispatchEvent(new SceneEvent(SceneEvent.SceneChange));
				}
			}
			else
			{
				throw new System.InvalidOperationException("Change scene's id is invalide while scene is loading");
			}
		}
	}

	public SceneState state
	{
		get
		{
			return _state;
		}
		set
		{
			if (_state != value)
			{
				_state = value;
				DispatchEvent(new SceneEvent(SceneEvent.StateChange));
			}
		}
	}

	public T FindSceneRole<T>(long id) where T : class, ISceneRole
	{
		T role = null;
        if (_sceneRoles.ContainsKey(id))
		{
            role = _sceneRoles[id] as T;
		}
		return role;
	}

	public bool AddSceneRole(ISceneRole sceneRole)
	{
		bool result = false;
        if (!_sceneRoles.ContainsKey(sceneRole.id))
		{
            _sceneRoles.Add(sceneRole.id, sceneRole);
            ListenSceneRole(sceneRole);
            DispatchEvent(new SceneEvent(SceneEvent.AddRole, sceneRole));
			result = true;
		}
		return result;
	}

	public bool RemoveSceneRole(long id)
	{
		bool result = false;
		ISceneRole sceneRole = FindSceneRole<ISceneRole>(id);
        if (sceneRole != null)
		{
            _sceneRoles.Remove(sceneRole.id);
            UnListenSceneRole(sceneRole);
            DispatchEvent(new SceneEvent(SceneEvent.RemoveRole, sceneRole));
			result = true;
		}
		return result;
	}

	public void RemoveRoleByType(RoleType type)
	{
        Dictionary<long, ISceneRole>.KeyCollection keys = _sceneRoles.Keys;
		foreach (long id in keys)
		{
            if (_sceneRoles[id].type == type)
			{
				RemoveSceneRole(id);
			}
		}
	}

	public void RemoveRoleAll()
	{
        Dictionary<long, ISceneRole>.KeyCollection keys = _sceneRoles.Keys;
		foreach (long id in keys)
		{
			RemoveSceneRole(id);
		}
	}

	private void ListenSceneRole(ISceneRole role)
	{
		switch (role.type)
		{
			case RoleType.Player:
			{
				role.AddEventListener(PlayerSceneRoleEvent.SelfChange, PlayerSelfChangeHandler);
				break;
			}
		}
	}

	private void UnListenSceneRole(ISceneRole role)
	{
		switch (role.type)
		{
            case RoleType.Player:
			{
				role.RemoveEventListener(PlayerSceneRoleEvent.SelfChange, PlayerSelfChangeHandler);
				break;
			}
		}
	}

	private void PlayerSelfChangeHandler(IEvent obj)
	{
		PlayerSceneRole targetPlayer = obj.target as PlayerSceneRole;
		if (targetPlayer.self)
		{
			foreach (ISceneRole each in _sceneRoles.Values)
			{
				if (each.type == RoleType.Player)
				{
					PlayerSceneRole eachPlayer = each as PlayerSceneRole;
					if (eachPlayer.self && eachPlayer != targetPlayer)
					{
						eachPlayer.self = false;
					}
				}
			}
		}
	}

	#region IModel implementation

	public ModelType type
	{
		get
		{
			return ModelType.Scene;
		}
	}

	#endregion
}