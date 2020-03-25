using System;
using System.Linq;
using UnityEngine;
using uzLib.Lite.ExternalCode.WinFormsSkins.Workers;

#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5

using UnityEngine.Extensions;

#endif

namespace uzLib.Lite.ExternalCode.Extensions
{
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
        /// Sums the y.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static Rect SumY(this Rect rect, float y)
        {
            return rect.SumTop(y).RestHeight(y);
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

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

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

#endif

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

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public static int GetLength(this Rect rect)
        {
            return Mathf.RoundToInt(rect.width * rect.width);
        }

#endif

        public static Rect ForceContainer(this Rect rect, GUIContent content, Rect container, RectOffset offset = default, GUIStyle style = null)
        {
            if (style == null) style = SkinWorker.MySkin.label;
            if (offset == default) offset = new RectOffset();

            var width = container.width - (offset.left - offset.right);
            var height = style.CalcHeight(content, width);

            rect.ForceBoth(width, height);

            return rect;
        }

        public static Rect ToRect(this System.Drawing.Rectangle rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }

    public static class RectExt
    {
        /// <summary>
        ///     Forces the width.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        public static Rect ForceWidth(this Rect rect, float width)
        {
            return new Rect(rect.x, rect.y, width, rect.height);
        }

        /// <summary>
        ///     Forces the both.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Rect ForceBoth(this Rect rect, float width, float height)
        {
            return new Rect(rect.x, rect.y, width, height);
        }

        /// <summary>
        ///     Forces the height.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Rect ForceHeight(this Rect rect, float height)
        {
            return new Rect(rect.x, rect.y, rect.width, height);
        }

        /// <summary>
        ///     Sums the left margin.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="leftMargin">The left margin.</param>
        /// <returns></returns>
        public static Rect SumLeft(this Rect rect, float leftMargin)
        {
            return new Rect(rect.position.x + leftMargin, rect.position.y, rect.width, rect.height);
        }

        /// <summary>
        ///     Rests the left margin.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="leftMargin">The left margin.</param>
        /// <returns></returns>
        public static Rect RestLeft(this Rect rect, float leftMargin)
        {
            return new Rect(rect.position.x - leftMargin, rect.position.y, rect.width, rect.height);
        }

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        ///     Sums the top margin.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="topMargin">The top margin.</param>
        /// <returns></returns>
        public static Rect SumTop(this Rect rect, float topMargin)
        {
            return new Rect(rect.position.x, rect.position.y + topMargin, rect.width, rect.height);
        }

#endif

        /// <summary>
        ///     Rests the top margin.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="topMargin">The top margin.</param>
        /// <returns></returns>
        public static Rect RestTop(this Rect rect, float topMargin)
        {
            return new Rect(rect.position.x, rect.position.y - topMargin, rect.width, rect.height);
        }
    }

    /// <summary>
    ///     The Rect Helper
    /// </summary>
    public class RectHelper
    {
        /// <summary>
        ///     The current height
        /// </summary>
        private float m_currentHeight;

        /// <summary>
        ///     The m rect
        /// </summary>
        private Rect m_rect;

        /// <summary>
        ///     Prevents a default instance of the <see cref="RectHelper" /> class from being created.
        /// </summary>
        private RectHelper()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RectHelper" /> class.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public RectHelper(Rect rect, float defaultHeight = 18)
        {
            m_rect = rect;
            DefaultHeight = defaultHeight;
            m_currentHeight += defaultHeight;
        }

        /// <summary>
        ///     Gets the default height.
        /// </summary>
        /// <value>
        ///     The default height.
        /// </value>
        public float DefaultHeight { get; }

        /// <summary>
        ///     Gets the height of the current.
        /// </summary>
        /// <value>
        ///     The height of the current.
        /// </value>
        public float CurrentHeight =>
            m_currentHeight == 0 || float.IsNaN(m_currentHeight) ? DefaultHeight : m_currentHeight;

        /// <summary>
        ///     Gets the maximum height.
        /// </summary>
        /// <value>
        ///     The maximum height.
        /// </value>
        public float MaxHeight { get; private set; }

        /// <summary>
        ///     Draws a vertical space
        /// </summary>
        /// <param name="height">The height.</param>
        public void VerticalSpace(float height)
        {
            NextHeight(height);
        }

        public Rect NextHeight(bool useMinInsteadOfMax = true, params GUIStyle[] styles)
        {
            return NextHeight(useMinInsteadOfMax
                ? styles.Min(style => style.CalcSize(GUIContent.none).y)
                : styles.Max(style => style.CalcSize(GUIContent.none).y), false);
        }

        /// <summary>
        ///     Sums the next the height.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="forceHeight">if set to <c>true</c> [force height].</param>
        /// <returns></returns>
        public Rect NextHeight(GUIStyle style, bool forceHeight = true)
        {
            return NextHeight(style.CalcSize(GUIContent.none).y, forceHeight);
        }

        /// <summary>
        ///     Sum the next the height.
        /// </summary>
        /// <returns></returns>
        public Rect NextHeight(float height = default, bool forceHeight = true)
        {
            var r = GetNextHeight(height, forceHeight, out var y);

            m_rect.y += y;

            if (forceHeight)
                m_currentHeight += y;
            else
                m_currentHeight = m_rect.y;

            if (m_currentHeight > MaxHeight) MaxHeight = m_currentHeight;

            if (float.IsNaN(m_currentHeight)) throw new ArgumentOutOfRangeException();

            return r;
        }

        /// <summary>
        ///     Gets the height of the next.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="forceHeight">if set to <c>true</c> [force height].</param>
        /// <returns></returns>
        public Rect GetNextHeight(float height, bool forceHeight = true)
        {
            return GetNextHeight(height, forceHeight, out var y);
        }

        /// <summary>
        ///     Gets the height of the next.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="forceHeight">if set to <c>true</c> [force height].</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        private Rect GetNextHeight(float height, bool forceHeight, out float y)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            y = height != default ? height : GUILayoutUtility.GetLastRect().height;

            var r = new Rect(m_rect.x, m_rect.y + y, m_rect.width, m_rect.height);
            if (forceHeight) r = r.ForceHeight(y);

            return r;
        }
    }
}