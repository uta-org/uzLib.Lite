using System.Diagnostics;
using System.Threading.Tasks;
using uzLib.Lite.Extensions;

namespace uzLib.Lite.Shells
{
    /// <summary>
    /// The GitShell class
    /// </summary>
    public class GitShell
    {
        /// <summary>
        /// Gets the current information.
        /// </summary>
        /// <value>
        /// The current information.
        /// </value>
        public ProcessStartInfo CurrentInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitShell"/> class.
        /// </summary>
        public GitShell()
        {
            CurrentInfo = new ProcessStartInfo("git");

            CurrentInfo.UseShellExecute = false;
            CurrentInfo.RedirectStandardInput = true;
            CurrentInfo.RedirectStandardOutput = true;
            CurrentInfo.RedirectStandardError = true;
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public Task<int> SendCommand(string command)
        {
            CurrentInfo.Arguments = command;

            return CurrentInfo.RunProcessAsync();
        }

        /// <summary>
        /// Reads the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<string> ReadCommand(string command)
        {
            CurrentInfo.Arguments = command;

            using (Process process = new Process())
            {
                await CurrentInfo.RunProcessAsync(process);
                return process.StandardOutput.ReadToEnd();
            }
        }
    }
}