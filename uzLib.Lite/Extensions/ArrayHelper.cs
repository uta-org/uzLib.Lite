using System.Linq;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The ArrayHelper class
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// Pushes the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T[] Push<T>(this T[] source, T value)
        {
            return source.Concat(new[] { value }).ToArray();
        }

        /// <summary>
        /// Pushes the specified values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static T[] Push<T>(this T[] source, T[] values)
        {
            return source.Concat(values).ToArray();
        }
    }
}