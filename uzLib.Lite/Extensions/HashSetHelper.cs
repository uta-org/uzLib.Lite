using System.Collections.Generic;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The HashSetHelper class
    /// </summary>
    public static class HashSetHelper
    {
        /// <summary>
        /// Removes at the specified index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashset">The hashset.</param>
        /// <param name="index">The index.</param>
        public static void RemoveAt<T>(this HashSet<T> hashset, int index)
        {
            hashset.Remove(hashset.ElementAt(index));
        }

        /// <summary>
        /// Get the index of the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashset">The hashset.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static int IndexOf<T>(this HashSet<T> hashset, T item)
        {
            for (int i = 0; i < hashset.Count; ++i)
                if (hashset.ElementAt(i).Equals(item))
                    return i;

            return -1;
        }
    }
}