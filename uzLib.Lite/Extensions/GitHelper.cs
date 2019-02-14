using LibGit2Sharp;
using System.IO;

namespace uzLib.Lite.Extensions
{
    public static class GitHelper
    {
        public static string GetRemoteUrl(string repoPath)
        {
            using (var repo = new Repository(repoPath))
                return repo.Config.GetValueOrDefault<string>("remote.origin.url");
        }

        public static void CloneRepo(string workingPath, string gitUrl, string folderName, UsernamePasswordCredentials credentials)
        {
            var co = new CloneOptions();
            co.CredentialsProvider = (_url, _user, _cred) => credentials;

            Repository.Clone(gitUrl, Path.Combine(workingPath, folderName));
        }

        public static void CloneRepo(string workingPath, string gitUrl, string folderName)
        {
            Repository.Clone(gitUrl, Path.Combine(workingPath, folderName));

            //StaticShell.MyShell.CurrentInfo.WorkingDirectory = workingPath;
            //await StaticShell.MyShell.SendCommand($"clone {gitUrl} {folderName}");
        }
    }
}