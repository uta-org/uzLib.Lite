using System;
using System.IO;
using System.Net;

namespace uzLib.Lite.Extensions
{
    public static class NetHelper
    {
        public static string MakeRequest(this string url, params Tuple<string, string>[] headerCollection)
        {
            try
            {
                var webRequest = WebRequest.Create(url) as HttpWebRequest;
                if (webRequest != null)
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

        public static string MakeRequest(this string url, string AcceptHeader = "")
        {
            try
            {
                var webRequest = WebRequest.Create(url) as HttpWebRequest;
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.UserAgent = "request";
                    webRequest.ContentType = "application/json";

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
            }

            return string.Empty;
        }

        public static void DownloadFile(string url, string filePath)
        {
            // Create a new WebClient instance.
            using (WebClient myWebClient = new WebClient())
                // Download the Web resource and save it into the current filesystem folder.
                myWebClient.DownloadFile(url, filePath);
        }
    }
}