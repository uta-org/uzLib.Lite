#if !(!UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5)

using UnityEngine.Core;

#else

using uzLib.Lite.ExternalCode.Core;

#endif

namespace uzLib.Lite.ExternalCode.Utils
{
    //[AutoInstantiate]
    public abstract class HandledMonoBehaviour : MonoSingleton<HandledMonoBehaviour>
    {
        public abstract void OnQuit();

        public void OnApplicationQuit()
        {
            OnQuit();
        }
    }
}