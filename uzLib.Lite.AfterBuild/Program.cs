using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

//using uzLib.Lite.Extensions;
//using uzLib.Lite.ExternalCode.Extensions;
//using IOHelper = uzLib.Lite.Extensions.IOHelper;

namespace uzLib.Lite.AfterBuild
{
    public static class Program
    {
        private static List<string> metaFiles = new List<string>();

        private const string FolderName = "uzLib.Lite.MetaFiles";

        private static string CompileDatePath => Path.Combine(Environment.CurrentDirectory, "compile.txt");

        [Flags]
        public enum RemoveState
        {
            Nothing,
            Editor,
            Normal
        }

        private static void Main(string[] args)
        {
            // First param must be $(MSBuildProjectFullPath)
            // Second param must be $(OutputPath)

            Console.WriteLine($@"Executing from '{Environment.CurrentDirectory}'...");

            var externalCodeFolder = Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory) ?? throw new InvalidOperationException(), "uzLib.Lite.ExternalCode");
            var unityEditorFolder = Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory) ?? throw new InvalidOperationException(), "uzLib.Lite.UnityEditor");

            //Thread.Sleep(2000);

            // TODO: Copy meta files from temp folder

            try
            {
                string MSBuildProjectFullPath = args.Try(0, "E:\\VISUAL STUDIO\\Visual Studio Projects\\uzLib.Lite\\uzLib.Lite\\uzLib.Lite.csproj", Path.GetDirectoryName);
                string OutputPath = args.Try(1, "..\\..\\..\\..\\United Teamwork Association\\Unity\\Assets\\UnitySourceToolkit\\Assets\\UnitedTeamworkAssociation\\UnitySourceToolkit\\Scripts\\Utilities\\uzLib.Lite",
                    alt => alt.Replace(@"""", ""));

                Console.WriteLine($@"{nameof(MSBuildProjectFullPath)}: {MSBuildProjectFullPath}");
                Console.WriteLine($@"{nameof(OutputPath)}: {OutputPath}");

                string FullPath = Path.GetFullPath(Path.Combine(MSBuildProjectFullPath ?? throw new InvalidOperationException(), OutputPath));
                if (FullPath.EndsWith("Editor"))
                    FullPath = Path.GetDirectoryName(FullPath);

                Console.WriteLine($@"{nameof(FullPath)}: {FullPath}");

                var files = Directory.GetFiles(FullPath, "*.*", SearchOption.TopDirectoryOnly);
                var folders = Directory.GetDirectories(FullPath);

                int count = 0;

                var editorFolder = files.First().Contains("Editor")
                    ? Path.GetDirectoryName(files.First())
                    : Path.Combine(Path.GetDirectoryName(files.First()) ?? throw new InvalidOperationException(), "Editor");

                foreach (var file in files)
                {
                    var editorFile = Path.Combine(editorFolder, Path.GetFileName(file) ?? throw new InvalidOperationException());
                    var fileName = Path.GetFileNameWithoutExtension(editorFile);

                    var remove = (!file.Contains("uzLib.Lite.") || file.Contains("ExternalCode") ? RemoveState.Normal : RemoveState.Nothing)
                                 | (!editorFile.Contains("uzLib.Lite.") || fileName != "UnityEditor" ? RemoveState.Editor : RemoveState.Nothing);

                    if (remove.Has(RemoveState.Editor) || remove.Has(RemoveState.Normal) && Path.GetExtension(file) != ".meta")
                    {
                        if (remove.Has(RemoveState.Normal))
                            count = RemoveFile(file, count);

                        //if (remove.Has(RemoveState.Editor) && editorFolderExists)
                        //    count = RemoveFile(editorFile, count);
                    }
                }

                foreach (var folder in folders)
                {
                    if (!folder.Contains(@"\.Code"))
                    {
                        F.DeleteDirectory(folder);

                        Console.WriteLine($@"Deleted folder '{folder}'!");
                        ++count;
                    }
                }

                // Debug information

                Console.WriteLine($@"Removed all files! ({count} of {files.Length})");

                // Copy folders

                CopyFolderContents(externalCodeFolder, FullPath, "ExternalCode");
                CopyFolderContents(unityEditorFolder, FullPath, "Editor");

                CopyMetaFiles(FullPath);

                var compileTime = DateTime.Now.ToString();
                File.WriteAllText(CompileDatePath, compileTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void CopyFolderContents(string externalCodeFolder, string fullPath, string folderName)
        {
            DateTime? compileTime =
                File.Exists(CompileDatePath) ? (DateTime?)DateTime.Parse(File.ReadAllText(CompileDatePath)) : null;

            var copyToFolder = Path.Combine(fullPath, folderName);
            Console.WriteLine($@"Copying folder '{externalCodeFolder}' to '{copyToFolder}'...");

            if (!Directory.Exists(copyToFolder))
                Directory.CreateDirectory(copyToFolder);

            foreach (var directory in Directory.GetDirectories(externalCodeFolder))
            {
                if (directory.Contains("bin") || directory.Contains("obj") || directory.Contains("Properties"))
                    continue;

                var folderPath = Path.Combine(copyToFolder, GetLastDirectoryName(directory));

                Console.WriteLine($@"Copying sub-folder '{externalCodeFolder}' to '{copyToFolder}'...");

                new DirectoryInfo(directory).CopyTo(folderPath, fileInfo =>
                {
                    //bool isModified = !(compileTime.HasValue && fileInfo.LastAccessTime > compileTime.Value || !compileTime.HasValue);
                    //Console.WriteLine(isModified ? $"{fileInfo.FullName} copied." : $"{fileInfo.FullName} skipped.");
                    //return isModified;

                    // TODO: This is already done by BeforeBuild app, but this implementation is smarter and also is done by Unity3D.
                    return true;
                });
            }
        }

        private static string GetLastDirectoryName(string path)
        {
            return Path.GetFileName(
                path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }

        private static int RemoveFile(string file, int count)
        {
            File.Delete(file);

            Console.WriteLine($@"Deleted file '{file}'!");
            ++count;
            return count;
        }

        private static void CopyMetaFiles(string fullPath)
        {
            string tempFolder = FindTempFolder();
            // F.GetTemporaryDirectory(F.GetTemporaryDirectory(FolderName));
            string tempFolderForFiles = Path.Combine(tempFolder, "Files");
            var jsonFile = Path.Combine(tempFolder, "files.json");

            Console.WriteLine($"Reading json: {jsonFile}");

            // If this file doesn't exists, the BeforeBuild didn't copied any file.
            if (!File.Exists(jsonFile))
                return;

            var json = File.ReadAllText(jsonFile);
            metaFiles = JsonConvert.DeserializeObject<List<string>>(json);

            Console.WriteLine($"Deserialized '{jsonFile}' reading {metaFiles.Count} files.");

            int count = 0;
            foreach (var metaFile in metaFiles)
            {
                var origFile = metaFile.Replace(tempFolderForFiles, string.Empty);
                origFile = origFile.Substring(1);
                origFile = Path.Combine(fullPath, origFile);

                //if (File.Exists(origFile))
                //{
                //    Console.WriteLine($@"File '{origFile}' already exists!");
                //    continue;
                //}

                try
                {
                    File.Copy(metaFile, origFile, true);
                    Console.WriteLine($"Copy meta file '{metaFile}' -> '{origFile}'...");
                    ++count;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($@"Couldn't copy file '{metaFile}' to '{origFile}'!\r\n{ex}");
                }
            }

            Console.WriteLine($@"Copied back {count} meta files!");
        }

        private static string FindTempFolder()
        {
            return Directory.GetDirectories(Path.GetTempPath()).Where(f => f.Contains(FolderName))
                .Select(f => new { FolderPath = f, Folder = new DirectoryInfo(f) }).OrderByDescending(o => o.Folder.LastAccessTime)
                .FirstOrDefault()?.FolderPath;
        }
    }
}