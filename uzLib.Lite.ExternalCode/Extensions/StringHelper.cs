using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        /// <summary>
        ///     Prints a detailed byte.
        /// </summary>
        /// <param name="byte">The byte.</param>
        /// <param name="hexFirst">if set to <c>true</c> [hexadecimal first].</param>
        /// <returns></returns>
        public static string PrintDetailedByte(this byte @byte, bool hexFirst = true)
        {
            if (hexFirst)
                return $"{@byte.ConvertToHex()} (Dec: {@byte})";
            return $"{@byte} (Hex: {@byte.ConvertToHex()})";
        }

        /// <summary>
        ///     Splits the into lines.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string[] SplitIntoLines(this string str)
        {
            return Regex.Split(str, "\r\n|\r|\n");
        }

        /// <summary>
        ///     Ases the string.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public static string AsString(char c)
        {
            return new string(c, 1);
        }

        /// <summary>
        ///     Gets the longest line.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <returns></returns>
        public static int GetLongestLine(this string[] lines)
        {
            return lines.Select(line => line.Length).Max();
        }

        /// <summary>
        ///     Gets the longest line.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="tuples">The tuples.</param>
        /// <returns></returns>
        public static int GetLongestLine(this string[] lines, params Tuple<string, int>[] tuples)
        {
            return lines.Select(line => SumOcurrences(line, tuples)).Max();
        }

        /// <summary>
        ///     Sums the ocurrences.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="tuples">The tuples.</param>
        /// <param name="withLength">if set to <c>true</c> [with length].</param>
        /// <returns></returns>
        private static int SumOcurrences(string line, Tuple<string, int>[] tuples, bool withLength = true)
        {
            var dict = tuples.ToDictionary(t => t.Item1, t => t.Item2);

            var length = 0;
            var oc = 0;
            foreach (var c in line)
            {
                var strChar = c.ToString();

                if (dict.ContainsKey(strChar))
                {
                    length += dict[strChar];
                    ++oc;
                }
            }

            if (!withLength)
                return length;
            return length + line.Length - oc;
        }
    }
}