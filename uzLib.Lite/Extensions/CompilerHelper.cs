﻿using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.Drawing;

//using Console = Colorful.Console;

namespace uzLib.Lite.Extensions
{
    using Core;
    using System.IO;

    public static class CompilerHelper
    {
        public static bool Compile(string solutionPath, string outputDir, out string outString, string configuration = "Debug", string platform = "Any CPU")
        {
            outString = "";

            bool isException = false;
            ProjectCollection pc = new ProjectCollection();

            // THERE ARE A LOT OF PROPERTIES HERE, THESE MAP TO THE MSBUILD CLI PROPERTIES

            Dictionary<string, string> globalProperties = new Dictionary<string, string>
            {
                { "Configuration", configuration }, // always "Debug"
                { "Platform", platform }, // always "Any CPU"
                // { "RebuildT4Templates" , "true" },
                { "VSToolsPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft Visual Studio", "2017", "BuildTools") },
                { "LangVersion", "6" },
                { "ToolsVersion", ToolLocationHelper.CurrentToolsVersion },
                { "VisualStudioVersion", ToolLocationHelper.CurrentToolsVersion },
                { "OutputPath", outputDir }
            };

            //Dictionary<string, string> globalProperty = new Dictionary<string, string>();
            //globalProperty.Add("OutputPath", outputDir);

            var logger = new MsBuildMemoryLogger();

            BuildParameters bp = new BuildParameters(pc);
            bp.Loggers = new[] { logger };

            BuildRequestData buildRequest = new BuildRequestData(solutionPath, globalProperties, ToolLocationHelper.CurrentToolsVersion, new string[] { "Build" }, null);

            // THIS IS WHERE THE MAGIC HAPPENS - IN PROCESS MSBUILD
            try
            {
                BuildResult buildResult = BuildManager.DefaultBuildManager.Build(bp, buildRequest);
                // A SIMPLE WAY TO CHECK THE RESULT
                if (buildResult.OverallResult == BuildResultCode.Success)
                {
                }
            }
            catch (Exception ex)
            {
                isException = true;
                outString = ex.ToString();
            }
            finally
            {
                pc.UnregisterAllLoggers();
                logger.Shutdown();
            }

            if (isException)
                return false;

            outString = logger.GetLog();
            return logger.HasErrors;
        }
    }
}