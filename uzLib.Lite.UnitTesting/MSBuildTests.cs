using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace uzLib.Lite.UnitTesting
{
    [TestClass]
    public class MSBuildTests
    {
        private const string projectContents =
            @"<Project>
              <PropertyGroup>
                <ImportIt>true</ImportIt>
              </PropertyGroup>

              <Import Project=""{0}"" Condition=""'$(ImportIt)' == 'true'""/>

              <!--- <Target Name=""Bazz"">
                <Message Text=""Buzz"" Importance=""High"" />
              </Target> -->

            </Project>";

        private const string projectImport =
            @"<Project>
                <Target Name=""WhereAmI"">
                    <Message Text="" Here I Am  "" />
                    <Exec Command=""dir ."" />
                    <Message Text="" "" />
                </Target>

                <Target Name=""ShowReservedProperties"" AfterTargets=""BeforeBuild"">
                    <Message Text="" MSBuildProjectDirectory  = $(MSBuildProjectDirectory)"" Importance=""high"" />
                    <Message Text="" MSBuildProjectFile  = $(MSBuildProjectFile)"" Importance=""high"" />
                    <Message Text="" MSBuildProjectExtension  = $(MSBuildProjectExtension)"" Importance=""high"" />
                    <Message Text="" MSBuildProjectFullPath  = $(MSBuildProjectFullPath)"" Importance=""high"" />
                    <Message Text="" MSBuildProjectName  = $(MSBuildProjectName)"" Importance=""high"" />
                    <Message Text="" MSBuildBinPath  = $(MSBuildBinPath)"" Importance=""high"" />
                    <Message Text="" MSBuildProjectDefaultTargets  = $(MSBuildProjectDefaultTargets)"" Importance=""high"" />
                    <Message Text="" MSBuildExtensionsPath  = $(MSBuildExtensionsPath)"" Importance=""high"" />
                    <Message Text="" MSBuildStartupDirectory  = $(MSBuildStartupDirectory)"" Importance=""high""/>
                </Target>

                  <Target Name=""ShowOtherProperties"">
                    <Message Text=""  "" />
                    <Message Text=""  "" />
                    <Message Text="" Environment (SET) Variables*       "" />
                    <Message Text="" ---------------------------        "" />
                    <Message Text="" COMPUTERNAME = *$(COMPUTERNAME)*   "" />
                    <Message Text="" USERDNSDOMAIN = *$(USERDNSDOMAIN)* "" />
                    <Message Text="" USERDOMAIN = *$(USERDOMAIN)*       "" />
                    <Message Text="" USERNAME = *$(USERNAME)*           "" />
                </Target>
            </Project>";

        [TestMethod]
        public static void TestTargets()
        {
            Project project;

            var results = CreateTest(out project);
            Assert.IsTrue(results.OverallResult == BuildResultCode.Success);

            project.SetProperty("ImportIt", "false");
            project.Save();

            results = CreateTest();
            Assert.IsTrue(results.OverallResult == BuildResultCode.Success);
        }

        public static BuildResult CreateTest()
        {
            Project project;
            return CreateTest(out project, false, null);
        }

        private static BuildResult CreateTest(out Project project, bool clearCache = true, string[] targets = null)
        {
            targets = targets ?? new[] { "WhereAmI", "ShowReservedProperties", "ShowOtherProperties" };

            string importPath = Path.GetTempFileName();
            File.WriteAllText(importPath, projectImport);

            var collection = new ProjectCollection();
            var root = ProjectRootElement.Create(XmlReader.Create(new StringReader(string.Format(projectContents, importPath))), collection);

            root.FullPath = Path.GetTempFileName();
            root.Save();

            project = new Project(root, new Dictionary<string, string>(), ToolLocationHelper.CurrentToolsVersion, collection);
            var instance = project.CreateProjectInstance(ProjectInstanceSettings.Immutable).DeepCopy(isImmutable: false);
            var manager = new BuildManager();
            var request = new BuildRequestData(instance, targetsToBuild: targets);
            var parameters = new BuildParameters()
            {
                DisableInProcNode = true,
                Loggers = new[] { new ConsoleLogger() }
            };

            return CreateTest(targets, manager, request, parameters, clearCache);
        }

        private static BuildResult CreateTest(string[] targets, BuildManager manager, BuildRequestData request, BuildParameters parameters, bool clearCache = true)
        {
            manager.BeginBuild(parameters);
            var submission = manager.PendBuildRequest(request);
            var result = submission.Execute();
            manager.EndBuild();

            if (clearCache)
                manager.ResetCaches();

            return result;
        }
    }
}