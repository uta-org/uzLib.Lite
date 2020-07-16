using System.IO;
using Newtonsoft.Json;

namespace UnityEngine.Extensions
{
    public static class JsonHelper
    {
        /// <summary>
        ///     Prettifies the JSON token.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static string JsonPrettify(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) {Formatting = Formatting.Indented};
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }
    }
}