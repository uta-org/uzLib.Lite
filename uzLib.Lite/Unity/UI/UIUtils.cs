using System;
using RectHelper = UnityEngine.Extensions.RectExtensions;
using UnityEngine.Global.IMGUI;
using uzLib.Lite.Extensions;

namespace UnityEngine.UI
{
    /// <summary>
    ///     The Screen Corner enum
    /// </summary>
    public enum ScreenCorner
    {
        /// <summary>
        ///     The top right corner
        /// </summary>
        TopRight,

        /// <summary>
        ///     The top left corner
        /// </summary>
        TopLeft,

        /// <summary>
        ///     The bottom right corner
        /// </summary>
        BottomRight,

        /// <summary>
        ///     The bottom left corner
        /// </summary>
        BottomLeft
    }

    /// <summary>
    ///     The UI Utils
    /// </summary>
    public static class UIUtils
    {
        /// <summary>
        /// THe Button delegate.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public delegate bool ButtonDelegate(string text, params GUILayoutOption[] options);

        /// <summary>
        /// The Custom Window delegate.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="clientRect">The client rect.</param>
        /// <param name="func">The function.</param>
        /// <param name="text">The text.</param>
        /// <param name="closeAction">The close action.</param>
        /// <returns></returns>
        public delegate Rect CustomWindowDelegate(int id, Rect clientRect, GUI.WindowFunction func, string text,
            Action closeAction);

        /// <summary>
        /// Gets the close style.
        /// </summary>
        /// <value>
        /// The close style.
        /// </value>
        private static GUIStyle CloseStyle => new GUIStyle("label")
        {
            normal = new GUIStyleState { textColor = Color.black },
            fontSize = 16,
            fontStyle = FontStyle.Bold
        };

        /// <summary>
        ///     Gets the screen rect.
        /// </summary>
        /// <value>
        ///     The screen rect.
        /// </value>
        public static Rect ScreenRect => new Rect(0, 0, Screen.width, Screen.height);

        /// <summary>
        /// Display a custom window.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="clientRect">The client rect.</param>
        /// <param name="func">The function.</param>
        /// <param name="text">The text.</param>
        /// <param name="closeAction">The close action.</param>
        /// <returns></returns>
        public static Rect CustomWindow(int id, Rect clientRect, GUI.WindowFunction func, string text, Action closeAction)
        {
            void LocalWindow(int _)
            {
                if (GUI.Button(new Rect(clientRect.width - 36, -5, 32, 24), "x", CloseStyle))
                    closeAction();

                func(_);
            }

            return GUI.Window(id, clientRect, LocalWindow, text);
        }

        /// <summary>
        /// Draws the texture with tooltip as button.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="style">The style.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static bool DrawTextureWithTooltipAsButton(Texture2D texture, string tooltip, GUIStyle style = null, params GUILayoutOption[] options)
        {
            GUILayout.Box(texture, GUI.skin.label, options);
            var rect = GUILayoutUtility.GetLastRect();

            Event e = Event.current;
            bool hover = rect.Contains(e.mousePosition);
            if (hover)
            {
                GUIContent content = new GUIContent(tooltip);

                (style ?? GUI.skin.box).alignment = TextAnchor.MiddleCenter;

                // Compute how large the button needs to be.
                Vector2? size = style?.CalcSize(content);

                if (size.HasValue)
                    GUI.Box(new Rect(e.mousePosition + Vector2.right * 20, size.Value), tooltip, style);
            }

            return hover && e.type == EventType.MouseDown && e.clickCount == 1;
        }

        /// <summary>
        /// Draws the texture with tooltip as button.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static bool DrawTextureWithTooltipAsButton(Rect rect, Texture2D texture, string tooltip, GUIStyle style = null)
        {
            GUI.DrawTexture(rect, texture);

            Event e = Event.current;
            bool hover = rect.Contains(e.mousePosition);
            if (hover)
            {
                GUIContent content = new GUIContent(tooltip);

                (style ?? GUI.skin.box).alignment = TextAnchor.MiddleCenter;

                // Compute how large the button needs to be.
                Vector2? size = style?.CalcSize(content);

                if (size.HasValue)
                    GUI.Box(new Rect(e.mousePosition + Vector2.right * 20, size.Value), tooltip, style);
            }

            return hover && e.type == EventType.MouseDown && e.clickCount == 1;
        }

