using System;
using System.Linq;
using UnityEngine;
using uzLib.Lite.ExternalCode.Extensions;
using Object = UnityEngine.Object;

namespace uzLib.Lite.ExternalCode.Unity.Extensions
{
    /// <summary>
    /// The ObjectHelper class
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Removes the components recursively.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object">The object.</param>
        public static void RemoveComponentsRecursively<T>(this GameObject @object)
            where T : Component
        {
            var components = @object.GetComponentsInChildren<T>();

            foreach (var comp in components)
                Object.Destroy(comp);
        }

        /// <summary>
        /// Removes the colliders.
        /// </summary>
        /// <param name="object">The object.</param>
        public static void RemoveColliders(this GameObject @object)
        {
            @object.RemoveComponentsRecursively<Collider>();
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

        /// <summary>
        /// Returns the topmost parent with a certain component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tr">The tr.</param>
        /// <returns></returns>
        public static Component GetTopmostParentComponent<T>(Transform tr) where T : Component
        {
            Component getting = null;

            while (tr.parent != null)
            {
                if (tr.parent.GetComponent<T>() != null)
                {
                    getting = tr.parent.GetComponent<T>();
                }

                tr = tr.parent;
            }

            return getting;
        }

        /// <summary>
        /// Gets the name of the component with.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root">The root.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static T GetComponentWithName<T>(this Component root, string name) where T : Component
        {
            return root.GetComponentsInChildren<T>().FirstOrDefault(x => x.name == name);
        }

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        /// Gets or add component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go">The go.</param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (null == comp)
                comp = go.AddComponent<T>();
            return comp;
        }

#endif

        /// <summary>
        /// Gets the component or throw.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go">The go.</param>
        /// <returns></returns>
        /// <exception cref="MissingComponentException"></exception>
        public static T GetComponentOrThrow<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (null == comp)
                throw new MissingComponentException(string.Format("Failed to get component of type: {0}, on game object: {1}", typeof(T), go.name));
            return comp;
        }

        /// <summary>
        /// Gets the component or throw.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comp">The comp.</param>
        /// <returns></returns>
        public static T GetComponentOrThrow<T>(this Component comp) where T : Component
        {
            return comp.gameObject.GetComponentOrThrow<T>();
        }

        /// <summary>
        /// Gets the component or log error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go">The go.</param>
        /// <returns></returns>
        public static T GetComponentOrLogError<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (null == comp)
                Debug.LogErrorFormat("Failed to get component of type: {0}, on game object: {1}", typeof(T), go.name);
            return comp;
        }

        /// <summary>
        /// Gets the component or log error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comp">The comp.</param>
        /// <returns></returns>
        public static T GetComponentOrLogError<T>(this Component comp) where T : Component
        {
            return comp.gameObject.GetComponentOrLogError<T>();
        }

        /// <summary>
        /// Makes the child.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="children">The children.</param>
        public static void MakeChild(this Transform parent, GameObject[] children)
        {
            MakeChild(parent, children, null);
        }

        /// <summary>
        /// Make the game objects children of the parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="children">The children.</param>
        /// <param name="actionPerLoop">The action per loop.</param>
        public static void MakeChild(this Transform parent, GameObject[] children, Action<Transform, GameObject> actionPerLoop)
        {
            foreach (GameObject child in children)
            {
                child.transform.parent = parent;
                actionPerLoop?.Invoke(parent, child);
            }
        }

        /// <summary>
        /// Safes the destroy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        public static void SafeDestroy<T>(this T obj) where T : Object
        {
            if (Application.isEditor)
                Object.DestroyImmediate(obj);
            else
                Object.Destroy(obj);
        }

        /// <summary>
        /// Safes the destroy game object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component">The component.</param>
        public static void SafeDestroyGameObject<T>(this T component) where T : Component
        {
            if (component != null)
                SafeDestroy(component.gameObject);
        }

        /// <summary>
        /// Gets the game object path.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string GetGameObjectPath(this GameObject obj)
        {
            string path = obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        /// <summary>
        /// Sends the type of the message to objects of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        public static void SendMessageToObjectsOfType<T>(string msg, params object[] args) where T : UnityEngine.Object
        {
            var objects = Object.FindObjectsOfType<T>();

            foreach (var obj in objects)
            {
                obj.InvokeExceptionSafe(msg, args);
            }
        }

        /// <summary>
        /// Gets the or add component.
        /// </summary>
        /// <param name="go">The go.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">go</exception>
        public static Component GetOrAddComponent(this GameObject go, Type type)
        {
            if (go == null)
                throw new ArgumentNullException(nameof(go));

            var comp = go.GetComponent(type);

            if (null == comp)
                comp = go.AddComponent(type);

            return comp;
        }
    }
}