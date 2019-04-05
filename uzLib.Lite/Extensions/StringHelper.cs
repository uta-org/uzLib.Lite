using System;
using System.Linq;
using System.Text.RegularExpressions;

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
    }
}