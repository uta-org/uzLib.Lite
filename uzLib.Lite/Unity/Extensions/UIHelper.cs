using System.Collections.Generic;
using UnityEngine.UI;
using uzLib.Lite.ExternalCode.Extensions;

namespace UnityEngine.Extensions
{
    /// <summary>
    /// The UI Helper
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// The style with background
        /// </summary>
        private static GUIStyle styleWithBackground = new GUIStyle();

        /// <summary>
        /// The centered label style
        /// </summary>
        private static GUIStyle s_centeredLabelStyle = null;

        /// <summary>
        /// Gets the centered label style.
        /// </summary>
        /// <value>
        /// The centered label style.
        /// </value>
        public static GUIStyle CenteredLabelStyle
        {
            get
            {
                if (null == s_centeredLabelStyle)
                    s_centeredLabelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                return s_centeredLabelStyle;
            }
        }

        /// <summary>
        /// Gets the screen rect.
        /// </summary>
        /// <value>
        /// The screen rect.
        /// </value>
        public static Rect ScreenRect { get { return new Rect(0, 0, Screen.width, Screen.height); } }

        /// <summary>
        /// Gets the corner rect.
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
        /// Gets the corner rect.
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

            return default(Rect);
        }

        /// <summary>
        /// Gets the centered rect.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static Rect GetCenteredRect(Vector2 size)
        {
            Vector2 pos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            pos -= size * 0.5f;

            return new Rect(pos, size);
        }

        /// <summary>
        /// Gets the centered rect perc.
        /// </summary>
        /// <param name="sizeInScreenPercentage">The size in screen percentage.</param>
        /// <returns></returns>
        public static Rect GetCenteredRectPerc(Vector2 sizeInScreenPercentage)
        {
            return GetCenteredRect(new Vector2(Screen.width * sizeInScreenPercentage.x, Screen.height * sizeInScreenPercentage.y));
        }

        /// <summary>
        /// Calculates the content of the screen size for.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static Vector2 CalcScreenSizeForContent(GUIContent content, GUIStyle style)
        {
            return style.CalcScreenSize(style.CalcSize(content));
        }

        /// <summary>
        /// Calculates the screen size for text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static Vector2 CalcScreenSizeForText(string text, GUIStyle style)
        {
            return CalcScreenSizeForContent(new GUIContent(text), style);
        }

        /// <summary>
        /// Buttons the calculated size of the with.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static bool ButtonWithCalculatedSize(string text)
        {
            Vector2 size = CalcScreenSizeForText(text, GUI.skin.button);

            return GUILayout.Button(text, GUILayout.Width(size.x), GUILayout.Height(size.y));
        }

        /// <summary>
        /// Buttons the color of the with.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static bool ButtonWithColor(Rect rect, string text, Color color)
        {
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = color;

            bool result = GUI.Button(rect, text);

            GUI.backgroundColor = oldColor;

            return result;
        }

        /// <summary>
        /// Draws the rect.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="color">The color.</param>
        /// <param name="content">The content.</param>
        public static void DrawRect(Rect position, Color color, GUIContent content = null)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            styleWithBackground.normal.background = Texture2D.whiteTexture;
            GUI.Box(position, content ?? GUIContent.none, styleWithBackground);
            GUI.backgroundColor = backgroundColor;
        }

        /// <summary>
        /// Draws the bar.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="fillPerc">The fill perc.</param>
        /// <param name="fillColor">Color of the fill.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="borderWidth">Width of the border.</param>
        public static void DrawBar(Rect rect, float fillPerc, Color fillColor, Color backgroundColor, float borderWidth)
        {
            fillPerc = Mathf.Clamp01(fillPerc);

            Rect fillRect = rect;
            fillRect.position += Vector2.one * borderWidth;
            fillRect.size -= Vector2.one * borderWidth * 2;

            // first fill with black - that will be the border
            DrawRect(rect, Color.black);

            // fill with background
            DrawRect(fillRect, backgroundColor);

            // draw filled part
            fillRect.width *= fillPerc;
            DrawRect(fillRect, fillColor);
        }

        /// <summary>
        /// Tabses the control.
        /// </summary>
        /// <param name="currentTabIndex">Index of the current tab.</param>
        /// <param name="tabNames">The tab names.</param>
        /// <returns></returns>
        public static int TabsControl(int currentTabIndex, params string[] tabNames)
        {
            return GUILayout.Toolbar(currentTabIndex, tabNames);
        }

        /// <summary>
        /// Gets the rect for bar as billboard.
        /// </summary>
        /// <param name="worldPos">The world position.</param>
        /// <param name="worldWidth">Width of the world.</param>
        /// <param name="worldHeight">Height of the world.</param>
        /// <param name="cam">The cam.</param>
        /// <returns></returns>
        public static Rect GetRectForBarAsBillboard(Vector3 worldPos, float worldWidth, float worldHeight, Camera cam)
        {
            Vector3 camRight = cam.transform.right;
            //	Vector3 camUp = cam.transform.up;

            //			Vector3 upperLeft = worldPos - camRight * worldWidth * 0.5f + camUp * worldHeight * 0.5f;
            //			Vector3 upperRight = upperLeft + camRight * worldWidth;
            //			Vector3 lowerLeft = upperLeft - camUp * worldHeight;
            //			Vector3 lowerRight = lowerLeft + camRight * worldWidth;

            Vector3 leftWorld = worldPos - camRight * worldWidth * 0.5f;
            Vector3 rightWorld = worldPos + camRight * worldWidth * 0.5f;

            Vector3 leftScreen = cam.WorldToScreenPoint(leftWorld);
            Vector3 rightScreen = cam.WorldToScreenPoint(rightWorld);

            if (leftScreen.z < 0 || rightScreen.z < 0)
                return Rect.zero;

            // transform to gui coordinates
            leftScreen.y = Screen.height - leftScreen.y;
            rightScreen.y = Screen.height - rightScreen.y;

            float screenWidth = rightScreen.x - leftScreen.x;
            float screenHeight = screenWidth * worldHeight / worldWidth;

            return new Rect(new Vector2(leftScreen.x, leftScreen.y - screenHeight * 0.5f), new Vector2(screenWidth, screenHeight));
        }

        /// <summary>
        /// Centereds the label.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="text">The text.</param>
        public static void CenteredLabel(Vector2 pos, string text)
        {
            Vector2 size = CalcScreenSizeForText(text, GUI.skin.label);

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
        /// Draws the items in a row perc.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="drawItem">The draw item.</param>
        /// <param name="items">The items.</param>
        /// <param name="widthPercs">The width percs.</param>
        /// <returns></returns>
        public static Rect DrawItemsInARowPerc(Rect rect, System.Action<Rect, string> drawItem, string[] items, float[] widthPercs)
        {
            Rect itemRect = rect;
            float x = rect.position.x;

            for (int i = 0; i < items.Length; i++)
            {
                float width = widthPercs[i] * rect.width;

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
        /// Draws the items in a row.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="drawItem">The draw item.</param>
        /// <param name="items">The items.</param>
        /// <param name="widths">The widths.</param>
        /// <returns></returns>
        public static Rect DrawItemsInARow(Rect rect, System.Action<Rect, string> drawItem, string[] items, float[] widths)
        {
            float[] widthPercs = new float[widths.Length];
            for (int i = 0; i < widths.Length; i++)
            {
                widthPercs[i] = widths[i] / rect.width;
            }

            return DrawItemsInARowPerc(rect, drawItem, items, widthPercs);
        }

        /// <summary>
        /// Gets the next rect in a row perc.
        /// </summary>
        /// <param name="rowRect">The row rect.</param>
        /// <param name="currentRectIndex">Index of the current rect.</param>
        /// <param name="spacing">The spacing.</param>
        /// <param name="widthPercs">The width percs.</param>
        /// <returns></returns>
        public static Rect GetNextRectInARowPerc(Rect rowRect, ref int currentRectIndex, float spacing, params float[] widthPercs)
        {
            float x = rowRect.position.x;

            for (int i = 0; i < currentRectIndex; i++)
            {
                x += widthPercs[i] * rowRect.width;
                x += spacing;
            }

            float width = widthPercs[currentRectIndex] * rowRect.width;
            currentRectIndex++;

            return new Rect(x, rowRect.position.y, width, rowRect.height);
        }

        /// <summary>
        /// Gets the next rect in a row.
        /// </summary>
        /// <param name="rowRect">The row rect.</param>
        /// <param name="currentRectIndex">Index of the current rect.</param>
        /// <param name="spacing">The spacing.</param>
        /// <param name="widths">The widths.</param>
        /// <returns></returns>
        public static Rect GetNextRectInARow(Rect rowRect, ref int currentRectIndex, float spacing, params float[] widths)
        {
            float[] widthPercs = new float[widths.Length];
            for (int i = 0; i < widths.Length; i++)
            {
                widthPercs[i] = widths[i] / rowRect.width;
            }

            return GetNextRectInARowPerc(rowRect, ref currentRectIndex, spacing, widthPercs);
        }

        /// <summary>
        /// Draws the paged view numbers.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="numPages">The number pages.</param>
        /// <returns></returns>
        public static int DrawPagedViewNumbers(Rect rect, int currentPage, int numPages)
        {
            int resultingPage = currentPage;

            float spacing = 1f;
            rect.width = 25f;

            if (GUI.Button(rect, "<"))
            {
                resultingPage--;
            }
            rect.position += new Vector2(rect.width + spacing, 0f);

            for (int i = 0; i < numPages; i++)
            {
                var style = currentPage == i + 1 ? GUI.skin.box : GUI.skin.button;
                if (GUI.Button(rect, (i + 1).ToString(), style))
                    resultingPage = i + 1;
                rect.position += new Vector2(rect.width + spacing, 0f);
            }

            if (GUI.Button(rect, ">"))
            {
                resultingPage++;
            }
            rect.position += new Vector2(rect.width + spacing, 0f);

            resultingPage = Mathf.Clamp(resultingPage, 1, numPages);

            return resultingPage;
        }

        /// <summary>
        /// Draws the paged view numbers.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="totalNumItems">The total number items.</param>
        /// <param name="numItemsPerPage">The number items per page.</param>
        /// <returns></returns>
        public static int DrawPagedViewNumbers(Rect rect, int currentPage, int totalNumItems, int numItemsPerPage)
        {
            int numPages = Mathf.CeilToInt(totalNumItems / (float)numItemsPerPage);
            return DrawPagedViewNumbers(rect, currentPage, numPages);
        }

        /// <summary>
        /// The marquee pairs
        /// </summary>
        private static Dictionary<string, Rect> marqueePairs = new Dictionary<string, Rect>();

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="tooltips">The tooltips.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        public static GUIContent GetNextContent(this TooltipAttribute[] tooltips, string label, ref int counter)
        {
            string tooltip = tooltips[counter] != null ? tooltips[counter].tooltip : "";
            ++counter;

            return new GUIContent(label, tooltip);
        }

        /// <summary>
        /// Marquees the label on hover.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="message">The message.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        public static void MarqueeLabelOnHover(Rect rect, string message, GUIStyle style = null, MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default(Vector2))
        {
            // TODO: isNeeded flag doesn't work correctly
            bool isNeeded = (style != null ? style : GUI.skin.label).CalcSize(new GUIContent(message)).x > rect.width;

            if (rect.Contains(Event.current.mousePosition) && isNeeded)
                MarqueeLabel(rect, message, style, marqueeType, scrollSpeed);
            else
            {
                if (style == null)
                    GUI.Label(rect, message);
                else
                    GUI.Label(rect, message, style);
            }
        }

        /// <summary>
        /// Marquees the label.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="message">The message.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        public static void MarqueeLabel(Rect rect, string message, GUIStyle style = null, MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default(Vector2))
        {
            MarqueeLabel(rect, rect, message, style, marqueeType, scrollSpeed);
        }

        /// <summary>
        /// Marquees the label.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="enclosingArea">The enclosing area.</param>
        /// <param name="message">The message.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        public static void MarqueeLabel(Rect rect, Rect enclosingArea, string message, GUIStyle style = null, MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default(Vector2))
        {
            // Create a default scroll if specified is null
            if (scrollSpeed == default(Vector2))
                scrollSpeed = new Vector2(30f, 30f);

            // Get rect from dictionary
            Rect messageRect = marqueePairs.AddAndGet(message, rect);

            //This increments the positions	accordingly
            messageRect.position = ScrollingText(messageRect.position, scrollSpeed, marqueeType);

            // This functions checks to see if the text has gone past the enclosing area
            // If it has, then it resets it
            messageRect = Reset(messageRect, enclosingArea);

            // Display the text...
            if (style == null)
                GUI.Label(messageRect, message);
            else
                GUI.Label(messageRect, message, style);

            // Update the marquee pair
            marqueePairs[message] = messageRect;
        }

        /// <summary>
        /// Scrollings the text.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <returns></returns>
        private static Vector2 ScrollingText(Vector2 position, Vector2 scrollSpeed, MarqueeType marqueeType)
        {
            switch (marqueeType)
            {
                case MarqueeType.LeftToRight:
                    position.x -= Time.deltaTime * scrollSpeed.x;
                    break;

                case MarqueeType.RightToLeft:
                    position.x += Time.deltaTime * scrollSpeed.x;
                    break;

                case MarqueeType.Upwards:
                    position.y += Time.deltaTime * scrollSpeed.y;
                    break;

                case MarqueeType.Downwards:
                    position.y -= Time.deltaTime * scrollSpeed.y;
                    break;
            }

            return position;
        }

        /// <summary>
        /// Resets the specified message rect.
        /// </summary>
        /// <param name="messageRect">The message rect.</param>
        /// <param name="enclosingArea">The enclosing area.</param>
        /// <returns></returns>
        private static Rect Reset(Rect messageRect, Rect enclosingArea)
        {
            if (messageRect.x > enclosingArea.width)
                messageRect.x = -enclosingArea.width - messageRect.width;

            if (messageRect.x < -enclosingArea.width - messageRect.width)
                messageRect.x = -messageRect.width;

            if (messageRect.height > enclosingArea.height)
                messageRect.y = -enclosingArea.height - messageRect.height;

            if (messageRect.height < -enclosingArea.height - messageRect.height)
                messageRect.y = -messageRect.height;

            return messageRect;
        }
    }
}