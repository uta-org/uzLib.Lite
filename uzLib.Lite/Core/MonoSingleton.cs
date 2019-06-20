using UnityEngine.Core.Interfaces;
using UnityEngine.Extensions;

namespace UnityEngine.Core
{
    /// <summary>
    ///     Inherit from this base class to create a singleton.
    ///     e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour, IStarted
        where T : MonoBehaviour
    {
        // Check to see if we're about to be destroyed.
        private static bool m_ShuttingDown;

        private static readonly object m_Lock = new object();
        protected static T m_Instance;

        /// <summary>
        ///     Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (m_ShuttingDown)
                {
                    // FIX: Shutting down takes affect after playing a scene, this makes (ie) SteamWorkshopWrapper unavailable on Editor
                    // (check for ExecuteInEditrMode attribute, if available, shutthing down shuld not be performed)
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                     "' already destroyed. Returning null.");
                    return null;
                }

                lock (m_Lock)
                {
                    if (m_Instance == null)
                    {
                        // Search for existing instance.
                        m_Instance = (T) FindObjectOfType(typeof(T));

                        // Create new instance if one doesn't already exist.
                        if (m_Instance == null)
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            m_Instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).Name + " (Singleton)";

                            //((MonoSingleton<T>)m_Instance).IsStarted = false;

                            // Make instance persistent.

                            if (Application.isPlaying) DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return m_Instance;
                }
            }
            protected set => m_Instance = value;
        }

        public bool ExecuteInEditMode => GetType().IsExecutingInEditMode();

        public bool IsStarted { get; set; }

        public static T Create()
        {
            var go = new GameObject(typeof(T).Name);
            return go.GetOrAddComponent<T>();
        }

        private void OnApplicationQuit()
        {
            if (!ExecuteInEditMode) m_ShuttingDown = true;
        }

        private void OnDestroy()
        {
            if (!ExecuteInEditMode) m_ShuttingDown = true;
        }
    }
}