using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Extensions;
using UnityEngine.Networking;
using uzLib.Lite.ExternalCode.Extensions;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The IOHelper class
    /// </summary>
    public static class IOHelper
    {
        /// <summary>
        /// Gets the file name valid character.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetFileNameValidChar(this string fileName)
        {
            foreach (var item in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(item.ToString(), "");

            return fileName;
        }

        /// <summary>
        /// Gets the file name from URL without extension.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string GetFileNameFromUrlWithoutExtension(this string url)
        {
            return Path.GetFileNameWithoutExtension(GetFileNameFromUrl(url));
        }

        /// <summary>
        /// Gets the file name from URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Determines whether [is valid path] [the specified allow relative paths].
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="allowRelativePaths">if set to <c>true</c> [allow relative paths].</param>
        /// <returns>
        ///   <c>true</c> if [is valid path] [the specified allow relative paths]; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Makes the relative path.
        /// </summary>
        /// <param name="workingDirectory">The working directory.</param>
        /// <param name="fullPath">The full path.</param>
        /// <returns></returns>
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
            for (int i = 0; i < baseDirs.Length - offset; i++)
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

        /// <summary>
        /// Determines whether the specified working path is relative.
        /// </summary>
        /// <param name="workingPath">The working path.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the specified working path is relative; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRelative(string workingPath, string path)
        {
            int times;
            return IsRelative(workingPath, path, out times);
        }

        /// <summary>
        /// Determines whether the specified working path is relative.
        /// </summary>
        /// <param name="workingPath">The working path.</param>
        /// <param name="path">The path.</param>
        /// <param name="times">The times.</param>
        /// <returns>
        ///   <c>true</c> if the specified working path is relative; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Gets the top level dir.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static string GetTopLevelDir(string filePath)
        {
            // TODO: This wouldn't work for Linux paths

            string tempFilePath = (string)filePath.Clone();
            if (tempFilePath.Contains("..\\"))
                tempFilePath = tempFilePath.Replace("..\\", "");

            string temp = Path.GetDirectoryName(tempFilePath);
            if (temp.Contains("\\"))
                temp = temp.Substring(0, temp.IndexOf("\\"));

            if (tempFilePath != filePath)
                return filePath.Substring(0, filePath.IndexOf(temp) + temp.Length);
            else
                return temp;
        }

        /// <summary>
        /// Goes up in tree.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="times">The times.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Cannot go up in the directory tree more times than allowed. (Max == {i}</exception>
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

        /// <summary>
        /// Gets the temporary directory.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="useRandomness">if set to <c>true</c> [use randomness].</param>
        /// <returns></returns>
        public static string GetTemporaryDirectory(string prefix = "", string suffix = "", bool useRandomness = true)
        {
            string tempPath = Path.GetTempPath(),
                   interfix = useRandomness ? Path.GetRandomFileName() : "-" + Directory.GetFiles(tempPath, "*", SearchOption.TopDirectoryOnly).Where(f => f.Contains(prefix)).Count().ToString();
            // If random is false, then avoid collision by cheking number of files with that prefix ^^^

            string tempDirectory = Path.Combine(tempPath, prefix + interfix + suffix);
            Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }

        /// <summary>
        /// Determines whether [is valid path].
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if [is valid path] [the specified path]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidPath(this string path) => !Regex.IsMatch(path, "[" + Regex.Escape(new string(Path.GetInvalidPathChars())) + "]");

        /// <summary>
        /// Determines whether [is valid filename].
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if [is valid filename] [the specified path]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidFilename(this string path) => !Regex.IsMatch(path, "[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="outputDir">The output dir.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="ArgumentException">
        /// Invalid path specified. - outputDir
        /// or
        /// Invalid path specified. - fileName
        /// </exception>
        public static void WriteToFile(this MemoryStream stream, string outputDir, string fileName)
        {
            if (!IsValidPath(outputDir))
                throw new ArgumentException("Invalid path specified.", "outputDir");

            if (!IsValidFilename(fileName))
                throw new ArgumentException("Invalid path specified.", "fileName");

            stream.WriteToFile(Path.Combine(outputDir, fileName));
        }

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="path">The path.</param>
        /// <exception cref="ArgumentException">Invalid path specified. - path</exception>
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

        /// <summary>
        /// Determines whether [is directory empty or null].
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>
        ///   <c>true</c> if [is directory empty or null] [the specified folder path]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDirectoryEmptyOrNull(this string folderPath)
        {
            return !Directory.Exists(folderPath) || Directory.Exists(folderPath) && Directory.GetFiles(folderPath).Length == 0;
        }

        /// <summary>
        /// Attempt to empty the folder. Return false if it fails (locked files...).
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns>true on success</returns>
        public static bool EmptyFolder(string folderPath)
        {
            if (!folderPath.IsDirectory())
                throw new ArgumentException("folderPath must be a folder.", "folderPath");

            bool errors = false;
            DirectoryInfo dir = new DirectoryInfo(folderPath);

            foreach (FileInfo fi in dir.EnumerateFiles())
            {
                try
                {
                    fi.IsReadOnly = false;
                    fi.Delete();

                    //Wait for the item to disapear (avoid 'dir not empty' error).
                    while (fi.Exists)
                    {
                        System.Threading.Thread.Sleep(10);
                        fi.Refresh();
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    errors = true;
                }
            }

            foreach (DirectoryInfo di in dir.EnumerateDirectories())
            {
                try
                {
                    EmptyFolder(di.FullName);
                    di.Delete();

                    //Wait for the item to disapear (avoid 'dir not empty' error).
                    while (di.Exists)
                    {
                        System.Threading.Thread.Sleep(10);
                        di.Refresh();
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    errors = true;
                }
            }

            Directory.Delete(folderPath);

            return !errors;
        }

        /// <summary>
        ///     Loads the texture from.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="action">The action.</param>
        /// <param name="isWWW">if set to <c>true</c> [is WWW].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">path</exception>
        /// <exception cref="ArgumentException">File doesn't exists! - path</exception>
        public static IEnumerator LoadTextureFrom(string path, Action<Texture2D> action, bool isWWW = true)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (!isWWW && !File.Exists(path))
                throw new ArgumentException("File doesn't exists!", nameof(path));

            const string prefixPath = "file:///";

            if (!isWWW && !path.StartsWith(prefixPath))
                path = prefixPath + path;

            using (var request = UnityWebRequestTexture.GetTexture(path))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                    Debug.LogError(request.error);
                else
                    action?.Invoke(((DownloadHandlerTexture)request.downloadHandler).texture);
            }
        }

        /// <summary>
        ///     Determines whether this instance is extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>
        ///     <c>true</c> if the specified extension is extension; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsExtension(this string extension)
        {
            return extension.StartsWith(".") && MimeTypeMap.HasExtension(extension);
        }

        /// <summary>
        /// Writes all bytes.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="bytes">The bytes.</param>
        /// <exception cref="ArgumentNullException">path</exception>
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            if (string.IsNullOrEmpty(path))

                throw new ArgumentNullException(nameof(path));

            string folder = Path.GetDirectoryName(path);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// Creates an empty file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public static void CreateEmptyFile(string filename)
        {
            File.Create(filename).Dispose();
        }

        /// <summary>
        /// Directories the copy.
        /// </summary>
        /// <param name="sourceDirName">Name of the source dir.</param>
        /// <param name="destDirName">Name of the dest dir.</param>
        /// <param name="copySubDirs">if set to <c>true</c> [copy sub dirs].</param>
        /// <exception cref="System.IO.DirectoryNotFoundException">Source directory does not exist or could not be found: "
        ///                     + sourceDirName</exception>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// Reads all text shared.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string ReadAllTextShared(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
                return sr.ReadToEnd();
        }

        /// <summary>
        /// Reads all lines shared.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string[] ReadAllLinesShared(string path)
        {
            string contents = ReadAllTextShared(path);
            return Regex.Split(contents, "\r\n|\r|\n");
        }
    }
}