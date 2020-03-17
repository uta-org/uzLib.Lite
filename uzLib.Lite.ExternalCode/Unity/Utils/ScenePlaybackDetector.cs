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

        //public static bool AboutToStartScene { get; private set; }

        //// InitializeOnLoad ensures that this constructor is called when the Unity Editor is started.
        //static ScenePlaybackDetector()
        //{
        //    EditorApplication.playModeStateChanged += state =>
        //    {
        //        Debug.Log($"State changed: {state}");

        //        // Before scene start:          isPlayingOrWillChangePlaymode = false;  isPlaying = false
        //        // Pressed Playback button:     isPlayingOrWillChangePlaymode = true;   isPlaying = false
        //        // Playing (after Start()):     isPlayingOrWillChangePlaymode = false;  isPlaying = true
        //        // Pressed stop button:         isPlayingOrWillChangePlaymode = true;   isPlaying = true
        //        // if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
        //        AboutToStartScene = state == PlayModeStateChange.EnteredPlayMode;

        //        // Detect when playback is stopped.
        //        if (!EditorApplication.isPlaying)
        //        {
        //            IsPlaying = false;
        //        }
        //    };
        //}

        //// This callback is notified after scripts have been reloaded.
        //[DidReloadScripts]
        //public static void OnDidReloadScripts()
        //{
        //    Debug.Log("Reloaded scripts!");

        //    // Filter DidReloadScripts callbacks to the moment where playmodeState transitions into isPlaying.
        //    if (AboutToStartScene)
        //    {
        //        IsPlaying = true;

        //        // Ensures the dispatcher GameObject is created by the main thread
        //        //MainThreadDispatcher.Initialize();
        //    }
        //}

        [PostProcessScene]
        public static void OnPostprocessScene()
        {
            //Debug.Log("OnPostprocessScene");
            IsPlaying = true;

            //MainThreadDispatcher.Initialize();
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