using System;

namespace uzLib.Lite.Plugins.SymLinker.LinkCreators
{
    /// <summary>
    /// The OSXSymLinkCreator class
    /// </summary>
    /// <seealso cref="uzLib.Lite.Plugins.SymLinker.ISymLinkCreator" />
    internal class OSXSymLinkCreator : ISymLinkCreator
    {
        /// <summary>
        /// Creates a symbolic link.
        /// </summary>
        /// <param name="linkPath">The link path.</param>
        /// <param name="targetPath">The target path.</param>
        /// <param name="file">if set to <c>true</c> [file].</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">OSXSymLinkCreator</exception>
        public bool CreateSymLink(string linkPath, string targetPath, bool file)
        {
            throw new NotImplementedException("OSXSymLinkCreator");
        }
    }
}