        /// <summary>
        /// Draws the texture with tooltip.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="style">The style.</param>
        public static void DrawTextureWithTooltip(Rect rect, Texture2D texture, string tooltip, GUIStyle style = null)
        {
            GUI.DrawTexture(rect, texture);

            Event e = Event.current;
            if (rect.Contains(e.mousePosition))
            {
                GUIContent content = new GUIContent(tooltip);

                (style ?? GUI.skin.box).alignment = TextAnchor.MiddleCenter;

                // Compute how large the button needs to be.
                Vector2? size = style?.CalcSize(content);

                if (size.HasValue)
                    GUI.Box(new Rect(e.mousePosition + Vector2.right * 20, size.Value), tooltip, style);
            }
        }

        /// <summary>
        ///     Gets the corner rect.
        /// </summary>
        /// <param name="corner">The corner.</param>
        /// <param name="size">The size.</param>
        /// <param name="padding">The padding.</param>
        /// <returns></returns>
        public static Rect GetCornerRect(ScreenCorner corner, Vector2 size, Vector2? padding = null)
        {
            return GetCornerRect(corner, size.x, size.y, padding);
        }

        /// <summary>
        ///     Gets the corner rect.
        /// </summary>
        /// <param name="corner">The corner.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="padding">The padding.</param>
        /// <returns></returns>
        public static Rect GetCornerRect(ScreenCorner corner, float width, float height, Vector2? padding = null)
        {
            float padX = 0,
                padY = 0;

            if (padding != null)
            {
                padX = padding.Value.x;
                padY = padding.Value.y;
            }

            switch (corner)
            {
                case ScreenCorner.TopLeft:
                    return new Rect(padX, padY, width, height);

                case ScreenCorner.TopRight:
                    return new Rect(Screen.width - (width + padX), padY, width, height);

                case ScreenCorner.BottomLeft:
                    return new Rect(padX, Screen.height - (height + padY), width, height);

                case ScreenCorner.BottomRight:
                    return new Rect(Screen.width - (width + padX), Screen.height - (height + padY), width, height);
            }

            return default;
        }

        /// <summary>
        ///     Gets the centered rect.
        /// </summary>
        /// <param name="xSize">Size of the x.</param>
        /// <param name="ySize">Size of the y.</param>
        /// <returns></returns>
        public static Rect GetCenteredRect(float xSize, float ySize)
        {
            return GetCenteredRect(new Vector2(xSize, ySize));
        }

        /// <summary>
        ///     Gets the centered rect.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static Rect GetCenteredRect(Vector2 size)
        {
            var pos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            pos -= size * 0.5f;

            return new Rect(pos, size);
        }

        /// <summary>
        ///     Gets the centered rect perc.
        /// </summary>
        /// <param name="sizeInScreenPercentage">The size in screen percentage.</param>
        /// <returns></returns>
        public static Rect GetCenteredRectPerc(Vector2 sizeInScreenPercentage)
        {
            return GetCenteredRect(new Vector2(Screen.width * sizeInScreenPercentage.x,
                Screen.height * sizeInScreenPercentage.y));
        }

        /// <summary>
        ///     Calculates the content of the screen size for.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static Vector2 CalcScreenSizeForContent(GUIContent content, GUIStyle style)
        {
            return style.CalcScreenSize(style.CalcSize(content));
        }

        /// <summary>
        ///     Calculates the screen size for text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static Vector2 CalcScreenSizeForText(string text, GUIStyle style)
        {
            return CalcScreenSizeForContent(new GUIContent(text), style);
        }

        /// <summary>
        ///     Buttons the calculated size of the with.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static bool ButtonWithCalculatedSize(string text, ButtonDelegate buttonDelegate = null)
        {
            var size = CalcScreenSizeForText(text, GUI.skin.button);

            return buttonDelegate?.Invoke(text, GUILayout.Width(size.x), GUILayout.Height(size.y)) ?? GUILayout.Button(text, GUILayout.Width(size.x), GUILayout.Height(size.y));
        }

