using System;

namespace uzLib.Lite.Core.Input.Internal
{
    /// <summary>
    /// The Console2 class
    /// </summary>
    /// <seealso cref="uzLib.Lite.Core.Input.Internal.IConsole" />
    internal class Console2 : IConsole
    {
        /// <summary>
        /// Gets the cursor left.
        /// </summary>
        /// <value>
        /// The cursor left.
        /// </value>
        public int CursorLeft => Console.CursorLeft;

        /// <summary>
        /// Gets the cursor top.
        /// </summary>
        /// <value>
        /// The cursor top.
        /// </value>
        public int CursorTop => Console.CursorTop;

        /// <summary>
        /// Gets the width of the buffer.
        /// </summary>
        /// <value>
        /// The width of the buffer.
        /// </value>
        public int BufferWidth => Console.BufferWidth;

        /// <summary>
        /// Gets the height of the buffer.
        /// </summary>
        /// <value>
        /// The height of the buffer.
        /// </value>
        public int BufferHeight => Console.BufferHeight;

        /// <summary>
        /// Gets or sets a value indicating whether [password mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [password mode]; otherwise, <c>false</c>.
        /// </value>
        public bool PasswordMode { get; set; }

        /// <summary>
        /// Sets the size of the buffer.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void SetBufferSize(int width, int height) => Console.SetBufferSize(width, height);

        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        public void SetCursorPosition(int left, int top)
        {
            if (!PasswordMode)
                Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(string value)
        {
            if (PasswordMode)
                value = new string(default(char), value.Length);

            Console.Write(value);
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteLine(string value) => Console.WriteLine(value);
    }
}