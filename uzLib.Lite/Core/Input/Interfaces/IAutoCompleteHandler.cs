namespace uzLib.Lite.Core.Input.Interfaces
{
    /// <summary>
    /// IAutoCompleteHandler interface
    /// </summary>
    public interface IAutoCompleteHandler
    {
        /// <summary>
        /// Gets or sets the separators.
        /// </summary>
        /// <value>
        /// The separators.
        /// </value>
        char[] Separators { get; set; }

        /// <summary>
        /// Gets the suggestions.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        string[] GetSuggestions(string text, int index);
    }
}