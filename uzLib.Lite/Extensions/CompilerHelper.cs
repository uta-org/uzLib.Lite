using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;

namespace uzLib.Lite.Extensions
{
    using Core;
    using Microsoft.Build.BuildEngine;
    using System.IO;

    public static class CompilerHelper
    {
        private const string EmitSolution = "MSBuildEmitSolution";

        public static bool Compile(string solutionPath, string outputDir, out string outString, string configuration = "Debug", string platform = "Any CPU", bool emit = true)
        {
            outString = "";

            bool isException = false;
            ProjectCollection pc = new ProjectCollection();

            //if (emit)
            //    Environment.SetEnvironmentVariable(EmitSolution, "1");
            //else if (!emit && Environment.GetEnvironmentVariable(EmitSolution) == "1")
            //    Environment.SetEnvironmentVariable(EmitSolution, "0");

            //// act
            //var instances = SolutionProjectGenerator.Generate(solution, null, null, new BuildEventContext(0, 0, 0, 0), null);

            //// assert
            //var projectBravoMetaProject = instances[1];

            //// saves the in-memory metaproj to disk
            //projectBravoMetaProject.ToProjectRootElement().Save(projectBravoMetaProject.FullPath);

            var foo = SolutionWrapperProject.Generate(
                solutionPath,
                ToolLocationHelper.CurrentToolsVersion,
                null);

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