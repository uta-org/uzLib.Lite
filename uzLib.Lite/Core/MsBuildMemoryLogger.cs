using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace uzLib.Lite.Core
{
    public class MsBuildMemoryLogger : ConsoleLogger
    {
        public bool HasErrors { get; private set; }

        private StringBuilder errorLog = new StringBuilder();

        private string BuildErrors { get; set; }

        /// <summary>
        /// This will gather info about the projects built
        /// </summary>
        private IList<string> BuildDetailsList { get; set; }

        private IList<string> BuildMessagesList { get; set; }

        private string BuildDetails => string.Join(Environment.NewLine, BuildDetails);

        private string BuildMessages => string.Join(Environment.NewLine, BuildMessagesList);

        /// <summary>
        /// Initialize is guaranteed to be called by MSBuild at the start of the build
        /// before any events are raised.
        /// </summary>
        public override void Initialize(IEventSource eventSource)
        {
            BuildDetailsList = new List<string>();
            BuildMessagesList = new List<string>();

            // FOR BREVITY, WE'LL ONLY REGISTER FOR CERTAIN EVENT TYPES.
            eventSource.ProjectStarted += EventSource_ProjectStarted;
            eventSource.MessageRaised += EventSource_MessageRaised;
            eventSource.ErrorRaised += EventSource_ErrorRaised;
        }

        private void EventSource_MessageRaised(object sender, BuildMessageEventArgs e)
        {
            BuildMessagesList.Add(e.Message);
        }

        private void EventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            if (!HasErrors)
                HasErrors = true;

            // BUILDERROREVENTARGS ADDS LINENUMBER, COLUMNNUMBER, FILE, AMONGST OTHER PARAMETERS
            string line = string.Format(": ERROR {0}({1},{2}): ", e.File, e.LineNumber, e.ColumnNumber);
            errorLog.Append(line + e.Message);
        }

        private void EventSource_ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            BuildDetailsList.Add(e.Message);
        }

        /// <summary>
        /// Shutdown() is guaranteed to be called by MSBuild at the end of the build, after all
        /// events have been raised.
        /// </summary>
        public override void Shutdown()
        {
            // DONE LOGGING, LET GO OF THE FILE
            BuildErrors = errorLog.ToString();
        }

        public string GetLog()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(BuildDetails);

            if (HasErrors)
                sb.AppendLine(BuildErrors);
            else
                sb.AppendLine(BuildMessages);

            return sb.ToString();
        }
    }
}