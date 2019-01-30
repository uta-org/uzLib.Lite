using System;

namespace uzLib.Lite.Extensions
{
    public static class ConsoleHelper
    {
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