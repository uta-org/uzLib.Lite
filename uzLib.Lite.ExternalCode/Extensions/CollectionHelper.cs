#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

using System;
using System.Collections.Generic;

namespace uzLib.Lite.ExternalCode.Extensions
{
    public static class CollectionHelper
    {
        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
                action(item);
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T, int> action)
        {
            // argument null checking omitted
            int i = 0;
            foreach (T item in sequence)
            {
                action(item, i);
                i++;
            }
        }
    }
}

#endif