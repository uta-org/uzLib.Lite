using Microsoft.Build.Evaluation;

//using Microsoft.Build.Execution;
//using Microsoft.Build.Locator;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Emit;
//using Microsoft.CodeAnalysis.FindSymbols;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Globalization;
using System.Linq;

//using Console = Colorful.Console;

namespace uzLib.Lite.Extensions
{
    public static class RoslynHelper
    {
        public static void AddItem(string projPath, string folderPath, string saveFilePath)
        {
            // Solved issue thanks to: https://stackoverflow.com/a/44260284/3286975

            Project project = new Project(projPath);

            string relPath = IOHelper.MakeRelativePath(folderPath, saveFilePath);
            if (!project.GetItems("Compile").Any(item => item.UnevaluatedInclude == relPath))
                project.AddItem("Compile", relPath);

            project.Save();
        }

        //private static int dupedItems = 0,
        //                   noDupedItems = 0;

        //private static HashSet<string> assemblyNames = new HashSet<string>();

        ///*private const string ProjectLiteral = "ResolveProjectReferences",
        //                     AssemblyLiteral = "ResolveAssemblyReferences";

        //public static VisualStudioInstance GetMSBuildInstance(string msBuildPath = "", bool batchMode = false)
        //{
        //    // найдём версию MSBuild
        //    var visualStudioInstances =
        //        MSBuildLocator.QueryVisualStudioInstances().ToArray();

        //    var instance =
        //        // указан путь, выбираем его
        //        !string.IsNullOrEmpty(msBuildPath) ? visualStudioInstances.SingleOrDefault(
        //                                           i => i.MSBuildPath == msBuildPath) :
        //        // только одна студия, выбираем её
        //        visualStudioInstances.Length == 1 ? visualStudioInstances[0] :
        //        // в пакетном режиме? падаем
        //        batchMode ? null :
        //        // спрашиваем у юзера
        //        SelectVisualStudioInstance(visualStudioInstances);

        //    if (instance == null)
        //    {
        //        Console.WriteLine("Cannot determine MSBuild path");
        //        return null;
        //    }

        //    return instance;
        //}

        //private static VisualStudioInstance SelectVisualStudioInstance(
        //VisualStudioInstance[] visualStudioInstances)
        //{
        //    Console.WriteLine("Multiple installs of MSBuild detected, please select one:");
        //    for (int i = 0; i < visualStudioInstances.Length; i++)
        //    {
        //        Console.WriteLine($"Instance {i + 1}");
        //        Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
        //        Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
        //        Console.WriteLine(
        //            $"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
        //    }

        //    while (true)
        //    {
        //        var userResponse = Console.ReadLine();
        //        if (int.TryParse(userResponse, out int instanceNumber) &&
        //            instanceNumber > 0 &&
        //            instanceNumber <= visualStudioInstances.Length)
        //        {
        //            return visualStudioInstances[instanceNumber - 1];
        //        }
        //        Console.WriteLine("Input not accepted, try again.");
        //    }
        //}*/

        //public static void StartDebugging()
        //{
        //    assemblyNames.Clear();
        //}

        //public static CSharpCompilation AddAllMissingReferences(this CSharpCompilation compilation, Solution solution, Project project)
        //{
        //    if (assemblyNames == null)
        //        assemblyNames = new HashSet<string>();

        //    foreach (var document in project.Documents)
        //    {
        //        var model = document.GetSemanticModelAsync().Result;
        //        var root = document.GetSyntaxRootAsync().Result;

        //        root.DescendantNodesAndSelf().OfType<UsingDirectiveSyntax>().ForEach(@using =>
        //        {
        //            // Thanks to: https://stackoverflow.com/a/13688863/3286975
        //            NameSyntax ns = @using.Name;

        //            if (assemblyNames.Add(ns.GetText().ToString()))
        //                ++noDupedItems;
        //            else
        //                ++dupedItems;
        //        });

        //        // WIP: Remove relative using directives
        //        // +++: Add system references with the typeof(object) trick
        //        // +++: Detect NuGet packages (they can be obtained by reading the csproj file (with the other class))
        //        // +++: Detect ProjectReferences (and compile it if needed (recursivity here))

        //        //var symbols = compilation.GetAllSymbols(root);

