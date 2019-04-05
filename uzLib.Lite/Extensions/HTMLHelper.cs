using System;
using System.Text.RegularExpressions;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The HTMLHelper class
    /// </summary>
    public static class HTMLHelper
    {
        /// <summary>
        /// Cleans the element.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static string CleanElement(this string html, string element)
        {
            string orEl = string.Format(@"\[{0}\]", element);

            return Regex.Replace(html, orEl, (m) => { return Callback(m, element, html); });
        }

        /// <summary>
        /// Callbacks the specified match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="element">The element.</param>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        private static string Callback(Match match, string element, string html)
        {
            int oc = FindOccurrences(html.Substring(0, match.Index + 1), match.Index);
            string befChar = html.Substring(match.Index - (element.Length + 3), 1);
            string sep = new string(Convert.ToChar(9), oc);
            return string.Format("{3}<{0}>{1}{2}", element, Environment.NewLine, sep, befChar.Replace(Environment.NewLine, " ").IsNullOrWhiteSpace() ? sep : "");
        }

        /// <summary>
        /// Finds the occurrences.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="maxIndex">The maximum index.</param>
        /// <returns></returns>
        private static int FindOccurrences(string str, int maxIndex)
        {
            int lio = str.LastIndexOf("</");
            //Debug.LogFormat("LastIndexOf: {0}\nMatchIndex: {1}\nLength: {2}", lio, maxIndex, str.Length);
            return Regex.Matches(str.Substring(lio, maxIndex - lio), @"<\w+").Count;
        }
    }
}