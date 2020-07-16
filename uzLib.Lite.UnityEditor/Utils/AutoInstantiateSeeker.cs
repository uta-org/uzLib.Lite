using UnityEditor;
using uzLib.Lite.ExternalCode.Unity.Utils;

namespace UnityEngine.Utils.Editor
{
    using Extensions;

    [InitializeOnLoad]
    public static class AutoInstantiateSeeker
    {
        static AutoInstantiateSeeker()
        {
            var types = typeof(AutoInstantiateAttribute).Assembly.GetTypesWithAttribute<AutoInstantiateAttribute>();

            foreach (var type in types)
            {
                string name = $"{type.FullName}_Instance";

                if (GameObject.Find(name) == null)
                {
                    GameObject obj = new GameObject(name);
                    var component = obj.GetOrAddComponent(type) as MonoBehaviour;

                    if (component == null)
                        continue;

                    // obj.SetActive(true);
                    component.enabled = true;
                }
            }
        }
    }
}