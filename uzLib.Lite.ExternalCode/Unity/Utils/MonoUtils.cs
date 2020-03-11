using System;
using System.Linq;
using UnityEngine;
using uzLib.Lite.ExternalCode.Core;
using uzLib.Lite.ExternalCode.Unity.Utils;
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

    [AutoInstantiate]
    public abstract class HandledMonoBehaviour : MonoSingleton<HandledMonoBehaviour>
    {
        public abstract void OnQuit();

        public void OnApplicationQuit()
        {
            OnQuit();
        }
    }
}