using System;
using System.Collections;
using System.IO;
using System.Reflection;
using CielaSpike;
using UnityEngine.Extensions;
using uzLib.Lite.Extensions;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace UnityEngine.Global.IMGUI
{
#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
    public static class GlobalGUIUtility
    {
        // As alternative fix?
        //public static object GifInstance { get; set; }

        public static object GetImageObject(Func<string, byte[], object, object> gifCallback, byte[] data, string path, string filenameOrExtension,
            object editorInstance)
        {
            if (gifCallback == null)
                throw new ArgumentNullException(nameof(gifCallback));

            var isExtension = filenameOrExtension.IsExtension();
            var extension = isExtension ? filenameOrExtension : Path.GetExtension(filenameOrExtension);

            // Note: I'm removing '/' and ':' because this a common part from the url that isn't relevant to get its filename (without extension). But we want to check for '?' or '&'
            var name = isExtension
                ? path.IsUrl() && path.MulticaseRemove("/", ":").CheckIfFileNameHasInvalidCharacters() ? string.Empty :
                Path.GetFileNameWithoutExtension(path)
                : Path.GetFileName(filenameOrExtension);

            switch (extension)
            {
                case ".gif":
                    return gifCallback(name, data, editorInstance);

                //return new UniGif.GifFile(name, mono, data, editorInstance);

                default:
                    var tex = new Texture2D(2, 2);
                    tex.LoadImage(data);
                    tex.name = path.IsUrl()
                        ? $"{Path.GetFileNameWithoutExtension(path)}{extension}"
                        : Path.GetFileName(path);

                    return tex;
            }
        }

        /// <summary>
        ///     Gets the image object.
        /// </summary>
        /// <param name="mono">The mono.</param>
        /// <param name="data">The data.</param>
        /// <param name="path">The path.</param>
        /// <param name="extension">The extension.</param>
        /// <param name="editorInstance">The editor instance.</param>
        /// <returns></returns>
        public static object GetImageObject(MonoBehaviour mono, byte[] data, string path, string filenameOrExtension,
            object editorInstance)
        {
            var isExtension = filenameOrExtension.IsExtension();
            var extension = isExtension ? filenameOrExtension : Path.GetExtension(filenameOrExtension);

            // Note: I'm removing '/' and ':' because this a common part from the url that isn't relevant to get its filename (without extension). But we want to check for '?' or '&'
            var name = isExtension
                ? path.IsUrl() && path.MulticaseRemove("/", ":").CheckIfFileNameHasInvalidCharacters() ? string.Empty :
                Path.GetFileNameWithoutExtension(path)
                : Path.GetFileName(filenameOrExtension);

            switch (extension)
            {
                case ".gif":
                    // return GifInstance;
                    return Activator.CreateInstance(Type.GetType("UnityGif.UniGif.GifFile") ?? throw new InvalidOperationException(),
                        BindingFlags.CreateInstance, Type.DefaultBinder, new[] { name, mono, data, editorInstance });
                //return new UniGif.GifFile(name, mono, data, editorInstance);

                default:
                    var tex = new Texture2D(2, 2);
                    tex.LoadImage(data);
                    tex.name = path.IsUrl()
                        ? $"{Path.GetFileNameWithoutExtension(path)}{extension}"
                        : Path.GetFileName(path);

                    return tex;
            }
        }

        /// <summary>
        ///     Load image from internet (taking care of extension)
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="mono">The mono.</param>
        /// <param name="editorInstance">The editor instance.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Data from url isn't an image!</exception>
        public static object LoadImageFromInternet(string url, MonoBehaviour mono, object editorInstance)
        {
            return LoadImageFromInternet(url, mono, editorInstance, out var data);
        }

        /// <summary>
        ///     Loads the texture from internet (taking care of extension)
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="mono">The mono.</param>
        /// <param name="editorInstance">The editor instance.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Data from url isn't an image!</exception>
        public static object LoadImageFromInternet(string url, MonoBehaviour mono, object editorInstance,
            out byte[] data)
        {
            var filename = NetHelper.GetNameAndExtensionFrom(url, out data).ToLowerInvariant();
            var extension = Path.GetExtension(filename);

            if (string.IsNullOrEmpty(extension) || !FileHelper.IsValidTextureExtension(extension))
                throw new Exception("Data from url isn't an image!"); // (Url: {url})

            return GetImageObject(mono, data, url, filename, editorInstance);
        }

        /// <summary>
        ///     Loads the texture from IO (taking care of extension)
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mono">The mono.</param>
        /// <param name="editorInstance">The editor instance.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Data from url isn't an image!</exception>
        public static object LoadImageFromIO(string path, MonoBehaviour mono, object editorInstance)
        {
            return LoadImageFromIO(path, mono, editorInstance, out var data);
        }

        /// <summary>
        ///     Loads the texture from IO (taking care of extension)
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mono">The mono.</param>
        /// <param name="editorInstance">The editor instance.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">path - File doesn't exists!</exception>
        /// <exception cref="Exception">Data from url isn't an image!</exception>
        public static object LoadImageFromIO(string path, MonoBehaviour mono, object editorInstance, out byte[] data)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                throw new ArgumentException("path", "File doesn't exists!");

            var extension = Path.GetExtension(path);

            if (string.IsNullOrEmpty(extension) || !FileHelper.IsValidTextureExtension(extension))
                throw new Exception("Data from url isn't an image!");

            data = File.ReadAllBytes(path);

            return GetImageObject(mono, data, path, extension, editorInstance);
        }

        /// <summary>
        ///     Loads the image from internet asynchronous.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="mono">The mono.</param>
        /// <param name="editorInstance">The editor instance.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">callback</exception>
        /// <exception cref="Exception">Data from url isn't an image!</exception>
        public static IEnumerator LoadImageFromInternetAsync(string url, MonoBehaviour mono, object editorInstance,
            Action<byte[], string> callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            yield return Ninja.JumpBack;

            var filename = NetHelper.GetNameAndExtensionFrom(url, out var data).ToLowerInvariant();
            var extension = Path.GetExtension(filename);

            if (string.IsNullOrEmpty(extension) || !FileHelper.IsValidTextureExtension(extension))
                throw new Exception("Data from url isn't an image!"); // (Url: {url})

            callback(data, filename);
        }

        /// <summary>
        ///     Loads the image from internet asynchronous.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="mono">The mono.</param>
        /// <param name="editorInstance">The editor instance.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Data from url isn't an image!</exception>
        public static Tuple<byte[], string> LoadImageFromInternetAsync(string url, MonoBehaviour mono,
            object editorInstance)
        {
            var filename = NetHelper.GetNameAndExtensionFrom(url, out var data).ToLowerInvariant();
            var extension = Path.GetExtension(filename);

            if (string.IsNullOrEmpty(extension) || !FileHelper.IsValidTextureExtension(extension))
                throw new Exception("Data from url isn't an image!"); // (Url: {url})

            return new Tuple<byte[], string>(data, filename);
        }

        /// <summary>
        ///     Gets the mouse position.
        /// </summary>
        /// <param name="f_isEditor">if set to <c>true</c> [f is editor].</param>
        /// <returns></returns>
        public static Vector2 GetMousePosition(bool f_isEditor)
        {
            // Review: If is editor... I'm not sure that it depends on a global position...

            return f_isEditor
                ? GUIUtility.GUIToScreenPoint(Event.current.mousePosition)
                : Event.current.mousePosition;
        }
    }
#endif
}