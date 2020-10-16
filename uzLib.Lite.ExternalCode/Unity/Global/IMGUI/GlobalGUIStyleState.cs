using System;

namespace UnityEngine.Global.IMGUI
{
    // Specialized values for the given states used by [[GUIStyle]] objects.
    [Serializable]
    public sealed class GlobalGUIStyleState
    {
        // Pointer to the source GUIStyle so it doesn't get garbage collected.
        // If NULL, it means we own m_Ptr and need to delete it when this gets disposed
        private readonly GUIStyle m_SourceStyle;

        // Pointer to the GUIStyleState INSIDE a GUIStyle.
        [NonSerialized] internal IntPtr m_Ptr;

        public Color textColor;

        //public GUIStyleState()
        //{
        //    m_Ptr = Init();
        //}

        private GlobalGUIStyleState(GUIStyle sourceStyle, IntPtr source)
        {
            m_SourceStyle = sourceStyle;
            m_Ptr = source;
        }

        //It's only safe to call this during a deserialization operation.
        internal static GlobalGUIStyleState ProduceGUIStyleStateFromDeserialization(GUIStyle sourceStyle, IntPtr source)
        {
            var newState = new GlobalGUIStyleState(sourceStyle, source);
            return newState;
        }

        internal static GlobalGUIStyleState GetGUIStyleState(GUIStyle sourceStyle, IntPtr source)
        {
            var newState = new GlobalGUIStyleState(sourceStyle, source);
            return newState;
        }

        ~GlobalGUIStyleState()
        {
            if (m_SourceStyle == null)
                //Cleanup();
                m_Ptr = IntPtr.Zero;
        }
    }
}