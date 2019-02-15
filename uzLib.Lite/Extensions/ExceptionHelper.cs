using System.Collections.Generic;

namespace uzLib.Lite.Extensions
{
    public static class ExceptionHelper
    {
        public static bool IsOutOfBounds<T>(this T[] arr, int index)
        {
            if (arr == null) return true;
            return !(index >= 0 && index < arr.Length);
        }

        public static bool IsOutOfBounds<T>(this IList<T> list, int index)
        {
            if (list == null) return true;
            return !(index >= 0 && index < list.Count);
        }

        public static bool IsOutOfBounds<TKey, TValue>(this IDictionary<TKey, TValue> dict, int index)
        {
            if (dict == null) return true;
            return !(index >= 0 && index < dict.Count);
        }
    }
}