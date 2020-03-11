using System;
using System.Collections.Generic;
using System.Reflection;
using uzLib.Lite.Extensions;

namespace UnityEngine.Extensions
{
    public static class AttributeHelper
    {
        /// <summary>
        /// Gets the types with custom attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttr">The type of the attribute.</typeparam>
        /// <param name="objs">The objs.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">objs</exception>
        public static IEnumerable<T> GetTypesWithCustomAttributes<T, TAttr>(this T[] objs)
            where TAttr : Attribute
        {
            if (objs.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(objs));

            foreach (var obj in objs)
                if (obj.GetType().GetCustomAttribute<TAttr>() != null)
                    yield return obj;
        }
    }
}