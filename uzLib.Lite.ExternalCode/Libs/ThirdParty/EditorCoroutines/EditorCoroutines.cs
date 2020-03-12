using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR

using System.Collections.Generic;

namespace marijnz
{
#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

    /// <summary>
    ///     The Editor Coroutines
    /// </summary>
    public class EditorCoroutines
    {
        /// <summary>
        ///     The instance
        /// </summary>
        private static EditorCoroutines instance;

        /// <summary>
        ///     The coroutine dictionary
        /// </summary>
        private readonly Dictionary<string, List<EditorCoroutine>> coroutineDict =
            new Dictionary<string, List<EditorCoroutine>>();

        /// <summary>
        ///     The coroutine owner dictionary
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, EditorCoroutine>> coroutineOwnerDict =
            new Dictionary<string, Dictionary<string, EditorCoroutine>>();

        /// <summary>
        ///     The temporary coroutine list
        /// </summary>
        private readonly List<List<EditorCoroutine>> tempCoroutineList = new List<List<EditorCoroutine>>();

        /// <summary>
        ///     The previous time since startup
        /// </summary>
        private DateTime previousTimeSinceStartup;

        /// <summary>
        ///     Starts the coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine.</param>
        /// <returns></returns>
        public static EditorCoroutine StartCoroutine(YieldCoroutine coroutine)
        {
            CreateInstanceIfNeeded();
            return instance.GoStartCoroutine(coroutine.enumerator, coroutine.thisReference);
        }

        /// <summary>Starts a coroutine.</summary>
        /// <param name="routine">The coroutine to start.</param>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static EditorCoroutine StartCoroutine(IEnumerator routine, object thisReference)
        {
            CreateInstanceIfNeeded();
            return instance.GoStartCoroutine(routine, thisReference);
        }

        /// <summary>Starts a coroutine.</summary>
        /// <param name="methodName">The name of the coroutine method to start.</param>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static EditorCoroutine StartCoroutine(string methodName, object thisReference)
        {
            return StartCoroutine(methodName, null, thisReference);
        }

        /// <summary>Starts a coroutine.</summary>
        /// <param name="methodName">The name of the coroutine method to start.</param>
        /// <param name="value">The parameter to pass to the coroutine.</param>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static EditorCoroutine StartCoroutine(string methodName, object value, object thisReference)
        {
            var methodInfo = thisReference.GetType()
                .GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
                Debug.LogError("Coroutine '" + methodName + "' couldn't be started, the method doesn't exist!");
            object returnValue;

            if (value == null)
                returnValue = methodInfo.Invoke(thisReference, null);
            else
                returnValue = methodInfo.Invoke(thisReference, new[] { value });

            if (returnValue is IEnumerator)
            {
                CreateInstanceIfNeeded();
                return instance.GoStartCoroutine((IEnumerator)returnValue, thisReference);
            }

            Debug.LogError("Coroutine '" + methodName +
                           "' couldn't be started, the method doesn't return an IEnumerator!");

            return null;
        }

        /// <summary>Stops all coroutines being the routine running on the passed instance.</summary>
        /// <param name="routine"> The coroutine to stop.</param>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static void StopCoroutine(IEnumerator routine, object thisReference)
        {
            CreateInstanceIfNeeded();
            instance.GoStopCoroutine(routine, thisReference);
        }

        /// <summary>
        ///     Stops all coroutines named methodName running on the passed instance.
        /// </summary>
        /// <param name="methodName"> The name of the coroutine method to stop.</param>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static void StopCoroutine(string methodName, object thisReference)
        {
            CreateInstanceIfNeeded();
            instance.GoStopCoroutine(methodName, thisReference);
        }

        /// <summary>
        ///     Stops all coroutines running on the passed instance.
        /// </summary>
        /// <param name="thisReference">Reference to the instance of the class containing the method.</param>
        public static void StopAllCoroutines(object thisReference)
        {
            CreateInstanceIfNeeded();
            instance.GoStopAllCoroutines(thisReference);
        }

        /// <summary>
        ///     Creates the instance if needed.
        /// </summary>
        private static void CreateInstanceIfNeeded()
        {
            if (instance == null)
            {
                instance = new EditorCoroutines();
                instance.Initialize();
            }
        }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            previousTimeSinceStartup = DateTime.Now;
            EditorApplication.update += OnUpdate;
        }

