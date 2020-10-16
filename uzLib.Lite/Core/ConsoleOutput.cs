using System;

namespace uzLib.Lite.Core
{
    /// <summary>
    /// The ConsoleOutput class
    /// </summary>
    public class ConsoleOutput
    {
        /// <summary>
        /// Gets or sets the type of the output.
        /// </summary>
        /// <value>
        /// The type of the output.
        /// </value>
        private ConsoleOutputType OutputType { get; set; }

        /// <summary>
        /// Gets my object.
        /// </summary>
        /// <value>
        /// My object.
        /// </value>
        private object MyObject { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is inserting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is inserting; otherwise, <c>false</c>.
        /// </value>
        private static bool IsInserting { get; set; }

        /// <summary>
        /// Gets or sets the default size of the cursor.
        /// </summary>
        /// <value>
        /// The default size of the cursor.
        /// </value>
        private static int DefaultCursorSize { get; set; }

        /// <summary>
        /// Gets the name of the key.
        /// </summary>
        /// <value>
        /// The name of the key.
        /// </value>
        public string KeyName => IsKey() && (ConsoleKeyInfo)MyObject != null ? ((ConsoleKeyInfo)MyObject).Key.ToString() : "Null";

        /// <summary>
        /// Gets the output string.
        /// </summary>
        /// <value>
        /// The output string.
        /// </value>
        public string OutputString => !IsKey() && MyObject != null ? (string)MyObject : string.Empty;

        /// <summary>
        /// Occurs when [read input].
        /// </summary>
        public static event Action<string> ReadInput = delegate { };

        /// <summary>
        /// Occurs when [read key].
        /// </summary>
        public static event Action<ConsoleKeyInfo> ReadKey = delegate { };

        /// <summary>
        /// Gets the current right pad.
        /// </summary>
        /// <value>
        /// The current right pad.
        /// </value>
        public int CurrentRightPad { get; private set; }

        /// <summary>
        /// Initializes the <see cref="ConsoleOutput"/> class.
        /// </summary>
        static ConsoleOutput()
        {
            DefaultCursorSize = Console.CursorSize;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ConsoleOutput"/> class from being created.
        /// </summary>
        private ConsoleOutput()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleOutput"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public ConsoleOutput(object obj)
        {
            MyObject = obj;

            OutputType = obj is ConsoleKeyInfo ? ConsoleOutputType.Key : ConsoleOutputType.Value;
        }

        /// <summary>
        /// Determines whether this instance is key.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is key; otherwise, <c>false</c>.
        /// </returns>
        public bool IsKey()
        {
            return OutputType == ConsoleOutputType.Key;
        }

        /// <summary>
        /// Determines whether [is exit key].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is exit key]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsExitKey()
        {
            if (!IsKey())
                return false;

            var info = (ConsoleKeyInfo)MyObject;
            return (info.Modifiers & ConsoleModifiers.Control) != 0 && info.Key == ConsoleKey.B;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            return (string)MyObject;
        }

        /// <summary>
        /// Reads the line or key.
        /// </summary>
        /// <param name="inline">if set to <c>true</c> [inline].</param>
        /// <returns></returns>
        // Note: Returns null if user pressed Escape, or the contents of the line if they pressed Enter.
        public static ConsoleOutput ReadLineOrKey(bool inline = false)
        {
            string retString = "";

            int curRightPad,
                curIndex = 0;

            do
            {
                ConsoleKeyInfo readKeyResult = Console.ReadKey(true);

                // handle Enter
                if (readKeyResult.Key == ConsoleKey.Enter)
                {
                    ReadInput?.Invoke(retString);

                    curRightPad = Console.CursorLeft;

                    if (!inline)
                        Console.WriteLine();

                    return new ConsoleOutput(retString) { CurrentRightPad = curRightPad };
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

                        --curIndex;
                    }
                }
                else if (readKeyResult.Key == ConsoleKey.Delete)
                {
                    if (retString.Length - curIndex > 0)
                    {
                        // Store current position
                        int curLeftPos = Console.CursorLeft;

                        // Redraw string
                        for (int i = curIndex + 1; i < retString.Length; ++i)
                            Console.Write(retString[i]);

                        // Remove last repeated char
                        Console.Write(' ');

                        // Restore position
                        Console.SetCursorPosition(curLeftPos, Console.CursorTop);

                        // Remove string
                        retString = retString.Remove(curIndex, 1);
                    }
                }
                else if (readKeyResult.Key == ConsoleKey.RightArrow)
                {
                    if (curIndex < retString.Length)
                    {
                        ++Console.CursorLeft;
                        ++curIndex;
                    }
                }
                else if (readKeyResult.Key == ConsoleKey.LeftArrow)
                {
                    if (curIndex > 0)
                    {
                        --Console.CursorLeft;
                        --curIndex;
                    }
                }
                else if (readKeyResult.Key == ConsoleKey.Insert)
                {
                    IsInserting = !IsInserting;
                    Console.CursorSize = IsInserting ? 100 : DefaultCursorSize;
                }
#if DEBUG
                else if (readKeyResult.Key == ConsoleKey.UpArrow)
                {
                    if (Console.CursorTop > 0)
                        --Console.CursorTop;
                }
                else if (readKeyResult.Key == ConsoleKey.DownArrow)
                {
                    if (Console.CursorTop < Console.BufferHeight - 1)
                        ++Console.CursorTop;
                }
#endif
                else
                // handle all other keypresses
                {
                    if (IsInserting || curIndex == retString.Length)
                    {
                        retString += readKeyResult.KeyChar;
                        Console.Write(readKeyResult.KeyChar);
                        ++curIndex;
                    }
                    else
                    {
                        // Store char
                        char c = readKeyResult.KeyChar;

                        // Write char at position
                        Console.Write(c);

                        // Store cursor position
                        int curLeftPos = Console.CursorLeft;

                        // Clear console from curIndex to end
                        for (int i = curIndex; i < retString.Length; ++i)
                            Console.Write(' ');

                        // Go back
                        Console.SetCursorPosition(curLeftPos, Console.CursorTop);

                        // Write the chars from curIndex to end (with the new appended char)
                        for (int i = curIndex; i < retString.Length; ++i)
                            Console.Write(retString[i]);

                        // Restore again
                        Console.SetCursorPosition(curLeftPos, Console.CursorTop);

                        // Store in the string
                        retString = retString.Insert(curIndex, new string(c, 1));

                        // Sum one to the cur index (we appended one char)
                        ++curIndex;
                    }

                    ReadInput?.Invoke(retString);
                }

                if (char.IsControl(readKeyResult.KeyChar) &&
                    readKeyResult.Key != ConsoleKey.Enter &&
                    readKeyResult.Key != ConsoleKey.Backspace &&
                    readKeyResult.Key != ConsoleKey.Tab &&
                    readKeyResult.Key != ConsoleKey.Delete &&
                    readKeyResult.Key != ConsoleKey.RightArrow &&
                    readKeyResult.Key != ConsoleKey.LeftArrow &&
                    readKeyResult.Key != ConsoleKey.Insert)
                {
#if DEBUG
                    if (readKeyResult.Key == ConsoleKey.UpArrow || readKeyResult.Key == ConsoleKey.DownArrow)
                        continue;
#endif

                    ReadKey?.Invoke(readKeyResult);

                    curRightPad = Console.CursorLeft;

                    if (!inline)
                        Console.WriteLine();

                    return new ConsoleOutput(readKeyResult) { CurrentRightPad = curRightPad };
                }
            }
            while (true);
        }
    }
}