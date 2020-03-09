using System;
using System.Collections;
using System.ComponentModel;

namespace UnityEngine.Extensions
{
    /// <summary>
    ///     The Async Helper
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        ///     Run a function asynchronously.
        /// </summary>
        /// <typeparam name="TPre">The type of the pre.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <param name="beforeExecute">The before execute.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///     func
        ///     or
        ///     beforeExecute
        ///     or
        ///     result
        /// </exception>
        public static BackgroundWorker RunAsync<TPre, T>(this Func<TPre, T> func, Func<TPre> beforeExecute,
            Action<T> result)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            if (beforeExecute == null) throw new ArgumentNullException(nameof(beforeExecute));

            if (result == null) throw new ArgumentNullException(nameof(result));

            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                //Some work...
                e.Result = func((TPre)e.Argument);
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                //e.Result "returned" from thread
                result((T)e.Result);
            };

            var argument = beforeExecute();
            worker.RunWorkerAsync(argument);

            return worker;
        }

        /// <summary>
        ///     Runs the asynchronous.
        /// </summary>
        /// <typeparam name="TPre">The type of the pre.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <param name="beforeExecute">The before execute.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// func
        /// or
        /// beforeExecute
        /// or
        /// result
        /// </exception>
        public static BackgroundWorker RunAsync<TPre, T>(this Func<TPre, BackgroundWorker, T> func, Func<TPre> beforeExecute,
            Action<T> result = null)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            if (beforeExecute == null) throw new ArgumentNullException(nameof(beforeExecute));

            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                //Some work...
                e.Result = func((TPre)e.Argument, s as BackgroundWorker);
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                //e.Result "returned" from thread
                result?.Invoke((T)e.Result);
            };

            var argument = beforeExecute();
            worker.RunWorkerAsync(argument);

            return worker;
        }

        /// <summary>
        ///     Run a function asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <param name="beforeExecute">The before execute.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///     func
        ///     or
        ///     beforeExecute
        ///     or
        ///     result
        /// </exception>
        public static BackgroundWorker RunAsync<T>(this Func<T> func, Action beforeExecute, Action<T> result)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            if (beforeExecute == null) throw new ArgumentNullException(nameof(beforeExecute));

            if (result == null) throw new ArgumentNullException(nameof(result));

            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                //Some work...
                e.Result = func();
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                //e.Result "returned" from thread
                result((T)e.Result);
            };

            beforeExecute();
            worker.RunWorkerAsync();

            return worker;
        }

        /// <summary>
        ///     Run a function asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">func</exception>
        public static BackgroundWorker RunAsync<T>(this Func<T> func, Action<T> result = null)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                //Some work...
                e.Result = func();
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                //e.Result "returned" from thread
                result?.Invoke((T)e.Result);
            };

            worker.RunWorkerAsync();

            return worker;
        }

        /// <summary>
        ///     Run a function asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">func</exception>
        public static BackgroundWorker RunAsync<T>(this Func<BackgroundWorker, T> func, Action<T> result = null)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                //Some work...
                e.Result = func(s as BackgroundWorker);
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                //e.Result "returned" from thread
                result?.Invoke((T)e.Result);
            };

            worker.RunWorkerAsync();

            return worker;
        }

        /// <summary>
        /// Runs the asynchronous catching exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">func</exception>
        public static BackgroundWorker RunAsyncCatchingExceptions<T>(this Func<T> func, Action<T> result = null)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                //Some work...
                e.Result = func();
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                //e.Result "returned" from thread
                result?.Invoke((T)e.Result);
            };

            try
            {
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return worker;
            }

            return worker;
        }

        /// <summary>
        ///     Run an action asynchronously.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">callback</exception>
        public static BackgroundWorker RunAsync(this Action callback, Action result = null)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                //Some work...
                callback();
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                //e.Result "returned" from thread
                result?.Invoke();
            };

            worker.RunWorkerAsync();

            return worker;
        }

        /// <summary>
        ///     Runs the asynchronous.
        /// </summary>
        /// <param name="coroutine">The coroutine.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">coroutine</exception>
        public static BackgroundWorker RunAsync(this IEnumerator coroutine, Action result = null)
        {
            if (coroutine == null) throw new ArgumentNullException(nameof(coroutine));

            Action action = () =>
            {
                while (coroutine.MoveNext())
                {
                }
            };

            return action.RunAsync(result);
        }

        /// <summary>
        ///     Waits for worker.
        /// </summary>
        /// <param name="backgroundWorker">The background worker.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">callback</exception>
        public static IEnumerator WaitForWorker(this BackgroundWorker backgroundWorker, Action callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            yield return new WaitUntil(() => !backgroundWorker.IsBusy);
            callback();
        }
    }
}