using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uzLib.Lite.ExternalCode.Extensions
{
    public static class StringHelper
    {
        /// <summary>
        ///     Prints the length of the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static string PrintListLength<T>(this List<T> list)
        {
            return list.GetLength() + (list == null ? " [Null]" : string.Empty);
        }
    }
}