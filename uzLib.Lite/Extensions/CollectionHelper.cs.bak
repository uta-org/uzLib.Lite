using System;
using System.Collections.Generic;
using System.Linq;

namespace uzLib.Lite.Extensions
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

        /// <summary>
        /// Determines whether [is null or empty].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified data]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> data)
        {
            return !(data != null && data.Any());
        }

        /// <summary>
        /// Distincts the by.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T, T2>(this IEnumerable<T> enumerable, Func<T, T2> selector)
        {
            List<KeyValuePair<T, T2>> list = new List<KeyValuePair<T, T2>>();

            foreach (var elem in enumerable)
            {
                var value = selector(elem);
                if (!list.Exists(item => item.Value.Equals(value)))
                    list.Add(new KeyValuePair<T, T2>(elem, value));
            }

            return list.Select(item => item.Key);
        }

        /// <summary>
        /// Finds the index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static int FindIndex<T>(this IEnumerable<T> enumerable, System.Predicate<T> predicate)
        {
            int i = 0;
            foreach (var elem in enumerable)
            {
                if (predicate(elem))
                    return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T value)
        {
            return enumerable.FindIndex(elem => elem.Equals(value));
        }
    }
}