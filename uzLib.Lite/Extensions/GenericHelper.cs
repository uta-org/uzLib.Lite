using System.Collections.Generic;

namespace UnityEngine.Extensions
{
    public static class GenericHelper
    {
        public static IEnumerable<T> GetArray<T>(params T[] items)
        {
            foreach (var item in items) yield return item;
        }
    }
}