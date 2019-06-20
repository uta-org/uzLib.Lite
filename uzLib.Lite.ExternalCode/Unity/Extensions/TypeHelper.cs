using System;

namespace UnityEngine.Extensions
{
    public static class TypeHelper
    {
        /// <summary>
        ///     Gets the instance from singleton.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object GetInstanceFromSingleton(this Type type)
        {
            try
            {
                return type.GetProperty("Instance")?.GetValue(null);
            }
            catch
            {
                return null;
            }
        }
    }
}