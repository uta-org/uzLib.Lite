using LibGit2Sharp;

namespace uzLib.Lite.Extensions
{
    public static class GitHelper
    {
        public static string GetRemoteUrl(string repoPath)
        {
            using (var repo = new Repository(repoPath))
                return repo.Config.GetValueOrDefault<string>("remote.origin.url");
        }
    }
}