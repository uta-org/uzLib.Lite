using System;

namespace uzLib.Lite.Plugins.SymLinker.LinkCreators
{
    internal class OSXSymLinkCreator : ISymLinkCreator
    {
        public bool CreateSymLink(string linkPath, string targetPath, bool file)
        {
            throw new NotImplementedException("OSXSymLinkCreator");
        }
    }
}