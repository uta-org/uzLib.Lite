using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace uzLib.Lite.ExternalCode.Utils
{
    public static class MonoUtils
    {
        public static MonoBehaviour FindExecutingInEditModeMonoBehaviour()
        {
            return Object.FindObjectsOfType<MonoBehaviour>().FirstOrDefault(c => Attribute.GetCustomAttribute(c.GetType(), typeof(ExecuteInEditMode)) != null);
        }
    }
}