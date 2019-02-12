namespace uzLib.Lite.Plugins.SymLinker
{
    internal interface ISymLinkCreator
    {
        bool CreateSymLink(string linkPath, string targetPath, bool file);
    }
}