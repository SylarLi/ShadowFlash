using UnityEngine;
using Core;

/// <summary>
/// 原则: 通用属性(例如泛用的GameObject属性)或者上层属性(例如人物属性,怪物AI树等)放置在此
/// 但是一些过于细节化的属性(例如Rigidbody2D.velocity或者Force等)基本上是直接由玩家操控或者怪物AI触发的，故直接放在View中处理
/// 同时同步一部分基础属性(例如位置旋转缩放)暴露给外部
/// </summary>
public interface ISceneRole : IEventDispatcher
{
    IRole role { get; }

    long id { get; }

    RoleType type { get; }

	/// <summary>
	/// 是否激活，对应GameObject.activeSelf
	/// </summary>
	bool active { get; set; }

	/// <summary>
	/// 是否剔除，会影响到是否加载和摄像机剔除
	/// </summary>
	bool culling { get; set; }

	int layer { get; set; }

	Vector3 position { get; set; }

	Vector3 rotation { get; set; }

	Vector3 localScale { get; set; }

	// ------------ 分界线 -------------- //

	void SyncPosition(Vector3 value);

	void SyncRotation(Vector3 value);

	void SyncLocalScale(Vector3 value);
}

