using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Newtonsoft.Json;

namespace uzLib.Lite.Extensions
{
    public static class NVCHelper
    {
        public static IDictionary<string, string> ToDictionary(
            this NameValueCollection source)
        {
            return source.AllKeys.ToDictionary(k => k, k => source[k]);
        }

        public static string ToJson(this NameValueCollection source)
        {
            return JsonConvert.SerializeObject(source.ToDictionary());
        }
    }
}