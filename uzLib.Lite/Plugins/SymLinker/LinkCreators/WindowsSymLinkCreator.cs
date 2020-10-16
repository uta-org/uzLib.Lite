using System;
using System.IO;
using System.Runtime.InteropServices;

namespace uzLib.Lite.Plugins.SymLinker.LinkCreators
{
    /// <summary>
    /// The WindowsSymLinkCreator class
    /// </summary>
    /// <seealso cref="uzLib.Lite.Plugins.SymLinker.ISymLinkCreator" />
    internal class WindowsSymLinkCreator : ISymLinkCreator
    {
        /// <summary>
        /// Creates the symbolic link.
        /// </summary>
        /// <param name="lpSymlinkFileName">Name of the lp symlink file.</param>
        /// <param name="lpTargetFileName">Name of the lp target file.</param>
        /// <param name="dwFlags">The dw flags.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        /// <summary>
        /// Creates a symbolic link.
        /// </summary>
        /// <param name="linkPath">The link path.</param>
        /// <param name="targetPath">The target path.</param>
        /// <param name="file">if set to <c>true</c> [file].</param>
        /// <returns></returns>
        public bool CreateSymLink(string linkPath, string targetPath, bool file)
        {
            var linkName = Path.GetFileName(targetPath);
            linkPath = Path.Combine(linkPath, linkName);
            bool success = false;
            try
            {
                var symbolicLinkType = file ? SymbolicLink.File : SymbolicLink.Directory;
                success = CreateSymbolicLink(linkPath, targetPath, symbolicLinkType);
            }
            catch (Exception ex)
            {
                // TODO: Handle exception
            }
            return success;
        }

        /// <summary>
        /// The SymbolicLink enum
        /// </summary>
        private enum SymbolicLink
        {
            /// <summary>
            /// Is a file
            /// </summary>
            File = 0,

            /// <summary>
            /// Is a directory
            /// </summary>
            Directory = 1
        }
    }
}