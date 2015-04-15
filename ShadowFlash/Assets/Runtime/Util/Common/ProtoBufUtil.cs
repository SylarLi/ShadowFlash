using Action;
using System;
using System.IO;
using UnityEngine;

public class ProtoBufUtil
{
    private static ProtoBuf.Meta.RuntimeTypeModel definedModel;

    public static ProtoBuf.Meta.RuntimeTypeModel DefinedModel
    {
        get
        {
            if (definedModel == null)
            {
                definedModel = ProtoBuf.Meta.RuntimeTypeModel.Default;
                definedModel[typeof(ICondition)].AddSubType(1, typeof(Condition));
                definedModel[typeof(IActionInfo)].AddSubType(2, typeof(ActionInfo));
                definedModel[typeof(IRole)].AddSubType(3, typeof(Role));

                definedModel.Add(typeof(Vector2), true);
                definedModel.Add(typeof(Vector3), true);
                definedModel.Add(typeof(Vector4), true);
                definedModel.Add(typeof(Keyframe), true);
                definedModel.Add(typeof(AnimationCurve), true);
            }
            return definedModel;
        }
    }

    public static string Serialize<T>(T info) where T : class
    {
        string serial = "";
        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DefinedModel.Serialize(stream, info);
                serial = Convert.ToBase64String(stream.ToArray());
            }
        }
        catch (Exception)
        {
            serial = "";
        }
        return serial;
    }

    public static T Deserialize<T>(string serial) where T : class
    {
        T info = null;
        try
        {
            byte[] bytes = Convert.FromBase64String(serial);
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
                stream.Flush();
                info = DefinedModel.Deserialize(stream, null, typeof(T)) as T;
            }
        }
        catch (Exception)
        {
            info = null;
        }
        return info;
    }
}
