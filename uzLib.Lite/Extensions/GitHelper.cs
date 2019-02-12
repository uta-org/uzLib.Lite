using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uzLib.Lite.Extensions
{
    public static class GitHelper
    {
        public static string GetRemoteUrl(string repoPath)
        {
            using (var repo = new Repository(repoPath))
            {
                var config = repo.Config;
            }

            return "";
        }
    }
}