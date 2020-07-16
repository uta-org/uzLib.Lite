using System;
using System.Collections.Generic;
using UnityEngine.Global.IMGUI;
using uzLib.Lite.ExternalCode.Extensions;

namespace UnityEngine.UI.Effects
{
    /// <summary>
    ///     Marquee Effect
    /// </summary>
    public static class MarqueeEffect
    {
        /// <summary>
        ///     The marquee pairs
        /// </summary>
        private static readonly Dictionary<string, Rect> marqueePairs = new Dictionary<string, Rect>();

        /// <summary>
        ///     Initializes the <see cref="MarqueeEffect" /> class staticly.
        /// </summary>
        static MarqueeEffect()
        {
            // IsEditor = Application.isEditor && !Application.isPlaying;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is editor.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is editor; otherwise, <c>false</c>.
        /// </value>
        private static bool IsEditor { get; set; }

        /// <summary>
        ///     Marquees the label on hover with area.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        public static void MarqueeLabelOnHoverWithArea(Rect rect, string message, GUIStyle style = null,
            MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default)
        {
            MarqueeLabelOnHover(rect, message, style, marqueeType, scrollSpeed, true);
        }

        /// <summary>
        ///     Marquees the label on hover.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        /// <param name="withArea">if set to <c>true</c> [with area].</param>
        public static void MarqueeLabelOnHover(Rect rect, string message, GUIStyle style = null,
            MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default, bool withArea = false)
        {
            // TODO: isNeeded flag doesn't work correctly
            var isNeeded = (style ?? GUI.skin.label).CalcSize(new GUIContent(message)).x > rect.width;

            //Rect orRect = GUILayoutUtility.GetLastRect();
            // FIX: BeginGroup doesn't work
            if (rect.Contains(Event.current.mousePosition) && isNeeded)
            {
                MarqueeLabel(rect, message, style, marqueeType, scrollSpeed, withArea);
            }
            else
            {
                if (!withArea)
                {
                    if (style == null)
                        GUI.Label(rect, message, GlobalStyles.CenteredLabelStyle);
                    else
                        GUI.Label(rect, message, style);
                }
                else
                {
                    GUILayout.BeginArea(rect);
                    {
                        if (style == null)
                            GUILayout.Label(message, GlobalStyles.CenteredLabelStyle);
                        else
                            GUILayout.Label(message, style);
                    }
                    GUILayout.EndArea();
                }
            }
        }

        /// <summary>
        ///     Marquees the label on hover with area.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="relPosition">The relative position.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        public static void MarqueeLabelOnHoverWithArea(Rect rect, Func<Vector2, Vector2> relPosition, string message,
            GUIStyle style = null, MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default)
        {
            MarqueeLabelOnHover(rect, relPosition, message, style, marqueeType, scrollSpeed, true);
        }

        /// <summary>
        ///     Marquees the label on hover.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="hoverRect">The hover rect.</param>
        /// <param name="relPosition">The relative position.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        /// <param name="withArea">if set to <c>true</c> [with area].</param>
        /// <exception cref="ArgumentNullException">relPosition</exception>
        public static void MarqueeLabelOnHover(
            Rect rect, Func<Vector2, Vector2> relPosition, string message, GUIStyle style = null,
            MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default, bool withArea = false)
        {
            MarqueeLabelOnHover(rect, rect, relPosition, message, style, marqueeType, scrollSpeed, withArea);
        }

