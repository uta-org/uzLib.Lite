using System;
using System.Diagnostics;
using System.Threading.Tasks;
using uzLib.Lite.Extensions;

namespace uzLib.Lite
{
    public class GitShell
    {
        //private Process CurrentProcess;

        public ProcessStartInfo CurrentInfo { get; private set; }

        public GitShell()
        {
            CurrentInfo = new ProcessStartInfo("git");
        }

        public Task<int> SendCommand(string command)
        {
            CurrentInfo.Arguments = command;

            return CurrentInfo.RunProcessAsync();
        }
    }
}