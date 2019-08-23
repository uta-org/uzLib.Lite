using System;
using System.Collections.Generic;
using System.IO;

#if !UNITY_2018 && !UNITY_2017 && !UNITY_5

using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Utilities;
using Console = Colorful.Console;

#else

using UnityEngine;

#endif

namespace uzLib.Lite.Extensions
{
    extern alias SysDrawing;

    /// <summary>
    /// The CompilerHelper class
    /// </summary>
    public static class CompilerHelper
    {
        /// <summary>
        /// The emit solution
        /// </summary>
        private const string EmitSolution = "MSBuildEmitSolution";

#if !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        /// Compiles the specified solution path.
        /// </summary>
        /// <param name="solutionPath">The solution path.</param>
        /// <param name="outputDir">The output dir.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="emit">if set to <c>true</c> [emit].</param>
        /// <returns></returns>
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

#endif

        /// <summary>
        /// Sets the environment variable.
        /// </summary>
        /// <param name="envVar">The env variable.</param>
        /// <param name="val">The value.</param>
        private static void SetEnv(string envVar, string val)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(envVar, EnvironmentVariableTarget.Machine)))
            {
#if !UNITY_2018 && !UNITY_2017 && !UNITY_5
                Console.WriteLine($"You must restart this process to take of the new env var '{envVar}'!", SysDrawing::System.Drawing.Color.Yellow);
#else
                Debug.LogWarning($"You must restart this proccess to take of the new env var '{envVar}'!");
#endif
                Environment.SetEnvironmentVariable(envVar, val, EnvironmentVariableTarget.Machine);
            }
        }

        /// <summary>
        /// Gets the envionment variable.
        /// </summary>
        /// <param name="envVar">The env variable.</param>
        /// <returns></returns>
        public static string GetEnv(string envVar)
        {
            return Environment.GetEnvironmentVariable(envVar);
        }
    }
}