        /// <summary>
        ///     Buttons the color of the with.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static bool ButtonWithColor(Rect rect, string text, Color color)
        {
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = color;

            var result = GUI.Button(rect, text);

            GUI.backgroundColor = oldColor;

            return result;
        }

        /// <summary>
        ///     Draws the rect.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="color">The color.</param>
        /// <param name="content">The content.</param>
        public static void DrawRect(Rect position, Color color, GUIContent content = null)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;

            GUI.Box(position, content ?? GUIContent.none, styleWithBackground);
            GUI.backgroundColor = backgroundColor;
        }

        #region "Missing GUIStyles copy"

        private static GUIStyle s_styleWithBackground;

        private static GUIStyle styleWithBackground =>
            s_styleWithBackground ??
            (s_styleWithBackground = new GUIStyle { normal = { background = Texture2D.whiteTexture } });

        #endregion "Missing GUIStyles copy"

        /// <summary>
        ///     Draws the bar.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="fillPerc">The fill perc.</param>
        /// <param name="fillColor">Color of the fill.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="borderWidth">Width of the border.</param>
        public static void DrawBar(Rect rect, float fillPerc, Color fillColor, Color backgroundColor, float borderWidth)
        {
            fillPerc = Mathf.Clamp01(fillPerc);

            var fillRect = rect;
            fillRect.position += Vector2.one * borderWidth;
            fillRect.size -= borderWidth * 2 * Vector2.one;

            // first fill with black - that will be the border
            DrawRect(rect, Color.black);

            // fill with background
            DrawRect(fillRect, backgroundColor);

            // draw filled part
            fillRect.width *= fillPerc;
            DrawRect(fillRect, fillColor);
        }

        /// <summary>
        ///     Tabses the control.
        /// </summary>
        /// <param name="currentTabIndex">Index of the current tab.</param>
        /// <param name="tabNames">The tab names.</param>
        /// <returns></returns>
        public static int TabsControl(int currentTabIndex, params string[] tabNames)
        {
            return GUILayout.Toolbar(currentTabIndex, tabNames);
        }

        /// <summary>
        ///     Gets the rect for bar as billboard.
        /// </summary>
        /// <param name="worldPos">The world position.</param>
        /// <param name="worldWidth">Width of the world.</param>
        /// <param name="worldHeight">Height of the world.</param>
        /// <param name="cam">The cam.</param>
        /// <returns></returns>
        public static Rect GetRectForBarAsBillboard(Vector3 worldPos, float worldWidth, float worldHeight, Camera cam)
        {
            var camRight = cam.transform.right;
            //	Vector3 camUp = cam.transform.up;

            //			Vector3 upperLeft = worldPos - camRight * worldWidth * 0.5f + camUp * worldHeight * 0.5f;
            //			Vector3 upperRight = upperLeft + camRight * worldWidth;
            //			Vector3 lowerLeft = upperLeft - camUp * worldHeight;
            //			Vector3 lowerRight = lowerLeft + camRight * worldWidth;

            var leftWorld = worldPos - camRight * worldWidth * 0.5f;
            var rightWorld = worldPos + camRight * worldWidth * 0.5f;

            var leftScreen = cam.WorldToScreenPoint(leftWorld);
            var rightScreen = cam.WorldToScreenPoint(rightWorld);

            if (leftScreen.z < 0 || rightScreen.z < 0) return Rect.zero;

            // transform to gui coordinates
            leftScreen.y = Screen.height - leftScreen.y;
            rightScreen.y = Screen.height - rightScreen.y;

            var screenWidth = rightScreen.x - leftScreen.x;
            var screenHeight = screenWidth * worldHeight / worldWidth;

            return new Rect(new Vector2(leftScreen.x, leftScreen.y - screenHeight * 0.5f),
                new Vector2(screenWidth, screenHeight));
        }

        /// <summary>
        ///     Centereds the label.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="text">The text.</param>
        public static void CenteredLabel(Vector2 pos, string text)
        {
            var size = CalcScreenSizeForText(text, GUI.skin.label);

            GUI.Label(new Rect(pos - size * 0.5f, size), text);
        }

