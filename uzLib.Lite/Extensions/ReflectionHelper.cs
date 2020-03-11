using System;
using System.Reflection;
using UnityEngine;

namespace uzLib.Lite.Extensions
{
    public static class ReflectionHelper
    {
        public static void InvokeExceptionSafe(this object obj, string methodName, params object[] args)
        {
            try
            {
                obj.Invoke(methodName, args);
            }
            catch (Exception ex)
            {
#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5
                Console.WriteLine(ex, Color.Red);
#else
                Debug.LogException(ex);
#endif
            }
        }

        public static void Invoke(this object obj, string methodName, params object[] args)
        {
            var method = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method != null)
            {
                method.Invoke(obj, args);
            }
        }
    }
}