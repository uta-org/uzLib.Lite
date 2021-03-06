﻿using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using static marijnz.EditorCoroutines;

#endif

namespace uzLib.Lite.ExternalCode.Extensions
{
    public static class CoroutineHelper
    {
        /// <summary>
        ///     Creates the smart corotine.
        /// </summary>
        /// <param name="enumerator">The enumerator.</param>
        /// <param name="thisReference">The this reference.</param>
        /// <param name="mono">The mono.</param>
        /// <returns></returns>
        public static YieldInstruction CreateSmartCorotine(this IEnumerator enumerator, object thisReference,
            MonoBehaviour mono)
        {
            if (thisReference == null)
            {
                // !isEditor
                if (mono == null)
                    throw new ArgumentNullException(nameof(mono));

                return mono.StartCoroutine(enumerator);
            }

#if UNITY_EDITOR
            return new YieldCoroutine(enumerator, thisReference);
#else
            throw new Exception("Editor coroutines must be called within the Editor!");
#endif
        }

        /// <summary>
        ///     Starts the smart corotine.
        /// </summary>
        /// <param name="enumerator">The enumerator.</param>
        /// <param name="thisReference">The this reference.</param>
        /// <param name="mono">The mono.</param>
        /// <returns></returns>
        public static YieldInstruction StartSmartCorotine(this IEnumerator enumerator, object thisReference,
            MonoBehaviour mono)
        {
            var instruction = CreateSmartCorotine(enumerator, thisReference, mono);

            if (thisReference != null) // isEditor
            {
#if UNITY_EDITOR
                StartCoroutine(instruction as YieldCoroutine);
#endif
            }

            return instruction;
        }

        /// <summary>
        ///     Stops the smart coroutine.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="thisReference">The this reference.</param>
        /// <param name="mono">The mono.</param>
        public static void StopSmartCoroutine(this YieldInstruction instruction, object thisReference,
            MonoBehaviour mono)
        {
            if (thisReference != null) // isEditor
            {
#if UNITY_EDITOR
                StopCoroutine((instruction as YieldCoroutine)?.enumerator, thisReference);
#endif
            }
            else
            {
                mono.StopCoroutine(instruction as Coroutine);
            }
        }
    }
}