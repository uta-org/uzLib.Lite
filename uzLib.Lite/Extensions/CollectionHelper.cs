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

        /// <summary>Indicates whether the specified array is null or has a length of zero.</summary>
        /// <param name="array">The array to test.</param>
        /// <returns>true if the array parameter is null or has a length of zero; otherwise, false.</returns>
        public static bool IsNullOrEmpty(this Array array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>Indicates whether the specified array is null or has a length of zero.</summary>
        /// <param name="array">The array to test.</param>
        /// <returns>true if the array parameter is null or has a length of zero; otherwise, false.</returns>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>
        ///     Fors the each current and previous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="matchFilling">The match filling.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///     items
        ///     or
        ///     matchFilling
        /// </exception>
        public static IEnumerable<T> ForEachCurAndPrev<T>(this IEnumerable<T> items, Func<T, T, bool> matchFilling)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            if (matchFilling == null) throw new ArgumentNullException(nameof(matchFilling));

            var prevItem = items.First();

            yield return prevItem;

            var i = 0;
            foreach (var item in items)
            {
                if (i > 0 && matchFilling(item, prevItem)) yield return item;

                prevItem = item;

                ++i;
            }
        }

        /// <summary>
        ///     Fors the each current and previous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentException">items</exception>
        /// <exception cref="ArgumentNullException">action</exception>
        public static void ForEachCurAndPrev<T>(this IEnumerable<T> items, Action<T, T> action)
        {
            if (items.IsNullOrEmpty()) throw new ArgumentNullException(nameof(items));

            if (action == null) throw new ArgumentNullException(nameof(action));

            var prevItem = items.First();

            var i = 0;
            foreach (var item in items)
            {
                if (i > 0) action.Invoke(item, prevItem);

                prevItem = item;

                ++i;
            }
        }

        /// <summary>
        ///     Fors the each current and previous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">items</exception>
        /// <exception cref="ArgumentNullException">func</exception>
        public static IEnumerable<TResult> ForEachCurAndPrev<T, TResult>(this IEnumerable<T> items,
            Func<T, T, IEnumerable<TResult>> func)
        {
            if (items.IsNullOrEmpty()) throw new ArgumentException("items");

            if (func == null) throw new ArgumentNullException(nameof(func));

            T prevItem = default;

            var i = 0;
            foreach (var item in items)
            {
                if (i > 0)
                    foreach (var result in func(item, prevItem))
                        yield return result;

                prevItem = item;

                ++i;
            }
        }

        /// <summary>
        ///     Fors the each current and previous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col">The col.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException">action</exception>
        public static void ForEachCurAndPrev<T>(this IList<T> col, Action<T, T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            var i = 0;
            foreach (var item in col)
            {
                var k = i == 0 ? col.Count - 1 : i - 1;

                action(item, col[k]);
                ++i;
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this List<T> list, params T[] items)
        {
            list.AddRange(items.AsEnumerable() ?? throw new InvalidOperationException());
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
    }
}