        //        //foreach (var symbol in symbols)
        //        //    foreach (var item in SymbolFinder.FindReferencesAsync(symbol, solution).Result)
        //        //        foreach (var location in item.Locations)
        //        //        {
        //        //            //Console.WriteLine($"Found assembly {location.Document.Project.AssemblyName}!", Color.Green);

        //        //            if (assemblyNames.Add(location.Document.Project.AssemblyName))
        //        //                ++noDupedItems;
        //        //            else
        //        //                ++dupedItems;
        //        //        }
        //    }

        //    //SymbolFinder.FindReferencesAsync(null, null).Result.ForEach(r => compilation.AddReferences());

        //    // Thanks to: https://stackoverflow.com/a/32770484/3286975
        //    //return compilation.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

        //    //return compilation.AddReferences(GetReferences(project.FilePath).Select(path => MetadataReference.CreateFromFile(path)));

        //    return null;
        //}

        //public static void EndDebugging()
        //{
        //    Console.WriteLine();

        //    Console.WriteLine($"There was found a total of {noDupedItems} references (not duped).");
        //    Console.WriteLine($"Duped item count: {dupedItems} (Total: {dupedItems + noDupedItems})");

        //    Console.WriteLine();

        //    assemblyNames.ForEach(name => Console.WriteLine($"Found assembly {name}!", Color.Green));

        //    Console.WriteLine();
        //}

        //public static IEnumerable<ISymbol> GetAllSymbols(this CSharpCompilation compilation, SyntaxNode root)
        //{
        //    var noDuplicates = new HashSet<ISymbol>();

        //    var model = compilation.GetSemanticModel(root.SyntaxTree);

        //    foreach (var node in root.DescendantNodesAndSelf())
        //    {
        //        switch (node.Kind())
        //        {
        //            case SyntaxKind.ExpressionStatement:
        //            case SyntaxKind.InvocationExpression:
        //                break;

        //            default:
        //                ISymbol symbol = model.GetSymbolInfo(node).Symbol;

        //                if (symbol != null)
        //                {
        //                    if (noDuplicates.Add(symbol))
        //                        yield return symbol;
        //                }
        //                break;
        //        }
        //    }
        //}

        //// Thanks to: https://csharp.hotexamples.com/examples/Microsoft.CodeAnalysis.Emit/EmitResult/-/php-emitresult-class-examples.html
        //public static string VerifyCompilationResults(this EmitResult compilationResult)
        //{
        //    if (compilationResult.Success)
        //        return string.Empty;

        //    string errorsString = string.Join(
        //           Environment.NewLine,
        //           compilationResult.Diagnostics
        //               .Where(diagnostic => diagnostic.IsWarningAsError ||
        //                                    diagnostic.Severity == DiagnosticSeverity.Error)
        //               .Select(diagnostic => $"{diagnostic.Id}: {diagnostic.GetMessage(CultureInfo.CurrentUICulture)}"));

        //    return errorsString;
        //}

        //// Thanks to: https://stackoverflow.com/a/43105401/3286975
        ///*private static IEnumerable<string> GetReferences(string projectFileName)
        //{
        //    var projectInstance = new ProjectInstance(projectFileName);
        //    var result = BuildManager.DefaultBuildManager.Build(
        //        new BuildParameters(),
        //        new BuildRequestData(projectInstance, new[]
        //        {
        //            ProjectLiteral,
        //            AssemblyLiteral
        //        }));

        //    IEnumerable<string> GetResultItems(string targetName)
        //    {
        //        if (!result.ResultsByTarget.ContainsKey(targetName))
        //            return null;

        //        var buildResult = result.ResultsByTarget[targetName];
        //        var buildResultItems = buildResult.Items;

        //        return buildResultItems.Select(item => item.ItemSpec);
        //    }

        //    var projectResults = GetResultItems(ProjectLiteral);

        //    //if (projectResults.IsNullOrEmpty())
        //    //    throw new Exception($"There was a problem calling '{ProjectLiteral}'.");

        //    var assemblyResults = GetResultItems(AssemblyLiteral);

        //    return assemblyResults.IsNullOrEmpty() ? projectResults : projectResults.Concat(assemblyResults);
        //}*/
    }
}