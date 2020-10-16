using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The SerializationHelper class
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Tries the deserialize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="obj">The object.</param>
        /// <param name="createFile">if set to <c>true</c> [create file].</param>
        /// <returns></returns>
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

        /// <summary>
        /// Determines whether [is valid json].
        /// </summary>
        /// <param name="strInput">The string input.</param>
        /// <returns>
        ///   <c>true</c> if [is valid json] [the specified string input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidJSON(this string strInput)
        {
            strInput = strInput.Trim();
            if (strInput.StartsWith("{") && strInput.EndsWith("}") || //For object
                strInput.StartsWith("[") && strInput.EndsWith("]")) //For array
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

        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the XML file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static byte[] Serialize<T>(this T objectToWrite)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);

                return stream.GetBuffer();
            }
        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the XML.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public static async Task<T> _Deserialize<T>(this byte[] arr)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                await stream.WriteAsync(arr, 0, arr.Length);
                stream.Position = 0;

                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Deserializes the specified arr.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <returns></returns>
        public static async Task<object> Deserialize(this byte[] arr)
        {
            object obj = await arr._Deserialize<object>();
            return obj;
        }
    }
}