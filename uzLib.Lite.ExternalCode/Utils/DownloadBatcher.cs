﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Global.IMGUI;
using UnityEngine.UI;
using UnityEngine.Utils.DebugTools;
using UnityEngine.Utils.Interfaces;
using uzLib.Lite.ExternalCode.Unity.Extensions;
using uzLib.Lite.ExternalCode.Utils.Interfaces;
using uzLib.Lite.ExternalCode.WinFormsSkins.Workers;

#if !(!UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5)
using UnityEngine.Extensions;
#endif

namespace uzLib.Lite.ExternalCode.Utils
{
    using Extensions;

    /// <summary>
    ///     The Download Batcher class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TFile">The type of the file.</typeparam>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="IDisposable" />
    public class DownloadBatcher<T, TFile> : IDisposable
        where TFile : IFileModel
        where T : IDownloadItem<TFile>
    {
        /// <summary>
        ///     The current pending items
        /// </summary>
        private int m_CurrentPendingItems;

        /// <summary>
        ///     The current progress
        /// </summary>
        private float m_CurrentProgress;

        /// <summary>
        ///     The download available
        /// </summary>
        private bool m_DownloadAvailable;

        /// <summary>
        ///     The last download rate
        /// </summary>
        private string m_LastDownloadRate;

        /// <summary>
        ///     The last eta
        /// </summary>
        private string m_LastETA;

        /// <summary>
        ///     The last measured bytes
        /// </summary>
        private long m_LastMeasuredBytes;

        /// <summary>
        ///     The last measured seconds
        /// </summary>
        private float m_LastMeasuredSeconds;

        /// <summary>
        ///     The measured bytes
        /// </summary>
        private long m_MeasuredBytes;

        /// <summary>
        ///     The queue
        /// </summary>
        private readonly ConcurrentQueue<T> m_Queue;

        /// <summary>
        ///     The timer
        /// </summary>
        private float m_Timer;

        /// <summary>
        ///     The total bytes
        /// </summary>
        private long m_TotalBytes;

        /// <summary>
        /// The m callback load percentage
        /// </summary>
        private float m_CallbackLoadPercentage;

        /// <summary>
        ///     The web clients
        /// </summary>
        private readonly List<IDownloadManager<dynamic>> m_Managers;

        /// <summary>
        /// The red label style
        /// </summary>
        private GUIStyle m_RedLabel;

        /// <summary>
        /// Occurs when [on finished asynchronous].
        /// </summary>
        public event Action<dynamic> OnFinishedAsync = delegate { };

        /// <summary>
        ///     Prevents a default instance of the <see cref="DownloadBatcher{T}" /> class from being created.
        /// </summary>
        private DownloadBatcher()
        {
            m_Queue = new ConcurrentQueue<T>();
            m_Managers = new List<IDownloadManager<dynamic>>();

            //SetManager<DefaultManager>(new DefaultManager());
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DownloadBatcher{T}" /> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <exception cref="ArgumentNullException">callback</exception>
        public DownloadBatcher(Action<byte[], T> callback)
            : this()
        {
            DataDownloaded = callback ?? throw new ArgumentNullException(nameof(callback));
            m_ThreadSafe = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadBatcher{T}"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="callbackLoadPercentage">[Range 0...1] The callback load percentage. (If specified then progress bars will take care of that load)</param>
        /// <param name="threadSafe">if set to <c>true</c> [executes the callback in thread safe mode].</param>
        /// <exception cref="ArgumentNullException">callback</exception>
        /// <exception cref="ArgumentException">Load percentage value for the callback can't be greater than 1.</exception>
        public DownloadBatcher(Action<byte[], T, IProgress<float>> callback, float callbackLoadPercentage)
            : this()
        {
            DataDownloadedWithProgress = callback ?? throw new ArgumentNullException(nameof(callback));
            m_CallbackLoadPercentage = callbackLoadPercentage;
            m_ThreadSafe = true;

            if (callbackLoadPercentage > 0 && callbackLoadPercentage < 1)
                m_ProgressValue = new Progress<float>(ProgressHandler);
            else if (callbackLoadPercentage > 1)
                throw new ArgumentException("Load percentage value for the callback can't be greater than 1.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadBatcher{T}"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <exception cref="ArgumentNullException">callback</exception>
        public DownloadBatcher(Func<byte[], T, dynamic> callback)
            : this()
        {
            DataDownloadedAsync = callback ?? throw new ArgumentNullException(nameof(callback));
            m_ThreadSafe = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadBatcher{T}"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="callbackLoadPercentage">The callback load percentage.</param>
        /// <exception cref="ArgumentNullException">callback</exception>
        /// <exception cref="ArgumentException">Load percentage value for the callback can't be greater than 1.</exception>
        public DownloadBatcher(Func<byte[], T, IProgress<float>, dynamic> callback, float callbackLoadPercentage)
            : this()
        {
            DataDownloadedWithProgressAsync = callback ?? throw new ArgumentNullException(nameof(callback));
            m_CallbackLoadPercentage = callbackLoadPercentage;
            m_ThreadSafe = false;

            if (callbackLoadPercentage > 0 && callbackLoadPercentage < 1)
                m_ProgressValue = new Progress<float>(ProgressHandler);
            else if (callbackLoadPercentage > 1)
                throw new ArgumentException("Load percentage value for the callback can't be greater than 1.");
        }

        /// <summary>
        ///     Gets the enqueued items.
        /// </summary>
        /// <value>
        ///     The enqueued items.
        /// </value>
        public List<T> EnqueuedItems => m_Queue?.ToList();

        /// <summary>
        ///     Gets the data downloaded.
        /// </summary>
        /// <value>
        ///     The data downloaded.
        /// </value>
        public Action<byte[], T> DataDownloaded { get; }

        /// <summary>
        /// Gets the data downloaded.
        /// </summary>
        /// <value>
        /// The data downloaded.
        /// </value>
        public Action<byte[], T, IProgress<float>> DataDownloadedWithProgress { get; }

        /// <summary>
        /// Gets the data downloaded asynchronously.
        /// </summary>
        /// <value>
        /// The data downloaded asynchronously.
        /// </value>
        public Func<byte[], T, dynamic> DataDownloadedAsync { get; }

        /// <summary>
        /// Gets the data downloaded with progress asynchronously.
        /// </summary>
        /// <value>
        /// The data downloaded with progress asynchronously.
        /// </value>
        public Func<byte[], T, IProgress<float>, dynamic> DataDownloadedWithProgressAsync { get; }

        /// <summary>
        /// The progress value
        /// </summary>
        private IProgress<float> m_ProgressValue { get; }

        /// <summary>
        /// The reported progress
        /// </summary>
        private float m_ReportedProgress;

        /// <summary>
        /// The thread safe
        /// </summary>
        private bool m_ThreadSafe;

        /// <summary>
        /// The pending asynchronous downloads flag
        /// </summary>
        private bool m_PendingAsyncDownloadsFlag;

        /// <summary>
        /// The m doing callback
        /// </summary>
        private bool m_DoingCallback;

        /// <summary>
        /// The download has exception
        /// </summary>
        private bool m_DownloadHasException;

        /// <summary>
        /// Gets or sets the name of the callback.
        /// </summary>
        /// <value>
        /// The name of the callback.
        /// </value>
        public static string CallbackName { private get; set; }

        /// <summary>
        ///     Gets the current download.
        /// </summary>
        /// <value>
        ///     The current download.
        /// </value>
        public T CurrentDownload { get; private set; }

        /// <summary>
        ///     Gets the state of the batcher.
        /// </summary>
        /// <value>
        ///     The state of the batcher.
        /// </value>
        public BatcherState BatcherState { get; private set; }

        public Func<IDownloadManager<dynamic>> OnExceptionResolver { get; set; }
        public Func<TFile, object> ResolveObject { get; set; }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var manager in m_Managers)
            {
                if (manager is IDisposable disposableObject)
                {
                    disposableObject.Dispose();
                }
            }
        }

        /// <summary>
        /// Creates a new manager.
        /// </summary>
        /// <returns></returns>
        private IDownloadManager<dynamic> CreateManager()
        {
            if (!m_ThreadSafe)
                m_PendingAsyncDownloadsFlag = false;

            var manager = new DefaultManager();
            // Activator.CreateInstance<IDownloadManager<TManagerType>>();

            manager.DownloadProgressChanged += DownloadProgressChanged;
            manager.DownloadCompleted += DownloadDataCompleted;

            return manager;
        }

        /// <summary>
        /// Callback for the progress.
        /// </summary>
        /// <param name="value">The value.</param>
        private void ProgressHandler(float value)
        {
            m_ReportedProgress = value;
        }

        private void DownloadDataCompleted(object managerObj, byte[] result)
        {
            //if (managerObj == null)
            //    throw new NullReferenceException("Something unexpected happened with WebClient!");

            var manager = (IDownloadManager<dynamic>)managerObj;
            manager?.UnloadEvents();

            // Dispose

            if (!m_Managers.Remove(manager))
                Debug.LogError("Couldn't find any web client to remove!");

            if (manager?.Target is IDisposable disposable)
                disposable.Dispose();

            m_DownloadAvailable = true;
            m_DoingCallback = true;

            if (m_ThreadSafe)
            {
                if (m_ProgressValue == null)
                    DataDownloaded(result, CurrentDownload);
                else
                    DataDownloadedWithProgress(result, CurrentDownload, m_ProgressValue);

                m_DoingCallback = false;
            }
            else
            {
                Func<dynamic> action;

                if (m_ProgressValue == null)
                    action = () => DataDownloadedAsync(result, CurrentDownload);
                else
                    action = () => DataDownloadedWithProgressAsync(result, CurrentDownload, m_ProgressValue);

                // If we are on async mode, we must call the callback first to avoid NREs
                void OnFinish(dynamic dyn)
                {
                    OnFinishedAsync(dyn);

                    m_DoingCallback = false;
                    if (m_Queue.Count == 0)
                        CurrentDownload = default;
                }

                action.RunAsync(OnFinish);
            }

            if (m_Queue.Count > 0)
            {
                if (DequeueItem())
                {
                    // Debug.Log("Continuing downloads!");
                }
            }
            else
            {
                // Reset CurrentDownload if everything is done (and is thread safe)
                if (m_ThreadSafe)
                    CurrentDownload = default;
                else
                    m_PendingAsyncDownloadsFlag = true;
            }

            --m_CurrentPendingItems;
        }

        /// <summary>
        ///     Handles the DownloadProgressChanged event of the WebClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DownloadProgressChangedEventArgs" /> instance containing the event data.</param>
        private void DownloadProgressChanged(ulong bytesReceived, ulong totalBytesToReceive)
        {
            m_CurrentProgress = (float)bytesReceived / totalBytesToReceive;

            m_MeasuredBytes = (long)bytesReceived;
            m_TotalBytes = (long)totalBytesToReceive;

            //Debug.Log($"Current progress {(m_CurrentProgress * 100f).ToString("F2")}% of download ({e.BytesReceived} bytes of {e.TotalBytesToReceive})");
        }

        /// <summary>
        ///     Draws the UI.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public BatcherState DrawUI(Rect rect)
        {
            if (CurrentDownload == null)
            {
                BatcherState = BatcherState.NoPendingDownloads;
                return BatcherState;
            }

            if (m_RedLabel == null)
            {
                m_RedLabel = new GUIStyle("label") { normal = new GUIStyleState { textColor = Color.red }, wordWrap = true };
                // m_WhiteLabel = new GUIStyle("label") { normal = new GUIStyleState { textColor = Color.white } };
                // m_BlackLabelStyle = new GUIStyle(SkinWorker.MySkin.label) { normal = new GUIStyleState { textColor = Color.black } };
                // m_EditorBoxStyle = new GUIStyle(SkinWorker.MySkin.box) { normal = new GUIStyleState { background = new Color(0, 0, 0, .85f).ToTexture(16, 16) } };
            }

            const float height = 20f,
                        border = 2f;

            if (!m_DownloadHasException)
            {
                GUI.BeginGroup(rect, SkinWorker.MySkin.box); // TODO: Depending if Editor or not use different skins, box style in the GUI.skin & SkinWorker.MySkin are broken
                {
                    Color fillColor = Color.white,
                          backgroundColor = Color.gray;

                    bool reportProgress = m_ProgressValue != null;
                    float progressValue = m_CurrentProgress * (1 - m_CallbackLoadPercentage) +
                                          m_ReportedProgress * m_CallbackLoadPercentage;
                    float finalValue = !reportProgress ? m_CurrentProgress : progressValue;

                    float fillPerc1 = finalValue,
                          fillPerc2 = finalValue / m_CurrentPendingItems;

                    var _rect = rect.ResetPosition();
                    var labelRect = GetRectFor(_rect, height * 2);
                    var labelCaption = $"Download item {fillPerc1 * 100f:F2}%" + (m_CurrentPendingItems > 1
                                           ? $" of total {fillPerc2 * 100f:F2}%"
                                           : string.Empty) +
                                       Environment.NewLine +
                                       $"({m_CurrentPendingItems} pending items -- {GetCaption()})";

                    GUI.Label(
                        labelRect,
                        labelCaption,
                        GlobalGUIStyle.WithCenteredRichText());

                    var bar1Rect = GetRectFor(_rect, height).SumTop(height * 2 + 5);
                    UIUtils.DrawBar(bar1Rect, fillPerc1, fillColor, backgroundColor, border);

                    var bar2Rect = GetRectFor(_rect, height).SumTop(height * 3 + 10);
                    UIUtils.DrawBar(bar2Rect, fillPerc2, fillColor, backgroundColor, border);
                }
                GUI.EndGroup();
            }

            if (!m_PendingAsyncDownloadsFlag)
            {
                BatcherState = Update();
                if (BatcherState == BatcherState.Exception) m_DownloadHasException = true;
            }
            else
                return BatcherState.PendingAsyncDownloads;

            return BatcherState;
        }

        public void DisableExceptionFlag()
        {
            m_DownloadHasException = false;
        }

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <returns></returns>
        private string GetCaption()
        {
            if (!m_DoingCallback)
                return $"{GetDownloadRate()}, ETA: {GetETA()}";

            return $"{(!string.IsNullOrEmpty(CallbackName) ? CallbackName : "Callback Progress")}: {m_ReportedProgress * 100f:F2} %";
        }

        /// <summary>
        ///     Updates this instance.
        /// </summary>
        /// <returns></returns>
        private BatcherState Update()
        {
            if (!m_DownloadAvailable)
                return BatcherState.DownloadingItems;

            var manager = CreateManager();

            try
            {
                manager.DownloadDataAsync(new Uri(CurrentDownload.FileModel.FileUrl));
            }
            catch
            {
                if (OnExceptionResolver == null)
                {
                    Debug.LogError($"[{CurrentDownload.FileModel.FileUrl}] Error occurred while downloading with Batcher!");

                    if (manager.Target is IDisposable disposable)
                        disposable.Dispose();

                    m_DownloadAvailable = false;
                    return BatcherState.Exception;
                }

                manager = OnExceptionResolver();
                manager.DownloadDataAsync(ResolveObject(CurrentDownload.FileModel));
            }

            m_Managers.Add(manager);
            m_DownloadAvailable = false;

            return BatcherState.CreatedWebClient;
        }

        private string GetDownloadRate()
        {
            // If has passed more than 1 seconds...
            var secDiff = Time.realtimeSinceStartup - m_Timer;

            if (secDiff >= 1)
            {
                var diff = m_MeasuredBytes - m_LastMeasuredBytes;
                if (diff == 0) diff = 1;

                m_LastMeasuredSeconds = (float)m_TotalBytes / diff / secDiff;
                m_LastMeasuredBytes = m_MeasuredBytes;

                var rate = diff / secDiff;
                m_LastDownloadRate = $"{((long)rate).GetPrefix()}B/s";
            }

            return m_LastDownloadRate;
        }

        private string GetETA()
        {
            if (Time.realtimeSinceStartup - m_Timer >= 1)
            {
                m_LastETA = m_LastMeasuredSeconds.ConvertSecondsToDate();

                // In this case, we are on the last call, update the timer...
                m_Timer = Time.realtimeSinceStartup;
            }

            return m_LastETA;
        }

        /// <summary>
        ///     Gets the rect for.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        private Rect GetRectFor(Rect rect, float height)
        {
            return rect.SumTop(10).RestWidth(20).SumLeft(10).ForceHeight(height);
        }

        /// <summary>
        ///     Enqueues the download.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="process">if set to <c>true</c> [process].</param>
        public void EnqueueDownload(T item, bool process = true)
        {
            ++m_CurrentPendingItems;

            var isFree = process && m_Queue.Count == 0;
            m_Queue.Enqueue(item);

            if (process && isFree && DequeueItem())
                m_DownloadAvailable = true;
        }

        /// <summary>
        ///     Dequeues the item.
        /// </summary>
        /// <returns></returns>
        private bool DequeueItem()
        {
            var isDequeued = m_Queue.TryDequeue(out var dequeuedItem);

            if (!isDequeued)
            {
                ThreadedDebug.LogError("Couldn't dequeue the item!");
                return false;
            }

            CurrentDownload = dequeuedItem;
            return true;
        }

        /// <summary>
        ///     Removes from queue.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="Exception">Item didn't found on queue</exception>
        public void RemoveFromQueue(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var index = Array.IndexOf(EnqueuedItems.ToArray(), item);

            if (index == -1) throw new Exception("Item didn't found on queue!");

            m_Queue.RemoveAt(index);
        }

        //public void SetManager<TManager, TManagerType>(TManager target)
        //    where TManagerType : new()
        //    where TManager : IDownloadManager<TManagerType>
        //{
        //    Manager = target;
        //}

        //public IDownloadManager<TManagerType> GetManager<TManagerType>(object obj)
        //    where TManagerType : new()
        //{
        //    return (IDownloadManager<TManagerType>)obj;
        //}

        //private object Manager;

        internal class DefaultManager : IDownloadManager<WebClient>, IDisposable
        {
            public WebClient Target { get; }

            public event Action<ulong, ulong> DownloadProgressChanged = delegate { };

            public event Action<object, byte[]> DownloadCompleted = delegate { };

            public void UnloadEvents()
            {
                Target.DownloadProgressChanged -= OnDownloadProgressChanged;
                Target.DownloadDataCompleted -= OnDownloadDataCompleted;
            }

            public void DownloadDataAsync(object fromWhere)
            {
                Target.DownloadDataAsync(new Uri((string)fromWhere));
            }

            public DefaultManager()
            {
                Target = new WebClient();
                Target.DownloadProgressChanged += OnDownloadProgressChanged;
                Target.DownloadDataCompleted += OnDownloadDataCompleted;
            }

            private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
            {
                DownloadProgressChanged((ulong)e.BytesReceived, (ulong)e.TotalBytesToReceive);
            }

            private void OnDownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
            {
                DownloadCompleted(sender, e.Result);
            }

            public void Dispose()
            {
                Target?.Dispose();
            }
        }
    }
}