using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace uzLib.Lite.Extensions
{
    public static class IOHelper
    {
        public static string GetFileNameValidChar(this string fileName)
        {
            foreach (var item in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(item.ToString(), "");

            return fileName;
        }

        public static string GetFileNameFromUrlWithoutExtension(this string url)
        {
            return Path.GetFileNameWithoutExtension(GetFileNameFromUrl(url));
        }

        public static string GetFileNameFromUrl(this string url)
        {
            string fileName = "";

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                fileName = GetFileNameValidChar(Path.GetFileName(uri.AbsolutePath));

            string ext = "";

            if (!string.IsNullOrEmpty(fileName))
            {
                ext = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(ext))
                    ext = ".html";
                else
                    ext = "";

                return GetFileNameValidChar(fileName + ext);
            }

            fileName = Path.GetFileName(url);

            if (string.IsNullOrEmpty(fileName))
                fileName = "noName";

            ext = Path.GetExtension(fileName);

            if (string.IsNullOrEmpty(ext))
                ext = ".html";
            else
                ext = "";

            fileName = fileName + ext;

            if (!fileName.StartsWith("?"))
                fileName = fileName.Split('?').FirstOrDefault();

            fileName = fileName.Split('&').LastOrDefault().Split('=').LastOrDefault();

            return GetFileNameValidChar(fileName);
        }

        public static bool IsValidPath(this string path, bool allowRelativePaths = false)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = Path.IsPathRooted(path);
                }
                else
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }

        public static string MakeRelativePath(string workingDirectory, string fullPath)
        {
            string result = string.Empty;
            int offset;

            // this is the easy case.  The file is inside of the working directory.
            if (fullPath.StartsWith(workingDirectory))
            {
                return fullPath.Substring(workingDirectory.Length + 1);
            }

            // the hard case has to back out of the working directory
            string[] baseDirs = workingDirectory.Split(new char[] { ':', '\\', '/' });
            string[] fileDirs = fullPath.Split(new char[] { ':', '\\', '/' });

            // if we failed to split (empty strings?) or the drive letter does not match
            if (baseDirs.Length <= 0 || fileDirs.Length <= 0 || baseDirs[0] != fileDirs[0])
            {
                // can't create a relative path between separate harddrives/partitions.
                return fullPath;
            }

            // skip all leading directories that match
            for (offset = 1; offset < baseDirs.Length; offset++)
            {
                if (baseDirs[offset] != fileDirs[offset])
                    break;
            }

            // back out of the working directory
            for (int i = 0; i < (baseDirs.Length - offset); i++)
            {
                result += "..\\";
            }

            // step into the file path
            for (int i = offset; i < fileDirs.Length - 1; i++)
            {
                result += fileDirs[i] + "\\";
            }

            // append the file
            result += fileDirs[fileDirs.Length - 1];

            return result;
        }

        public static bool IsRelative(string workingPath, string path)
        {
            int times;
            return IsRelative(workingPath, path, out times);
        }

        public static bool IsRelative(string workingPath, string path, out int times)
        {
            times = 0;
            string upperPath = workingPath;
            while (!(path.Contains(upperPath) || string.IsNullOrEmpty(upperPath)))
            {
                upperPath = Path.GetDirectoryName(upperPath);
                ++times;
            }

            return !string.IsNullOrEmpty(upperPath);
        }

        public static bool IsDirectory(this string path)
        {
            try
            {
                FileAttributes fa = File.GetAttributes(path);
                return (fa & FileAttributes.Directory) != 0;
            }
            catch
            { // The provided path doesn't exists
                return false;
            }
        }

        public static string GetTopLevelDir(string filePath)
        { // This wouldn't work for Linux paths
            string tempFilePath = (string)filePath.Clone();
            if (tempFilePath.Contains(("..\\")))
                tempFilePath = tempFilePath.Replace("..\\", "");

            string temp = Path.GetDirectoryName(tempFilePath);
            if (temp.Contains("\\"))
                temp = temp.Substring(0, temp.IndexOf("\\"));

            if (tempFilePath != filePath)
                return filePath.Substring(0, filePath.IndexOf(temp) + temp.Length);
            else
                return temp;
        }

        public static string GoUpInTree(string path, int times)
        {
            for (int i = 0; i < times; i++)
            {
                path = Path.GetDirectoryName(path);

                if (string.IsNullOrEmpty(path))
                    throw new Exception($"Cannot go up in the directory tree more times than allowed. (Max == {i})");
            }

            return path;
        }

        public static string GetTemporaryDirectory(string prefix = "", string suffix = "")
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), prefix + Path.GetRandomFileName() + suffix);
            Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }

        public static bool IsValidPath(this string path) => !Regex.IsMatch(path, "[" + Regex.Escape(new string(Path.GetInvalidPathChars())) + "]");

        public static bool IsValidFilename(this string path) => !Regex.IsMatch(path, "[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");

        public static void WriteToFile(this MemoryStream stream, string outputDir, string fileName)
        {
            if (!IsValidPath(outputDir))
                throw new ArgumentException("Invalid path specified.", "outputDir");

            if (!IsValidFilename(fileName))
                throw new ArgumentException("Invalid path specified.", "fileName");

            stream.WriteToFile(Path.Combine(outputDir, fileName));
        }

        public static void WriteToFile(this MemoryStream stream, string path)
        {
            if (!IsValidFilename(path))
                throw new ArgumentException("Invalid path specified.", "path");

            using (FileStream file = File.Create(path))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(file);
            }
        }

        public static bool IsDirectoryEmptyOrNull(this string folderPath)
        {
            return !Directory.Exists(folderPath) || Directory.Exists(folderPath) && Directory.GetFiles(folderPath).Length == 0;
        }
    }
}