        /// <summary>
        ///     Marquees the label on hover with area.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="hoverRect">The hover rect.</param>
        /// <param name="relPosition">The relative position.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        public static void MarqueeLabelOnHoverWithArea(Rect rect, Rect hoverRect, Func<Vector2, Vector2> relPosition,
            string message, GUIStyle style = null, MarqueeType marqueeType = MarqueeType.LeftToRight,
            Vector2 scrollSpeed = default)
        {
            MarqueeLabelOnHover(rect, hoverRect, relPosition, message, style, marqueeType, scrollSpeed, true);
        }

        /// <summary>
        ///     Marquees the label on hover.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="hoverRect">The hover rect.</param>
        /// <param name="relPosition">The relative position.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        /// <param name="withArea">if set to <c>true</c> [with area].</param>
        /// <exception cref="ArgumentNullException">relPosition</exception>
        public static void MarqueeLabelOnHover(
            Rect rect, Rect hoverRect, Func<Vector2, Vector2> relPosition, string message, GUIStyle style = null,
            MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default, bool withArea = false)
        {
            if (relPosition == null)
                throw new ArgumentNullException(nameof(relPosition));

            var isNeeded = WillMarquee(hoverRect, relPosition, message, style, IsEditor);

            if (isNeeded)
            {
                MarqueeLabel(rect, rect, message, style, marqueeType, scrollSpeed, withArea, hoverRect);
            }
            else
            {
                if (!withArea)
                {
                    if (style == null)
                        GUI.Label(rect, message);
                    else
                        GUI.Label(rect, message, style);
                }
                else
                {
                    GUILayout.BeginArea(hoverRect);
                    {
                        if (style == null)
                            GUILayout.Label(message, GlobalStyles.CenteredLabelStyle);
                        else
                            GUILayout.Label(message, style);
                    }
                    GUILayout.EndArea();
                }
            }
        }

        /// <summary>
        ///     Marquees the label.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        /// <param name="withArea">if set to <c>true</c> [with area].</param>
        public static void MarqueeLabel(Rect rect, string message, GUIStyle style = null,
            MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default, bool withArea = false)
        {
            MarqueeLabel(rect, rect, message, style, marqueeType, scrollSpeed, withArea);
        }

        /// <summary>
        ///     Marquees the label.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="enclosingArea">The enclosing area.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        /// <param name="withArea">if set to <c>true</c> [with area].</param>
        private static void MarqueeLabel(Rect rect, Rect enclosingArea, string message, GUIStyle style = null,
            MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default, bool withArea = false)
        {
            MarqueeLabel(rect, enclosingArea, message, style, marqueeType, scrollSpeed, (object) withArea);
        }

        /// <summary>
        ///     Marquees the label.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="enclosingArea">The enclosing area.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        /// <param name="withArea">if set to <c>true</c> [with area].</param>
        private static void MarqueeLabel(Rect rect, Rect enclosingArea, string message, GUIStyle style = null,
            MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default, params object[] objs)
        {
            var withArea = objs.GetValue<bool>(0);
            var hoverRect = objs.GetValue<Rect>(1);

            // Create a default scroll if specified is null
            if (scrollSpeed == default) scrollSpeed = new Vector2(30f, 30f);

            // Get rect from dictionary
            var messageRect = marqueePairs.AddAndGet(message, rect);

            //This increments the positions	accordingly
            messageRect.position = ScrollingText(messageRect.position, scrollSpeed, marqueeType);

            // This functions checks to see if the text has gone past the enclosing area
            // If it has, then it resets it
            messageRect = Reset(messageRect, enclosingArea);

            // Display the text...
            if (!withArea)
            {
                if (style == null)
                    GUI.Label(messageRect, message);
                else
                    GUI.Label(messageRect, message, style);
            }
            else
            {
                GUILayout.BeginArea(hoverRect);
                {
                    GUILayout.BeginArea(messageRect);
                    {
                        if (style == null)
                            GUILayout.Label(message);
                        else
                            GUILayout.Label(message, style);
                    }
                    GUILayout.EndArea();
                }
                GUILayout.EndArea();
            }

            // Update the marquee pair
            marqueePairs[message] = messageRect;
        }

        /// <summary>
        ///     Scrollings the text.
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
        ///     Resets the specified message rect.
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

        /// <summary>
        ///     Checks if the selected text will display as a marquee.
        /// </summary>
        /// <param name="hoverRect">The hover rect.</param>
        /// <param name="relPosition">The relative position.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static bool WillMarquee(Rect hoverRect, Func<Vector2, Vector2> relPosition, string message,
            GUIStyle style, bool f_isEditor)
        {
            return hoverRect.Contains(
                       relPosition(
                           GUIUtility.GUIToScreenPoint(Event.current
                               .mousePosition))) //GlobalGUIUtility.GetMousePosition(f_isEditor)))
                   && (style ?? GUI.skin.label).CalcSize(new GUIContent(message)).x > hoverRect.width;
        }
    }
}