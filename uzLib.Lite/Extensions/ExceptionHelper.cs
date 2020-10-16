using System.Collections.Generic;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The ExceptionHelper class
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Determines whether [is out of bounds] [the specified index].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">The arr.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if [is out of bounds] [the specified index]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOutOfBounds<T>(this T[] arr, int index)
        {
            if (arr == null) return true;
            return !(index >= 0 && index < arr.Length);
        }

        /// <summary>
        /// Determines whether [is out of bounds] [the specified index].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if [is out of bounds] [the specified index]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOutOfBounds<T>(this IList<T> list, int index)
        {
            if (list == null) return true;
            return !(index >= 0 && index < list.Count);
        }

        /// <summary>
        /// Determines whether [is out of bounds] [the specified index].
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if [is out of bounds] [the specified index]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOutOfBounds<TKey, TValue>(this IDictionary<TKey, TValue> dict, int index)
        {
            if (dict == null) return true;
            return !(index >= 0 && index < dict.Count);
        }
    }
}