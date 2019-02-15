using System;
using System.Reflection;

namespace uzLib.Lite.Extensions
{
    public static class ReflectionHelper
    {
        public static void InvokeStaticMethod(this Assembly asm, string className, string methodName, params object[] parameters)
        {
            asm.GetType(className).GetMethod(methodName).Invoke(null, parameters);

            //foreach (var _a in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    foreach (var _t in _a.GetTypes())
            //    {
            //        try
            //        {
            //            if ((_t.Namespace == namespaceName) && _t.IsClass) _t.GetMethod(methodName, (BindingFlags.Static | BindingFlags.Public))?.Invoke(null, parameters);
            //        }
            //        catch { }
            //    }
            //}
        }
    }
}