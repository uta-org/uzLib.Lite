namespace UnityEngine.UI
{
    public class UILayout
    {
        public static GUISkin Skin { get; set; } = GUI.skin;
        public static GUIStyle[] Styles { get; set; }

        public delegate void DoubleClickCallback(int index);

        public static int SelectionList(int selected, GUIContent[] list)
        {
            return SelectionList(selected, list, Skin.button, null);
        }

        public static int SelectionList(int selected, GUIContent[] list, GUIStyle elementStyle)
        {
            return SelectionList(selected, list, elementStyle, null);
        }

        public static int SelectionList(int selected, GUIContent[] list, DoubleClickCallback callback)
        {
            return SelectionList(selected, list, Skin.button, callback);
        }

        public static int SelectionList(int selected, GUIContent[] list, GUIStyle elementStyle,
            DoubleClickCallback callback)
        {
            for (var i = 0; i < list.Length; ++i)
            {
                var elementRect = GUILayoutUtility.GetRect(list[i], elementStyle);
                var hover = elementRect.Contains(Event.current.mousePosition);
                if (hover && Event.current.type == EventType.MouseDown && Event.current.clickCount == 1
                ) // added " && Event.current.clickCount == 1"
                {
                    selected = i;
                    Event.current.Use();
                }
                else if (hover && callback != null && Event.current.type == EventType.MouseDown &&
                         Event.current.clickCount == 2) //Changed from MouseUp to MouseDown
                {
                    callback(i);
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.Repaint)
                {
                    (Styles != null
                        ? selected == i ? Styles[1] : Styles[0]
                        : elementStyle)
                        .Draw(elementRect, list[i], hover, false, i == selected, false);
                }
            }

            return selected;
        }

        public static int SelectionList(int selected, string[] list)
        {
            return SelectionList(selected, list, "button", null);
        }

        public static int SelectionList(int selected, string[] list, GUIStyle elementStyle)
        {
            return SelectionList(selected, list, elementStyle, null);
        }

        public static int SelectionList(int selected, string[] list, DoubleClickCallback callback)
        {
            return SelectionList(selected, list, "button", callback);
        }

        public static int SelectionList(int selected, string[] list, GUIStyle elementStyle,
            DoubleClickCallback callback)
        {
            for (var i = 0; i < list.Length; ++i)
            {
                var elementRect = GUILayoutUtility.GetRect(new GUIContent(list[i]), elementStyle);
                var hover = elementRect.Contains(Event.current.mousePosition);
                if (hover && Event.current.type == EventType.MouseDown)
                {
                    selected = i;
                    Event.current.Use();
                }
                else if (hover && callback != null && Event.current.type == EventType.MouseUp &&
                         Event.current.clickCount == 2)
                {
                    callback(i);
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.Repaint)
                {
                    elementStyle.Draw(elementRect, list[i], hover, false, i == selected, false);
                }
            }

            return selected;
        }
    }
}