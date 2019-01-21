using System;
using System.Collections.Generic;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    public static class ListHelper
    {
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
    }
}