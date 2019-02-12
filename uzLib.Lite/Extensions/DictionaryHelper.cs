using System.Collections.Generic;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    public static class DictionaryHelper
    {
        public static void AddOrSet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
            else
                dictionary[key] = value;
        }

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

        public static TValue AddAndGet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);

            return dictionary[key];
        }

        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
            where TValue : class, new()
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, new TValue());

            return dictionary[key];
        }

        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key)
        {
            return !(dictionary.ContainsKey(key) && dictionary[key].Count > 0);
        }
    }
}