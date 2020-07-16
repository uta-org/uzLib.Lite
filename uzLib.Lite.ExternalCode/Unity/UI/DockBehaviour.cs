using System;
using UnityEngine.Extensions;
using uzLib.Lite.ExternalCode.Unity.Utils;

namespace UnityEngine.UI
{
    [AutoInstantiate]
    public class DockBehaviour : MonoBehaviour
    {
        public event Action OnUpdate = delegate { };

        internal static bool IsShown { get; set; }

        //public static bool? IsEditor { get; set; }

        private static GUIStyle buttonStyle;

        private bool isInit;

        private void Update()
        {
            OnUpdate();
        }

        private void OnGUI()
        {
            if (!isInit)
            {
                var normal = new GUIStyle(GUI.skin.button).normal;
                normal.background = TextureHelper.CreateTexture(16, 16, new Color(0, 0, 0, .5f));
                // Texture2D.blackTexture;

                buttonStyle = new GUIStyle("button")
                {
                    margin = new RectOffset(),
                    padding = new RectOffset(),
                    border = new RectOffset(),
                    active = normal,
                    hover = normal,
                    normal = normal,
                    onNormal = normal,
                    onHover = normal,
                    onActive = normal
                };
                // , active = normal, hover = normal, normal = normal };

                isInit = true;
            }

            if (IsShown)
            {
                GUI.depth = 1;
                GUILayout.Button("", buttonStyle, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));
            }
        }
    }
}