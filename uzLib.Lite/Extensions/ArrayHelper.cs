using System.Linq;

namespace uzLib.Lite.Extensions
{
    public static class ArrayHelper
    {
        public static T[] Push<T>(this T[] source, T value)
        {
            return source.Concat(new[] { value }).ToArray();
        }

        public static T[] Push<T>(this T[] source, T[] values)
        {
            return source.Concat(values).ToArray();
        }
    }
}