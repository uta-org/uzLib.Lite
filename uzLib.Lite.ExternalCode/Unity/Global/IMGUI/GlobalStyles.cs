using System;

namespace UnityEngine.Global.IMGUI
{
    // Common GUIStyles used for EditorGUI controls.
    public class GlobalStyles
    {
        // the editor styles currently in use
        public static GlobalStyles s_Current;

        // the list of editor styles to use
        private static GlobalStyles[] s_CachedStyles = { null, null };

        private readonly Vector2 m_KnobSize = new Vector2(40, 40);
        private readonly Vector2 m_MiniKnobSize = new Vector2(29, 29);

        private GUIStyle m_AssetLabel;
        private GUIStyle m_AssetLabelIcon;
        private GUIStyle m_AssetLabelPartial;

        public Font m_BoldFont;

        private GUIStyle m_BoldLabel;

        private GUIStyle m_CenteredGreyMiniLabel;

        private GUIStyle m_ColorField;
        private GUIStyle m_ColorPickerBox;
        private GUIStyle m_DropDownList;

        private GUIStyle m_Foldout;

        private GUIStyle m_FoldoutPreDrop;
        private GUIStyle m_FoldoutSelected;
        private GUIStyle m_HelpBox;
        private GUIStyle m_IconButton;
        private GUIStyle m_InspectorBig;
        private GUIStyle m_InspectorDefaultMargins;
        private GUIStyle m_InspectorFullWidthMargins;
        private GUIStyle m_InspectorTitlebar;
        private GUIStyle m_InspectorTitlebarText;

        public GUIStyle m_Label;

        private GUIStyle m_LargeLabel;

        private GUIStyle m_LayerMaskField;

        private GUIStyle m_LinkLabel;

        public Font m_MiniBoldFont;

        private GUIStyle m_MiniBoldLabel;

        private GUIStyle m_MiniButton;

        private GUIStyle m_MiniButtonLeft;

        private GUIStyle m_MiniButtonMid;

        private GUIStyle m_MiniButtonRight;

        public Font m_MiniFont;

        private GUIStyle m_MiniLabel;
        private GUIStyle m_MiniPullDown;

        private GUIStyle m_MiniTextField;
        private GUIStyle m_MinMaxHorizontalSliderThumb;
        private GUIStyle m_MinMaxStateDropdown;

        private GUIStyle m_NotificationBackground;

        private GUIStyle m_NotificationText;

        private GUIStyle m_NumberField;

        private GUIStyle m_ObjectField;

        private GUIStyle m_ObjectFieldMiniThumb;

        private GUIStyle m_ObjectFieldThumb;
        private GUIStyle m_OverrideMargin;

        private GUIStyle m_Popup;
        private GUIStyle m_ProgressBarBar, m_ProgressBarText, m_ProgressBarBack;

        private GUIStyle m_RadioButton;
        private GUIStyle m_SearchField;
        private GUIStyle m_SearchFieldCancelButton;
        private GUIStyle m_SearchFieldCancelButtonEmpty;
        private GUIStyle m_SelectionRect;

        public Font m_StandardFont;

        public GUIStyle m_TextArea;

        public GUIStyle m_TextField;
        private GUIStyle m_TextFieldDropDown;
        private GUIStyle m_TextFieldDropDownText;

        private GUIStyle m_Toggle;

        private GUIStyle m_ToggleGroup;
        private GUIStyle m_ToggleMixed;

        private GUIStyle m_Toolbar;

        private GUIStyle m_ToolbarButton;

        private GUIStyle m_ToolbarDropDown;

        private GUIStyle m_ToolbarPopup;
        private GUIStyle m_ToolbarSearchField;
        private GUIStyle m_ToolbarSearchFieldCancelButton;
        private GUIStyle m_ToolbarSearchFieldCancelButtonEmpty;
        private GUIStyle m_ToolbarSearchFieldPopup;

        private GUIStyle m_ToolbarTextField;

        private GUIStyle m_Tooltip;

        private GUIStyle m_WhiteBoldLabel;

        private GUIStyle m_WhiteLabel;

