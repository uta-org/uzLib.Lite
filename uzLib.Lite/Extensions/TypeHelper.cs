using System;
using System.Collections.Generic;
using System.Linq;
using uzLib.Lite.Extensions;

namespace UnityEngine.Extensions
{
    /// <summary>
    ///     The Type Helper
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        ///     Gets the name of the friendly type.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="fullName">if set to <c>true</c> [full name].</param>
        /// <param name="genericChar">The generic character.</param>
        /// <returns></returns>
        public static string GetFriendlyTypeName(this Type t, bool fullName = true,
            EnclosingGenericChar genericChar = EnclosingGenericChar.Brackets)
        {
            return t.IsGenericType ? t.Namespace + "." + t.GetFriendlyTypeName(genericChar) : t.FullName;
        }

        /// <summary>
        ///     Gets the name of the friendly type.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="genericChar">The generic character.</param>
        /// <returns></returns>
        public static string GetFriendlyTypeName(this Type t,
            EnclosingGenericChar genericChar = EnclosingGenericChar.Brackets)
        {
            var typeName = t.Name.StripStartingWith("`");
            var genericArgs = t.GetGenericArguments();
            if (genericArgs.Length > 0)
            {
                typeName += GetEnclosingChar(true, genericChar);
                foreach (var genericArg in genericArgs) typeName += genericArg.GetFriendlyTypeName(genericChar) + ", ";
                typeName = typeName.TrimEnd(',', ' ') + GetEnclosingChar(false, genericChar);
            }

            return typeName;
        }

        /// <summary>
        ///     Gets the enclosing character.
        /// </summary>
        /// <param name="start">if set to <c>true</c> [start].</param>
        /// <param name="genericChar">The generic character.</param>
        /// <returns></returns>
        private static char GetEnclosingChar(bool start, EnclosingGenericChar genericChar)
        {
            switch (genericChar)
            {
                case EnclosingGenericChar.Brackets:
                    return start ? '[' : ']';

                case EnclosingGenericChar.MathSigns:
                    return start ? '<' : '>';
            }

            return default;
        }

        /// <summary>
        ///     Gets the attributes from fields in.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T[] GetAttributesFromFieldsIn<T>(this Type type)
            where T : class
        {
            return type.GetFields().Select(f => f.GetCustomAttributes(typeof(T), true))
                .Select(o => o.Select(t => t as T)).SelectMany(t => t).ToArray();
        }

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

        /// <summary>
        ///     Gets the inheritance hierarchy.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetInheritanceHierarchy(this Type type)
        {
            for (var current = type; current != null; current = current.BaseType) yield return current;
        }
    }
}