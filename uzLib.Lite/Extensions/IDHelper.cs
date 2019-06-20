using System.Linq;
using System.Reflection;

namespace UnityEngine.Extensions
{
    public static class IDHelper
    {
        public static string GetUniqueId<T>(this T o, bool original = false)
        {
            if (o == null) return "";

            var flags = BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic;

            var fieldStrings = string.Join(",",
                o.GetType().GetFields(flags).Where(f => f.FieldType == typeof(string))
                    .Select(s => s.GetValue(o).ToString()));
            var propStrings = string.Join(",",
                o.GetType().GetProperties(flags).Where(p => p.PropertyType == typeof(string))
                    .Select(s => s.GetValue(o, null).ToString()));

            var stringMix = fieldStrings == propStrings ? fieldStrings : fieldStrings + propStrings;
            var val = original ? stringMix : stringMix.Base64Encode();

            return val;
        }

        public static string GetUniqueId<T>(this T[] arr)
        {
            return EncryptionHelper.GetEncryptedStringArray(arr.Select(e => e.GetUniqueId(true)).ToArray());
        }

        public static string GetUniqueId<T>(this T[] arr, int index)
        {
            return arr[index].GetUniqueId(true).Base64Encode();
        }
    }
}