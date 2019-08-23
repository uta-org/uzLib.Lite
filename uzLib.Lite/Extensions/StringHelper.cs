using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using uzLib.Lite.ExternalCode.Extensions;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The StringHelper class
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Checks the comma numbers.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static bool CheckCommaNumbers(this string input)
        {
            int value;
            if (int.TryParse(input, out value))
                return false;

            return Regex.IsMatch(input, "^([0-9]+(,|, ))*[0-9]+$");
        }

        /// <summary>
        /// Formats the specified arguments.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static string Format(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Set the first character from string to uppercase.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="exSensitive">if set to <c>true</c> [ex sensitive].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">input</exception>
        /// <exception cref="ArgumentException">input</exception>
        public static string FirstCharToUpper(this string input, bool exSensitive = false)
        {
            switch (input)
            {
                case null:
                    if (exSensitive)
                        throw new ArgumentNullException(nameof(input));
                    else
                        return null;

                case "":
                    if (exSensitive)
                        throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                    else
                        return string.Empty;

                default:
                    if (char.IsLower(input.First()))
                    {
                        return input.First().ToString().ToUpper() + input.Substring(1);
                    }
                    else
                    {
                        for (int i = 0; i < input.Length; ++i)
                            if (char.IsLower(input[i]))
                                return input.Substring(0, i - 1) + input[i].ToString().ToUpper() + input.Substring(i + 1);
                    }
                    break;
            }

            return input;
        }

        /// <summary>
        /// Determines whether [is null or white space].
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is null or white space] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        ///     Determines whether [is base64 string].
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///     <c>true</c> if [is base64 string] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBase64String(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            if (str.Length % 4 != 0) return false;

            //decode - encode and compare
            try
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(str));
                var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(decoded));
                if (str.Equals(encoded, StringComparison.InvariantCultureIgnoreCase)) return true;
            }
            catch
            {
            }

            return false;

            //Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            //return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }

        /// <summary>
        ///     Strips the starting with.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="stripAfter">The strip after.</param>
        /// <returns></returns>
        public static string StripStartingWith(this string s, string stripAfter)
        {
            if (s == null) return null;

            var indexOf = s.IndexOf(stripAfter, StringComparison.Ordinal);

            if (indexOf > -1) return s.Substring(0, indexOf);

            return s;
        }

        /// <summary>
        ///     Replaces the new lines.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string ReplaceNewLines(this string str, string replacement)
        {
            return Regex.Replace(str, @"\t|\n|\r", replacement);
        }

        /// <summary>
        ///     Removes the new lines.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string RemoveNewLines(this string str)
        {
            return ReplaceNewLines(str, string.Empty);
        }

        /// <summary>
        ///     Replace several ocurrences in a string at once.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="replacements">The replacements.</param>
        /// <returns></returns>
        public static string MulticaseReplace(this string str, params Tuple<string, string>[] replacements)
        {
            foreach (var replacement in replacements) str = str.Replace(replacement.Item1, replacement.Item2);

            return str;
        }

        /// <summary>
        ///     Removes several ocurrences in a string at once.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="replacements">The replacements.</param>
        /// <returns></returns>
        public static string MulticaseRemove(this string str, params string[] replacements)
        {
            foreach (var replacement in replacements) str = str.Replace(replacement, string.Empty);

            return str;
        }

        /// <summary>
        ///     Puts the string into the Clipboard.
        /// </summary>
        /// <param name="str"></param>
        public static void CopyToClipboard(this string str)
        {
            var textEditor = new TextEditor();
            textEditor.text = str;
            textEditor.SelectAll();
            textEditor.Copy();
        }

        /// <summary>
        ///     Cuts the specified cut length.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="cutLen">Length of the cut.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">String length must be greater than 5.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Cut(this string str, int cutLen)
        {
            if (str.Length < 5) throw new ArgumentException("String length must be greater than 5.");

            if (cutLen >= str.Length - 2) throw new ArgumentOutOfRangeException();

            cutLen += 3;
            var splitLength = Mathf.CeilToInt(cutLen / 2f);
            var startingIndex = Mathf.RoundToInt(str.Length / 2);

            return $"{str.Substring(0, splitLength - startingIndex)}...{str.Substring(splitLength + startingIndex)}";
        }

        /// <summary>
        /// Gets the links.
        /// </summary>
        /// <param name="rawString">The raw string.</param>
        /// <returns></returns>
        public static IEnumerable<Match> GetLinks(this string rawString)
        {
            var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return linkParser.Matches(rawString).Cast<Match>();
        }

        /// <summary>
        /// Determines whether this instance is hexadecimal.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if the specified input is hexadecimal; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public static bool IsHex(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            IEnumerable<char> hexString = input.ToCharArray();
            return hexString.Select(currentCharacter =>
                currentCharacter >= '0' && currentCharacter <= '9' ||
                currentCharacter >= 'a' && currentCharacter <= 'f' ||
                currentCharacter >= 'A' && currentCharacter <= 'F').All(isHexCharacter => isHexCharacter);
        }

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = t[j - 1] == s[i - 1] ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public static string FindNearestString(this string input, IEnumerable<string> values)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException(nameof(input));

            if (values.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(values));

            return values
                .Select(value => new { Distance = Compute(input, value), Value = value })
                .OrderBy(x => x.Distance)
                .FirstOrDefault()
                .Value;
        }
    }
}