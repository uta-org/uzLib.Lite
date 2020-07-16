using System.IO;
using System.Threading.Tasks;
using UnityEngine.Extensions;

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
        /// Copies directories.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="overwiteFiles">if set to <c>true</c> [overwite files].</param>
        public static void CopyTo(this DirectoryInfo source, DirectoryInfo target, bool overwiteFiles = true)
        {
            if (!source.Exists) return;
            if (!target.Exists) target.Create();

            Parallel.ForEach(source.GetDirectories(), (sourceChildDirectory) =>
                CopyTo(sourceChildDirectory, new DirectoryInfo(Path.Combine(target.FullName, sourceChildDirectory.Name))));

            foreach (var sourceFile in source.GetFiles())
                sourceFile.CopyTo(Path.Combine(target.FullName, sourceFile.Name), overwiteFiles);
        }

        /// <summary>
        /// Copies directories.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="overwiteFiles">if set to <c>true</c> [overwite files].</param>
        public static void CopyTo(this DirectoryInfo source, string target, bool overwiteFiles = true)
        {
            CopyTo(source, new DirectoryInfo(target), overwiteFiles);
        }

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        ///     Determines whether this instance is extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>
        ///     <c>true</c> if the specified extension is extension; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsExtension(this string extension)
        {
            return extension.StartsWith(".") && MimeTypeMap.HasExtension(extension);
        }

#endif
    }
}