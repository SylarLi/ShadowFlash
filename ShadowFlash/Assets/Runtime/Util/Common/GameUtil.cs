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
    /// 拷贝2D刚体属性
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static void CopyRigidbody2DProperty(Rigidbody2D from, Rigidbody2D to)
    {
        to.mass = from.mass;
        to.drag = from.drag;
        to.angularDrag = from.angularDrag;
        to.fixedAngle = from.fixedAngle;
        to.gravityScale = from.gravityScale;
        to.isKinematic = from.isKinematic;
    }

    public static float Position3DZ22DY(float z, float angle = GameConst.CameraAngle)
    {
        return z * Mathf.Tan((90 - angle) * Mathf.PI / 180);
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
        return new Vector2(origin.x, origin.y + Position3DZ22DY(origin.z, angle));
	}

    public static float Position2DY23DZ(float y, float projectY, float angle = GameConst.CameraAngle)
    {
        return (y - projectY) / Mathf.Tan((90 - angle) * Mathf.PI / 180);
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
        return new Vector3(origin.x, y, Position2DY23DZ(origin.y, y, angle));
	}
}