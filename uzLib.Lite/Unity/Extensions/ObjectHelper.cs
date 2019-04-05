using UnityEngine;

namespace uzLib.Lite.Unity.Extensions
{
    /// <summary>
    /// The ObjectHelper class
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Removes the colliders.
        /// </summary>
        /// <param name="object">The object.</param>
        public static void RemoveColliders(GameObject @object)
        {
            var colliders = @object.GetComponentsInChildren<Collider>();

            foreach (var col in colliders)
                Object.Destroy(col);
        }

        /// <summary>
        /// Sets the layer recursively.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="newLayer">The new layer.</param>
        public static void SetLayerRecursively(GameObject obj, string newLayer)
        {
            SetLayerRecursively(obj, LayerMask.NameToLayer(newLayer));
        }

        /// <summary>
        /// Sets the layer recursively.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="newLayer">The new layer.</param>
        public static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            obj.layer = newLayer;

            foreach (var child in obj.transform.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.layer != newLayer)
                    SetLayerRecursively(child.gameObject, newLayer);
            }
        }

        /// <summary>
        /// Finds the name of the parent with.
        /// </summary>
        /// <param name="childTransform">The child transform.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static Transform FindParentWithName(this Transform childTransform, string name)
        {
            Transform t = childTransform;
            while (t.parent != null)
            {
                if (t.parent.name == name)
                    return t.parent;

                t = t.parent.transform;
            }

            return null; // Could not find a parent with given tag.
        }
    }
}