using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Extensions
{
    /// <summary>
    ///     The Assembly Helper
    /// </summary>
    public static class AssemblyHelper
    {
        /// <summary>
        ///     Gets the types with attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesWithAttribute<T>(this Assembly[] assemblies)
            where T : Attribute
        {
            foreach (var assembly in assemblies)
            foreach (var type in assembly.GetTypesWithAttribute<T>())
                yield return type;
        }

        /// <summary>
        ///     Gets the custom attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this Assembly[] assemblies)
            where T : Attribute
        {
            foreach (var assembly in assemblies)
            foreach (var attr in CustomAttributeExtensions.GetCustomAttributes<T>(assembly))
                yield return attr;
        }

        /// <summary>
        ///     Gets the types with attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesWithAttribute<T>(this Assembly assembly)
            where T : Attribute
        {
            foreach (var type in assembly.GetTypes())
                if (type.GetCustomAttributes(typeof(T), true).Length > 0)
                    yield return type;
        }

        /// <summary>
        ///     Gets the custom attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this Assembly assembly)
            where T : Attribute
        {
            foreach (var type in assembly.GetTypes())
            {
                var attrs = type.GetCustomAttributes(typeof(T), true);
                if (attrs.Length > 0)
                    foreach (var attr in attrs)
                        yield return (T) attr;
            }
        }
    }
}