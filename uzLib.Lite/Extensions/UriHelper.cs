using System;

namespace uzLib.Lite.Extensions
{
    public static class UriHelper
    {
        public static bool CheckURLValid(this string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult);
        }
    }
}