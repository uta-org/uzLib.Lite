using System.Collections.Generic;

namespace UnityEngine.UI.Effects
{
    /// <summary>
    ///     Marquee Effect
    /// </summary>
    public static class MarqueeEffectLayout
    {
        /// <summary>
        ///     The marquee pairs
        /// </summary>
        private static readonly Dictionary<string, Rect> marqueePairs = new Dictionary<string, Rect>();

        /// <summary>
        ///     Marquees the label on hover.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="message">The message.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        public static void MarqueeLabelOnHover(Rect rect, string message, GUIStyle style = null,
            MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default)
        {
            // TODO: isNeeded flag doesn't work correctly
            var isNeeded = (style ?? GUI.skin.label).CalcSize(new GUIContent(message)).x > rect.width;

            if (rect.Contains(Event.current.mousePosition) && isNeeded)
            {
                MarqueeLabel(rect, message, style, marqueeType, scrollSpeed);
            }
            else
            {
                GUILayout.BeginArea(rect);

                if (style == null)
                    GUILayout.Label(message);
                else
                    GUILayout.Label(message, style);

                GUILayout.EndArea();
            }
        }

        /// <summary>
        ///     Marquees the label.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="message">The message.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        public static void MarqueeLabel(Rect rect, string message, GUIStyle style = null,
            MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default)
        {
            MarqueeLabel(rect, rect, message, style, marqueeType, scrollSpeed);
        }

        /// <summary>
        ///     Marquees the label.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="enclosingArea">The enclosing area.</param>
        /// <param name="message">The message.</param>
        /// <param name="marqueeType">Type of the marquee.</param>
        /// <param name="scrollSpeed">The scroll speed.</param>
        public static void MarqueeLabel(Rect rect, Rect enclosingArea, string message, GUIStyle style = null,
            MarqueeType marqueeType = MarqueeType.LeftToRight, Vector2 scrollSpeed = default)
        {
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
            GUILayout.BeginArea(messageRect);

            if (style == null)
                GUILayout.Label(message);
            else
                GUILayout.Label(message, style);

            GUILayout.EndArea();

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
            if (messageRect.x > enclosingArea.width) messageRect.x = -enclosingArea.width - messageRect.width;

            if (messageRect.x < -enclosingArea.width - messageRect.width) messageRect.x = -messageRect.width;

            if (messageRect.height > enclosingArea.height) messageRect.y = -enclosingArea.height - messageRect.height;

            if (messageRect.height < -enclosingArea.height - messageRect.height) messageRect.y = -messageRect.height;

            return messageRect;
        }
    }
}