using System;
using UnityEngine;

namespace uzLib.Lite.ExternalCode.Extensions
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

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        ///     Determines whether [is executing in edit mode].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if [is executing in edit mode] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsExecutingInEditMode(this Type type)
        {
            return Attribute.GetCustomAttribute(type, typeof(ExecuteInEditMode)) != null;
        }

#endif

        /// <summary>
        ///     Determines whether this instance is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="struct">The structure.</param>
        /// <returns>
        ///     <c>true</c> if the specified structure is null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull<T>(this T @struct)
            where T : struct
        {
            return @struct.Equals(null) || @struct.Equals(default);
        }
    }
}