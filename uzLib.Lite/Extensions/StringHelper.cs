using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace uzLib.Lite.Extensions
{
    public static class StringHelper
    {
        public static bool CheckCommaNumbers(this string input)
        {
            int value;
            if (int.TryParse(input, out value))
                return false;

            return Regex.IsMatch(input, "^([0-9]+(,|, ))*[0-9]+$");
        }

        public static string Format(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null:
                    throw new ArgumentNullException(nameof(input));

                case "":
                    throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));

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