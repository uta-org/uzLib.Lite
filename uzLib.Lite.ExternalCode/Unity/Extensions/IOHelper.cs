using System.IO;

namespace uzLib.Lite.ExternalCode.Extensions
{
    public static class IOHelper
    {
        /// <summary>
        /// Determines whether this instance is directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is directory; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDirectory(this string path)
        {
            try
            {
                FileAttributes fa = File.GetAttributes(path);
                return (fa & FileAttributes.Directory) != 0;
            }
            catch
            { // The provided path doesn't exists
                return false;
            }
        }
    }
}