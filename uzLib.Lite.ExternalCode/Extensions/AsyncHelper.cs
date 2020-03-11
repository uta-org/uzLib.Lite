using System;
using System.ComponentModel;
using UnityEngine;

namespace uzLib.Lite.ExternalCode.Extensions
{
    public static class AsyncHelper
    {
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
    }
}