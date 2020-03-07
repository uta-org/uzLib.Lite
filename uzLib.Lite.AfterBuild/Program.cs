﻿using System;
using System.IO;
using uzLib.Lite.ExternalCode.Extensions;

namespace uzLib.Lite.AfterBuild
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // First param must be $(MSBuildProjectFullPath)
            // Second param must be $(OutputPath)

            // TODO: Compiling from Solutuon top-root has an unexpected behaviour, but Editor is compiled

            try
            {
                string MSBuildProjectFullPath = Path.GetDirectoryName(args[0]);
                string OutputPath = args[1].Replace(@"""", "");

                Console.WriteLine($"{nameof(MSBuildProjectFullPath)}: {MSBuildProjectFullPath}");
                Console.WriteLine($"{nameof(OutputPath)}: {OutputPath}");

                string FullPath = Path.GetFullPath(Path.Combine(MSBuildProjectFullPath ?? throw new InvalidOperationException(), OutputPath));

                Console.WriteLine($"{nameof(FullPath)}: {FullPath}");

                var files = Directory.GetFiles(FullPath, "*.*", SearchOption.TopDirectoryOnly);
                var folders = Directory.GetDirectories(FullPath);

                int count = 0;

                bool isEditor = FullPath.Contains("Editor");

                Console.WriteLine($"Instance is editor?: {isEditor}");

                foreach (var file in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    if (!isEditor && !file.Contains("uzLib.Lite."))
                    {
                        //  || !file.Contains("ExternalCode") // bugfix: ExternalCode is needed on the same folder
                        count = RemoveFile(file, count);
                    }
                    else if (isEditor && (!file.Contains("UnityEditor") || fileName == "UnityEditor"))
                    {
                        count = RemoveFile(file, count);
                    }
                }

                foreach (var folder in folders)
                {
                    if (!folder.Contains(@"\.Code") || !folder.Contains("Editor"))
                    {
                        Directory.Delete(folder, true);

                        Console.WriteLine($"Deleted folder '{folder}'!");
                        ++count;
                    }
                }

                // Debug information

                Console.WriteLine($"Removed all files! ({count} of {files.Length})");

                // Copy ExternalCode folder

                if (!isEditor)
                {
                    string sourceExternalCodeFolder = Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory) ?? throw new InvalidOperationException(), "uzLib.Lite.AfterBuild", "ExternalCode");

                    Console.WriteLine($"Copying folder '{sourceExternalCodeFolder}' to '{FullPath}'...");
                    IOHelper.DirectoryCopy(sourceExternalCodeFolder, FullPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static int RemoveFile(string file, int count)
        {
            File.Delete(file);

            Console.WriteLine($"Deleted file '{file}'!");
            ++count;
            return count;
        }
    }
}