        /// <summary>
        ///     Goes the stop coroutine.
        /// </summary>
        /// <param name="routine">The routine.</param>
        /// <param name="thisReference">The this reference.</param>
        private void GoStopCoroutine(IEnumerator routine, object thisReference)
        {
            GoStopActualRoutine(CreateCoroutine(routine, thisReference));
        }

        /// <summary>
        ///     Goes the stop coroutine.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="thisReference">The this reference.</param>
        private void GoStopCoroutine(string methodName, object thisReference)
        {
            GoStopActualRoutine(CreateCoroutineFromString(methodName, thisReference));
        }

        /// <summary>
        ///     Goes the stop actual routine.
        /// </summary>
        /// <param name="routine">The routine.</param>
        private void GoStopActualRoutine(EditorCoroutine routine)
        {
            if (coroutineDict.ContainsKey(routine.routineUniqueHash))
            {
                coroutineOwnerDict[routine.ownerUniqueHash].Remove(routine.routineUniqueHash);
                coroutineDict.Remove(routine.routineUniqueHash);
            }
        }

        /// <summary>
        ///     Goes the stop all coroutines.
        /// </summary>
        /// <param name="thisReference">The this reference.</param>
        private void GoStopAllCoroutines(object thisReference)
        {
            var coroutine = CreateCoroutine(null, thisReference);
            if (coroutineOwnerDict.ContainsKey(coroutine.ownerUniqueHash))
            {
                foreach (var couple in coroutineOwnerDict[coroutine.ownerUniqueHash])
                    coroutineDict.Remove(couple.Value.routineUniqueHash);
                coroutineOwnerDict.Remove(coroutine.ownerUniqueHash);
            }
        }

        /// <summary>
        ///     Goes the start coroutine.
        /// </summary>
        /// <param name="routine">The routine.</param>
        /// <param name="thisReference">The this reference.</param>
        /// <returns></returns>
        private EditorCoroutine GoStartCoroutine(IEnumerator routine, object thisReference)
        {
            if (routine == null) Debug.LogException(new Exception("IEnumerator is null!"), null);
            var coroutine = CreateCoroutine(routine, thisReference);
            GoStartCoroutine(coroutine);
            return coroutine;
        }

        /// <summary>
        ///     Goes the start coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine.</param>
        private void GoStartCoroutine(EditorCoroutine coroutine)
        {
            if (!coroutineDict.ContainsKey(coroutine.routineUniqueHash))
            {
                var newCoroutineList = new List<EditorCoroutine>();
                coroutineDict.Add(coroutine.routineUniqueHash, newCoroutineList);
            }

            coroutineDict[coroutine.routineUniqueHash].Add(coroutine);

            if (!coroutineOwnerDict.ContainsKey(coroutine.ownerUniqueHash))
            {
                var newCoroutineDict = new Dictionary<string, EditorCoroutine>();
                coroutineOwnerDict.Add(coroutine.ownerUniqueHash, newCoroutineDict);
            }

            // If the method from the same owner has been stored before, it doesn't have to be stored anymore,
            // One reference is enough in order for "StopAllCoroutines" to work
            if (!coroutineOwnerDict[coroutine.ownerUniqueHash].ContainsKey(coroutine.routineUniqueHash))
                coroutineOwnerDict[coroutine.ownerUniqueHash].Add(coroutine.routineUniqueHash, coroutine);

            MoveNext(coroutine);
        }

        /// <summary>
        ///     Creates the coroutine.
        /// </summary>
        /// <param name="routine">The routine.</param>
        /// <param name="thisReference">The this reference.</param>
        /// <returns></returns>
        private EditorCoroutine CreateCoroutine(IEnumerator routine, object thisReference)
        {
            return new EditorCoroutine(routine, thisReference.GetHashCode(), thisReference.GetType().ToString());
        }

        /// <summary>
        ///     Creates the coroutine from string.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="thisReference">The this reference.</param>
        /// <returns></returns>
        private EditorCoroutine CreateCoroutineFromString(string methodName, object thisReference)
        {
            return new EditorCoroutine(methodName, thisReference.GetHashCode(), thisReference.GetType().ToString());
        }

