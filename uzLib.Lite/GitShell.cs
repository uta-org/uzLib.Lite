using System;
using System.Diagnostics;
using System.Threading.Tasks;
using uzLib.Lite.Extensions;

namespace uzLib.Lite
{
    public class GitShell
    {
        public ProcessStartInfo CurrentInfo { get; private set; }

        public GitShell()
        {
            CurrentInfo = new ProcessStartInfo("git");

            CurrentInfo.UseShellExecute = false;
            CurrentInfo.RedirectStandardInput = true;
            CurrentInfo.RedirectStandardOutput = true;
            CurrentInfo.RedirectStandardError = true;
        }

        public Task<int> SendCommand(string command)
        {
            CurrentInfo.Arguments = command;

            return CurrentInfo.RunProcessAsync();
        }
    }
}