namespace UnityEngine.Global.IMGUI
{
    //
    // Summary:
    //     Enum that selects which skin to return from EditorGUIUtility.GetBuiltinSkin.
    public enum GlobalSkin
    {
        //
        // Summary:
        //     The skin used for game views.
        Game = 0,

        //
        // Summary:
        //     The skin used for inspectors.
        Inspector = 1,

        //
        // Summary:
        //     The skin used for Scene views.
        Scene = 2
    }

    // How image and text is placed inside [[GUIStyle]].
    public enum ImagePosition
    {
        // Image is to the left of the text.
        ImageLeft = 0,

        // Image is above the text.
        ImageAbove = 1,

        // Only the image is displayed.
        ImageOnly = 2,

        // Only the text is displayed.
        TextOnly = 3
    }

    // Different methods for how the GUI system handles text being too large to fit the rectangle allocated.
    public enum TextClipping
    {
        // Text flows freely outside the element.
        Overflow = 0,

        // Text gets clipped to be inside the element.
        Clip = 1

        // Text gets truncated with dots to show it is too long
        //  Truncate = 2
    }

    public enum ScreenCenterContent
    {
        None,
        Vertical,
        Horizontal
    }
}