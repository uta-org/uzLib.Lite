using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Console = Colorful.Console;

namespace uzLib.Lite.Extensions
{
    using Core;

    public static class CompilerHelper
    {
        private const string EmitSolution = "MSBuildEmitSolution";

        public static bool Compile(string solutionPath, string outputDir, string configuration = "Debug", string platform = "Any CPU", bool emit = false)
        {
            ProjectCollection pc = new ProjectCollection();

            if (emit)
            {
                SetEnv(EmitSolution, "1");
                SetEnv("MSBUILD_EXE_PATH", @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin");
                SetEnv("VSINSTALLDIR", @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional");
                SetEnv("VisualStudioVersion", @"15.0");

                if (Directory.GetFiles(Path.GetDirectoryName(solutionPath), "*.cache", SearchOption.TopDirectoryOnly).Length == 0)
                {
                    var foo = SolutionWrapperProject.Generate(
                        solutionPath,
                        ToolLocationHelper.CurrentToolsVersion,
                        null);

                    //Console.WriteLine(foo);
                }
            }
            else if (!emit && Environment.GetEnvironmentVariable(EmitSolution, EnvironmentVariableTarget.Machine) == "1")
                Environment.SetEnvironmentVariable(EmitSolution, "0", EnvironmentVariableTarget.Machine);

            // THERE ARE A LOT OF PROPERTIES HERE, THESE MAP TO THE MSBUILD CLI PROPERTIES

            Dictionary<string, string> globalProperties = new Dictionary<string, string>
            {
                { "Configuration", configuration }, // always "Debug"
                { "Platform", platform }, // always "Any CPU"
                { "ToolsVersion", ToolLocationHelper.CurrentToolsVersion },
                { "OutputPath", outputDir },
            };

            var logger = new ConsoleLogger();

            BuildParameters bp = new BuildParameters(pc)
            {
                Loggers = new[] { logger }
            };

            BuildRequestData buildRequest = new BuildRequestData(
                projectFullPath: solutionPath,
                globalProperties: globalProperties,
                toolsVersion: ToolLocationHelper.CurrentToolsVersion,
                targetsToBuild: new string[] { "Build" },
                hostServices: null);

            // THIS IS WHERE THE MAGIC HAPPENS - IN PROCESS MSBUILD
            try
            {
                BuildResult buildResult = BuildManager.DefaultBuildManager.Build(bp, buildRequest);
                // A SIMPLE WAY TO CHECK THE RESULT
                return buildResult.OverallResult == BuildResultCode.Success;
            }
            catch
            {
                throw;
            }
            finally
            {
                pc.UnregisterAllLoggers();
                logger.Shutdown();
            }
        }

        private static void SetEnv(string envVar, string val)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(envVar, EnvironmentVariableTarget.Machine)))
            {
                Console.WriteLine($"You must restart this proccess to take of the new env var '{envVar}'!", Color.Yellow);
                Environment.SetEnvironmentVariable(envVar, val, EnvironmentVariableTarget.Machine);
            }
        }

        public static string GetEnv(string envVar)
        {
            return Environment.GetEnvironmentVariable(envVar);
        }
    }
}