using System.Collections.Generic;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The DictionaryHelper class
    /// </summary>
    public static class DictionaryHelper
    {
        /// <summary>
        /// Adds or set the specified value.
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

        /// <summary>
        /// Adds or append the specified value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool AddOrAppend<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue[] values)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, values.ToList());
                return true;
            }
            else
            {
                dictionary[key] = dictionary[key].AddRangeAndGet(values);
                return false;
            }
        }

        /// <summary>
        /// Adds or append the specified value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool AddOrAppend<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, (new[] { value }).ToList());
                return true;
            }
            else
            {
                dictionary[key] = dictionary[key].AddAndGet(value);
                return false;
            }
        }

        /// <summary>
        /// Adds or append the specified value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool AddOrAppend<TKey, TValue>(this Dictionary<TKey, TValue[]> dictionary, TKey key, params TValue[] values)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, values);
                return true;
            }
            else
            {
                dictionary[key] = dictionary[key].Push(values);
                return false;
            }
        }

        /// <summary>
        /// Adds or append the specified value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool AddOrAppend<TKey, TValue>(this Dictionary<TKey, TValue[]> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, new[] { value });
                return true;
            }
            else
            {
                dictionary[key] = dictionary[key].Push(value);
                return false;
            }
        }

        /// <summary>
        /// Adds and get the specified value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TValue AddAndGet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);

            return dictionary[key];
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
            where TValue : class, new()
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, new TValue());

            return dictionary[key];
        }

        /// <summary>
        /// Determines whether [is null or empty] [the specified key].
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key)
        {
            return !(dictionary.ContainsKey(key) && dictionary[key].Count > 0);
        }
    }
}