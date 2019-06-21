namespace UnityEngine.Extensions
{
    extern alias TexLib;

    /// <summary>
    /// Some Rect extensions
    /// </summary>
    public static class RectExtensions
    {
        /// <summary>
        /// Paddings the top.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Rect PaddingTop(this Rect rect, float height)
        {
            return new Rect(rect.x, rect.y + height, rect.width, rect.height);
        }

        /// <summary>
        /// Clamps all coordinates between 0 and 1.
        /// </summary>
        public static Rect Clamp01(this Rect rect)
        {
            float xMin = rect.xMin;
            float xMax = rect.xMax;
            float yMin = rect.yMin;
            float yMax = rect.yMax;

            xMin = Mathf.Clamp01(xMin);
            xMax = Mathf.Clamp01(xMax);
            yMin = Mathf.Clamp01(yMin);
            yMax = Mathf.Clamp01(yMax);

            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        /// <summary>
        /// Determines whether [contains] [the specified other].
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified other]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this Rect rect, Rect other)
        {
            return rect.xMax >= other.xMax && rect.xMin <= other.xMin && rect.yMax >= other.yMax && rect.yMin <= other.yMin;
        }

        /// <summary>
        /// Intersections the specified other.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public static Rect Intersection(this Rect rect, Rect other)
        {
            float xMax = Mathf.Min(rect.xMax, other.xMax);
            float yMax = Mathf.Min(rect.yMax, other.yMax);

            float xMin = Mathf.Max(rect.xMin, other.xMin);
            float yMin = Mathf.Max(rect.yMin, other.yMin);

            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        /// <summary>
        /// Normalizeds the specified outter.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="outter">The outter.</param>
        /// <returns></returns>
        public static Rect Normalized(this Rect rect, Rect outter)
        {
            float xMin = (rect.xMin - outter.xMin) / outter.width;
            float xMax = (rect.xMax - outter.xMin) / outter.width;

            float yMin = (rect.yMin - outter.yMin) / outter.height;
            float yMax = (rect.yMax - outter.yMin) / outter.height;

            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        /// <summary>
        /// Creates the rect.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static Rect CreateRect(Vector2 center, Vector2 size)
        {
            return new Rect(center - size / 2.0f, size);
        }

        /// <summary>
        ///     Creates the offset.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="top">The top.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static RectOffset CreateOffset(int left, int right, int top, int bottom)
        {
            return new RectOffset(left, right, top, bottom);
        }

        /// <summary>
        ///     Creates the offset.
        /// </summary>
        /// <param name="horizontal">The horizontal.</param>
        /// <param name="vertical">The vertical.</param>
        /// <returns></returns>
        public static RectOffset CreateOffset(int horizontal, int vertical)
        {
            return new RectOffset(horizontal, horizontal, vertical, vertical);
        }

        /// <summary>
        ///     Creates the horizontal offset.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static RectOffset CreateHorizontalOffset(int left, int right)
        {
            return new RectOffset(left, right, 0, 0);
        }

        /// <summary>
        ///     Creates the vertical offset.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static RectOffset CreateVerticalOffset(int top, int bottom)
        {
            return new RectOffset(0, 0, top, bottom);
        }

        /// <summary>
        ///     Creates the upper offset.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public static RectOffset CreateUpperOffset(int left, int top)
        {
            return new RectOffset(left, 0, top, 0);
        }

        /// <summary>
        ///     Creates the bottom offset.
        /// </summary>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        public static RectOffset CreateBottomOffset(int right, int bottom)
        {
            return new RectOffset(0, right, 0, bottom);
        }

        /// <summary>
        ///     Sums the rects.
        /// </summary>
        /// <param name="rect1">The rect1.</param>
        /// <param name="rect2">The rect2.</param>
        /// <returns></returns>
        public static Rect SumRects(Rect rect1, Rect rect2)
        {
            var width = Mathf.Max(rect1.width, rect2.width);
            var height = Mathf.Max(rect1.height, rect2.height);

            return new Rect(rect1.xMin + rect2.xMin, rect1.yMin + rect2.yMin, width, height);
        }

        /// <summary>
        ///     Rests the rects.
        /// </summary>
        /// <param name="rect1">The rect1.</param>
        /// <param name="rect2">The rect2.</param>
        /// <returns></returns>
        public static Rect RestRects(Rect rect1, Rect rect2)
        {
            var width = Mathf.Max(rect1.width, rect2.width);
            var height = Mathf.Max(rect1.height, rect2.height);

            return new Rect(rect1.xMin - rect2.xMin, rect1.yMin - rect2.yMin, width, height);
        }

        /// <summary>
        ///     Sums the position.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static Rect SumPosition(this Rect rect, Vector2 position)
        {
            // Debug.Log(position);
            return new Rect(rect.position + position, rect.size);
        }

        /// <summary>
        ///     Rests the position.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static Rect RestPosition(this Rect rect, Vector2 position)
        {
            // Debug.Log(position);
            return new Rect(rect.position - position, rect.size);
        }

        /// <summary>
        ///     Sums the height.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Rect SumHeight(this Rect rect, float height)
        {
            return new Rect(rect.position.x, rect.position.y, rect.width, rect.height + height);
        }

        /// <summary>
        ///     Rests the height.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Rect RestHeight(this Rect rect, float height)
        {
            return new Rect(rect.position.x, rect.position.y, rect.width, rect.height - height);
        }

        /// <summary>
        ///     Sums the width.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        public static Rect SumWidth(this Rect rect, float width)
        {
            return new Rect(rect.position.x, rect.position.y, rect.width + width, rect.height);
        }

        /// <summary>
        ///     Rests the width.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        public static Rect RestWidth(this Rect rect, float width)
        {
            return new Rect(rect.position.x, rect.position.y, rect.width - width, rect.height);
        }

        /// <summary>
        ///     Resets the position.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public static Rect ResetPosition(this Rect rect)
        {
            return new Rect(Vector2.zero, rect.size);
        }

        /// <summary>
        /// Gets the centered rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="margin">The margin.</param>
        /// <returns></returns>
        public static Rect GetCenteredRect(this Rect rect, float margin)
        {
            return new Rect(
                rect.xMin + margin,
                rect.yMin + margin,
                rect.width - margin * 2,
                rect.height - margin * 2);
        }

        /// <summary>
        /// Rounds the values.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public static Rect RoundValues(this Rect rect)
        {
            return new Rect(
                Mathf.RoundToInt(rect.xMin),
                Mathf.RoundToInt(rect.yMin),
                Mathf.RoundToInt(rect.width),
                Mathf.RoundToInt(rect.height));
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public static int GetLength(this Rect rect)
        {
            return Mathf.RoundToInt(rect.width * rect.width);
        }

        public static Rect ToRect(this TexLib::System.Drawing.Rectangle rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}