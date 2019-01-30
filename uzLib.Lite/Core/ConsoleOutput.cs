using System;

namespace uzLib.Lite.Core
{
    public class ConsoleOutput
    {
        private ConsoleOutputType OutputType { get; set; }
        public object MyObject { get; }

        public static event Action<string> ReadInput = delegate { };

        public static event Action<ConsoleKeyInfo> ReadKey = delegate { };

        private ConsoleOutput()
        {
        }

        public ConsoleOutput(object obj)
        {
            MyObject = obj;

            OutputType = obj is ConsoleKeyInfo ? ConsoleOutputType.Key : ConsoleOutputType.Value;
        }

        public bool IsExitKey()
        {
            var info = ((ConsoleKeyInfo)MyObject);
            return (info.Modifiers & ConsoleModifiers.Control) != 0 && info.Key == ConsoleKey.B;
        }

        public string GetValue()
        {
            return (string)MyObject;
        }

        // returns null if user pressed Escape, or the contents of the line if they pressed Enter.
        public static ConsoleOutput ReadLineOrKey()
        {
            string retString = "";

            int curIndex = 0;
            do
            {
                ConsoleKeyInfo readKeyResult = Console.ReadKey(true);

                // handle Enter
                if (readKeyResult.Key == ConsoleKey.Enter)
                {
                    ReadInput?.Invoke(retString);

                    Console.WriteLine();
                    return new ConsoleOutput(retString);
                }

                // handle backspace
                if (readKeyResult.Key == ConsoleKey.Backspace)
                {
                    if (curIndex > 0)
                    {
                        retString = retString.Remove(retString.Length - 1);
                        Console.Write(readKeyResult.KeyChar);
                        Console.Write(' ');
                        Console.Write(readKeyResult.KeyChar);
                        curIndex--;
                    }
                }
                else
                // handle all other keypresses
                {
                    retString += readKeyResult.KeyChar;
                    Console.Write(readKeyResult.KeyChar);
                    curIndex++;
                }

                if (char.IsControl(readKeyResult.KeyChar) && readKeyResult.Key != ConsoleKey.Enter && readKeyResult.Key != ConsoleKey.Backspace && readKeyResult.Key != ConsoleKey.Tab)
                {
                    ReadKey?.Invoke(readKeyResult);

                    Console.WriteLine();
                    return new ConsoleOutput(readKeyResult);
                }
            }
            while (true);
        }
    }
}