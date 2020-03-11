using System.Collections.Generic;

namespace uzLib.Lite.Extensions
{
    public static class DicionaryHelper
    {
        /// <summary>
        ///     Safes the get.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static TValue SafeGet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];

            return default;
        }

        /// <summary>
        ///     Adds or set the specified value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddOrSet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
            else
                dictionary[key] = value;
        }
    }
}