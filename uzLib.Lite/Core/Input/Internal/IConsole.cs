namespace uzLib.Lite.Core.Input.Internal
{
    /// <summary>
    /// IConsole interface
    /// </summary>
    internal interface IConsole
    {
        /// <summary>
        /// Gets the cursor left.
        /// </summary>
        /// <value>
        /// The cursor left.
        /// </value>
        int CursorLeft { get; }

        /// <summary>
        /// Gets the cursor top.
        /// </summary>
        /// <value>
        /// The cursor top.
        /// </value>
        int CursorTop { get; }

        /// <summary>
        /// Gets the width of the buffer.
        /// </summary>
        /// <value>
        /// The width of the buffer.
        /// </value>
        int BufferWidth { get; }

        /// <summary>
        /// Gets the height of the buffer.
        /// </summary>
        /// <value>
        /// The height of the buffer.
        /// </value>
        int BufferHeight { get; }

        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Sets the size of the buffer.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        void SetBufferSize(int width, int height);

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        void Write(string value);

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="value">The value.</param>
        void WriteLine(string value);
    }
}