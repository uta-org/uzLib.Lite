using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.Global.IMGUI
{
    // Defines how GUI looks and behaves.
    [Serializable]
    [ExecuteInEditMode]
    public sealed class GlobalGUISkin : ScriptableObject
    {
        internal static GUIStyle ms_Error;

        //internal static SkinChangedDelegate m_SkinChanged;

        // Make this the current skin used by the GUI
        internal static GlobalGUISkin current;

        // The default font to use for all styles.
        //public Font font { get { return m_Font; } set { m_Font = value; if (current == this) GUIStyle.SetDefaultFont(m_Font); Apply(); } }

        [SerializeField] //yes the attribute applies to all fields on the line below.
        private GUIStyle m_box, m_button, m_toggle, m_label, m_textField, m_textArea, m_window;

        [SerializeField] internal GUIStyle[] m_CustomStyles;

        [SerializeField] private Font m_Font;

        [SerializeField] private GUIStyle m_horizontalScrollbar;

        [SerializeField] private GUIStyle m_horizontalScrollbarLeftButton;

        [SerializeField] private GUIStyle m_horizontalScrollbarRightButton;

        [SerializeField] private GUIStyle m_horizontalScrollbarThumb;

        [SerializeField] private GUIStyle m_horizontalSlider;

        [SerializeField] private GUIStyle m_horizontalSliderThumb;

        // Background style for scroll views.
        [SerializeField] private GUIStyle m_ScrollView;

        private Dictionary<string, GUIStyle> m_Styles;

        [SerializeField] private GUIStyle m_verticalScrollbar;

        [SerializeField] private GUIStyle m_verticalScrollbarDownButton;

        [SerializeField] private GUIStyle m_verticalScrollbarThumb;

        [SerializeField] private GUIStyle m_verticalScrollbarUpButton;

        [SerializeField] private GUIStyle m_verticalSlider;

        [SerializeField] private GUIStyle m_verticalSliderThumb;

        // *undocumented*
        public GlobalGUISkin()
        {
            m_CustomStyles = new GUIStyle[1];
        }

        // Style used by default for GUI::ref::Box controls.
        public GUIStyle box
        {
            get => m_box;
            set
            {
                m_box = value;
                Apply();
            }
        }

        // Style used by default for GUI::ref::Label controls.
        public GUIStyle label
        {
            get => m_label;
            set
            {
                m_label = value;
                Apply();
            }
        }

        // Style used by default for GUI::ref::TextField controls.
        public GUIStyle textField
        {
            get => m_textField;
            set
            {
                m_textField = value;
                Apply();
            }
        }

        // Style used by default for GUI::ref::TextArea controls.
        public GUIStyle textArea
        {
            get => m_textArea;
            set
            {
                m_textArea = value;
                Apply();
            }
        }

        // Style used by default for GUI::ref::Button controls.
        public GUIStyle button
        {
            get => m_button;
            set
            {
                m_button = value;
                Apply();
            }
        }

        // Style used by default for GUI::ref::Toggle controls.
        public GUIStyle toggle
        {
            get => m_toggle;
            set
            {
                m_toggle = value;
                Apply();
            }
        }

        // Style used by default for Window controls (SA GUI::ref::Window).
        public GUIStyle window
        {
            get => m_window;
            set
            {
                m_window = value;
                Apply();
            }
        }

        // Style used by default for the background part of GUI::ref::HorizontalSlider controls.
        public GUIStyle horizontalSlider
        {
            get => m_horizontalSlider;
            set
            {
                m_horizontalSlider = value;
                Apply();
            }
        }

        // Style used by default for the thumb that is dragged in GUI::ref::HorizontalSlider controls.
        public GUIStyle horizontalSliderThumb
        {
            get => m_horizontalSliderThumb;
            set
            {
                m_horizontalSliderThumb = value;
                Apply();
            }
        }

        // Style used by default for the background part of GUI::ref::VerticalSlider controls.
        public GUIStyle verticalSlider
        {
            get => m_verticalSlider;
            set
            {
                m_verticalSlider = value;
                Apply();
            }
        }

        // Style used by default for the thumb that is dragged in GUI::ref::VerticalSlider controls.
        public GUIStyle verticalSliderThumb
        {
            get => m_verticalSliderThumb;
            set
            {
                m_verticalSliderThumb = value;
                Apply();
            }
        }

        // Style used by default for the background part of GUI::ref::HorizontalScrollbar controls.
        public GUIStyle horizontalScrollbar
        {
            get => m_horizontalScrollbar;
            set
            {
                m_horizontalScrollbar = value;
                Apply();
            }
        }

        // Style used by default for the thumb that is dragged in GUI::ref::HorizontalScrollbar controls.
        public GUIStyle horizontalScrollbarThumb
        {
            get => m_horizontalScrollbarThumb;
            set
            {
                m_horizontalScrollbarThumb = value;
                Apply();
            }
        }

        // Style used by default for the left button on GUI::ref::HorizontalScrollbar controls.
        public GUIStyle horizontalScrollbarLeftButton
        {
            get => m_horizontalScrollbarLeftButton;
            set
            {
                m_horizontalScrollbarLeftButton = value;
                Apply();
            }
        }

        // Style used by default for the right button on GUI::ref::HorizontalScrollbar controls.
        public GUIStyle horizontalScrollbarRightButton
        {
            get => m_horizontalScrollbarRightButton;
            set
            {
                m_horizontalScrollbarRightButton = value;
                Apply();
            }
        }

        // Style used by default for the background part of GUI::ref::VerticalScrollbar controls.
        public GUIStyle verticalScrollbar
        {
            get => m_verticalScrollbar;
            set
            {
                m_verticalScrollbar = value;
                Apply();
            }
        }

        // Style used by default for the thumb that is dragged in GUI::ref::VerticalScrollbar controls.
        public GUIStyle verticalScrollbarThumb
        {
            get => m_verticalScrollbarThumb;
            set
            {
                m_verticalScrollbarThumb = value;
                Apply();
            }
        }

        // Style used by default for the up button on GUI::ref::VerticalScrollbar controls.
        public GUIStyle verticalScrollbarUpButton
        {
            get => m_verticalScrollbarUpButton;
            set
            {
                m_verticalScrollbarUpButton = value;
                Apply();
            }
        }

        // Style used by default for the down button on GUI::ref::VerticalScrollbar controls.
        public GUIStyle verticalScrollbarDownButton
        {
            get => m_verticalScrollbarDownButton;
            set
            {
                m_verticalScrollbarDownButton = value;
                Apply();
            }
        }

        // Style used by default for the background of ScrollView controls (see GUI::ref::BeginScrollView).
        public GUIStyle scrollView
        {
            get => m_ScrollView;
            set
            {
                m_ScrollView = value;
                Apply();
            }
        }

        // Array of GUI styles for specific needs.
        public GUIStyle[] customStyles
        {
            get => m_CustomStyles;
            set
            {
                m_CustomStyles = value;
                Apply();
            }
        }

        // Generic settings for how controls should behave with this skin.
        [field: SerializeField] public GUISettings settings { get; } = new GUISettings();

        internal static GUIStyle error
        {
            get
            {
                if (ms_Error == null)
                {
                    ms_Error = new GUIStyle();
                    ms_Error.name = "StyleNotFoundError";
                }

                return ms_Error;
            }
        }

        internal void OnEnable()
        {
            Apply();
        }

        internal static void CleanupRoots()
        {
            // See GUI.CleanupRoots
            current = null;
            ms_Error = null;
        }

        internal void Apply()
        {
            if (m_CustomStyles == null) Debug.Log("custom styles is null");

            BuildStyleCache();
        }

        private void BuildStyleCache()
        {
            if (m_box == null) m_box = new GUIStyle();

            if (m_button == null) m_button = new GUIStyle();

            if (m_toggle == null) m_toggle = new GUIStyle();

            if (m_label == null) m_label = new GUIStyle();

            if (m_window == null) m_window = new GUIStyle();

            if (m_textField == null) m_textField = new GUIStyle();

            if (m_textArea == null) m_textArea = new GUIStyle();

            if (m_horizontalSlider == null) m_horizontalSlider = new GUIStyle();

            if (m_horizontalSliderThumb == null) m_horizontalSliderThumb = new GUIStyle();

            if (m_verticalSlider == null) m_verticalSlider = new GUIStyle();

            if (m_verticalSliderThumb == null) m_verticalSliderThumb = new GUIStyle();

            if (m_horizontalScrollbar == null) m_horizontalScrollbar = new GUIStyle();

            if (m_horizontalScrollbarThumb == null) m_horizontalScrollbarThumb = new GUIStyle();

            if (m_horizontalScrollbarLeftButton == null) m_horizontalScrollbarLeftButton = new GUIStyle();

            if (m_horizontalScrollbarRightButton == null) m_horizontalScrollbarRightButton = new GUIStyle();

            if (m_verticalScrollbar == null) m_verticalScrollbar = new GUIStyle();

            if (m_verticalScrollbarThumb == null) m_verticalScrollbarThumb = new GUIStyle();

            if (m_verticalScrollbarUpButton == null) m_verticalScrollbarUpButton = new GUIStyle();

            if (m_verticalScrollbarDownButton == null) m_verticalScrollbarDownButton = new GUIStyle();

            if (m_ScrollView == null) m_ScrollView = new GUIStyle();

            m_Styles = new Dictionary<string, GUIStyle>(StringComparer.OrdinalIgnoreCase);

            m_Styles["box"] = m_box;
            m_box.name = "box";

            m_Styles["button"] = m_button;
            m_button.name = "button";

            m_Styles["toggle"] = m_toggle;
            m_toggle.name = "toggle";

            m_Styles["label"] = m_label;
            m_label.name = "label";

            m_Styles["window"] = m_window;
            m_window.name = "window";

            m_Styles["textfield"] = m_textField;
            m_textField.name = "textfield";

            m_Styles["textarea"] = m_textArea;
            m_textArea.name = "textarea";

            m_Styles["horizontalslider"] = m_horizontalSlider;
            m_horizontalSlider.name = "horizontalslider";

            m_Styles["horizontalsliderthumb"] = m_horizontalSliderThumb;
            m_horizontalSliderThumb.name = "horizontalsliderthumb";

            m_Styles["verticalslider"] = m_verticalSlider;
            m_verticalSlider.name = "verticalslider";

            m_Styles["verticalsliderthumb"] = m_verticalSliderThumb;
            m_verticalSliderThumb.name = "verticalsliderthumb";

            m_Styles["horizontalscrollbar"] = m_horizontalScrollbar;
            m_horizontalScrollbar.name = "horizontalscrollbar";

            m_Styles["horizontalscrollbarthumb"] = m_horizontalScrollbarThumb;
            m_horizontalScrollbarThumb.name = "horizontalscrollbarthumb";

            m_Styles["horizontalscrollbarleftbutton"] = m_horizontalScrollbarLeftButton;
            m_horizontalScrollbarLeftButton.name = "horizontalscrollbarleftbutton";

            m_Styles["horizontalscrollbarrightbutton"] = m_horizontalScrollbarRightButton;
            m_horizontalScrollbarRightButton.name = "horizontalscrollbarrightbutton";

            m_Styles["verticalscrollbar"] = m_verticalScrollbar;
            m_verticalScrollbar.name = "verticalscrollbar";

            m_Styles["verticalscrollbarthumb"] = m_verticalScrollbarThumb;
            m_verticalScrollbarThumb.name = "verticalscrollbarthumb";

            m_Styles["verticalscrollbarupbutton"] = m_verticalScrollbarUpButton;
            m_verticalScrollbarUpButton.name = "verticalscrollbarupbutton";

            m_Styles["verticalscrollbardownbutton"] = m_verticalScrollbarDownButton;
            m_verticalScrollbarDownButton.name = "verticalscrollbardownbutton";

            m_Styles["scrollview"] = m_ScrollView;
            m_ScrollView.name = "scrollview";

            if (m_CustomStyles != null)
                for (var i = 0; i < m_CustomStyles.Length; i++)
                {
                    if (m_CustomStyles[i] == null) continue;

                    m_Styles[m_CustomStyles[i].name] = m_CustomStyles[i];
                }

            error.stretchHeight = true;
            error.normal.textColor = Color.red;
        }

        // Get a named [[GUIStyle]].
        public GUIStyle GetStyle(string styleName)
        {
            var s = FindStyle(styleName);
            if (s != null) return s;

            Debug.LogWarning("Unable to find style '" + styleName + "' in skin '" + name + "' " +
                             (Event.current != null ? Event.current.type.ToString() : "<called outside OnGUI>"));
            return error;
        }

        // Try to search for a [[GUIStyle]]. This functions returns NULL and does not give an error.
        public GUIStyle FindStyle(string styleName)
        {
            if (this == null)
            {
                Debug.LogError("GUISkin is NULL");
                return null;
            }

            if (m_Styles == null) BuildStyleCache();

            if (m_Styles.TryGetValue(styleName, out var style)) return style;

            return null;
        }

        //*undocumented* Documented separately
        public IEnumerator GetEnumerator()
        {
            if (m_Styles == null) BuildStyleCache();

            return m_Styles.Values.GetEnumerator();
        }

        internal delegate void SkinChangedDelegate();
    }
}