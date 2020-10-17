using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;

#if !(!UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5)

using UnityEngine.Core;

#else

using uzLib.Lite.ExternalCode.Core;

#endif

namespace uzLib.Lite.ExternalCode.Unity.Utils.Threading
{
    [AutoInstantiate]
    [AddComponentMenu("Window/Utils/Dispatcher (ExternalCode)")]
    public class Dispatcher : MonoSingleton<Dispatcher>
    {
        private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
        private readonly ConcurrentQueue<Func<object>> _funcs = new ConcurrentQueue<Func<object>>(); // TODO?
        public static Dispatcher Current { get; private set; }

        private static bool m_Stop;

        private static Coroutine m_ActionsCoroutine;
        private static Coroutine m_FuncCoroutine;

        public static void Invoke(Action action)
        {
            //if (Application.isEditor && !Application.isPlaying)
            //    return;

            InvokeAsync(action).Wait();
        }

        public static async Task InvokeAsync(Action action)
        {
            if (Current == null)
                return;

            // This will prompt on the editor... So we need to comment this
            // throw new Exception("please run Dispatcher.Initialize() or attach the component to a gameobject in the starting scene");

            var tks = new TaskCompletionSource<bool>();

            Current._actions.Enqueue(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

                tks.SetResult(true);
            });

            await tks.Task;
        }

        public static void Initialize()
        {
            if (!Application.isPlaying)
                return;

            if (Current != null) return;
            var g = new GameObject("Dispatcher");
            var d = g.AddComponent<Dispatcher>();
            d.InitializeAsThis();
        }

        private void InitializeAsThis()
        {
            Current = this;
            DontDestroyOnLoad(this);
        }

        private void Awake()
        {
            if (Current == null)
                InitializeAsThis();
            else
                Destroy(this);
        }

        private void Update()
        {
            while (_actions.TryDequeue(out var action))
            {
                if (action == null)
                    break;

                action();
            }

            //if (m_ActionsCoroutine == null && !_actions.IsNullOrEmpty())
            //    m_ActionsCoroutine = StartCoroutine(UpdateForAction());

            //if (m_FuncCoroutine == null && !_funcs.IsNullOrEmpty())
            //    m_FuncCoroutine = StartCoroutine(UpdateForFunc());
        }

        // TODO
        private IEnumerator UpdateForAction()
        {
            while (true)
            {
                if (_actions.TryDequeue(out var action))
                    action?.Invoke();

                if (m_Stop) break;

                yield return new WaitForEndOfFrame();
            }

            m_Stop = false;
            m_ActionsCoroutine = null;
        }

        // TODO
        private IEnumerator UpdateForFunc()
        {
            while (true)
            {
                if (_funcs.TryDequeue(out var func))
                    func();

                if (m_Stop) break;

                yield return new WaitForEndOfFrame();
            }

            m_Stop = false;
            m_FuncCoroutine = null;
        }

        public static void Stop()
        {
            m_Stop = true;
        }

        private void OnDestroy()
        {
            if (Current == this)
            {
                Current = null;
            }
        }

        //public class CoroutineTask
        //{
        //    public Coroutine Coroutine { get; }
        //    public object Result { get; private set; }

        //    private IEnumerator m_Target;

        //    public CoroutineTask(MonoBehaviour owner, IEnumerator target)
        //    {
        //        m_Target = target;
        //        Coroutine = owner.StartCoroutine(Run());
        //    }

        //    private IEnumerator Run()
        //    {
        //        while (m_Target.MoveNext())
        //        {
        //            Result = m_Target.Current;
        //            yield return Result;
        //        }
        //    }
        //}
    }

    public static class TaskExtensions
    {
        /// <summary>
        /// start a couroutine that waits on the specified task in a global component
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Coroutine Yield(this Task task)
        {
            return task.Yield(Dispatcher.Current);
        }

        /// <summary>
        /// start a coroutine on the specified container that waits on the specified task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static Coroutine Yield(this Task task, MonoBehaviour container)
        {
            return container.StartCoroutine(CompletionEnumerator(task));
        }

        private static IEnumerator CompletionEnumerator(Task task)
        {
            while (!task.IsCompleted)
                yield return null;
        }
    }
}