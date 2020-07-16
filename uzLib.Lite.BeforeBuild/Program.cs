using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using uzLib.Lite.Extensions;

namespace uzLib.Lite.BeforeBuild
{
    internal class Program
    {
        private static List<string> metaFiles = new List<string>();

        private const string FolderName = "uzLib.Lite.MetaFiles";

        private static void Main(string[] args)
        {
            // First param must be $(MSBuildProjectFullPath)
            // Second param must be $(OutputPath)

            Console.WriteLine($@"Executing from '{Environment.CurrentDirectory}'...");

            try
            {
                string MSBuildProjectFullPath = Path.GetDirectoryName(args.Try(0, "E:\\VISUAL STUDIO\\Visual Studio Projects\\uzLib.Lite\\uzLib.Lite"));
                string OutputPath = args.Try(1, "..\\..\\..\\..\\United Teamwork Association\\Unity\\Assets\\UnitySourceToolkit\\Assets\\UnitedTeamworkAssociation\\UnitySourceToolkit\\Scripts\\Utilities\\uzLib.Lite")
                    .Replace(@"""", "");

                Console.WriteLine($@"{nameof(MSBuildProjectFullPath)}: {MSBuildProjectFullPath}");
                Console.WriteLine($@"{nameof(OutputPath)}: {OutputPath}");

                string FullPath = Path.GetFullPath(Path.Combine(MSBuildProjectFullPath ?? throw new InvalidOperationException(), OutputPath));
                if (FullPath.EndsWith("Editor"))
                    FullPath = Path.GetDirectoryName(FullPath);

                Console.WriteLine($@"{nameof(FullPath)}: {FullPath}");

                string tempFolder = IOHelper.GetTemporaryDirectory(FolderName);
                var tempFolderForFiles = Path.Combine(tempFolder, "Files");

                //if (Directory.Exists(tempFolderForFiles))
                //    Directory.Delete(tempFolderForFiles, true);

                var treeResult = ScanFolder(new DirectoryInfo(FullPath));
                File.WriteAllText(Path.Combine(tempFolder, "tree.txt"), treeResult);

                int count = 0;
                count = DirSearch(FullPath, file =>
                {
                    // Copy meta file to another dir, store on list
                    var extension = Path.GetExtension(file);
                    //Console.WriteLine(extension);

                    if (extension != ".meta")
                        return;

                    var fullFileName = file.Replace(FullPath ?? throw new InvalidOperationException(), string.Empty).Substring(1);
                    var tempFile = Path.Combine(tempFolderForFiles, fullFileName);

                    metaFiles.Add(tempFile);
                    var folder = Path.GetDirectoryName(tempFile);

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder ?? throw new InvalidOperationException());

                    // Update the temp file
                    if (File.Exists(tempFile))
                        File.Delete(tempFile);

                    File.Copy(file, tempFile, true);
                    // Console.WriteLine($@"Copying meta file from '{file}' to '{tempFile}'...");

                    //Console.WriteLine($"File: {file} --> {FullPath}" +
                    //                  "\r\n" +
                    //                  $"FullName: {fullFileName}" +
                    //                  "\r\n" +
                    //                  $"TempFile: {tempFile}" +
                    //                  "\r\n" +
                    //                  $"TempFolder: {tempFolder}");
                }, count);

                Console.WriteLine($@"Copied {count} meta files!");

                if (!metaFiles.IsNullOrEmpty())
                {
                    var jsonFile = Path.Combine(tempFolder, "files.json");
                    File.WriteAllText(jsonFile, JsonConvert.SerializeObject(metaFiles, Formatting.Indented));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static int DirSearch(string sDir, Action<string> callback, int count)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    callback(f);
                    ++count;
                }

                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        callback(f);
                        ++count;
                    }

                    count = DirSearch(d, callback, count);
                }
            }
            catch (Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return count;
        }

        private static string ScanFolder(DirectoryInfo directory, string indentation = "\t", int maxLevel = -1, int deep = 0)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Concat(Enumerable.Repeat(indentation, deep)) + directory.Name);

            if (maxLevel == -1 || maxLevel < deep)
            {
                foreach (var subdirectory in directory.GetDirectories())
                    builder.Append(ScanFolder(subdirectory, indentation, maxLevel, deep + 1));
            }

            foreach (var file in directory.GetFiles())
                builder.AppendLine(string.Concat(Enumerable.Repeat(indentation, deep + 1)) + file.Name);

            return builder.ToString();
        }
    }
}