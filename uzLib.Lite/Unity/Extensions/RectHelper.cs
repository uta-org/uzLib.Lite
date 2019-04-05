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
    }
}