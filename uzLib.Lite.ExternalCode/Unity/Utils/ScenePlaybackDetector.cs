//#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5

#if UNITY_EDITOR

#if false
using UnityEditor;
using UnityEngine;
#endif

using UnityEditor.Callbacks;

namespace uzLib.Lite.ExternalCode.Unity.Utils
{
    public class ScenePlaybackDetector
    {
        /// <summary>
        ///     Gets a value indicating whether this instance is playing. (Alternative to check if we are on editor)
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        public static bool IsPlaying { get; private set; }

        [PostProcessScene]
        public static void OnPostprocessScene()
        {
            // Thanks to: https://github.com/neuecc/UniRx/issues/33
            IsPlaying = true;
        }
    }
}

#else

namespace uzLib.Lite.ExternalCode.Unity.Utils
{
    public static ScenePlaybackDetector
    {
        /// <summary>
        ///     Gets a value indicating whether this instance is playing. (Alternative to check if we are on editor)
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        public static bool IsPlaying => true;
    }
}

#endif
//#endif