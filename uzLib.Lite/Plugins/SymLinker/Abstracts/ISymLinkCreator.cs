namespace uzLib.Lite.Plugins.SymLinker
{
    /// <summary>
    /// ISymLinkCreator interface
    /// </summary>
    internal interface ISymLinkCreator
    {
        /// <summary>
        /// Creates a symbolic link.
        /// </summary>
        /// <param name="linkPath">The link path.</param>
        /// <param name="targetPath">The target path.</param>
        /// <param name="file">if set to <c>true</c> [file].</param>
        /// <returns></returns>
        bool CreateSymLink(string linkPath, string targetPath, bool file);
    }
}