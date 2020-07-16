using System;

namespace uzLib.Lite.Plugins.SymLinker.LinkCreators
{
    /// <summary>
    /// The LinuxSymLinkCreator class
    /// </summary>
    /// <seealso cref="uzLib.Lite.Plugins.SymLinker.ISymLinkCreator" />
    internal class LinuxSymLinkCreator : ISymLinkCreator
    {
        /// <summary>
        /// Creates a symbolic link.
        /// </summary>
        /// <param name="linkPath">The link path.</param>
        /// <param name="targetPath">The target path.</param>
        /// <param name="file">if set to <c>true</c> [file].</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">LinuxSymLinkCreator</exception>
        public bool CreateSymLink(string linkPath, string targetPath, bool file)
        {
            throw new NotImplementedException("LinuxSymLinkCreator");
        }
    }
}