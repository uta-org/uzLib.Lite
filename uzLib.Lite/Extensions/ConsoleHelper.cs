using System;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The ConsoleHelper class
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Gets the valid path.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <returns></returns>
        public static string GetValidPath(string caption)
        {
            string val = "";
            bool isInvalid = true;

            do
            {
                Console.Write(caption);

                val = Console.ReadLine();
                isInvalid = !val.IsValidPath();
            }
            while (isInvalid);

            return val;
        }
    }
}