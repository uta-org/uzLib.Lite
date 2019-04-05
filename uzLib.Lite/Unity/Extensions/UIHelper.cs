using System.Collections.Generic;
using UnityEngine;
using uzLib.Lite.Extensions;

namespace uzLib.Lite.Unity.Extensions
{
    /// <summary>
    /// The UI Helper
    /// </summary>
    public static class UIHelper
    {
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