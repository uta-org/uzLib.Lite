using System;

namespace uzLib.Lite.Tests
{
    using Core;

    internal class Program
    {
        private static void Main(string[] args)
        {
            //Console.ReadLine();

            Console.Write("Write test string: ");
            var test = ConsoleOutput.ReadLineOrKey();

            if (test.IsKey())
                Console.WriteLine(test.KeyName);
            else
                Console.WriteLine($"Output string: {test.OutputString}");
            Console.Read();
        }
    }
}