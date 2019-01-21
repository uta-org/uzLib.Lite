using System;
using System.IO;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    public static class StringHelper
    {
        public static string GetFileNameValidChar(this string fileName)
        {
            foreach (var item in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(item.ToString(), "");

            return fileName;
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
    }
}