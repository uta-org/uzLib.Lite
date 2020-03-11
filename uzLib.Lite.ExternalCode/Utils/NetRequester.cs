using System;
using System.Net;
using System.Threading;
using uzLib.Lite.ExternalCode.Core;

namespace uzLib.Lite.ExternalCode.Utils
{
    using Extensions;

    /// <summary>
    ///     The Net Requester
    /// </summary>
    /// <seealso cref="Singleton{T}" />
    public class NetRequester : Singleton<NetRequester>
    {
        /// <summary>
        ///     Makes a safe request.
        /// </summary>
        /// <param name="safeAction">The safe action.</param>
        /// <param name="continueWithCallback">The continue with callback.</param>
        /// <param name="noInternetCallback">The no internet callback.</param>
        /// <param name="settings">The settings.</param>
        public void MakeSafeRequest(Action safeAction, Action continueWithCallback, Action noInternetCallback,
            Settings settings = null)
        {
            MakeSafeRequest(safeAction, continueWithCallback, noInternetCallback, null, null, settings);
        }

        /// <summary>
        ///     Makes a safe request.
        /// </summary>
        /// <param name="safeAction">The safe action.</param>
        /// <param name="continueWithCallback">The continue with callback.</param>
        /// <param name="noInternetCallback">The no internet callback.</param>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException">
        ///     safeAction
        ///     or
        ///     continueWithCallback
        ///     or
        ///     noInternetCallback
        /// </exception>
        public void MakeSafeRequest(Action safeAction, Action continueWithCallback, Action noInternetCallback,
            Action<Settings> frameCallback, Action<bool> retryingConnecion, Settings settings = null)
        {
            if (safeAction == null)
                throw new ArgumentNullException(nameof(safeAction));

            if (continueWithCallback == null)
                throw new ArgumentNullException(nameof(continueWithCallback));

            if (noInternetCallback == null)
                throw new ArgumentNullException(nameof(noInternetCallback));

            if (settings == null)
                settings = Settings.CreateDef();

            try
            {
                safeAction();
            }
            catch (WebException)
            {
                var noInternet = !NetHelper.CheckForInternetConnection();
                do
                {
                    Thread.Sleep(settings.ThreadSleepInterval);

                    var deltaInterval = settings.ThreadSleepInterval / 1000f;
                    settings.m_CurrentTimer += deltaInterval;
                    settings.m_NextRetry -= deltaInterval;

                    frameCallback?.Invoke(settings);

                    if (settings.m_NextRetry <= 0)
                    {
                        settings.ResetCounter();

                        noInternet = !NetHelper.CheckForInternetConnection();
                        ++settings.m_RetryCount;

                        retryingConnecion?.Invoke(!noInternet);
                    }
                } while (noInternet && settings.m_RetryCount <= settings.MaxRetries);

                if (!noInternet)
                {
                    safeAction();
                }
                else
                {
                    noInternetCallback();
                    return;
                }
            }

            continueWithCallback();
        }

        /// <summary>
        ///     The Settings class
        /// </summary>
        public class Settings
        {
            /// <summary>
            ///     The current timer
            /// </summary>
            internal float m_CurrentTimer;

            /// <summary>
            ///     The next retry
            /// </summary>
            internal float m_NextRetry;

            /// <summary>
            ///     The retry count
            /// </summary>
            internal int m_RetryCount;

            public float CurrentTimer => m_CurrentTimer;
            public float NextRetry => m_NextRetry;
            public int RetryCount => m_RetryCount;

            /// <summary>
            ///     Prevents a default instance of the <see cref="Settings" /> class from being created.
            /// </summary>
            private Settings()
            {
                RetryEvery = 5f;
                ThreadSleepInterval = 20;
                MaxRetries = 10;

                m_NextRetry = RetryEvery;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Settings" /> class.
            /// </summary>
            /// <param name="retryEvery">The retry every.</param>
            /// <param name="threadSleepInterval">The thread sleep interval.</param>
            /// <param name="maxRetries">The maximum retries.</param>
            public Settings(float retryEvery, int threadSleepInterval, int maxRetries)
            {
                RetryEvery = retryEvery;
                ThreadSleepInterval = threadSleepInterval;
                MaxRetries = maxRetries;

                m_NextRetry = RetryEvery;
            }

            /// <summary>
            ///     Gets the retry every.
            /// </summary>
            /// <value>
            ///     The retry every.
            /// </value>
            public float RetryEvery { get; }

            /// <summary>
            ///     Gets the thread sleep interval.
            /// </summary>
            /// <value>
            ///     The thread sleep interval.
            /// </value>
            public int ThreadSleepInterval { get; }

            /// <summary>
            ///     Gets the maximum retries.
            /// </summary>
            /// <value>
            ///     The maximum retries.
            /// </value>
            public int MaxRetries { get; }

            /// <summary>
            ///     Resets the counter.
            /// </summary>
            public void ResetCounter()
            {
                m_NextRetry = RetryEvery;
            }

            /// <summary>
            ///     Creates a default instance.
            /// </summary>
            /// <returns></returns>
            public static Settings CreateDef()
            {
                return new Settings();
            }
        }
    }
}