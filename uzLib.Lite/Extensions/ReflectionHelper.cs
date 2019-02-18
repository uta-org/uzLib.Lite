using System;
using System.Reflection;

namespace uzLib.Lite.Extensions
{
    public static class ReflectionHelper
    {
        public static void InvokeStaticMethod(this Assembly asm, string className, string methodName, params object[] parameters)
        {
            asm.GetType(className).GetMethod(methodName).Invoke(null, parameters);
        }

        public static bool HasMethod(this Assembly asm, string className, string methodName)
        {
            return asm.GetType(className).GetMethod(methodName) != null;
        }
    }
}