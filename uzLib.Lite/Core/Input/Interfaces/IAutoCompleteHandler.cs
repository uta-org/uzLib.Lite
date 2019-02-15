namespace uzLib.Lite.Core.Input.Interfaces
{
    public interface IAutoCompleteHandler
    {
        char[] Separators { get; set; }

        string[] GetSuggestions(string text, int index);
    }
}