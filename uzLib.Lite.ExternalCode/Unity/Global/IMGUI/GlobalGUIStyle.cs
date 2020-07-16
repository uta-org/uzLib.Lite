using System;

namespace UnityEngine.Global.IMGUI
{
    [Serializable]
    public sealed class GlobalGUIStyle
    {
        /// <summary>
        ///     The null GUIStyle
        /// </summary>
        private static GUIStyle s_None;

        [NonSerialized] private RectOffset m_Border, m_Padding, m_Margin, m_Overflow;

        //Called during Deserialization from cpp
        //internal void InternalOnAfterDeserialize()
        //{
        //    m_Normal = GlobalGUIStyleState.ProduceGlobalGUIStyleStateFromDeserialization(this, GetStyleStatePtr(0));
        //    m_Hover = GlobalGUIStyleState.ProduceGlobalGUIStyleStateFromDeserialization(this, GetStyleStatePtr(1));
        //    m_Active = GlobalGUIStyleState.ProduceGlobalGUIStyleStateFromDeserialization(this, GetStyleStatePtr(2));
        //    m_Focused = GlobalGUIStyleState.ProduceGlobalGUIStyleStateFromDeserialization(this, GetStyleStatePtr(3));
        //    m_OnNormal = GlobalGUIStyleState.ProduceGlobalGUIStyleStateFromDeserialization(this, GetStyleStatePtr(4));
        //    m_OnHover = GlobalGUIStyleState.ProduceGlobalGUIStyleStateFromDeserialization(this, GetStyleStatePtr(5));
        //    m_OnActive = GlobalGUIStyleState.ProduceGlobalGUIStyleStateFromDeserialization(this, GetStyleStatePtr(6));
        //    m_OnFocused = GlobalGUIStyleState.ProduceGlobalGUIStyleStateFromDeserialization(this, GetStyleStatePtr(7));
        //}

        //[NonSerialized]
        //internal IntPtr m_Ptr;

        [NonSerialized]
        private GlobalGUIStyleState m_Normal,
            m_Hover,
            m_Active,
            m_Focused,
            m_OnNormal,
            m_OnHover,
            m_OnActive,
            m_OnFocused;

        /// <summary>
        ///     The name
        /// </summary>
        public string name;

        /// <summary>
        ///     The stretch height
        /// </summary>
        public bool stretchHeight;

        // Rendering settings for when the component is displayed normally.
        /*public GlobalGUIStyleState normal
        {
            get
            {
                //GlobalGUIStyleState can't be initialized in the constructor
                //since constructors can be called within and outside a serialization operation
                //So we delay the initialization here where we know we will be on the main thread, outside
                //any loading operation.
                return m_Normal ?? (m_Normal = GlobalGUIStyleState.GetGlobalGUIStyleState(this, GetStyleStatePtr(0)));
            }
            set { AssignStyleState(0, value.m_Ptr); }
        }

        // Rendering settings for when the mouse is hovering over the control
        public GlobalGUIStyleState hover
        {
            get { return m_Hover ?? (m_Hover = GlobalGUIStyleState.GetGlobalGUIStyleState(this, GetStyleStatePtr(1))); }
            set { AssignStyleState(1, value.m_Ptr); }
        }

        // Rendering settings for when the control is pressed down.
        public GlobalGUIStyleState active
        {
            get { return m_Active ?? (m_Active = GlobalGUIStyleState.GetGlobalGUIStyleState(this, GetStyleStatePtr(2))); }
            set { AssignStyleState(2, value.m_Ptr); }
        }

        // Rendering settings for when the control is turned on.
        public GlobalGUIStyleState onNormal
        {
            get { return m_OnNormal ?? (m_OnNormal = GlobalGUIStyleState.GetGlobalGUIStyleState(this, GetStyleStatePtr(4))); }
            set { AssignStyleState(4, value.m_Ptr); }
        }

        // Rendering settings for when the control is turned on and the mouse is hovering it.
        public GlobalGUIStyleState onHover
        {
            get { return m_OnHover ?? (m_OnHover = GlobalGUIStyleState.GetGlobalGUIStyleState(this, GetStyleStatePtr(5))); }
            set { AssignStyleState(5, value.m_Ptr); }
        }

        // Rendering settings for when the element is turned on and pressed down.
        public GlobalGUIStyleState onActive
        {
            get { return m_OnActive ?? (m_OnActive = GlobalGUIStyleState.GetGlobalGUIStyleState(this, GetStyleStatePtr(6))); }
            set { AssignStyleState(6, value.m_Ptr); }
        }

        // Rendering settings for when the element has keyboard focus.
        public GlobalGUIStyleState focused
        {
            get { return m_Focused ?? (m_Focused = GlobalGUIStyleState.GetGlobalGUIStyleState(this, GetStyleStatePtr(3))); }
            set { AssignStyleState(3, value.m_Ptr); }
        }

        // Rendering settings for when the element has keyboard and is turned on.
        public GlobalGUIStyleState onFocused
        {
            get { return m_OnFocused ?? (m_OnFocused = GlobalGUIStyleState.GetGlobalGUIStyleState(this, GetStyleStatePtr(7))); }
            set { AssignStyleState(7, value.m_Ptr); }
        }

        // The borders of all background images.
        public RectOffset border
        {
            get { return m_Border ?? (m_Border = new RectOffset(this, GetRectOffsetPtr(0))); }
            set { AssignRectOffset(0, value.m_Ptr); }
        }

        // The margins between elements rendered in this style and any other GUI elements
        public RectOffset margin
        {
            get { return m_Margin ?? (m_Margin = new RectOffset(this, GetRectOffsetPtr(1))); }
            set { AssignRectOffset(1, value.m_Ptr); }
        }

        // Space from the edge of [[GUIStyle]] to the start of the contents.
        public RectOffset padding
        {
            get { return m_Padding ?? (m_Padding = new RectOffset(this, GetRectOffsetPtr(2))); }
            set { AssignRectOffset(2, value.m_Ptr); }
        }

        // Extra space to be added to the background image.
        public RectOffset overflow
        {
            get { return m_Overflow ?? (m_Overflow = new RectOffset(this, GetRectOffsetPtr(3))); }
            set { AssignRectOffset(3, value.m_Ptr); }
        }

        // The height of one line of text with this style, measured in pixels.
        public float lineHeight => Mathf.Round(Internal_GetLineHeight(m_Ptr));*/

