using System;
using System.Linq;

namespace UnityEngine.Extensions
{
    /// <summary>
    /// The Rect Helper
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
        /// Sums the next the height.
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
        /// Gets the height of the next.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="forceHeight">if set to <c>true</c> [force height].</param>
        /// <returns></returns>
        public Rect GetNextHeight(float height, bool forceHeight = true)
        {
            return GetNextHeight(height, forceHeight, out var y);
        }

        /// <summary>
        /// Gets the height of the next.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="forceHeight">if set to <c>true</c> [force height].</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        private Rect GetNextHeight(float height, bool forceHeight, out float y)
        {
            y = height != default ? height : GUILayoutUtility.GetLastRect().height;

            var r = new Rect(m_rect.x, m_rect.y + y, m_rect.width, m_rect.height);
            if (forceHeight) r = r.ForceHeight(y);

            return r;
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
}