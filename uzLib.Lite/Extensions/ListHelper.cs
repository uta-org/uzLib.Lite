using System;
using System.Collections.Generic;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    public static class ListHelper
    {
        public static void Insert<T>(this List<T> list, T newItem, Func<T, bool> predicate)
        {
            bool firstTime;
            Insert(list, newItem, predicate, out firstTime);
        }

        public static void Insert<T>(this List<T> list, T newItem, Func<T, bool> predicate, out bool firstTime)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            firstTime = false;

            foreach (T item in list)
                if (predicate(item))
                    return;

            firstTime = true;

            list.Insert(0, newItem);
            list.First();
        }

        public static T InsertOrGet<T>(this List<T> list, T newItem, Func<T, bool> predicate)
        {
            bool firstTime;
            return InsertOrGet(list, newItem, predicate, out firstTime);
        }

        public static T InsertOrGet<T>(this List<T> list, T newItem, Func<T, bool> predicate, out bool firstTime)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            firstTime = false;

            foreach (T item in list)
                if (predicate(item))
                    return item;

            firstTime = true;

            list.Insert(0, newItem);
            return list.First();
        }

        public static T AddValue<T>(this List<T> list, T item)
        {
            list.Insert(0, item);
            return list.First();
        }

        public static List<T> AddAndGet<T>(this List<T> list, T item)
        {
            if (item == null)
                return list;

            list.Add(item);
            return list;
        }

        public static List<T> AddRangeAndGet<T>(this List<T> list, IEnumerable<T> items)
        {
            if (items == null)
                return list;

            list.AddRange(items);
            return list;
        }

        public static void AddNullableRange<T>(this List<T> list, IEnumerable<T> col)
        {
            if (col.IsNullOrEmpty())
                return;

            list.AddRange(col);
        }
    }
}