namespace UnityEngine.Global.IMGUI
{
    public class GlobalStylesUtility
    {
        // Get one of the built-in GUI skins, which can be the game view, inspector or scene view skin as chosen by the parameter.
        public static GUISkin GetBuiltinSkin(GlobalSkin skin)
        {
            return Resources.Load<GUISkin>($"Skins/{skin.ToString()}");
        }
    }
}