        /// <summary> Draws the texture flipped around Y axis. </summary>
        public static void DrawTextureWithYFlipped(Rect rect, Texture2D tex)
        {
            var savedMatrix = GUI.matrix;

            GUIUtility.ScaleAroundPivot(new Vector2(1, -1), rect.center);

            GUI.DrawTexture(rect, tex);

            GUI.matrix = savedMatrix;
        }

        /// <summary>
        ///     Draws the items in a row perc.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="drawItem">The draw item.</param>
        /// <param name="items">The items.</param>
        /// <param name="widthPercs">The width percs.</param>
        /// <returns></returns>
        public static Rect DrawItemsInARowPerc(Rect rect, Action<Rect, string> drawItem, string[] items,
            float[] widthPercs)
        {
            var itemRect = rect;
            var x = rect.position.x;

            for (var i = 0; i < items.Length; i++)
            {
                var width = widthPercs[i] * rect.width;

                itemRect.position = new Vector2(x, itemRect.position.y);
                itemRect.width = width;

                drawItem(itemRect, items[i]);

                x += width;
            }

            rect.position += new Vector2(x, 0f);
            rect.width -= x;
            return rect;
        }

        /// <summary>
        ///     Draws the items in a row.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="drawItem">The draw item.</param>
        /// <param name="items">The items.</param>
        /// <param name="widths">The widths.</param>
        /// <returns></returns>
        public static Rect DrawItemsInARow(Rect rect, Action<Rect, string> drawItem, string[] items, float[] widths)
        {
            var widthPercs = new float[widths.Length];
            for (var i = 0; i < widths.Length; i++) widthPercs[i] = widths[i] / rect.width;

            return DrawItemsInARowPerc(rect, drawItem, items, widthPercs);
        }

        /// <summary>
        ///     Gets the next rect in a row perc.
        /// </summary>
        /// <param name="rowRect">The row rect.</param>
        /// <param name="currentRectIndex">Index of the current rect.</param>
        /// <param name="spacing">The spacing.</param>
        /// <param name="widthPercs">The width percs.</param>
        /// <returns></returns>
        public static Rect GetNextRectInARowPerc(Rect rowRect, ref int currentRectIndex, float spacing,
            params float[] widthPercs)
        {
            var x = rowRect.position.x;

            for (var i = 0; i < currentRectIndex; i++)
            {
                x += widthPercs[i] * rowRect.width;
                x += spacing;
            }

            var width = widthPercs[currentRectIndex] * rowRect.width;
            currentRectIndex++;

            return new Rect(x, rowRect.position.y, width, rowRect.height);
        }

        /// <summary>
        ///     Gets the next rect in a row.
        /// </summary>
        /// <param name="rowRect">The row rect.</param>
        /// <param name="currentRectIndex">Index of the current rect.</param>
        /// <param name="spacing">The spacing.</param>
        /// <param name="widths">The widths.</param>
        /// <returns></returns>
        public static Rect GetNextRectInARow(Rect rowRect, ref int currentRectIndex, float spacing,
            params float[] widths)
        {
            var widthPercs = new float[widths.Length];
            for (var i = 0; i < widths.Length; i++) widthPercs[i] = widths[i] / rowRect.width;

            return GetNextRectInARowPerc(rowRect, ref currentRectIndex, spacing, widthPercs);
        }

        /// <summary>
        ///     Draws the paged view numbers.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="numPages">The number pages.</param>
        /// <returns></returns>
        public static int DrawPagedViewNumbers(Rect rect, int currentPage, int numPages)
        {
            var resultingPage = currentPage;

            var spacing = 1f;
            rect.width = 25f;

            if (GUI.Button(rect, "<")) resultingPage--;
            rect.position += new Vector2(rect.width + spacing, 0f);

            for (var i = 0; i < numPages; i++)
            {
                var style = currentPage == i + 1 ? GUI.skin.box : GUI.skin.button;
                if (GUI.Button(rect, (i + 1).ToString(), style)) resultingPage = i + 1;

                rect.position += new Vector2(rect.width + spacing, 0f);
            }

            if (GUI.Button(rect, ">")) resultingPage++;
            rect.position += new Vector2(rect.width + spacing, 0f);

            resultingPage = Mathf.Clamp(resultingPage, 1, numPages);

            return resultingPage;
        }

