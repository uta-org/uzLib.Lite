#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

using System;
using System.Globalization;
using System.Text;
using System.Drawing;

using Console = Colorful.Console;

namespace uzLib.Lite.Core.Input
{
    /// <summary>
    /// The SmartInput class
    /// </summary>
    public static class SmartInput
    {
        /// <summary>
        /// Read a confirmation input [yN] from console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public static bool NextConfirm(string title)
        {
            return NextConfirm(title, System.Drawing.Color.LightGray);
        }

        /// <summary>
        /// Read a confirmation input [yN] from console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static bool NextConfirm(string title, System.Drawing.Color color)
        {
            ConsoleKey response;
            do
            {
                Console.Write($"{title} [yN] ", color);
                response = Console.ReadKey(false).Key;
                if (response != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return response == ConsoleKey.Y;
        }

        /// <summary>
        /// Reads a string token from the console
        /// skipping any leading and trailing whitespace.
        /// </summary>
        public static string NextToken()
        {
            StringBuilder tokenChars = new StringBuilder();
            bool tokenFinished = false;
            bool skipWhiteSpaceMode = true;
            while (!tokenFinished)
            {
                int nextChar = Console.Read();
                if (nextChar == -1)
                {
                    // End of stream reached
                    tokenFinished = true;
                }
                else
                {
                    char ch = (char)nextChar;
                    if (char.IsWhiteSpace(ch))
                    {
                        // Whitespace reached (' ', '\r', '\n', '\t') -->
                        // skip it if it is a leading whitespace
                        // or stop reading anymore if it is trailing
                        if (!skipWhiteSpaceMode)
                        {
                            tokenFinished = true;
                            if (ch == '\r' && Environment.NewLine == "\r\n")
                            {
                                // Reached '\r' in Windows --> skip the next '\n'
                                Console.Read();
                            }
                        }
                    }
                    else
                    {
                        // Character reached --> append it to the output
                        skipWhiteSpaceMode = false;
                        tokenChars.Append(ch);
                    }
                }
            }

            string token = tokenChars.ToString();
            return token;
        }

        /// <summary>
        /// Reads an integer number from the console
        /// skipping any leading and trailing whitespace.
        /// </summary>
        public static int NextInt()
        {
            string token = NextToken();
            return int.Parse(token);
        }

        /// <summary>
        /// Reads a floating-point number from the console
        /// skipping any leading and trailing whitespace.
        /// </summary>
        /// <param name="acceptAnyDecimalSeparator">
        /// Specifies whether to accept any decimal separator
        /// ("." and ",") or the system's default separator only.
        /// </param>
        public static double NextDouble(bool acceptAnyDecimalSeparator = true)
        {
            string token = NextToken();
            if (acceptAnyDecimalSeparator)
            {
                token = token.Replace(',', '.');
                double result = double.Parse(token, CultureInfo.InvariantCulture);
                return result;
            }
            else
            {
                double result = double.Parse(token);
                return result;
            }
        }

        /// <summary>
        /// Reads a decimal number from the console
        /// skipping any leading and trailing whitespace.
        /// </summary>
        /// <param name="acceptAnyDecimalSeparator">
        /// Specifies whether to accept any decimal separator
        /// ("." and ",") or the system's default separator only.
        /// </param>
        public static decimal NextDecimal(bool acceptAnyDecimalSeparator = true)
        {
            string token = NextToken();
            if (acceptAnyDecimalSeparator)
            {
                token = token.Replace(',', '.');
                decimal result = decimal.Parse(token, CultureInfo.InvariantCulture);
                return result;
            }
            else
            {
                decimal result = decimal.Parse(token);
                return result;
            }
        }
    }
}

#endif