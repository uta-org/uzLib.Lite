using System;
using System.Collections.Generic;
using System.Linq;

namespace uzLib.Lite.ExternalCode.Extensions
{
    /// <summary>
    ///     The DictionaryHelper class
    /// </summary>
    public static class DictionaryHelper
    {
#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

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

#endif

        /// <summary>
        ///     Adds or append the specified value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool AddOrAppend<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key,
            TValue[] values)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, values.ToList());
                return true;
            }

            dictionary[key] = dictionary[key].AddRangeAndGet(values);
            return false;
        }

        /// <summary>
        ///     Adds or append the specified value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool AddOrAppend<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key,
            TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, new[] { value }.ToList());
                return true;
            }

            dictionary[key] = dictionary[key].AddAndGet(value);
            return false;
        }

        /// <summary>
        ///     Adds or append the specified value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool AddOrAppend<TKey, TValue>(this Dictionary<TKey, TValue[]> dictionary, TKey key,
            params TValue[] values)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, values);
                return true;
            }

            dictionary[key] = dictionary[key].Push(values);
            return false;
        }

        /// <summary>
        ///     Adds or append the specified value.
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

            dictionary[key] = dictionary[key].Push(value);
            return false;
        }

        /// <summary>
        ///     Adds and get the specified value.
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
        ///     Gets the specified key.
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
        ///     Determines whether [is null or empty] [the specified key].
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     <c>true</c> if [is null or empty] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key)
        {
            return !(dictionary.ContainsKey(key) && dictionary[key].Count > 0);
        }

        /// <summary>
        ///     Indexes the of.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static int IndexOf<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            var i = 0;
            foreach (var pair in dictionary)
            {
                if (pair.Key.Equals(key))
                    return i;
                i++;
            }

            return -1;
        }

        /// <summary>
        ///     Indexes the of.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int IndexOf<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
        {
            var i = 0;
            foreach (var pair in dictionary)
            {
                if (pair.Value.Equals(value))
                    return i;
                i++;
            }

            return -1;
        }

        /// <summary>
        ///     Adds the or get.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static int? AddOrGet<TKey>(this Dictionary<TKey, int?> dictionary, TKey key)
        {
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, dictionary.Count);

            return dictionary[key];
        }

        /// <summary>
        ///     Adds the or get.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TValue AddOrGet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            return AddOrGet(dictionary, key, value, out var firstTime);
        }

        /// <summary>
        ///     Adds the or get.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="firstTime">if set to <c>true</c> [first time].</param>
        /// <returns></returns>
        public static TValue AddOrGet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value,
            out bool firstTime)
        {
            if (!dictionary.ContainsKey(key))
            {
                firstTime = true;
                dictionary.Add(key, value);
            }

            firstTime = false;

            //return value;
            return dictionary[key];
        }

        /// <summary>
        ///     Adds value to dictionary once.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool AddOnce<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public static string ToString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, string separator = null)
        {
            return string.Join(string.IsNullOrEmpty(separator) ? Environment.NewLine : separator,
                dictionary.Select(kv => $"Key = {kv.Key}, Value = {kv.Value}"));
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : default;
        }

        /// <summary>
        ///     Gets the index of a key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static int IndexOfKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (!dictionary.ContainsKey(key)) return -1;

            return Array.IndexOf(dictionary.Keys.ToArray(), key);
        }

        /// <summary>
        ///     Gets the index of a value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int IndexOfValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
        {
            if (!dictionary.ContainsValue(value)) return -1;

            return Array.IndexOf(dictionary.Values.ToArray(), value);
        }

        /// <summary>
        ///     Checks if two dictionaries are equal.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary1">The dictionary1.</param>
        /// <param name="dictionary2">The dictionary2.</param>
        /// <param name="compareKeys">if set to <c>true</c> [compare keys].</param>
        /// <returns></returns>
        public static bool AreEqual<TKey, TValue>(Dictionary<TKey, TValue> dictionary1,
            Dictionary<TKey, TValue> dictionary2, bool compareKeys = true)
        {
            if (compareKeys)
                return dictionary1.Keys.SequenceEqual(dictionary2.Keys);
            return dictionary1.Values.SequenceEqual(dictionary2.Values);
        }
    }
}