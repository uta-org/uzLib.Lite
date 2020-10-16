using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace UnityEngine.Utils.Threading
{
#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
    //[AutoInstantiate]
    public class Dispatcher : MonoBehaviour
    {
        private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
        public static Dispatcher Current { get; private set; }

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
            Action action;
            while (_actions.TryDequeue(out action))
            {
                if (action == null)
                    break;
                action();
            }
        }

        private void OnDestroy()
        {
            if (Current == this)
            {
                Current = null;
            }
        }
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
#endif
}