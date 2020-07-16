using System;
using System.Net;

namespace UnityEngine.Extensions
{
    public static class NetHelper
    {
        /// <summary>
        ///     Gets the name and extension from.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetNameAndExtensionFrom(string url, out byte[] data)
        {
            string filename;

            using (var wc = new WebClient())
            {
                data = wc.DownloadData(url);

                filename = wc.GetNameFrom();
                filename += wc.GetExtensionFrom();
            }

            return filename;
        }

        /// <summary>
        ///     Gets the name from.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string GetNameFrom(string url)
        {
            return GetNameFrom(url, out var data);
        }

        /// <summary>
        ///     Gets the name from.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetNameFrom(string url, out byte[] data)
        {
            using (var wc = new WebClient())
            {
                return wc.GetExtensionFrom(url, out data);
            }
        }

        /// <summary>
        ///     Gets the name from.
        /// </summary>
        /// <param name="wc">The wc.</param>
        /// <returns></returns>
        public static string GetNameFrom(this WebClient wc)
        {
            if (!string.IsNullOrEmpty(wc.ResponseHeaders["Content-Disposition"]))
                return wc.ResponseHeaders["Content-Disposition"]
                    .Substring(wc.ResponseHeaders["Content-Disposition"].IndexOf("filename=") + 9).Replace("\"", "");

            return string.Empty;
        }

        /// <summary>
        ///     Gets the name from.
        /// </summary>
        /// <param name="wc">The wc.</param>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetNameFrom(this WebClient wc, string url, out byte[] data)
        {
            data = wc.DownloadData(url);
            if (!string.IsNullOrEmpty(wc.ResponseHeaders["Content-Disposition"]))
                return wc.ResponseHeaders["Content-Disposition"]
                    .Substring(wc.ResponseHeaders["Content-Disposition"].IndexOf("filename=") + 9).Replace("\"", "");

            return string.Empty;
        }

        /// <summary>
        ///     Gets the extension from.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string GetExtensionFrom(string url)
        {
            return GetExtensionFrom(url, out var data);
        }

        /// <summary>
        ///     Gets the extension from.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetExtensionFrom(string url, out byte[] data)
        {
            using (var wc = new WebClient())
            {
                return wc.GetExtensionFrom(url, out data);
            }
        }

        /// <summary>
        ///     Gets the extension from.
        /// </summary>
        /// <param name="wc">The wc.</param>
        /// <returns></returns>
        public static string GetExtensionFrom(this WebClient wc)
        {
            var contentType = wc.ResponseHeaders["Content-Type"];
            // Debug.Log("Content-Type: " + contentType);

            return MimeTypeMap.GetExtension(contentType);
        }

        /// <summary>
        ///     Gets the extension from.
        /// </summary>
        /// <param name="wc">The wc.</param>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetExtensionFrom(this WebClient wc, string url, out byte[] data)
        {
            data = wc.DownloadData(url);
            var contentType = wc.ResponseHeaders["Content-Type"];
            // Debug.Log("Content-Type: " + contentType);

            return MimeTypeMap.GetExtension(contentType);
        }

        /// <summary>
        ///     Determines whether this instance is URL.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        ///     <c>true</c> if the specified p is URL; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUrl(this string p)
        {
            return !new Uri(p).IsFile;
        }
    }
}