        // Shortcut for an empty GUIStyle.
        public static GUIStyle none => s_None ?? (s_None = new GUIStyle());

        // Constructor for empty GUIStyle.
        //public GlobalGUIStyle()
        //{
        //    m_Ptr = Internal_Create(this);
        //}

        //// Constructs GUIStyle identical to given other GUIStyle.
        //public GUIStyle(GUIStyle other)
        //{
        //    m_Ptr = Internal_Copy(this, other);
        //}

        //~GUIStyle()
        //{
        //    if (m_Ptr != IntPtr.Zero)
        //    {
        //        Internal_Destroy(m_Ptr);
        //        m_Ptr = IntPtr.Zero;
        //    }
        //}

        internal static void CleanupRoots()
        {
            // See GUI.CleanupRoots
            s_None = null;
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"GUIStyle '{name}'";
        }

        /// <summary>
        ///     Creates a GUIStyle with the rich text property.
        /// </summary>
        /// <param name="baseName">Name of the base style.</param>
        /// <returns></returns>
        public static GUIStyle WithRichText(string baseName = "label")
        {
            var style = new GUIStyle(baseName)
            {
                richText = true
            };

            return style;
        }

        /// <summary>
        ///     Creates a GUIStyle with the rich text property.
        /// </summary>
        /// <param name="_style">The style.</param>
        /// <returns></returns>
        public static GUIStyle WithRichText(GUIStyle _style)
        {
            var style = new GUIStyle(_style)
            {
                richText = true
            };

            return style;
        }

        /// <summary>
        ///     Creates a GUIStyle with the centered rich text property.
        /// </summary>
        /// <param name="baseName">Name of the base style.</param>
        /// <returns></returns>
        public static GUIStyle WithCenteredRichText(string baseName = "label")
        {
            var style = new GUIStyle(baseName)
            {
                richText = true,
                alignment = TextAnchor.MiddleCenter
            };

            return style;
        }

        /// <summary>
        ///     Creates a GUIStyle with the centered rich text property.
        /// </summary>
        /// <param name="_style">The style.</param>
        /// <returns></returns>
        public static GUIStyle WithCenteredRichText(GUIStyle _style)
        {
            var style = new GUIStyle(_style)
            {
                richText = true,
                alignment = TextAnchor.MiddleCenter
            };

            return style;
        }

        /// <summary>
        ///     Creates a GUIStyle with the aligned rich text property.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <param name="baseName">Name of the base style.</param>
        /// <returns></returns>
        public static GUIStyle WithAlignedRichText(TextAnchor alignment, string baseName = "label")
        {
            var style = new GUIStyle(baseName)
            {
                richText = true,
                alignment = alignment
            };

            return style;
        }

        /// <summary>
        ///     Creates a GUIStyle with the aligned rich text property.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <param name="_style">The style.</param>
        /// <returns></returns>
        public static GUIStyle WithAlignedRichText(TextAnchor alignment, GUIStyle _style)
        {
            var style = new GUIStyle(_style)
            {
                richText = true,
                alignment = alignment
            };

            return style;
        }

        /// <summary>
        ///     Creates a GUIStyle with the size of the font property.
        /// </summary>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="richText">if set to <c>true</c> [rich text].</param>
        /// <param name="baseName">Name of the base style.</param>
        /// <returns></returns>
        public static GUIStyle WithFontSize(int fontSize, bool richText = true, string baseName = "label")
        {
            var style = new GUIStyle(baseName)
            {
                richText = richText,
                fontSize = fontSize
            };

            return style;
        }

        /// <summary>
        ///     Creates a GUIStyle with the size of the centered font property.
        /// </summary>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="richText">if set to <c>true</c> [rich text].</param>
        /// <param name="baseName">Name of the base style.</param>
        /// <returns></returns>
        public static GUIStyle WithCenteredFontSize(int fontSize, bool richText = true, string baseName = "label")
        {
            var style = new GUIStyle(baseName)
            {
                fontSize = fontSize,
                richText = richText,
                alignment = TextAnchor.MiddleCenter
            };

            return style;
        }
    }
}