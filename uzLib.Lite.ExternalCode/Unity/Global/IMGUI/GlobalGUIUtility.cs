using System;
using System.Collections;
using System.IO;
using CielaSpike;
using UnityEngine;
using UnityEngine.Extensions;
using UnityGif;
using uzLib.Lite.ExternalCode.Extensions;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace uzLib.Lite.ExternalCode.Unity.Global.IMGUI
{
    public static class GlobalGUIUtility
    {
        /// <summary>
        ///     Gets the containing rect.
        /// </summary>
        /// <param name="originalSize">Size of the original.</param>
        /// <returns></returns>
        public static Rect GetContainingRect(Rect? originalSize = null)
        {
            // Is editor
            if (!originalSize.HasValue)
            {
#if UNITY_EDITOR
                if (EditorWindow.focusedWindow != null)
                    return EditorWindow.focusedWindow.position;
                return Rect.zero;
#else
                return Rect.zero;
#endif
            }

            return originalSize.Value;
        }

        /// <summary>
        ///     Gets the size of the containing.
        /// </summary>
        /// <param name="originalSize">Size of the original.</param>
        /// <returns></returns>
        public static Vector2 GetContainingSize(Vector2? originalSize = null)
        {
            // Is editor
            if (!originalSize.HasValue)
            {
#if UNITY_EDITOR
                if (EditorWindow.focusedWindow != null)
                    return EditorWindow.focusedWindow.position.size;
                return Vector2.zero;
#else
                return Vector2.zero;
#endif
            }

            return originalSize.Value;
        }

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

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
            string extension;
            var isExtension = filenameOrExtension.IsExtension();
            if (isExtension)
                extension = filenameOrExtension;
            else
                extension = Path.GetExtension(filenameOrExtension);

            // Note: I'm removing '/' and ':' because this a common part from the url that isn't relevant to get its filename (without extension). But we want to check for '?' or '&'
            var name = isExtension
                ? path.IsUrl() && path.MulticaseRemove("/", ":").CheckIfFileNameHasInvalidCharacters() ? string.Empty :
                Path.GetFileNameWithoutExtension(path)
                : Path.GetFileName(filenameOrExtension);

            switch (extension)
            {
                case ".gif":
                    return new UniGif.GifFile(name, mono, data, editorInstance);

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

#endif

        public static string Wrap(this string str, float maxWidth)
        {
            return Wrap(str, maxWidth, null);
        }

        public static string Wrap(this string str, float maxWidth, Font font)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException(nameof(str));

            if (font == null)
                font = Font.CreateDynamicFontFromOSFont("Arial", 12);

            int nLastWordInd = 0;
            int nIter = 0;
            float fCurWidth = 0.0f;
            float fCurWordWidth = 0.0f;
            while (nIter < str.Length)
            {
                // get char info
                char c = str[nIter];
                CharacterInfo charInfo;
                if (!font.GetCharacterInfo(c, out charInfo))
                    throw new Exception("Unrecognized character encountered (" + (int)c + "): " + c);

                if (c == '\n')
                {
                    nLastWordInd = nIter;
                    fCurWidth = 0.0f;
                }
                else
                {
                    if (c == ' ')
                    {
                        nLastWordInd = nIter; // replace this character with '/n' if breaking here
                        fCurWordWidth = 0.0f;
                    }

                    fCurWidth += charInfo.width;
                    fCurWordWidth += charInfo.width;
                    if (fCurWidth >= maxWidth)
                    {
                        str = str.Remove(nLastWordInd, 1);
                        str = str.Insert(nLastWordInd, "\n");
                        fCurWidth = fCurWordWidth;
                    }
                }

                ++nIter;
            }

            return str;
        }
    }
}