        /// <summary>
        ///     Called when [update].
        /// </summary>
        private void OnUpdate()
        {
            var deltaTime = (float)(DateTime.Now.Subtract(previousTimeSinceStartup).TotalMilliseconds / 1000.0f);

            previousTimeSinceStartup = DateTime.Now;
            if (coroutineDict.Count == 0) return;

            tempCoroutineList.Clear();
            foreach (var pair in coroutineDict) tempCoroutineList.Add(pair.Value);

            //ThreadedDebug.Log($"Coroutine dictionary count: {coroutineDict.Count}");

            for (var i = tempCoroutineList.Count - 1; i >= 0; i--)
            {
                var coroutines = tempCoroutineList[i];

                for (var j = coroutines.Count - 1; j >= 0; j--)
                {
                    var coroutine = coroutines[j];

                    if (!coroutine.currentYield.IsDone(deltaTime)) continue;

                    if (!MoveNext(coroutine))
                    {
                        coroutines.RemoveAt(j);
                        coroutine.currentYield = null;
                        coroutine.finished = true;
                    }

                    if (coroutines.Count == 0) coroutineDict.Remove(coroutine.ownerUniqueHash);
                }
            }
        }

        /// <summary>
        ///     Moves the next.
        /// </summary>
        /// <param name="coroutine">The coroutine.</param>
        /// <returns></returns>
        private static bool MoveNext(EditorCoroutine coroutine)
        {
            if (coroutine.routine.MoveNext()) return Process(coroutine);

            return false;
        }

        /// <summary>
        ///     Processes the coroutine.
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        // returns false if no next, returns true if OK
        private static bool Process(EditorCoroutine coroutine)
        {
            var current = coroutine.routine.Current;
            // ThreadedDebug.Log($"Current type: {(current != null ? current.GetType().GetFriendlyTypeName(true) : "Null")}");

            if (current == null)
                coroutine.currentYield = new YieldDefault();
            else if (current is WaitForSeconds)
            {
                var seconds = float.Parse(GetInstanceField(typeof(WaitForSeconds), current, "m_Seconds").ToString());
                coroutine.currentYield = new YieldWaitForSeconds { timeLeft = seconds };
            }
            else if (current is CustomYieldInstruction)
                coroutine.currentYield = new YieldCustomYieldInstruction
                {
                    customYield = current as CustomYieldInstruction
                };
            else if (current is WWW)
                coroutine.currentYield = new YieldWWW { Www = (WWW)current };
            else if (current is UnityWebRequest)
                coroutine.currentYield = new YieldWebRequest { UnityWebRequest = (UnityWebRequest)current };
            else if (current is UnityWebRequestAsyncOperation)
                coroutine.currentYield = new YieldWebRequestAsyncOperation
                { WebRequestAsyncOperation = (UnityWebRequestAsyncOperation)current };
            else if (current is WaitForFixedUpdate || current is WaitForEndOfFrame)
                coroutine.currentYield = new YieldDefault();
            else if (current is AsyncOperation)
                coroutine.currentYield = new YieldAsync { asyncOperation = current as AsyncOperation };
            else if (current is EditorCoroutine)
                coroutine.currentYield = new YieldNestedCoroutine { coroutine = current as EditorCoroutine };
            else if (current is YieldCoroutine)
            {
                EditorCoroutine editorCoroutine = current as YieldCoroutine;
                coroutine.currentYield = new YieldNestedCoroutine { coroutine = editorCoroutine };

                // Tip: Force move next makes the first "yield return" from a nested coroutine to move
                // I need to add to its internal dictionary... Starting the coroutine in the implicit cast should be a good way
                //MoveNext(editorCoroutine);
            }
            else
            {
                Debug.LogException(
                    new Exception("<" + coroutine.MethodName + "> yielded an unknown or unsupported type! (" +
                                  current.GetType() + ")"),
                    null);
                coroutine.currentYield = new YieldDefault();
            }

            return true;
        }

        /// <summary>
        ///     Gets the instance field.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private static object GetInstanceField(Type type, object instance, string fieldName)
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var field = type.GetField(fieldName, bindFlags);

