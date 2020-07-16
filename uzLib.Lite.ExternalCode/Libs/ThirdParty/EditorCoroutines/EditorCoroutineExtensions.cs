using System;
using System.Collections;

#if UNITY_EDITOR

using UnityEditor;

namespace marijnz
{
#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

    /// <summary>
    ///     Editor Coroutine Extensions
    /// </summary>
    public static class EditorCoroutineExtensions
    {
        /// <summary>
        ///     Starts the coroutine.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <param name="coroutine">The coroutine.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static EditorCoroutines.EditorCoroutine StartCoroutine(this EditorWindow thisRef, IEnumerator coroutine)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            return EditorCoroutines.StartCoroutine(coroutine, thisRef);
        }

        /// <summary>
        ///     Starts the coroutine.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static EditorCoroutines.EditorCoroutine StartCoroutine(this EditorWindow thisRef, string methodName)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            return EditorCoroutines.StartCoroutine(methodName, thisRef);
        }

        /// <summary>
        ///     Starts the coroutine.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static EditorCoroutines.EditorCoroutine StartCoroutine(this EditorWindow thisRef, string methodName,
            object value)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            return EditorCoroutines.StartCoroutine(methodName, value, thisRef);
        }

        /// <summary>
        ///     Stops the coroutine.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <param name="coroutine">The coroutine.</param>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static void StopCoroutine(this EditorWindow thisRef, IEnumerator coroutine)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            EditorCoroutines.StopCoroutine(coroutine, thisRef);
        }

        /// <summary>
        ///     Stops the coroutine.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static void StopCoroutine(this EditorWindow thisRef, string methodName)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            EditorCoroutines.StopCoroutine(methodName, thisRef);
        }

        /// <summary>
        ///     Stops all coroutines.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static void StopAllCoroutines(this EditorWindow thisRef)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            EditorCoroutines.StopAllCoroutines(thisRef);
        }

        /// <summary>
        ///     Starts the coroutine.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <param name="coroutine">The coroutine.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static EditorCoroutines.EditorCoroutine StartCoroutine(object thisRef, IEnumerator coroutine)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            return EditorCoroutines.StartCoroutine(coroutine, thisRef);
        }

        /// <summary>
        ///     Starts the coroutine.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static EditorCoroutines.EditorCoroutine StartCoroutine(object thisRef, string methodName)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            return EditorCoroutines.StartCoroutine(methodName, thisRef);
        }

        /// <summary>
        ///     Starts the coroutine.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static EditorCoroutines.EditorCoroutine StartCoroutine(object thisRef, string methodName, object value)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            return EditorCoroutines.StartCoroutine(methodName, value, thisRef);
        }

        /// <summary>
        ///     Stops the coroutine.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <param name="coroutine">The coroutine.</param>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static void StopCoroutine(object thisRef, IEnumerator coroutine)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            EditorCoroutines.StopCoroutine(coroutine, thisRef);
        }

        /// <summary>
        ///     Stops the coroutine.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static void StopCoroutine(object thisRef, string methodName)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            EditorCoroutines.StopCoroutine(methodName, thisRef);
        }

        /// <summary>
        ///     Stops all coroutines.
        /// </summary>
        /// <param name="thisRef">The this reference.</param>
        /// <exception cref="ArgumentNullException">thisRef</exception>
        public static void StopAllCoroutines(object thisRef)
        {
            if (thisRef == null) throw new ArgumentNullException(nameof(thisRef));

            EditorCoroutines.StopAllCoroutines(thisRef);
        }
    }

#endif
}

#endif