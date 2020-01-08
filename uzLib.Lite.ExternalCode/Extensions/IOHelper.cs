using System.IO;

namespace uzLib.Lite.ExternalCode.Extensions
{
    public static class IOHelper
    {
        /// <summary>
        ///     Determines whether this instance is directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///     <c>true</c> if the specified path is directory; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDirectory(this string path)
        {
            try
            {
                var fa = File.GetAttributes(path);
                return (fa & FileAttributes.Directory) != 0;
            }
            catch
            {
                // The provided path doesn't exists
                return false;
            }
        }

        /// <summary>
        ///     Copies the entire folder.
        /// </summary>
        /// <param name="sourceDirName">Name of the source dir.</param>
        /// <param name="destDirName">Name of the dest dir.</param>
        /// <param name="copySubDirs">if set to <c>true</c> [copy sub dirs].</param>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        ///     Source directory does not exist or could not be found: "
        ///     + sourceDirName
        /// </exception>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
                foreach (var subdir in dirs)
                {
                    var temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
        }
    }
}