            return field.GetValue(instance);
        }

        /// <summary>
        ///     The Editor Coroutine
        /// </summary>
        public class EditorCoroutine
        {
            /// <summary>
            ///     The current yield
            /// </summary>
            public ICoroutineYield currentYield = new YieldDefault();

            /// <summary>
            ///     The finished
            /// </summary>
            public bool finished;

            /// <summary>
            ///     The method name
            /// </summary>
            public string MethodName = "";

            /// <summary>
            ///     The owner hash
            /// </summary>
            public int ownerHash;

            /// <summary>
            ///     The owner type
            /// </summary>
            public string ownerType;

            /// <summary>
            ///     The owner unique hash
            /// </summary>
            public string ownerUniqueHash;

            /// <summary>
            ///     The routine
            /// </summary>
            public IEnumerator routine;

            /// <summary>
            ///     The routine unique hash
            /// </summary>
            public string routineUniqueHash;

            /// <summary>
            ///     Initializes a new instance of the <see cref="EditorCoroutine" /> class.
            /// </summary>
            /// <param name="routine">The routine.</param>
            /// <param name="ownerHash">The owner hash.</param>
            /// <param name="ownerType">Type of the owner.</param>
            public EditorCoroutine(IEnumerator routine, int ownerHash, string ownerType)
            {
                this.routine = routine;
                this.ownerHash = ownerHash;
                this.ownerType = ownerType;
                ownerUniqueHash = ownerHash + "_" + ownerType;

                if (routine != null)
                {
                    var split = routine.ToString().Split('<', '>');
                    if (split.Length == 3) MethodName = split[1];
                }

                routineUniqueHash = ownerHash + "_" + ownerType + "_" + MethodName;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="EditorCoroutine" /> class.
            /// </summary>
            /// <param name="methodName">Name of the method.</param>
            /// <param name="ownerHash">The owner hash.</param>
            /// <param name="ownerType">Type of the owner.</param>
            public EditorCoroutine(string methodName, int ownerHash, string ownerType)
            {
                MethodName = methodName;
                this.ownerHash = ownerHash;
                this.ownerType = ownerType;
                ownerUniqueHash = ownerHash + "_" + ownerType;
                routineUniqueHash = ownerHash + "_" + ownerType + "_" + MethodName;
            }

            /// <summary>
            ///     Performs an implicit conversion from <see cref="YieldCoroutine" /> to <see cref="EditorCoroutine" />.
            /// </summary>
            /// <param name="v">The v.</param>
            /// <returns>
            ///     The result of the conversion.
            /// </returns>
            public static implicit operator EditorCoroutine(YieldCoroutine v)
            {
                return StartCoroutine(v);
            }
        }

        /// <summary>
        ///     The Coroutine Yield interface
        /// </summary>
        public interface ICoroutineYield
        {
            /// <summary>
            ///     Determines whether the specified delta time is done.
            /// </summary>
            /// <param name="deltaTime">The delta time.</param>
            /// <returns>
            ///     <c>true</c> if the specified delta time is done; otherwise, <c>false</c>.
            /// </returns>
            bool IsDone(float deltaTime);
        }

        /// <summary>
        ///     YieldDefault
        /// </summary>
        /// <seealso cref="ICoroutineYield" />
        private struct YieldDefault : ICoroutineYield
        {
            /// <summary>
            ///     Determines whether the specified delta time is done.
            /// </summary>
            /// <param name="deltaTime">The delta time.</param>
            /// <returns>
            ///     <c>true</c> if the specified delta time is done; otherwise, <c>false</c>.
            /// </returns>
            public bool IsDone(float deltaTime)
            {
                return true;
            }
        }

        /// <summary>
        ///     YieldWaitForSeconds
        /// </summary>
        /// <seealso cref="ICoroutineYield" />
        private struct YieldWaitForSeconds : ICoroutineYield
        {
            /// <summary>
            ///     The time left
            /// </summary>
            public float timeLeft;

            /// <summary>
            ///     Determines whether the specified delta time is done.
            /// </summary>
            /// <param name="deltaTime">The delta time.</param>
            /// <returns>
            ///     <c>true</c> if the specified delta time is done; otherwise, <c>false</c>.
            /// </returns>
            public bool IsDone(float deltaTime)
            {
                timeLeft -= deltaTime;
                return timeLeft < 0;
            }
        }

        /// <summary>
        ///     YieldCustomYieldInstruction
        /// </summary>
        /// <seealso cref="ICoroutineYield" />
        private struct YieldCustomYieldInstruction : ICoroutineYield
        {
            /// <summary>
            ///     The custom yield
            /// </summary>
            public CustomYieldInstruction customYield;

            /// <summary>
            ///     Determines whether the specified delta time is done.
            /// </summary>
            /// <param name="deltaTime">The delta time.</param>
            /// <returns>
            ///     <c>true</c> if the specified delta time is done; otherwise, <c>false</c>.
            /// </returns>
            public bool IsDone(float deltaTime)
            {
                return !customYield.keepWaiting;
            }
        }

        /// <summary>
        ///     YieldWWW
        /// </summary>
        /// <seealso cref="ICoroutineYield" />
        private struct YieldWWW : ICoroutineYield
        {
            /// <summary>
            ///     The WWW
            /// </summary>
            public WWW Www;

            /// <summary>
            ///     Determines whether the specified delta time is done.
            /// </summary>
            /// <param name="deltaTime">The delta time.</param>
            /// <returns>
            ///     <c>true</c> if the specified delta time is done; otherwise, <c>false</c>.
            /// </returns>
            public bool IsDone(float deltaTime)
            {
                return Www.isDone;
            }
        }

        /// <summary>
        ///     YieldWebRequest
        /// </summary>
        /// <seealso cref="ICoroutineYield" />
        private struct YieldWebRequest : ICoroutineYield
        {
            /// <summary>
            ///     The unity web request
            /// </summary>
            public UnityWebRequest UnityWebRequest;

            /// <summary>
            ///     Determines whether the specified delta time is done.
            /// </summary>
            /// <param name="deltaTime">The delta time.</param>
            /// <returns>
            ///     <c>true</c> if the specified delta time is done; otherwise, <c>false</c>.
            /// </returns>
            public bool IsDone(float deltaTime)
            {
                return UnityWebRequest.isDone;
            }
        }

        /// <summary>
        ///     YieldWebRequestAsyncOperation
        /// </summary>
        /// <seealso cref="ICoroutineYield" />
        private struct YieldWebRequestAsyncOperation : ICoroutineYield
        {
            /// <summary>
            ///     The web request asynchronous operation
            /// </summary>
            public UnityWebRequestAsyncOperation WebRequestAsyncOperation;

            /// <summary>
            ///     Determines whether the specified delta time is done.
            /// </summary>
            /// <param name="deltaTime">The delta time.</param>
            /// <returns>
            ///     <c>true</c> if the specified delta time is done; otherwise, <c>false</c>.
            /// </returns>
            public bool IsDone(float deltaTime)
            {
                return WebRequestAsyncOperation.isDone;
            }
        }

        /// <summary>
        ///     YieldAsync
        /// </summary>
        /// <seealso cref="ICoroutineYield" />
        private struct YieldAsync : ICoroutineYield
        {
            /// <summary>
            ///     The asynchronous operation
            /// </summary>
            public AsyncOperation asyncOperation;

            /// <summary>
            ///     Determines whether the specified delta time is done.
            /// </summary>
            /// <param name="deltaTime">The delta time.</param>
            /// <returns>
            ///     <c>true</c> if the specified delta time is done; otherwise, <c>false</c>.
            /// </returns>
            public bool IsDone(float deltaTime)
            {
                return asyncOperation.isDone;
            }
        }

        /// <summary>
        ///     YieldNestedCoroutine
        /// </summary>
        /// <seealso cref="ICoroutineYield" />
        private struct YieldNestedCoroutine : ICoroutineYield
        {
            /// <summary>
            ///     The coroutine
            /// </summary>
            public EditorCoroutine coroutine;

            /// <summary>
            ///     Determines whether the specified delta time is done.
            /// </summary>
            /// <param name="deltaTime">The delta time.</param>
            /// <returns>
            ///     <c>true</c> if the specified delta time is done; otherwise, <c>false</c>.
            /// </returns>
            public bool IsDone(float deltaTime)
            {
                return coroutine.finished;
            }
        }

        /// <summary>
        ///     YieldCoroutine (used as a replacement of the current UnityEngine.Coroutines)
        /// </summary>
        /// <seealso cref="YieldInstruction" />
        public class YieldCoroutine : YieldInstruction
        {
            /// <summary>
            ///     The enumerator
            /// </summary>
            public IEnumerator enumerator;

            /// <summary>
            ///     The this reference
            /// </summary>
            public object thisReference;

            /// <summary>
            ///     Prevents a default instance of the <see cref="YieldCoroutine" /> class from being created.
            /// </summary>
            private YieldCoroutine()
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="YieldCoroutine" /> class.
            /// </summary>
            /// <param name="enumerator">The enumerator.</param>
            /// <param name="thisReference">The this reference.</param>
            public YieldCoroutine(IEnumerator enumerator, object thisReference)
            {
                this.enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
                this.thisReference = thisReference ?? throw new ArgumentNullException(nameof(thisReference));
            }
        }
    }

#endif
}

#endif