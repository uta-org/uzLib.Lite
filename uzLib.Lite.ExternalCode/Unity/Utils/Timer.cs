using System;
using System.Collections;
using UnityEngine;
using uzLib.Lite.ExternalCode.Extensions;

namespace uzLib.Lite.ExternalCode.Unity.Utils
{
    /// <summary>
    ///     Creates a Timer.
    /// </summary>
    public class Timer
    {
        /// <summary>
        ///     The internal coroutine
        /// </summary>
        private YieldInstruction m_coroutine;

        /// <summary>
        ///     The editor instance
        /// </summary>
        private object m_editorInstance;

        /// <summary>
        ///     The finish callback
        /// </summary>
        private Action m_finish;

        /// <summary>
        ///     The flag to solve if finished coroutine
        /// </summary>
        private bool m_finishedCoroutine;

        /// <summary>
        ///     The initial seconds
        /// </summary>
        private float m_initialSeconds;

        /// <summary>
        ///     The mono behaviour
        /// </summary>
        private MonoBehaviour m_monoBehaviour;

        /// <summary>
        ///     Prevents a default instance of the <see cref="Timer" /> class from being created.
        /// </summary>
        private Timer()
        {
        }

        /// <summary>
        ///     Gets the current seconds.
        /// </summary>
        /// <value>
        ///     The current seconds.
        /// </value>
        public float CurrentSeconds { get; private set; }

        /// <summary>
        ///     Gets the formatted seconds.
        /// </summary>
        /// <value>
        ///     The formatted seconds.
        /// </value>
        public string FormattedSeconds => CurrentSeconds.ToString("F2");

        /// <summary>
        ///     Creates the coundown.
        /// </summary>
        /// <param name="monoBehaviour">The mono behaviour.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="finish">The finish.</param>
        /// <returns></returns>
        public static Timer CreateCoundown(MonoBehaviour monoBehaviour, float timeout, Action finish)
        {
            return CreateCoundown(monoBehaviour, null, timeout, finish);
        }

        /// <summary>
        ///     Creates the coundown.
        /// </summary>
        /// <param name="monoBehaviour">The mono behaviour.</param>
        /// <param name="editorInstance">The editor instance.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="finish">The finish.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///     monoBehaviour
        ///     or
        ///     finish
        /// </exception>
        /// <exception cref="Exception">Timeout must be positive float.</exception>
        public static Timer CreateCoundown(MonoBehaviour monoBehaviour, object editorInstance, float timeout,
            Action finish, bool start = true)
        {
            if (timeout <= 0)
                throw new Exception("Timeout must be positive float.");

            var timer = new Timer
            {
                m_initialSeconds = timeout,
                m_finish = finish,
                m_monoBehaviour = monoBehaviour ?? throw new ArgumentNullException(nameof(monoBehaviour)),
                m_editorInstance = editorInstance
            };

            if (start)
                timer.Start(finish);

            return timer;
        }

        /// <summary>
        ///     Starts this instance.
        /// </summary>
        public void Start()
        {
            Start(m_finish);
        }

        /// <summary>
        ///     Starts the specified finish.
        /// </summary>
        /// <param name="finish">The finish.</param>
        public void Start(Action finish)
        {
#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
            if (m_coroutine != null)
            {
                Debug.LogWarning("The current Timer internal coroutine has been overrided.");
                Stop(false);
            }

            CurrentSeconds = m_initialSeconds;
            m_coroutine = Countdown(finish)
                .StartSmartCorotine(m_editorInstance, m_monoBehaviour);
#else
            Debug.LogError("Cannot use this Timer outside of Unity3D!");
#endif
        }

        /// <summary>
        ///     Stops this instance.
        /// </summary>
        public void Stop()
        {
            Stop(true);
        }

        /// <summary>
        ///     Stops the specified check null coroutine.
        /// </summary>
        /// <param name="checkNullCoroutine">if set to <c>true</c> [check null coroutine].</param>
        private void Stop(bool checkNullCoroutine)
        {
#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
            if (m_coroutine == null)
            {
                if (checkNullCoroutine)
                    Debug.LogError("Couldn't stop the timer!");

                return;
            }

            m_coroutine.StopSmartCoroutine(m_editorInstance, m_monoBehaviour);
            m_coroutine = null;
#else
            Debug.LogError("Cannot use this Timer outside of Unity3D!");
#endif
        }

        /// <summary>
        ///     Restarts this instance.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public void Restart(bool force = false)
        {
            Reset(force);
            Start();
        }

        /// <summary>
        ///     Resets this instance.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public void Reset(bool force = false)
        {
            if (!force && m_coroutine != null)
            {
                Debug.LogError("Couldn't stop reset a running coroutine if 'force' param isn't specified.");
                return;
            }

            Stop(false);
            CurrentSeconds = m_initialSeconds;
        }

        /// <summary>
        ///     Countdowns this instance.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Countdown(Action finish)
        {
            while (CurrentSeconds > 0)
            {
                yield return new WaitForFixedUpdate();
                CurrentSeconds -= Time.fixedDeltaTime;
            }

            finish?.Invoke();
            m_coroutine = null;
        }
    }
}