using System;

namespace UnityEngine.Extensions
{
    public static class F
    {
        /// <summary>
        ///     Modifies the component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component">The component.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///     component
        ///     or
        ///     action
        /// </exception>
        public static GameObject ModifyComponent<T>(this T component, Action<T> action)
            where T : Component
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            action(component);
            return component.gameObject;
        }

        public static T Get<T>(this T[] array, int index)
        {
            if (index >= 0 && index < array.Length) return array[index];

            return default;
        }
    }
}