using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using uzLib.Lite.Extensions;

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

        public static async Task<long> DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<float> progress = null, CancellationToken cancellationToken = default)
        {
            // Get the http headers first to examine the content length
            using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var contentLength = response.Content.Headers.ContentLength;

                using (var download = await response.Content.ReadAsStreamAsync())
                {
                    // Ignore progress reporting when no progress reporter was
                    // passed or when the content length is unknown
                    if (progress == null || !contentLength.HasValue)
                    {
                        await download.CopyToAsync(destination);
                        return destination.Length;
                    }

                    // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
                    var relativeProgress = new Progress<long>(totalBytes => progress.Report((float)totalBytes / contentLength.Value));
                    // Use extension method to report progress while downloading
                    await download.CopyToAsync(destination, 81920, relativeProgress, cancellationToken);
                    progress.Report(1);

                    return contentLength.Value;
                }
            }
        }
    }
}