        private GUIStyle m_WhiteLargeLabel;

        private GUIStyle m_WhiteMiniLabel;

        private GUIStyle m_WordWrappedLabel;

        private GUIStyle m_WordWrappedMiniLabel;

        static GlobalStyles()
        {
            s_Current = new GlobalStyles();
            s_Current.InitSharedStyles();
        }

        // Style used for the labeled on all EditorGUI overloads that take a prefix label
        public static GUIStyle label => s_Current.m_Label;

        // Style for label with small font.
        public static GUIStyle miniLabel => s_Current.m_MiniLabel;

        // Style for label with large font.
        public static GUIStyle largeLabel => s_Current.m_LargeLabel;

        // Style for bold label.
        public static GUIStyle boldLabel => s_Current.m_BoldLabel;

        // Style for mini bold label.
        public static GUIStyle miniBoldLabel => s_Current.m_MiniBoldLabel;

        // Style for centered grey mini label.
        public static GUIStyle centeredGreyMiniLabel => s_Current.m_CenteredGreyMiniLabel;

        // Style for word wrapped mini label.
        public static GUIStyle wordWrappedMiniLabel => s_Current.m_WordWrappedMiniLabel;

        // Style for word wrapped label.
        public static GUIStyle wordWrappedLabel => s_Current.m_WordWrappedLabel;

        // Style for link label.
        public static GUIStyle linkLabel => s_Current.m_LinkLabel;

        // Style for white label.
        public static GUIStyle whiteLabel => s_Current.m_WhiteLabel;

        // Style for white mini label.
        public static GUIStyle whiteMiniLabel => s_Current.m_WhiteMiniLabel;

        // Style for white large label.
        public static GUIStyle whiteLargeLabel => s_Current.m_WhiteLargeLabel;

        // Style for white bold label.
        public static GUIStyle whiteBoldLabel => s_Current.m_WhiteBoldLabel;

        // Style used for a radio button
        public static GUIStyle radioButton => s_Current.m_RadioButton;

        // Style used for a standalone small button.
        public static GUIStyle miniButton => s_Current.m_MiniButton;

        // Style used for the leftmost button in a horizontal button group.
        public static GUIStyle miniButtonLeft => s_Current.m_MiniButtonLeft;

        // Style used for the middle buttons in a horizontal group.
        public static GUIStyle miniButtonMid => s_Current.m_MiniButtonMid;

        // Style used for the rightmost button in a horizontal group.
        public static GUIStyle miniButtonRight => s_Current.m_MiniButtonRight;

        public static GUIStyle miniPullDown => s_Current.m_MiniPullDown;

        // Style used for EditorGUI::ref::TextField
        public static GUIStyle textField => s_Current.m_TextField;

        // Style used for EditorGUI::ref::TextArea
        public static GUIStyle textArea => s_Current.m_TextArea;

        // Smaller text field
        public static GUIStyle miniTextField => s_Current.m_MiniTextField;

        // Style used for field editors for numbers
        public static GUIStyle numberField => s_Current.m_NumberField;

        // Style used for EditorGUI::ref::Popup, EditorGUI::ref::EnumPopup,
        public static GUIStyle popup => s_Current.m_Popup;

        // Style used for headings for structures (Vector3, Rect, etc)
        [Obsolete("structHeadingLabel is deprecated, use GlobalStyles.label instead.")]
        public static GUIStyle structHeadingLabel => s_Current.m_Label;

        // Style used for headings for object fields.
        public static GUIStyle objectField => s_Current.m_ObjectField;

        // Style used for headings for the Select button in object fields.
        public static GUIStyle objectFieldThumb => s_Current.m_ObjectFieldThumb;

        // Style used for texture object field with minimal height (useful for single line texture objectfields)
        public static GUIStyle objectFieldMiniThumb => s_Current.m_ObjectFieldMiniThumb;

        // Style used for headings for Color fields.
        public static GUIStyle colorField => s_Current.m_ColorField;

        // Style used for headings for Layer masks.
        public static GUIStyle layerMaskField => s_Current.m_LayerMaskField;