        /// <summary>
        ///     Draws the paged view numbers.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="totalNumItems">The total number items.</param>
        /// <param name="numItemsPerPage">The number items per page.</param>
        /// <returns></returns>
        public static int DrawPagedViewNumbers(Rect rect, int currentPage, int totalNumItems, int numItemsPerPage)
        {
            var numPages = Mathf.CeilToInt(totalNumItems / (float)numItemsPerPage);
            return DrawPagedViewNumbers(rect, currentPage, numPages);
        }

        /// <summary>
        ///     Gets the style with margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithMargin(string style, RectOffset offset)
        {
            return new GUIStyle(style) { margin = offset };
        }

        /// <summary>
        ///     Gets the style with margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithMargin(GUIStyle style, RectOffset offset)
        {
            style.margin = offset;

            return style;
        }

        /// <summary>
        ///     Gets the style with margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="top">The top.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithMargin(string style, int left, int right, int top, int bottom)
        {
            return new GUIStyle(style) { margin = RectHelper.CreateOffset(left, right, top, bottom) };
        }

        /// <summary>
        ///     Gets the style with margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="top">The top.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithMargin(GUIStyle style, int left, int right, int top, int bottom)
        {
            style.margin = RectHelper.CreateOffset(left, right, top, bottom);

            return style;
        }

        /// <summary>
        ///     Gets the style with margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="horizontal">The horizontal.</param>
        /// <param name="vertical">The vertical.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithMargin(string style, int horizontal, int vertical)
        {
            return new GUIStyle(style) { margin = RectHelper.CreateOffset(horizontal, horizontal, vertical, vertical) };
        }

        /// <summary>
        ///     Gets the style with margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="horizontal">The horizontal.</param>
        /// <param name="vertical">The vertical.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithMargin(GUIStyle style, int horizontal, int vertical)
        {
            style.margin = RectHelper.CreateOffset(horizontal, horizontal, vertical, vertical);

            return style;
        }

        /// <summary>
        ///     Gets the style with horizontal margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithHorizontalMargin(string style, int left, int right)
        {
            return new GUIStyle(style) { margin = RectHelper.CreateOffset(left, right, 0, 0) };
        }

        /// <summary>
        ///     Gets the style with horizontal margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithHorizontalMargin(GUIStyle style, int left, int right)
        {
            style.margin = RectHelper.CreateOffset(left, right, 0, 0);

            return style;
        }

        /// <summary>
        ///     Gets the style with vertical margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="top">The top.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithVerticalMargin(string style, int top, int bottom)
        {
            return new GUIStyle(style) { margin = RectHelper.CreateOffset(0, 0, top, bottom) };
        }

        /// <summary>
        ///     Gets the style with vertical margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="top">The top.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithVerticalMargin(GUIStyle style, int top, int bottom)
        {
            style.margin = RectHelper.CreateOffset(0, 0, top, bottom);

            return style;
        }

        /// <summary>
        ///     Gets the style with upper margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithUpperMargin(string style, int left, int top)
        {
            return new GUIStyle(style) { margin = RectHelper.CreateOffset(left, 0, top, 0) };
        }

        /// <summary>
        ///     Gets the style with upper margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithUpperMargin(GUIStyle style, int left, int top)
        {
            style.margin = RectHelper.CreateOffset(left, 0, top, 0);

            return style;
        }

        /// <summary>
        ///     Gets the style with bottom margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithBottomMargin(string style, int right, int bottom)
        {
            return new GUIStyle(style) { margin = RectHelper.CreateOffset(0, right, 0, bottom) };
        }

        /// <summary>
        ///     Gets the style with bottom margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithBottomMargin(GUIStyle style, int right, int bottom)
        {
            style.margin = RectHelper.CreateOffset(0, right, 0, bottom);

            return style;
        }

        /// <summary>
        ///     Gets the style with left margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="left">The left.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithLeftMargin(string style, int left)
        {
            return new GUIStyle(style) { margin = RectHelper.CreateOffset(left, 0, 0, 0) };
        }

        /// <summary>
        ///     Gets the style with left margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="left">The left.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithLeftMargin(GUIStyle style, int left)
        {
            style.margin = RectHelper.CreateOffset(left, 0, 0, 0);

            return style;
        }

