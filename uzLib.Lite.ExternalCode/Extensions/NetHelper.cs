using System;
using System.IO;
using System.Net;
using UnityEngine.Extensions;

#if !(!UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5)
using uzLib.Lite.Extensions;
#endif

namespace uzLib.Lite.ExternalCode.Extensions
{
    /// <summary>
    /// The NetHelper class
    /// </summary>
    public static class NetHelper
    {
        /// <summary>
        /// Makes the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="headerCollection">The header collection.</param>
        /// <returns></returns>
        public static string MakeRequest(this string url, params Tuple<string, string>[] headerCollection)
        {
            try
            {
                if (WebRequest.Create(url) is HttpWebRequest webRequest)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.UserAgent = "request";
                    webRequest.ContentType = "application/json";

                    if (!headerCollection.IsNullOrEmpty())
                        foreach (var col in headerCollection)
                            webRequest.Headers.Add(col.Item1, col.Item2);

                    using (Stream s = webRequest.GetResponse().GetResponseStream())
                    using (StreamReader sr = new StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        return jsonResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return string.Empty;
        }

        /// <summary>
        /// Makes the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="AcceptHeader">The accept header.</param>
        /// <returns></returns>
        public static string MakeRequest(this string url, string contentType = null, string userAgent = null, string AcceptHeader = null, bool notifyException = false)
        {
            try
            {
                if (WebRequest.Create(url) is HttpWebRequest webRequest)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.UserAgent = userAgent ?? "request";
                    webRequest.ContentType = contentType ?? "application/json";

                    if (!string.IsNullOrEmpty(AcceptHeader))
                        webRequest.Accept = AcceptHeader;

                    using (Stream s = webRequest.GetResponse().GetResponseStream())
                    using (StreamReader sr = new StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        return jsonResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                if (notifyException)
                    throw;
            }

            return string.Empty;
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="filePath">The file path.</param>
        public static void DownloadFile(string url, string filePath)
        {
            // Create a new WebClient instance.
            using (WebClient myWebClient = new WebClient())
                // Download the Web resource and save it into the current filesystem folder.
                myWebClient.DownloadFile(url, filePath);
        }

        /// <summary>
        ///     Checks for internet connection.
        /// </summary>
        /// <returns></returns>
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

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
                    .Substring(wc.ResponseHeaders["Content-Disposition"].IndexOf("filename=", StringComparison.Ordinal) + 9).Replace("\"", "");

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
                    .Substring(wc.ResponseHeaders["Content-Disposition"].IndexOf("filename=", StringComparison.Ordinal) + 9).Replace("\"", "");

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

#endif

        /// <summary>
        ///     Downloads the HTML with progress.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="downloadProgress">The download progress.</param>
        /// <param name="downloadString">The downloaded string.</param>
        /// <returns></returns>
        public static string DownloadHtmlWithProgress(string url, DownloadProgressChangedEventHandler downloadProgress,
            DownloadStringCompletedEventHandler downloadString)
        {
            using (var wc = new WebClient())
            {
                wc.DownloadProgressChanged += downloadProgress;
                wc.DownloadStringCompleted += downloadString;

                return wc.DownloadString(url);
            }
        }

        public static HttpStatusCode GetHttpStatusCode(string url)
        {
            HttpWebResponse resp = null;
            HttpStatusCode code;
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "HEAD";
                req.AllowAutoRedirect = false;
                resp = (HttpWebResponse)req.GetResponse();
                code = resp.StatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default;
            }
            finally
            {
                resp?.Close();
            }

            return code;
        }

        public static string GetFinalRedirect(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            int maxRedirCount = 8;  // prevent infinite loops
            string newUrl = url;
            do
            {
                HttpWebResponse resp = null;
                try
                {
                    var req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = "HEAD";
                    req.AllowAutoRedirect = false;
                    resp = (HttpWebResponse)req.GetResponse();
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            return newUrl;

                        case HttpStatusCode.Redirect:
                        case HttpStatusCode.MovedPermanently:
                        case HttpStatusCode.RedirectKeepVerb:
                        case HttpStatusCode.RedirectMethod:
                            newUrl = resp.Headers["Location"];
                            if (newUrl == null)
                                return url;

                            if (newUrl.IndexOf("://", StringComparison.Ordinal) == -1)
                            {
                                // Doesn't have a URL Schema, meaning it's a relative or absolute URL
                                Uri u = new Uri(new Uri(url), newUrl);
                                newUrl = u.ToString();
                            }
                            break;

                        default:
                            return newUrl;
                    }
                    url = newUrl;
                }
                catch (WebException)
                {
                    // Return the last known good URL
                    return newUrl;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                finally
                {
                    resp?.Close();
                }
            } while (maxRedirCount-- > 0);

            return newUrl;
        }
    }
}