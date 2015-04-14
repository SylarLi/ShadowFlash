using UnityEngine;

public class GameUtil
{
	public static void Destroy(Object o)
	{
		if (o != null)
		{
			Object.Destroy(o);
		}
	}

	/// <summary>
	/// 递归设置layer
	/// </summary>
	/// <param name="go">Go.</param>
	public static void SetLayer(GameObject go, int layer)
	{
		go.layer = layer;
		foreach (Transform child in go.transform)
		{
			SetLayer(child.gameObject, layer);
		}
	}

	/// <summary>
	/// 透视坐标转正交坐标
	/// 将z坐标合并到y中
	/// </summary>
	/// <param name="origin"></param>
	/// <param name="angle">0 <= camera euler angle x < 90</param>
	/// <returns></returns>
	public static Vector2 Position3DZ22DY(Vector3 origin, float angle = GameConst.CameraAngle)
	{
		return new Vector2(origin.x, origin.y + origin.z * Mathf.Tan((90 - angle) * Mathf.PI / 180));
	}
	
	/// <summary>
	/// 正交坐标转为透视坐标
	/// 将z坐标从y中分离出来
	/// </summary>
	/// <param name="origin"></param>
	/// <param name="y"></param>
	/// <param name="angle"></param>
	/// <returns></returns>
	public static Vector3 Position2DY23DZ(Vector2 origin, float y, float angle = GameConst.CameraAngle)
	{
		return new Vector3(origin.x, y, (origin.y - y) / Mathf.Tan((90 - angle) * Mathf.PI / 180));
	}
}