using System;
using System.IO;
using System.Linq;

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
    }
}