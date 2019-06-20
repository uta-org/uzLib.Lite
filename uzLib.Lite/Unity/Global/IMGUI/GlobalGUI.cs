using System;
using UnityGif;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace UnityEngine.Global.IMGUI
{
    /// <summary>
    ///     The Global GUI (used in Editor and in-game)
    /// </summary>
    public static class GlobalGUI
    {
        /// <summary>
        ///     Gets the containing rect.
        /// </summary>
        /// <param name="originalSize">Size of the original.</param>
        /// <returns></returns>
        public static Rect GetContainingRect(object editorInstace, Rect? originalSize = null)
        {
            // Is editor
            if (!originalSize.HasValue)
            {
#if UNITY_EDITOR
                // Fixing inconsistent values from EditorWindow.focusedWindow
                if (editorInstace != null)
                    return (editorInstace as EditorWindow).position;

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

        /// <summary>
        ///     Draws the GIF.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="gif">The GIF.</param>
        public static bool DrawGIF(Rect rect, UniGif.GifFile gif)
        {
            if (gif == null) throw new ArgumentNullException(nameof(gif), "Null gif!");

            return gif.Draw(rect);
        }

        /// <summary>
        ///     Draws the texture from object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="rect">The rect.</param>
        /// <param name="drawAction">The draw action.</param>
        /// <exception cref="ArgumentNullException">
        ///     obj
        ///     or
        ///     drawAction
        /// </exception>
        public static void DrawTextureFromObject(this object obj, Rect rect, Action<Texture2D> drawAction)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (drawAction == null) throw new ArgumentNullException(nameof(drawAction));

            if (obj is UniGif.GifFile)
                DrawGIF(rect, obj as UniGif.GifFile);
            else if (obj is Texture2D) drawAction(obj as Texture2D);
        }

        /// <summary>
        ///     Draws the texture from object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="rect">The rect.</param>
        /// <exception cref="ArgumentNullException">obj</exception>
        public static bool DrawTextureFromObject(this object obj, Rect rect)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            // Note: Don't use GlobalGUILayout.DrawTexture due to a problem with GetRect and BeginArea (in Texture2D case)

            if (obj is UniGif.GifFile) return DrawGIF(rect, obj as UniGif.GifFile);

            if (obj is Texture2D)
            {
                GUI.DrawTexture(rect, obj as Texture2D);
                return true;
            }

            Debug.LogWarning("Unrecognized object type!");
            return false;
        }

        /// <summary>
        ///     Displays a button.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="str">The string.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static bool Button(Rect r, string str, GUIStyle style = null)
        {
            return Button(r, str, false, out var isHovering, style);
        }

        /// <summary>
        ///     Displays a button.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="str">The string.</param>
        /// <param name="isHovering">if set to <c>true</c> [is hovering].</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static bool Button(Rect r, string str, out bool isHovering, GUIStyle style = null)
        {
            return Button(r, str, false, out isHovering, style);
        }

        /// <summary>
        ///     Displays a button.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="str">The string.</param>
        /// <param name="f_isEditor">if set to <c>true</c> [f is editor].</param>
        /// <param name="style">The style.</param>
        /// <param name="mousePos">The mouse position.</param>
        /// <returns></returns>
        public static bool Button(Rect r, string str, bool f_isEditor, out bool isHovering, GUIStyle style = null,
            Func<Vector2> mousePos = null)
        {
            var mPos = mousePos?.Invoke() ?? GlobalGUIUtility.GetMousePosition(f_isEditor);

            if (style != null)
                GUI.Label(r, str, style);
            else
                GUI.Label(r, str);

            isHovering = r.Contains(mPos);
            return isHovering && Event.current.rawType == EventType.MouseUp;
        }

        /// <summary>
        ///     Displays a text field.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="str">The string.</param>
        /// <param name="style">The style.</param>
        /// <param name="showClearButton">if set to <c>true</c> [show clear button].</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static string TextField(Rect rect, string str, GUIStyle style = null, bool showClearButton = true,
            Action callback = null)
        {
            str = style != null ? GUI.TextField(rect, str, style) : GUI.TextField(rect, str);

            if (showClearButton && !string.IsNullOrEmpty(str) && Button(
                    new Rect(rect.x + rect.width - 12, rect.y + 1, 12, 12), "x",
                    GlobalGUIStyle.WithCenteredFontSize(10)))
            {
                // Empty text field when click x
                str = string.Empty;
                callback?.Invoke();
            }

            return str;
        }
    }
}