        // Style used for headings for EditorGUI::ref::Toggle.
        public static GUIStyle toggle => s_Current.m_Toggle;

        public static GUIStyle toggleMixed => s_Current.m_ToggleMixed;

        // Style used for headings for EditorGUI::ref::Foldout.
        public static GUIStyle foldout => s_Current.m_Foldout;

        // Style used for headings for EditorGUI::ref::Foldout.
        public static GUIStyle foldoutPreDrop => s_Current.m_FoldoutPreDrop;

        // Style used for headings for EditorGUILayout::ref::BeginToggleGroup.
        public static GUIStyle toggleGroup => s_Current.m_ToggleGroup;

        public static GUIStyle textFieldDropDown => s_Current.m_TextFieldDropDown;

        public static GUIStyle textFieldDropDownText => s_Current.m_TextFieldDropDownText;

        public static GUIStyle overrideMargin => s_Current.m_OverrideMargin;

        // Standard font.
        public static Font standardFont => s_Current.m_StandardFont;

        // Bold font.
        public static Font boldFont => s_Current.m_BoldFont;

        // Mini font.
        public static Font miniFont => s_Current.m_MiniFont;

        // Mini Bold font.
        public static Font miniBoldFont => s_Current.m_MiniBoldFont;

        // Toolbar background from top of windows.
        public static GUIStyle toolbar => s_Current.m_Toolbar;

        // Style for Button and Toggles in toolbars.
        public static GUIStyle toolbarButton => s_Current.m_ToolbarButton;

        // Toolbar Popup
        public static GUIStyle toolbarPopup => s_Current.m_ToolbarPopup;

        // Toolbar Dropdown
        public static GUIStyle toolbarDropDown => s_Current.m_ToolbarDropDown;

        // Toolbar text field
        public static GUIStyle toolbarTextField => s_Current.m_ToolbarTextField;

        public static GUIStyle inspectorDefaultMargins => s_Current.m_InspectorDefaultMargins;

        public static GUIStyle inspectorFullWidthMargins => s_Current.m_InspectorFullWidthMargins;

        public static GUIStyle helpBox => s_Current.m_HelpBox;

        public static GUIStyle toolbarSearchField => s_Current.m_ToolbarSearchField;

        public static GUIStyle toolbarSearchFieldPopup => s_Current.m_ToolbarSearchFieldPopup;

        public static GUIStyle toolbarSearchFieldCancelButton => s_Current.m_ToolbarSearchFieldCancelButton;

        public static GUIStyle toolbarSearchFieldCancelButtonEmpty => s_Current.m_ToolbarSearchFieldCancelButtonEmpty;

        public static GUIStyle colorPickerBox => s_Current.m_ColorPickerBox;

        public static GUIStyle inspectorBig => s_Current.m_InspectorBig;

        public static GUIStyle inspectorTitlebar => s_Current.m_InspectorTitlebar;

        public static GUIStyle inspectorTitlebarText => s_Current.m_InspectorTitlebarText;

        public static GUIStyle foldoutSelected => s_Current.m_FoldoutSelected;

        public static GUIStyle iconButton => s_Current.m_IconButton;

        // Style for tooltips
        public static GUIStyle tooltip => s_Current.m_Tooltip;

        // Style for notification text.
        public static GUIStyle notificationText => s_Current.m_NotificationText;

        // Style for notification background area.
        public static GUIStyle notificationBackground => s_Current.m_NotificationBackground;

        public static GUIStyle assetLabel => s_Current.m_AssetLabel;

        public static GUIStyle assetLabelPartial => s_Current.m_AssetLabelPartial;

        public static GUIStyle assetLabelIcon => s_Current.m_AssetLabelIcon;

        public static GUIStyle searchField => s_Current.m_SearchField;

        public static GUIStyle searchFieldCancelButton => s_Current.m_SearchFieldCancelButton;

        public static GUIStyle searchFieldCancelButtonEmpty => s_Current.m_SearchFieldCancelButtonEmpty;

        public static GUIStyle selectionRect => s_Current.m_SelectionRect;

        public static GUIStyle minMaxHorizontalSliderThumb => s_Current.m_MinMaxHorizontalSliderThumb;

