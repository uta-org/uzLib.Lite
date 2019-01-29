using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static bool HasValues<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key)
        {
            return dictionary.ContainsKey(key) && dictionary[key].Count > 0;
        }
    }
}