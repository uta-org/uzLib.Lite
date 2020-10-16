using System;
using System.Collections.Generic;

namespace uzLib.Lite.Core.Input
{
    using Internal;
    using Interfaces;

    /// <summary>
    /// The SmartPsswrd class
    /// </summary>
    public static class SmartPsswrd
    {
        /// <summary>
        /// The history
        /// </summary>
        private static List<string> _history;

        /// <summary>
        /// Initializes the <see cref="SmartPsswrd"/> class.
        /// </summary>
        static SmartPsswrd()
        {
            _history = new List<string>();
        }

        /// <summary>
        /// Adds the history.
        /// </summary>
        /// <param name="text">The text.</param>
        public static void AddHistory(params string[] text) => _history.AddRange(text);

        /// <summary>
        /// Gets the history.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHistory() => _history;

        /// <summary>
        /// Clears the history.
        /// </summary>
        public static void ClearHistory() => _history = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether [history enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [history enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool HistoryEnabled { get; set; }

        /// <summary>
        /// Gets or sets the automatic completion handler.
        /// </summary>
        /// <value>
        /// The automatic completion handler.
        /// </value>
        public static IAutoCompleteHandler AutoCompletionHandler { private get; set; }

        /// <summary>
        /// Reads the specified prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="default">The default.</param>
        /// <returns></returns>
        public static string Read(string prompt = "", string @default = "")
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2(), _history, AutoCompletionHandler);
            string text = GetText(keyHandler);

            if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(@default))
            {
                text = @default;
            }
            else
            {
                if (HistoryEnabled)
                    _history.Add(text);
            }

            return text;
        }

        /// <summary>
        /// Reads the password.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public static string ReadPassword(string prompt = "")
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2() { PasswordMode = true }, null, null);
            return GetText(keyHandler);
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="keyHandler">The key handler.</param>
        /// <returns></returns>
        private static string GetText(KeyHandler keyHandler)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.Enter)
            {
                keyHandler.Handle(keyInfo);
                keyInfo = Console.ReadKey(true);
            }

            Console.WriteLine();
            return keyHandler.Text;
        }
    }
}