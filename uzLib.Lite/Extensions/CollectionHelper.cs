using System;
using System.Collections.Generic;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    public static class CollectionHelper
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
                action(item);
        }

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

        public static T AddValue<T>(this List<T> list, T item)
        {
            list.Insert(0, item);
            return list.First();
        }

        public static List<T> AddAndGet<T>(this List<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> data)
        {
            return !(data != null && data.Any());
        }
    }
}