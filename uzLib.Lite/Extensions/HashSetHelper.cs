using System.Collections.Generic;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    public static class HashSetHelper
    {
        public static void RemoveAt<T>(this HashSet<T> hashset, int index)
        {
            hashset.Remove(hashset.ElementAt(index));
        }

        public static int IndexOf<T>(this HashSet<T> hashset, T item)
        {
            for (int i = 0; i < hashset.Count; ++i)
                if (hashset.ElementAt(i).Equals(item))
                    return i;

            return -1;
        }
    }
}