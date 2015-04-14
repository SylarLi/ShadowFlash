using Core;
using System.Collections.Generic;

public class SceneModel : EventDispatcher, IModel
{
	private int _mapId;

	private int _sceneId;

	private SceneState _state;

	private Dictionary<long, ISceneRole> _roles;

	public SceneModel()
	{
		_mapId = 0;
		_sceneId = 0;
		_state = SceneState.Loaded;
		_roles = new Dictionary<long, ISceneRole>();
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

	public T FindRole<T>(long id) where T : class, ISceneRole
	{
		T role = null;
		if (_roles.ContainsKey(id))
		{
			role = _roles[id] as T;
		}
		return role;
	}

	public bool AddRole(ISceneRole role)
	{
		bool result = false;
		if (!_roles.ContainsKey(role.id))
		{
			_roles.Add(role.id, role);
			ListenRole(role);
			DispatchEvent(new SceneEvent(SceneEvent.AddRole, role));
			result = true;
		}
		return result;
	}

	public bool RemoveRole(long id)
	{
		bool result = false;
		ISceneRole role = FindRole<ISceneRole>(id);
		if (role != null)
		{
			_roles.Remove(role.id);
			UnListenRole(role);
			DispatchEvent(new SceneEvent(SceneEvent.RemoveRole, role));
			result = true;
		}
		return result;
	}

	public void RemoveRoleByType(SceneRoleType type)
	{
		Dictionary<long, ISceneRole>.KeyCollection keys = _roles.Keys;
		foreach (long id in keys)
		{
			if (_roles[id].type == type)
			{
				RemoveRole(id);
			}
		}
	}

	public void RemoveRoleAll()
	{
		Dictionary<long, ISceneRole>.KeyCollection keys = _roles.Keys;
		foreach (long id in keys)
		{
			RemoveRole(id);
		}
	}

	private void ListenRole(ISceneRole role)
	{
		switch (role.type)
		{
			case SceneRoleType.Player:
			{
				role.AddEventListener(PlayerSceneRoleEvent.SelfChange, PlayerSelfChangeHandler);
				break;
			}
		}
	}

	private void UnListenRole(ISceneRole role)
	{
		switch (role.type)
		{
			case SceneRoleType.Player:
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
			foreach (ISceneRole each in _roles.Values)
			{
				if (each.type == SceneRoleType.Player)
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