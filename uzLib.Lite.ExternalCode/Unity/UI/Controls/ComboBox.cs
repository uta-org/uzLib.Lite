using System;
using uzLib.Lite.ExternalCode.WinFormsSkins.Core;
using uzLib.Lite.ExternalCode.WinFormsSkins.Workers;

namespace UnityEngine.UI.Controls
{
    // Popup list created by Eric Haines
    // ComboBox Extended by Hyungseok Seo.(Jerry) sdragoon@nate.com
    //
    // -----------------------------------------------
    // This code working like ComboBox Control.
    // I just changed some part of code,
    // because I want to seperate ComboBox button and List.
    // ( You can see the result of this code from Description's last picture )
    // -----------------------------------------------
    //
    // === usage ======================================
    //
    // public class SomeClass : MonoBehaviour
    // {
    //	GUIContent[] comboBoxList;
    //	private ComboBox comboBoxControl = new ComboBox();
    //	private GUIStyle listStyle = new GUIStyle();
    //
    //	private void Start()
    //	{
    //	    comboBoxList = new GUIContent[5];
    //	    comboBoxList[0] = new GUIContent("Thing 1");
    //	    comboBoxList[1] = new GUIContent("Thing 2");
    //	    comboBoxList[2] = new GUIContent("Thing 3");
    //	    comboBoxList[3] = new GUIContent("Thing 4");
    //	    comboBoxList[4] = new GUIContent("Thing 5");
    //
    //	    listStyle.normal.textColor = Color.white;
    //	    listStyle.onHover.background =
    //	    listStyle.hover.background = new Texture2D(2, 2);
    //	    listStyle.padding.left =
    //	    listStyle.padding.right =
    //	    listStyle.padding.top =
    //	    listStyle.padding.bottom = 4;
    //	}
    //
    //	private void OnGUI ()
    //	{
    //	    int selectedItemIndex = comboBoxControl.GetSelectedItemIndex();
    //	    selectedItemIndex = comboBoxControl.List(
    //			new Rect(50, 100, 100, 20), comboBoxList[selectedItemIndex].text, comboBoxList, listStyle );
    //          GUI.Label( new Rect(50, 70, 400, 21),
    //			"You picked " + comboBoxList[selectedItemIndex].text + "!" );
    //	}
    // }
    //
    // =================================================

    public class ComboBox
    {
        private static bool forceToUnShow;
        private static int useControlID = -1;
        private bool isClickedComboButton;

        private int selectedItemIndex;

        private CustomGUI customUI;

        public int List(Rect rect, string buttonText, GUIContent[] listContent, GUIStyle listStyle)
        {
            return List(rect, new GUIContent(buttonText), listContent, "button", "box", listStyle);
        }

        public int List(Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle)
        {
            return List(rect, buttonContent, listContent, "button", "box", listStyle);
        }

        public int List(Rect rect, string buttonText, GUIContent[] listContent, GUIStyle buttonStyle, GUIStyle boxStyle,
            GUIStyle listStyle)
        {
            return List(rect, new GUIContent(buttonText), listContent, buttonStyle, boxStyle, listStyle);
        }

        // TODO: buttonStyle, boxStyle && listStyle doesn't work right now (compare to GUI.skin)
        public int List(Rect rect, GUIContent buttonContent, GUIContent[] listContent,
            GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle)
        {
            customUI = new CustomGUI(SkinWorker.MySkin);

            if (listContent == null)
                throw new ArgumentNullException(nameof(listContent));

            if (forceToUnShow)
            {
                forceToUnShow = false;
                isClickedComboButton = false;
            }

            var done = false;
            var controlID = GUIUtility.GetControlID(FocusType.Passive);

            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.MouseUp:
                    {
                        if (isClickedComboButton) done = true;
                    }
                    break;
            }

            GUIStyle _style = null;

            if (customUI.Button(rect, buttonContent, style =>
            {
                _style = style;
                return style;
            }))
            {
                if (useControlID == -1)
                {
                    useControlID = controlID;
                    isClickedComboButton = false;
                }

                if (useControlID != controlID)
                {
                    forceToUnShow = true;
                    useControlID = controlID;
                }

                isClickedComboButton = true;
            }

            if (isClickedComboButton)
            {
                var listRect = new Rect(rect.x, rect.y + listStyle.CalcHeight(listContent[0], 1.0f),
                    rect.width, listStyle.CalcHeight(listContent[0], 1.0f) * listContent.Length);

                //GUI.Box(listRect, "", _style ?? boxStyle);
                var newSelectedItemIndex = GUI.SelectionGrid(listRect, selectedItemIndex, listContent, 1, _style ?? listStyle);
                if (newSelectedItemIndex != selectedItemIndex) selectedItemIndex = newSelectedItemIndex;
            }

            if (done) isClickedComboButton = false;

            return GetSelectedItemIndex();
        }

        public int GetSelectedItemIndex()
        {
            return selectedItemIndex;
        }
    }
}