using System.Collections.Generic;
using System.Linq;

namespace uzLib.Lite.ExternalCode.Extensions
{
    public static class IteratorHelper
    {
        /// <summary>
        /// Determines whether [is null or empty].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified data]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> data)
        {
            return !(data != null && data.Any());
        }
    }
}