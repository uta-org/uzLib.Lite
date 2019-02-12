using System;

namespace uzLib.Lite.Plugins.SymLinker.LinkCreators
{
    internal class LinuxSymLinkCreator : ISymLinkCreator
    {
        public bool CreateSymLink(string linkPath, string targetPath, bool file)
        {
            throw new NotImplementedException("LinuxSymLinkCreator");
        }
    }
}