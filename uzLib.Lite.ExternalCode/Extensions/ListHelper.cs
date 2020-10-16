using System;
using System.Collections.Generic;
using System.Linq;

#if !(!UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5)
using uzLib.Lite.Extensions;
#endif

namespace uzLib.Lite.ExternalCode.Extensions
{
    /// <summary>
    ///     The ListHelper class
    /// </summary>
    public static class ListHelper
    {
        /// <summary>
        ///     Inserts the specified new item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="newItem">The new item.</param>
        /// <param name="predicate">The predicate.</param>
        public static void Insert<T>(this List<T> list, T newItem, Func<T, bool> predicate)
        {
            bool firstTime;
            Insert(list, newItem, predicate, out firstTime);
        }

        /// <summary>
        ///     Inserts the specified new item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="newItem">The new item.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="firstTime">if set to <c>true</c> [first time].</param>
        /// <exception cref="ArgumentNullException">predicate</exception>
        public static void Insert<T>(this List<T> list, T newItem, Func<T, bool> predicate, out bool firstTime)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            firstTime = false;

            foreach (var item in list)
                if (predicate(item))
                    return;

            firstTime = true;

            list.Insert(0, newItem);
            list.First();
        }

        /// <summary>
        ///     Inserts or get the specified value if predicate is met.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="newItem">The new item.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static T InsertOrGet<T>(this List<T> list, T newItem, Func<T, bool> predicate)
        {
            bool firstTime;
            return InsertOrGet(list, newItem, predicate, out firstTime);
        }

        /// <summary>
        ///     Inserts or get the specified value if predicate is met.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="newItem">The new item.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="firstTime">if set to <c>true</c> [first time].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">predicate</exception>
        public static T InsertOrGet<T>(this List<T> list, T newItem, Func<T, bool> predicate, out bool firstTime)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            firstTime = false;

            foreach (var item in list)
                if (predicate(item))
                    return item;

            firstTime = true;

            list.Insert(0, newItem);
            return list.First();
        }

        /// <summary>
        ///     Adds the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static T AddValue<T>(this List<T> list, T item)
        {
            list.Insert(0, item);
            return list.First();
        }

        /// <summary>
        ///     Adds the and get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static List<T> AddAndGet<T>(this List<T> list, T item)
        {
            if (item == null)
                return list;

            list.Add(item);
            return list;
        }

        /// <summary>
        ///     Adds the range and get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static List<T> AddRangeAndGet<T>(this List<T> list, IEnumerable<T> items)
        {
            if (items == null)
                return list;

            list.AddRange(items);
            return list;
        }

        /// <summary>
        ///     Adds the nullable range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="col">The col.</param>
        public static void AddNullableRange<T>(this List<T> list, IEnumerable<T> col)
        {
            if (col.IsNullOrEmpty())
                return;

            list.AddRange(col);
        }

        /// <summary>
        ///     Determines whether [is null or empty].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns>
        ///     <c>true</c> if [is null or empty] [the specified data]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this List<T> data)
        {
            return data == null || data != null && data.Count == 0;
        }

        /// <summary>
        ///     Adds the safe.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static List<T> AddSafe<T>(this List<T> list, T item)
        {
            if (list == null)
                list = new List<T>();

            list.Add(item);

            return list;
        }

        /// <summary>
        ///     Gets the length.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static int GetLength<T>(this List<T> list)
        {
            return list?.Count ?? 0;
        }
    }
}