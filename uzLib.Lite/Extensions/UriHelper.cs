using System;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The UriHelper class
    /// </summary>
    public static class UriHelper
    {
        /// <summary>
        /// Checks the URL valid.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static bool CheckURLValid(this string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult);
        }
    }
}