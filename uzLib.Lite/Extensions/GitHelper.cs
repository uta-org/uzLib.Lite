using System.Threading.Tasks;

using LibGit2Sharp;

namespace uzLib.Lite.Extensions
{
    using Shells;

    public static class GitHelper
    {
        public static string GetRemoteUrl(string repoPath)
        {
            using (var repo = new Repository(repoPath))
                return repo.Config.GetValueOrDefault<string>("remote.origin.url");
        }

        public static async Task CloneRepo(string workingPath, string gitUrl, string folderName)
        {
            StaticShell.MyShell.CurrentInfo.WorkingDirectory = workingPath;
            await StaticShell.MyShell.SendCommand($"clone {gitUrl} {folderName}");
        }
    }
}