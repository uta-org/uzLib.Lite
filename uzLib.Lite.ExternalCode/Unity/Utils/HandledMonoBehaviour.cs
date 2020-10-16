using uzLib.Lite.ExternalCode.Core;
using uzLib.Lite.ExternalCode.Unity.Utils;

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