using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Core;
using UnityEngine.Extensions;
using UnityEngine.UI;
using uzLib.Lite.Core;
using uzLib.Lite.ExternalCode.Extensions;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace UnityEngine.Global.IMGUI
{
    /// <summary>
    ///     The Global GUI Layout (used in Editor and in-game)
    /// </summary>
    /// <seealso cref="Singleton{T}" />
    public sealed class GlobalGUILayout : Singleton<GlobalGUILayout>
    {
        /// <summary>
        ///     The instances
        /// </summary>
        private static readonly Dictionary<int, GlobalGUILayout> Instances = new Dictionary<int, GlobalGUILayout>();

        private static ScreenCenterContent m_screenCenterContent;

        /// <summary>
        ///     The first time
        /// </summary>
        private bool f_firstTime;

        /// <summary>
        ///     Gets the selected value.
        /// </summary>
        /// <value>
        ///     The selected value.
        /// </value>
        public int? SelectedValue { get; private set; }

        /// <summary>
        ///     Gets my file browser.
        /// </summary>
        /// <value>
        ///     My file browser.
        /// </value>
        public FileBrowser MyFileBrowser { get; private set; }

        /// <summary>
        ///     Creates the specified map.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns></returns>
        public static GlobalGUILayout Create(GlobalIdentifier map, int? selectedValue = null)
        {
            var id = map.GetId();

            var ins = Instances.AddOrGet(id, new GlobalGUILayout());

            if (selectedValue.HasValue)
            {
                ins.SelectedValue = selectedValue;
                ins.f_firstTime = true;
            }

            return ins;
        }

        /// <summary>
        ///     Display a group of buttons.
        /// </summary>
        /// <param name="texts">The texts.</param>
        /// <returns></returns>
        public int? ButtonGroup(params Texture2D[] texts)
        {
            return ButtonGroup(texts.Select(t => new GUIContent(t)).ToArray());
        }

        /// <summary>
        ///     Display a group of buttons.
        /// </summary>
        /// <param name="texts">The texts.</param>
        /// <returns></returns>
        public int? ButtonGroup(params Texture[] texts)
        {
            return ButtonGroup(texts.Select(t => new GUIContent(t)).ToArray());
        }

        /// <summary>
        ///     Display a group of buttons.
        /// </summary>
        /// <param name="texts">The texts.</param>
        /// <returns></returns>
        public int? ButtonGroup(params string[] texts)
        {
            return ButtonGroup(texts.Select(t => new GUIContent(t)).ToArray());
        }

        /// <summary>
        ///     Display a group of buttons.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="texts">The texts.</param>
        /// <returns></returns>
        public int? ButtonGroup(Action callback, params Texture2D[] texts)
        {
            return ButtonGroup(callback, texts.Select(t => new GUIContent(t)).ToArray());
        }

        /// <summary>
        ///     Display a group of buttons.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="texts">The texts.</param>
        /// <returns></returns>
        public int? ButtonGroup(Action callback, params Texture[] texts)
        {
            return ButtonGroup(callback, texts.Select(t => new GUIContent(t)).ToArray());
        }

        /// <summary>
        ///     Display a group of buttons.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="texts">The texts.</param>
        /// <returns></returns>
        public int? ButtonGroup(Action callback, params string[] texts)
        {
            return ButtonGroup(callback, texts.Select(t => new GUIContent(t)).ToArray());
        }

        /// <summary>
        ///     Display a group of buttons.
        /// </summary>
        /// <param name="contents">The contents.</param>
        /// <returns></returns>
        public int? ButtonGroup(params GUIContent[] contents)
        {
            return ButtonGroup(null, contents);
        }

        /// <summary>
        ///     Display a group of buttons.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="contents">The contents.</param>
        /// <returns></returns>
        public int? ButtonGroup(Action callback, params GUIContent[] contents)
        {
            var key = "";

            GUILayout.BeginHorizontal();

            var i = 0;
            foreach (var content in contents)
            {
                if (GUILayout.Button(content,
                        i == 0 ? GlobalStyles.miniButtonLeft :
                        i == contents.Length - 1 ? GlobalStyles.miniButtonRight : GlobalStyles.miniButtonMid) ||
                    f_firstTime)
                {
                    if (!f_firstTime)
                    {
                        key = contents.GetUniqueId(i);
                        SelectedValue = i;
                    }

                    callback?.Invoke();

                    if (f_firstTime) f_firstTime = false;
                }

                ++i;
            }

            GUILayout.EndHorizontal();

            return SelectedValue;
        }

        /// <summary>
        ///     Display a inputs to select file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fEditor">if set to <c>true</c> [f editor].</param>
        /// <param name="verticalSpacing">The vertical spacing.</param>
        /// <returns></returns>
        public string InputFile(string path, bool fEditor = false, int verticalSpacing = 7)
        {
            if (verticalSpacing > 0) GUILayout.Space(verticalSpacing);

            GUILayout.BeginHorizontal(GUI.skin.box);

            GUILayout.Label(path ?? "Select a file...", GlobalStyles.CenteredLabelStyle);

            if (GUILayout.Button("Browse...", GUILayout.MaxWidth(100)))
            {
                if (fEditor)
                {
#if UNITY_EDITOR
                    var p = EditorUtility.OpenFilePanelWithFilters("Select a file", path,
                        new[] { "Image files", "png,jpg,jpeg,bmp,gif,tif" });
                    path = string.IsNullOrEmpty(p) ? null : p;
#endif
                }
                else
                {
                    if (MyFileBrowser == null) MyFileBrowser = FileBrowser.Create();

                    path = MyFileBrowser.Open();
                }
            }

            GUILayout.EndHorizontal();

            if (!fEditor && MyFileBrowser != null && MyFileBrowser.IsReady()) path = MyFileBrowser.CurrentPath;

            return path;
        }

        /// <summary>
        ///     Draws a texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <returns></returns>
        public static Rect DrawTexture(Texture2D texture)
        {
            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return Rect.zero;
            }

            var rect = GetRectForTexture(texture);
            rect = rect.ForceBoth(texture.width, texture.height);

            GUI.DrawTexture(rect, texture);
            // GUI.DrawTexture(rect.RoundValues(), texture);
            return rect;
        }

        /// <summary>
        ///     Draws a texture.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="texture">The texture.</param>
        /// <returns></returns>
        public static Rect DrawTexture(Vector2 size, Texture2D texture)
        {
            return DrawTexture(size.x, size.y, texture);
        }

        /// <summary>
        ///     Draws a texture.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="texture">The texture.</param>
        /// <returns></returns>
        public static Rect DrawTexture(float width, float height, Texture2D texture)
        {
            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return Rect.zero;
            }

            var rect = GetRectForTexture(width, height);
            rect = rect.ForceBoth(width, height);

            // ScaleMode.ScaleToFit, true, 0
            GUI.DrawTexture(rect, texture);
            // GUI.DrawTexture(rect.RoundValues(), texture);
            return rect;
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <returns></returns>
        public static Rect DrawTexture(Texture2D texture, ScaleMode scaleMode)
        {
            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return Rect.zero;
            }

            var rect = GetRectForTexture(texture);
            rect = rect.ForceBoth(texture.width, texture.height);

            GUI.DrawTexture(rect, texture, scaleMode);
            // GUI.DrawTexture(rect.RoundValues(), texture, scaleMode);
            return rect;
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <returns></returns>
        public static Rect DrawTexture(Vector2 size, Texture2D texture, ScaleMode scaleMode)
        {
            return DrawTexture(size.x, size.y, texture, scaleMode);
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <returns></returns>
        public static Rect DrawTexture(float width, float height, Texture2D texture, ScaleMode scaleMode)
        {
            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return Rect.zero;
            }

            var rect = GetRectForTexture(width, height);
            rect = rect.ForceBoth(width, height);

            // ScaleMode.ScaleToFit, true, 0
            GUI.DrawTexture(rect, texture, scaleMode);
            // GUI.DrawTexture(rect.RoundValues(), texture, scaleMode);
            return rect;
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <param name="alphaBlend">if set to <c>true</c> [alpha blend].</param>
        /// <returns></returns>
        public static Rect DrawTexture(Texture2D texture, ScaleMode scaleMode, bool alphaBlend)
        {
            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return Rect.zero;
            }

            var rect = GetRectForTexture(texture);
            rect = rect.ForceBoth(texture.width, texture.height);

            GUI.DrawTexture(rect, texture, scaleMode, alphaBlend);
            // GUI.DrawTexture(rect.RoundValues(), texture, scaleMode, alphaBlend);
            return rect;
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <param name="alphaBlend">if set to <c>true</c> [alpha blend].</param>
        /// <returns></returns>
        public static Rect DrawTexture(Vector2 size, Texture2D texture, ScaleMode scaleMode, bool alphaBlend)
        {
            return DrawTexture(size.x, size.y, texture, scaleMode, alphaBlend);
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <param name="alphaBlend">if set to <c>true</c> [alpha blend].</param>
        /// <returns></returns>
        public static Rect DrawTexture(float width, float height, Texture2D texture, ScaleMode scaleMode, bool alphaBlend)
        {
            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return Rect.zero;
            }

            var rect = GetRectForTexture(width, height);
            rect = rect.ForceBoth(width, height);

            // ScaleMode.ScaleToFit, true, 0
            GUI.DrawTexture(rect, texture, scaleMode, alphaBlend);
            // GUI.DrawTexture(rect.RoundValues(), texture, scaleMode, alphaBlend);
            return rect;
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <param name="alphaBlend">if set to <c>true</c> [alpha blend].</param>
        /// <param name="imageAspect">The image aspect.</param>
        /// <returns></returns>
        public static Rect DrawTexture(Texture2D texture, ScaleMode scaleMode, bool alphaBlend, float imageAspect)
        {
            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return Rect.zero;
            }

            var rect = GetRectForTexture(texture);
            rect = rect.ForceBoth(texture.width, texture.height);

            GUI.DrawTexture(rect, texture, scaleMode, alphaBlend, imageAspect);
            // GUI.DrawTexture(rect.RoundValues(), texture, scaleMode, alphaBlend, imageAspect);
            return rect;
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <param name="alphaBlend">if set to <c>true</c> [alpha blend].</param>
        /// <param name="imageAspect">The image aspect.</param>
        /// <returns></returns>
        public static Rect DrawTexture(Vector2 size, Texture2D texture, ScaleMode scaleMode, bool alphaBlend, float imageAspect)
        {
            return DrawTexture(size.x, size.y, texture, scaleMode, alphaBlend, imageAspect);
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <param name="alphaBlend">if set to <c>true</c> [alpha blend].</param>
        /// <param name="imageAspect">The image aspect.</param>
        /// <returns></returns>
        public static Rect DrawTexture(float width, float height, Texture2D texture, ScaleMode scaleMode, bool alphaBlend, float imageAspect)
        {
            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return Rect.zero;
            }

            var rect = GetRectForTexture(width, height);
            rect = rect.ForceBoth(width, height);

            // ScaleMode.ScaleToFit, true, 0
            GUI.DrawTexture(rect, texture, scaleMode, alphaBlend, imageAspect);
            // GUI.DrawTexture(rect.RoundValues(), texture, scaleMode, alphaBlend, imageAspect);
            return rect;
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <param name="alphaBlend">if set to <c>true</c> [alpha blend].</param>
        /// <param name="imageAspect">The image aspect.</param>
        /// <param name="color">The color.</param>
        /// <param name="borderWidths">The border widths.</param>
        /// <param name="borderRadiuses">The border radiuses.</param>
        /// <returns></returns>
        public static Rect DrawTexture(Texture2D texture, ScaleMode scaleMode, bool alphaBlend, float imageAspect, Color color, Vector4 borderWidths, Vector4 borderRadiuses)
        {
            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return Rect.zero;
            }

            var rect = GetRectForTexture(texture);
            rect = rect.ForceBoth(texture.width, texture.height);

            GUI.DrawTexture(rect, texture, scaleMode, alphaBlend, imageAspect, color, borderWidths, borderRadiuses);
            // GUI.DrawTexture(rect.RoundValues(), texture, scaleMode, alphaBlend, imageAspect, color, borderWidths, borderRadiuses);
            return rect;
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <param name="alphaBlend">if set to <c>true</c> [alpha blend].</param>
        /// <param name="imageAspect">The image aspect.</param>
        /// <param name="color">The color.</param>
        /// <param name="borderWidths">The border widths.</param>
        /// <param name="borderRadiuses">The border radiuses.</param>
        /// <returns></returns>
        public static Rect DrawTexture(Vector2 size, Texture2D texture, ScaleMode scaleMode, bool alphaBlend, float imageAspect, Color color, Vector4 borderWidths, Vector4 borderRadiuses)
        {
            return DrawTexture(size.x, size.y, texture, scaleMode, alphaBlend, imageAspect, color, borderWidths, borderRadiuses);
        }

        /// <summary>
        /// Draws a texture.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="scaleMode">The scale mode.</param>
        /// <param name="alphaBlend">if set to <c>true</c> [alpha blend].</param>
        /// <param name="imageAspect">The image aspect.</param>
        /// <param name="color">The color.</param>
        /// <param name="borderWidths">The border widths.</param>
        /// <param name="borderRadiuses">The border radiuses.</param>
        /// <returns></returns>
        public static Rect DrawTexture(float width, float height, Texture2D texture, ScaleMode scaleMode, bool alphaBlend, float imageAspect, Color color, Vector4 borderWidths, Vector4 borderRadiuses)
        {
            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return Rect.zero;
            }

            var rect = GetRectForTexture(width, height);
            rect = rect.ForceBoth(width, height);

            GUI.DrawTexture(rect, texture, scaleMode, alphaBlend, imageAspect, color, borderWidths, borderRadiuses);
            // GUI.DrawTexture(rect.RoundValues(), texture, scaleMode, alphaBlend, imageAspect, color, borderWidths, borderRadiuses);
            return rect;
        }

        /// <summary>
        ///     Draws a GIF image.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="gif">The GIF.</param>
        public static void DrawGIF(float width, float height, UniGif.GifFile gif)
        {
            var texture = gif?.GetFirstFrame();

            if (texture == null)
                // Debug.LogWarning("Null texture passed!");
                return;

            var rect = GetRectForTexture(width, height);
            rect = rect.ForceBoth(width, height);

            gif.Draw(rect);
        }

        /// <summary>
        ///     Draws a GIF.
        /// </summary>
        /// <param name="gif">The GIF.</param>
        public static void DrawGIF(UniGif.GifFile gif)
        {
            var texture = gif.GetFirstFrame();

            if (texture == null)
            {
                Debug.LogWarning("Null texture passed!");
                return;
            }

            var rect = GetRectForTexture(texture);
            rect = rect.ForceBoth(texture.width, texture.height);

            gif.Draw(rect);
        }

        /// <summary>
        ///     Gets the rect for texture.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Rect GetRectForTexture(float width, float height)
        {
            //var content = new GUIContent(texture);
            var rect = GUILayoutUtility.GetRect(GUIContent.none, "label", GUILayout.Width(width), GUILayout.Height(height));

            return rect;
        }

        /// <summary>
        ///     Gets the rect for texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <returns></returns>
        public static Rect GetRectForTexture(Texture2D texture)
        {
            float width = texture.width;
            float height = texture.height;

            //var content = new GUIContent(texture);
            var rect = GUILayoutUtility.GetRect(GUIContent.none, "label", GUILayout.Width(width), GUILayout.Height(height));

            return rect;
        }

        /// <summary>
        ///     Draws the texture from object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <exception cref="ArgumentNullException">obj</exception>
        public static void DrawTextureFromObject(object obj, float width, float height, bool debug = false)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            Rect rect = default;

            if (obj is UniGif.GifFile)
            {
                var gif = obj as UniGif.GifFile;
                var texture = gif.GetFirstFrame();

                rect = GetRectForTexture(width, height);

                GlobalGUI.DrawGIF(rect, gif);
            }
            else if (obj is Texture2D)
            {
                var texture = obj as Texture2D;
                rect = GetRectForTexture(width, height);

                // Fixed undesired space
                // DrawTexture(width, height, texture);

                GUI.DrawTexture(rect, texture);
                // GUI.DrawTexture(rect.RoundValues(), texture);
            }

            if (debug) Debug.Log(rect);
        }

        // TODO: Implement all methods like this
        // TODO: Use GUILayout.ExpandWidth(true) when needed instead of doing complex calculations

        /// <summary>
        ///     Begins the scroll view.
        /// </summary>
        /// <param name="f_isEditor">if set to <c>true</c> [f is editor].</param>
        /// <param name="scrollPosition">The scroll position.</param>
        /// <param name="viewSize">Size of the view.</param>
        /// <param name="alwaysShowHorizontal">if set to <c>true</c> [always show horizontal].</param>
        /// <param name="alwaysShowVertical">if set to <c>true</c> [always show vertical].</param>
        /// <returns></returns>
        public static Vector2 BeginScrollView(bool f_isEditor, Vector2 scrollPosition, Vector2? viewSize = null,
            bool alwaysShowHorizontal = false, bool alwaysShowVertical = false)
        {
            float width = viewSize.GetValue(Vector2.left).x,
                height = viewSize.GetValue(Vector2.down).y;

            if (f_isEditor)
            {
#if UNITY_EDITOR
                if (width > -1 && height > -1)
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal,
                        alwaysShowVertical, GUILayout.Width(width), GUILayout.Height(height));
                else if (width > -1)
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal,
                        alwaysShowVertical, GUILayout.Width(width));
                else if (height > -1)
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal,
                        alwaysShowVertical, GUILayout.Height(height));
                else
                    scrollPosition =
                        EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical);
