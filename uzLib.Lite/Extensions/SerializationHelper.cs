using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace uzLib.Lite.Extensions
{
    public static class SerializationHelper
    {
        public static bool TryDeserialize<T>(string path, out T obj, bool createFile = true)
            where T : new()
        {
            if (!File.Exists(path) && !createFile)
            {
                obj = default(T);
                return false;
            }
            else if (!File.Exists(path))
                File.WriteAllText(path, "");

            string content = File.ReadAllText(path);

            if (!string.IsNullOrEmpty(content) && !IsValidJSON(content))
            {
                obj = default(T);
                return false;
            }
            else if (string.IsNullOrEmpty(content))
            {
                obj = new T();
                return true;
            }

            obj = JsonConvert.DeserializeObject<T>(content);
            return true;
        }

        public static bool IsValidJSON(this string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}