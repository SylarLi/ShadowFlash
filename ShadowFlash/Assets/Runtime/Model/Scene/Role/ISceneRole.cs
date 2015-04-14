using UnityEngine;
using Core;

/// <summary>
/// 原则: 通用属性(例如泛用的GameObject属性)或者上层属性(例如人物属性,怪物AI树等)可放置在此
/// 但是一些过于细节化的属性(例如Rigidbody2D.velocity或者Force等)基本上是直接由玩家操控或者怪物AI触发的，故直接放在View中处理
/// 同时同步一部分基础属性(例如位置旋转缩放)暴露给外部
/// </summary>
public interface ISceneRole : IEventDispatcher
{
	long id { get; }

	SceneRoleType type { get; }

	int entityId { get; set; }

	/// <summary>
	/// 是否激活，对应GameObject.activeSelf
	/// </summary>
	bool active { get; set; }

	/// <summary>
	/// 是否剔除，会影响到是否加载和摄像机剔除
	/// 剔除之后少数属性的改变不会影响view，直到解除剔除
	/// 这些属性一般都列举在TriggerProperty中
	/// </summary>
	bool culling { get; set; }

	/// <summary>
	/// 是否渲染，对应renderer.enabled
	/// </summary>
	/// <value><c>true</c> if render; otherwise, <c>false</c>.</value>
	bool render { get; set; }

	int layer { get; set; }

	Vector3 position { get; set; }

	Vector3 rotation { get; set; }

	Vector3 localScale { get; set; }

	bool animatorEnabled { get; set; }

	float animatorSpeed { get; set; }

	// ------------ 分界线 -------------- //

	void SyncPosition(Vector3 value);

	void SyncRotation(Vector3 value);

	void SyncLocalScale(Vector3 value);
}

