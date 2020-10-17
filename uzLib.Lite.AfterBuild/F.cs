using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace uzLib.Lite.AfterBuild
{
    // Class for extensions methods that can't be linked to uzLib.Lite due to circular references.
    public static class F
    {
        public static T Try<T>(this T[] args, int index, T alternative, Func<T, T> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            try
            {
                return alternative?.Equals(default) == false ? transform(args[index]) : args[index];
            }
            catch
            {
                return alternative?.Equals(default) == false
                    ? transform(alternative)
                    : default;
            }
        }

        public static void DeleteDirectory(string path)
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                Thread.Sleep(1);
                DeleteDir(directory);
            }
            DeleteDir(path);
        }

        private static void DeleteDir(string dir)
        {
            try
            {
                Thread.Sleep(1);
                Directory.Delete(dir, true);
            }
            catch (IOException ex)
            {
                //DeleteDir(dir);
                Console.WriteLine(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                //DeleteDir(dir);
                Console.WriteLine(ex);
            }
        }

        public static void CopyTo(this DirectoryInfo source, DirectoryInfo target, Func<FileInfo, string, bool> predicate = null, bool overwiteFiles = true)
        {
            if (!source.Exists) return;
            if (!target.Exists) target.Create();

            Parallel.ForEach(source.GetDirectories(), (sourceChildDirectory) =>
                CopyTo(sourceChildDirectory, new DirectoryInfo(Path.Combine(target.FullName, sourceChildDirectory.Name))));

            foreach (var sourceFile in source.GetFiles())
            {
                var targetFile = Path.Combine(target.FullName, sourceFile.Name);
                if (predicate?.Invoke(sourceFile, targetFile) == true) continue;
                sourceFile.CopyTo(targetFile, overwiteFiles);
            }
        }

        public static void CopyTo(this DirectoryInfo source, string target, Func<FileInfo, string, bool> predicate = null, bool overwiteFiles = true)
        {
            CopyTo(source, new DirectoryInfo(target), predicate, overwiteFiles);
        }

        public static bool Has<T>(this T type, dynamic value)
            where T : struct, IConvertible
        {
            //var flags = ToFlags<T>();
            return (type & value) == value;
        }

        public static string GetTemporaryDirectory(string prefix = "", string suffix = "", bool useRandomness = true)
        {
            string tempPath = Path.GetTempPath(),
                interfix = useRandomness ? Path.GetRandomFileName() : "-" + Directory.GetFiles(tempPath, "*", SearchOption.TopDirectoryOnly).Count(f => f.Contains(prefix));
            // If random is false, then avoid collision by cheking number of files with that prefix ^^^

            string tempDirectory = Path.Combine(tempPath, prefix + interfix + suffix);
            Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }
    }
}