#endif
            }
            else
            {
                if (width > -1 && height > -1)
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical,
                        GUILayout.Width(width), GUILayout.Height(height));
                else if (width > -1)
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical,
                        GUILayout.Width(width));
                else if (height > -1)
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical,
                        GUILayout.Height(height));
                else
                    scrollPosition =
                        GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical);
            }

            return scrollPosition;
        }

        /// <summary>
        ///     Ends the scroll view.
        /// </summary>
        /// <param name="f_isEditor">if set to <c>true</c> [f is editor].</param>
        public static void EndScrollView(bool f_isEditor)
        {
            if (f_isEditor)
            {
#if UNITY_EDITOR
                EditorGUILayout.EndScrollView();
#endif
            }
            else
            {
                GUILayout.EndScrollView();
            }
        }

        public static Rect? BeginHorizontal(bool f_isEditor, params GUILayoutOption[] options)
        {
            if (f_isEditor)
            {
#if UNITY_EDITOR
                if (options != null)
                    return EditorGUILayout.BeginHorizontal(options);
                return EditorGUILayout.BeginHorizontal();
#endif
            }

            if (options != null)
                GUILayout.BeginHorizontal(options);
            else
                GUILayout.BeginHorizontal();

            return null;
        }

        public static void EndHorizontal(bool f_isEditor)
        {
            if (f_isEditor)
            {
#if UNITY_EDITOR
                EditorGUILayout.EndHorizontal();
#endif
            }
            else
            {
                GUILayout.EndHorizontal();
            }
        }

        public static Rect? BeginVertical(bool f_isEditor, params GUILayoutOption[] options)
        {
            if (f_isEditor)
            {
#if UNITY_EDITOR
                if (options != null)
                    return EditorGUILayout.BeginVertical(options);
                return EditorGUILayout.BeginVertical();
#endif
            }

            if (options != null)
                GUILayout.BeginVertical(options);
            else
                GUILayout.BeginVertical();

            return null;
        }

        public static void EndVertical(bool f_isEditor)
        {
            if (f_isEditor)
            {
#if UNITY_EDITOR
                EditorGUILayout.EndVertical();
#endif
            }
            else
            {
                GUILayout.EndVertical();
            }
        }

        public static void BeginScreenCenter()
        {
            BeginScreenCenter(ScreenCenterContent.None);
        }

        public static void BeginScreenCenter(ScreenCenterContent centerContent)
        {
            m_screenCenterContent = centerContent;

            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            switch (centerContent)
            {
                case ScreenCenterContent.Horizontal:
                    GUILayout.BeginHorizontal();
                    break;

                case ScreenCenterContent.Vertical:
                    GUILayout.BeginVertical();
                    break;
            }
        }

        public static void EndScreenCenter()
        {
            switch (m_screenCenterContent)
            {
                case ScreenCenterContent.Horizontal:
                    GUILayout.EndHorizontal();
                    break;

                case ScreenCenterContent.Vertical:
                    GUILayout.EndVertical();
                    break;
            }

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Begins the margin.
        /// </summary>
        /// <param name="margin">The margin.</param>
        /// <exception cref="ArgumentNullException">margin</exception>
        public static void BeginMargin(float margin)
        {
            if (margin == default || margin <= 0)
                throw new ArgumentNullException(nameof(margin));

            BeginMargin(new Vector2(margin, margin));
        }

        /// <summary>
        /// Ends the margin.
        /// </summary>
        /// <param name="margin">The margin.</param>
        /// <exception cref="ArgumentNullException">margin</exception>
        public static void EndMargin(float margin)
        {
            if (margin == default || margin <= 0)
                throw new ArgumentNullException(nameof(margin));

            EndMargin(new Vector2(margin, margin));
        }

        /// <summary>
        /// Begins the margin.
        /// </summary>
        /// <param name="margin">The margin.</param>
        /// <exception cref="ArgumentNullException">margin</exception>
        public static void BeginMargin(Vector2 margin)
        {
            if (margin == default)
                throw new ArgumentNullException(nameof(margin));

            BeginMargin(new RectOffset(
                (int)margin.x,
                (int)margin.x,
                (int)margin.y,
                (int)margin.y));
        }

        /// <summary>
        /// Ends the margin.
        /// </summary>
        /// <param name="margin">The margin.</param>
        /// <exception cref="ArgumentNullException">margin</exception>
        public static void EndMargin(Vector2 margin)
        {
            if (margin == default)
                throw new ArgumentNullException(nameof(margin));

            BeginMargin(new RectOffset(
                (int)margin.x,
                (int)margin.x,
                (int)margin.y,
                (int)margin.y));
        }

        /// <summary>
        /// Begins the margin.
        /// </summary>
        /// <param name="margin">The margin.</param>
        /// <exception cref="ArgumentNullException">margin</exception>
        public static void BeginMargin(RectOffset margin)
        {
            if (margin == default)
                throw new ArgumentNullException(nameof(margin));

            // Debug.Log("Starting margin: " + margin);

            GUILayout.BeginVertical();

            GUILayout.Space(margin.top);

            GUILayout.BeginHorizontal();

            GUILayout.Space(margin.left);
        }

        /// <summary>
        /// Ends the margin.
        /// </summary>
        /// <param name="margin">The margin.</param>
        /// <exception cref="ArgumentNullException">margin</exception>
        public static void EndMargin(RectOffset margin)
        {
            if (margin == default)
                throw new ArgumentNullException(nameof(margin));

            // Debug.Log("End margin: " + margin);

            GUILayout.Space(margin.right);

            GUILayout.EndHorizontal();

            GUILayout.Space(margin.bottom);

            GUILayout.EndVertical();
        }

        /// <summary>
        ///     Displays a text field.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="style">The style.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static string TextField(string str, GUIStyle style = null, params GUILayoutOption[] options)
        {
            return TextField(str, style, true, null, options);
        }

        /// <summary>
        ///     Displays a text field.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="style">The style.</param>
        /// <param name="showClearButton">if set to <c>true</c> [show clear button].</param>
        /// <param name="callback">The callback.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static string TextField(string str, GUIStyle style = null, bool showClearButton = true,
            Action callback = null, params GUILayoutOption[] options)
        {
            var searchRect = style != null
                ? GUILayoutUtility.GetRect(new GUIContent(str), style, options)
                : GUILayoutUtility.GetRect(new GUIContent(str), "textField", options);

            var e = Event.current;
            var buttonRect = new Rect(searchRect.x + searchRect.width - 12, searchRect.y + 1, 12, 12);

            var isHovering = buttonRect.Contains(e.mousePosition);
            var displayCloseButton = showClearButton && !string.IsNullOrEmpty(str);

            if (!isHovering)
            {
                if (style != null)
                    str = GUI.TextField(searchRect, str, style);
                else
                    str = GUI.TextField(searchRect, str);
            }
            else
            {
                GUI.Label(searchRect, str, new GUIStyle("textField") { normal = GUI.skin.textField.focused });
            }

            if (displayCloseButton)
            {
                GUI.Label(buttonRect, "x", GlobalGUIStyle.WithCenteredFontSize(10));

                if (isHovering && e.rawType == EventType.MouseUp)
                {
                    // Empty text field when click x
                    str = string.Empty;

                    callback?.Invoke();
                }
            }

            return str;
        }
    }
}