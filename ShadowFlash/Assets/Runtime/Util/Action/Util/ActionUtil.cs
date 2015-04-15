using System;
using System.IO;
using UnityEngine;

namespace Action
{
    public static class ActionUtil
    {
        public static Type ActionTypeToType(ActionType type)
        {
            switch (type)
            {
                default:
                    {
                        return typeof(ActionInfo);
                    }
            }
        }

        public static string GetTransformPath(GameObject gameObject)
        {
            string path = "";
            GameObject go = gameObject;
            while (go != null)
            {
                path = (go != gameObject ? go.name : "") + (!string.IsNullOrEmpty(path) ? "/" + path : path);
                go = go.transform.parent != null ? go.transform.parent.gameObject : null;
            }
            int first = path.IndexOf("/");
            if (first != -1)
            {
                path = path.Substring(first + 1);
            }
            return path;
        }
    }
}
