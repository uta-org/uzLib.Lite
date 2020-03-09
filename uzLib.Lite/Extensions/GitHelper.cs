#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

using LibGit2Sharp;
using System.IO;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The GitHelper class
    /// </summary>
    public static class GitHelper
    {
        /// <summary>
        /// Gets the remote URL.
        /// </summary>
        /// <param name="repoPath">The repo path.</param>
        /// <returns></returns>
        public static string GetRemoteUrl(string repoPath)
        {
            using (var repo = new Repository(repoPath))
                return repo.Config.GetValueOrDefault<string>("remote.origin.url");
        }

        /// <summary>
        /// Clones the repo.
        /// </summary>
        /// <param name="workingPath">The working path.</param>
        /// <param name="gitUrl">The git URL.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="credentials">The credentials.</param>
        public static void CloneRepo(string workingPath, string gitUrl, string folderName, UsernamePasswordCredentials credentials)
        {
            var co = new CloneOptions();
            co.CredentialsProvider = (_url, _user, _cred) => credentials;

            Repository.Clone(gitUrl, Path.Combine(workingPath, folderName));
        }

        /// <summary>
        /// Clones the repo.
        /// </summary>
        /// <param name="workingPath">The working path.</param>
        /// <param name="gitUrl">The git URL.</param>
        /// <param name="folderName">Name of the folder.</param>
        public static void CloneRepo(string workingPath, string gitUrl, string folderName)
        {
            Repository.Clone(gitUrl, Path.Combine(workingPath, folderName));

            //StaticShell.MyShell.CurrentInfo.WorkingDirectory = workingPath;
            //await StaticShell.MyShell.SendCommand($"clone {gitUrl} {folderName}");
        }
    }
}

#endif