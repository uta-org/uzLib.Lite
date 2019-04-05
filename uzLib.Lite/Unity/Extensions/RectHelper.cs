using UnityEngine;

namespace uzLib.Lite.Unity.Extensions
{
    /// <summary>
    /// The Rect Helper
    /// </summary>
    public class RectHelper
    {
        /// <summary>
        /// Gets the default height.
        /// </summary>
        /// <value>
        /// The default height.
        /// </value>
        public float DefaultHeight { get; private set; }

        /// <summary>
        /// The current height
        /// </summary>
        private float m_currentHeight;

        /// <summary>
        /// Gets the height of the current.
        /// </summary>
        /// <value>
        /// The height of the current.
        /// </value>
        public float CurrentHeight { get { return m_currentHeight == 0 ? DefaultHeight : m_currentHeight; } }

        /// <summary>
        /// Gets the maximum height.
        /// </summary>
        /// <value>
        /// The maximum height.
        /// </value>
        public float MaxHeight { get; private set; }

        /// <summary>
        /// The m rect
        /// </summary>
        private Rect m_rect;

        /// <summary>
        /// Prevents a default instance of the <see cref="RectHelper"/> class from being created.
        /// </summary>
        private RectHelper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectHelper"/> class.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public RectHelper(Rect rect, float defaultHeight = 18)
        {
            m_rect = rect;
            DefaultHeight = defaultHeight;
            m_currentHeight += defaultHeight;
        }

        /// <summary>
        /// Nexts the height.
        /// </summary>
        /// <returns></returns>
        public Rect NextHeight(float height = default(float), bool forceHeight = true)
        {
            float y = height != default(float) ? height : GUILayoutUtility.GetLastRect().height;

            var r = new Rect(m_rect.x, m_rect.y + y, m_rect.width, m_rect.height);
            if (forceHeight)
                r = r.ForceHeight(y);

            m_rect.y += y;

            if (forceHeight)
                m_currentHeight += y;
            else
                m_currentHeight = m_rect.y;

            if (m_currentHeight > MaxHeight)
                MaxHeight = m_currentHeight;

            return r;
        }
    }

    public static class RectExtensions
    {
        /// <summary>
        /// Forces the height.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Rect ForceHeight(this Rect rect, float height)
        {
            return new Rect(rect.x, rect.y, rect.width, height);
        }

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
    }
}