        public static GUIStyle dropDownList => s_Current.m_DropDownList;

        public static GUIStyle minMaxStateDropdown => s_Current.m_MinMaxStateDropdown;

        public static GUIStyle progressBarBack => s_Current.m_ProgressBarBack;
        public static GUIStyle progressBarBar => s_Current.m_ProgressBarBar;
        public static GUIStyle progressBarText => s_Current.m_ProgressBarText;

        public static Vector2 knobSize => s_Current.m_KnobSize;
        public static Vector2 miniKnobSize => s_Current.m_MiniKnobSize;

        private void InitSharedStyles()
        {
            m_ColorPickerBox = GetStyle("ColorPickerBox");
            m_InspectorBig = GetStyle("In BigTitle");
            m_MiniLabel = GetStyle("miniLabel");
            m_LargeLabel = GetStyle("LargeLabel");
            m_BoldLabel = GetStyle("BoldLabel");
            m_MiniBoldLabel = GetStyle("MiniBoldLabel");
            m_WordWrappedLabel = GetStyle("WordWrappedLabel");
            m_WordWrappedMiniLabel = GetStyle("WordWrappedMiniLabel");
            m_WhiteLabel = GetStyle("WhiteLabel");
            m_WhiteMiniLabel = GetStyle("WhiteMiniLabel");
            m_WhiteLargeLabel = GetStyle("WhiteLargeLabel");
            m_WhiteBoldLabel = GetStyle("WhiteBoldLabel");
            m_MiniTextField = GetStyle("MiniTextField");
            m_RadioButton = GetStyle("Radio");
            m_MiniButton = GetStyle("miniButton");
            m_MiniButtonLeft = GetStyle("miniButtonLeft");
            m_MiniButtonMid = GetStyle("miniButtonMid");
            m_MiniButtonRight = GetStyle("miniButtonRight");
            m_MiniPullDown = GetStyle("MiniPullDown");
            m_Toolbar = GetStyle("toolbar");
            m_ToolbarButton = GetStyle("toolbarbutton");
            m_ToolbarPopup = GetStyle("toolbarPopup");
            m_ToolbarDropDown = GetStyle("toolbarDropDown");
            m_ToolbarTextField = GetStyle("toolbarTextField");
            m_ToolbarSearchField = GetStyle("ToolbarSeachTextField");
            m_ToolbarSearchFieldPopup = GetStyle("ToolbarSeachTextFieldPopup");
            m_ToolbarSearchFieldCancelButton = GetStyle("ToolbarSeachCancelButton");
            m_ToolbarSearchFieldCancelButtonEmpty = GetStyle("ToolbarSeachCancelButtonEmpty");
            m_SearchField = GetStyle("SearchTextField");
            m_SearchFieldCancelButton = GetStyle("SearchCancelButton");
            m_SearchFieldCancelButtonEmpty = GetStyle("SearchCancelButtonEmpty");
            m_HelpBox = GetStyle("HelpBox");
            m_AssetLabel = GetStyle("AssetLabel");
            m_AssetLabelPartial = GetStyle("AssetLabel Partial");
            m_AssetLabelIcon = GetStyle("AssetLabel Icon");
            m_SelectionRect = GetStyle("selectionRect");
            m_MinMaxHorizontalSliderThumb = GetStyle("MinMaxHorizontalSliderThumb");
            m_DropDownList = GetStyle("DropDownButton");
            m_MinMaxStateDropdown = GetStyle("IN MinMaxStateDropdown");
            m_BoldFont = GetStyle("BoldLabel")?.font;
            m_StandardFont = GetStyle("Label")?.font;
            m_MiniFont = GetStyle("MiniLabel")?.font;
            m_MiniBoldFont = GetStyle("MiniBoldLabel")?.font;
            m_ProgressBarBack = GetStyle("ProgressBarBack");
            m_ProgressBarBar = GetStyle("ProgressBarBar");
            m_ProgressBarText = GetStyle("ProgressBarText");
            m_FoldoutPreDrop = GetStyle("FoldoutPreDrop");
            m_InspectorTitlebar = GetStyle("IN Title");
            m_InspectorTitlebarText = GetStyle("IN TitleText");
            m_ToggleGroup = GetStyle("BoldToggle");
            m_Tooltip = GetStyle("Tooltip");
            m_NotificationText = GetStyle("NotificationText");
            m_NotificationBackground = GetStyle("NotificationBackground");

            // Former LookLikeControls styles
            m_Popup = m_LayerMaskField = GetStyle("MiniPopup");
            m_TextField = m_NumberField = GetStyle("textField");
            m_Label = GetStyle("ControlLabel");
            m_ObjectField = GetStyle("ObjectField");
            m_ObjectFieldThumb = GetStyle("ObjectFieldThumb");
            m_ObjectFieldMiniThumb = GetStyle("ObjectFieldMiniThumb");
            m_Toggle = GetStyle("Toggle");
            m_ToggleMixed = GetStyle("ToggleMixed");
            m_ColorField = GetStyle("ColorField");
            m_Foldout = GetStyle("Foldout");
            m_FoldoutSelected = GUIStyle.none;
            m_IconButton = GetStyle("IconButton");
            m_TextFieldDropDown = GetStyle("TextFieldDropDown");
            m_TextFieldDropDownText = GetStyle("TextFieldDropDownText");

            m_OverrideMargin = GetStyle("OverrideMargin");

            m_LinkLabel = new GUIStyle(m_Label)
            {
                normal = { textColor = new Color(0.25f, 0.5f, 0.9f, 1f) },
                stretchWidth = false
            };

            // Match selection color which works nicely for both light and dark skins

            m_TextArea = new GUIStyle(m_TextField) { wordWrap = true };

            m_InspectorDefaultMargins = new GUIStyle
            {
                padding = new RectOffset(
                    GlobalConstants.kInspectorPaddingLeft,
                    GlobalConstants.kInspectorPaddingRight, 0, 0)
            };

            // For the full width margins, use padding from right side in both sides,
            // though adjust for overdraw by adding one in left side to get even margins.
            m_InspectorFullWidthMargins = new GUIStyle
            {
                padding = new RectOffset(
                    GlobalConstants.kInspectorPaddingRight + 1,
                    GlobalConstants.kInspectorPaddingRight, 0, 0)
            };

            // Derive centered grey mini label from base minilabel
            m_CenteredGreyMiniLabel = new GUIStyle(m_MiniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.grey }
            };
        }