        /// <summary>
        ///     Gets the style with right margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithRightMargin(string style, int right)
        {
            return new GUIStyle(style) { margin = RectHelper.CreateOffset(0, right, 0, 0) };
        }

        /// <summary>
        ///     Gets the style with right margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithRightMargin(GUIStyle style, int right)
        {
            style.margin = RectHelper.CreateOffset(0, right, 0, 0);

            return style;
        }

        /// <summary>
        ///     Gets the style with top margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithTopMargin(string style, int top)
        {
            return new GUIStyle(style) { margin = RectHelper.CreateOffset(0, 0, top, 0) };
        }

        /// <summary>
        ///     Gets the style with top margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithTopMargin(GUIStyle style, int top)
        {
            style.margin = RectHelper.CreateOffset(0, 0, top, 0);

            return style;
        }

        /// <summary>
        ///     Gets the style with bottom margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithBottomMargin(string style, int bottom)
        {
            return new GUIStyle(style) { margin = RectHelper.CreateOffset(0, 0, 0, bottom) };
        }

        /// <summary>
        ///     Gets the style with bottom margin.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static GUIStyle GetStyleWithBottomMargin(GUIStyle style, int bottom)
        {
            style.margin = RectHelper.CreateOffset(0, 0, 0, bottom);

            return style;
        }

        /// <summary>
        ///     Gets the string content fitting to the desired width.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="style">The style.</param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">width - Expected width is smaller than no content width!</exception>
        public static string GetFittingContent(string str, GUIStyle style, float width)
        {
            var noContentWidth = style.CalcSize(GUIContent.none).x;

            if (noContentWidth > width)
                throw new ArgumentOutOfRangeException("width", @"Expected width is smaller than no content width!");

            var curWidth = style.CalcSize(new GUIContent(str)).x;
            var substring = str.Length - 1;

            while (curWidth > width && substring > 0)
            {
                str = str.Substring(0, substring);
                curWidth = style.CalcSize(new GUIContent(str)).x;

                --substring;
            }

            return str;
        }

        /// <summary>
        ///     Gets the string content fitting to the desired width.
        /// </summary>
        /// <param name="str1">The original string.</param>
        /// <param name="str2">The string to cut inside the original string.</param>
        /// <param name="style">The style.</param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        public static string GetFittingContent(string str1, string str2, GUIStyle style, float width)
        {
            // Get cut string
            var str = GetFittingContent(str1, style, width);
            if (str != str1)
            {
                var diffLng = str1.Length - str.Length;
                return str1.Replace(str2, str.Cut(diffLng));
            }

            return str;
        }

        /// <summary>
        /// Shadows the content.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="shadowRect">The shadow rect.</param>
        /// <param name="backgroundTexture">The background texture.</param>
        /// <exception cref="ArgumentNullException">
        /// size
        /// or
        /// shadowRect
        /// or
        /// backgroundTexture
        /// </exception>
        public static void ShadowContent(Vector2 size, Rect shadowRect, Texture2D backgroundTexture)
        {
            if (size == default)
                throw new ArgumentNullException(nameof(size));

            if (shadowRect == default)
                throw new ArgumentNullException(nameof(shadowRect));

            if (backgroundTexture == null)
                throw new ArgumentNullException(nameof(backgroundTexture));

            if (shadowRect.xMin > 0)
                GUI.DrawTexture(new Rect(0, 0, size.x, shadowRect.yMin), backgroundTexture);

            if (shadowRect.yMin > 0)
                GUI.DrawTexture(new Rect(0, shadowRect.yMin, shadowRect.xMin, size.y), backgroundTexture);

            if (shadowRect.xMax < size.x)
                GUI.DrawTexture(new Rect(shadowRect.xMin, shadowRect.yMax, size.x - shadowRect.xMin, size.y - shadowRect.yMax), backgroundTexture);

            if (shadowRect.yMax < size.y)
                GUI.DrawTexture(new Rect(shadowRect.xMax, shadowRect.yMin, size.x - shadowRect.xMax, shadowRect.height), backgroundTexture);
        }
    }
}