        public static GUIStyle GetStyle(string styleName)
        {
            // TODO: Pass JSON serialized buildin skin for Inspector, Editor and Game...

            try
            {
                var s = GUI.skin.FindStyle(styleName) ??
                        GlobalStylesUtility.GetBuiltinSkin(GlobalSkin.Inspector).FindStyle(styleName);

                //  ?? GlobalStylesUtility.GetBuiltinSkin(GlobalSkin.Inspector).FindStyle(styleName.ToLower())
                if (s == null)
                {
                    PromptMissingStyle(styleName);
                    s = GlobalGUISkin.error;
                }

                return s;
            }
            catch
            {
                // Not found style
                // PromptMissingStyle(styleName);

                return null;
            }
        }

        private static void PromptMissingStyle(string styleName)
        {
            Debug.LogError("Missing built-in guistyle " + styleName);
        }

        #region "Custom GlobalStyles"

        private static GUIStyle s_styleWithBackground;

        public static GUIStyle styleWithBackground =>
            s_styleWithBackground ??
            (s_styleWithBackground = new GUIStyle { normal = { background = Texture2D.whiteTexture } });

        private static GUIStyle s_centeredLabelStyle;

        public static GUIStyle CenteredLabelStyle =>
            s_centeredLabelStyle ?? (s_centeredLabelStyle = new GUIStyle("label")
            { alignment = TextAnchor.MiddleCenter });

        public static GUIStyle CenteredStyle(string name, int fontSize = 12)
        {
            return new GUIStyle(name) { alignment = TextAnchor.MiddleCenter, fontSize = fontSize };
        }

        #endregion "Custom GlobalStyles"
    }
}