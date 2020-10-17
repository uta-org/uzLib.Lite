using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Controls;
using UnityEngine.Global;
using UnityEngine.UI.Interfaces;
using uzLib.Lite.ExternalCode.Extensions;
using uzLib.Lite.ExternalCode.Unity.Utils;

#if !(!UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5)

//using uzLib.Lite.Extensions;
using uzLib.Lite.Core;

#else

using uzLib.Lite.ExternalCode.Core;

#endif

namespace UnityEngine.UI
{
#if !(!UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5)
    using Extensions;
#endif

    public class DockWindow<T> : Singleton<DockWindow<T>>
        where T : IUnityForm, new()
    {
        private static int IdCounter;

        private T _form;

        public DockWindow()
        {
        }

        public DockWindow(int id, Rect position, string title, params GUILayoutOption[] options)
            : this(id, position, new GUIContent(title), null, options)
        {
        }

        public DockWindow(int id, Rect position, GUIContent content, params GUILayoutOption[] options)
            : this(id, position, content, null, options)
        {
        }

        public DockWindow(int id, Rect position, string title, GUIStyle style,
            params GUILayoutOption[] options)
            : this(id, position, new GUIContent(title), style, options)
        {
        }

        public DockWindow(int id, Rect position, GUIContent content, GUIStyle style,
            params GUILayoutOption[] options)
            : this()
        {
            Init(id, position, content, style, options);
        }

        public static bool IsHover { get; set; }
        public int Id { get; private set; }

        private Rect m_Position;

        public Rect Position
        {
            get => m_Position;
            set
            {
                m_Position = value;
                UpdatePosition();
            }
        }

        public Vector2 Size
        {
            get => m_Position.size;
            set => Position = new Rect(Vector2.zero, value);
        }

        public GUIContent Content { get; private set; }

        public GUIStyle Style { get; private set; }
        public GUILayoutOption[] Options { get; private set; }
        public bool DrawUI { get; set; }

        public Action EditorGUI { get; set; }

        public ICommonUI<T> Worker { get; set; }

        private UIDisplayer m_Display;
        public DockForm DockForm { get; private set; }

        public T Form
        {
            get
            {
                if (_form == null)
                    _form = new T();

                return _form;
            }
        }

        private void Init(int id, Rect position, GUIContent content, GUIStyle style,
            GUILayoutOption[] options)
        {
            Id = id;

            if (Camera.main == null)
                throw new NullReferenceException();

            Content = content;
            Style = style;
            Options = options;

            if (ScenePlaybackDetector.IsPlaying)
                CreateForm(position, content);

            DrawUI = true;
        }

        /// <summary>
        /// Sloppy patch to creates the form.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="content">The content.</param>
        private void CreateForm(Rect position, GUIContent content)
        {
            DockForm = new DockForm();

            if (content != null)
                DockForm.Text = content.text;
            else
            {
                DockForm.Location = position.position;
                DockForm.Size = position.size;
            }

            m_Display = new UIDisplayer(position.SumY(25), DrawWindow);
            DockForm.Controls.Add(m_Display);
        }

        private void UpdatePosition()
        {
            if (DockForm == null) return;

            DockForm.Location = GetCenterLocation();
            DockForm.Size = m_Position.size;
        }

        private Vector2 GetCenterLocation()
        {
            Vector2 _size = m_Position.size;
            return new Vector2(Screen.width / 2f - _size.x / 2, Screen.height / 2f - _size.y / 2);
        }

        protected virtual void DrawWindow()
        {
            // TODO
            IsHover = Position.Contains(GlobalInput.MousePosition);

            Worker?.DrawInterface(string.Empty, null);
        }

        public DockWindow<T> Start(string title, ICommonUI<T> worker, Action editorGUI = null)
        {
#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
            // Sloppy patch
            if (IsStarted && ScenePlaybackDetector.IsPlaying)
                IsStarted = false;
#endif

            if (!IsStarted)
            {
                var m_pos = new Rect(0, 0, Screen.width, Screen.height);

                Init(IdCounter, m_pos, new GUIContent(title), null, null);

                EditorGUI = editorGUI ?? delegate { };

                ++IdCounter;

                Worker = worker;
                IsStarted = true;
            }

            return this;
        }

        public void Show(int nElements = 30, float size = 138)
        {
            // TODO: Center window
            var _size = new Resolution().GetSize();
            Position = new Rect(new Vector2(Screen.width / 2f - _size.x / 2, Screen.height / 2f - _size.y / 2), _size);

            // Don't use this approach
            if (DockForm == null) CreateForm(Position, null);
            DockForm.Show();
        }

        public virtual void Update(bool enabled)
        {
            if (Instance != null)
                Instance.DrawUI = enabled;

            // Sloppy patch
            var centerPosition = GetCenterLocation();
            if (DockForm != null && DockForm.Location != centerPosition)
                DockForm.Location = centerPosition;
        }

        public void UpdatePosition(Vector2 v)
        {
            DockForm.Location += v;
        }

        internal class Resolution
        {
            // TODO
            //private int nElements;

            public List<Tuple<int, int>> Grids { get; } = new List<Tuple<int, int>>();

            public Resolution()
            {
                // For 30 elements
                Grids.Add(new Tuple<int, int>(30, 1));
                Grids.Add(new Tuple<int, int>(15, 2));
                Grids.Add(new Tuple<int, int>(10, 3));
                Grids.Add(new Tuple<int, int>(6, 5));

                Grids.Add(new Tuple<int, int>(1, 30));
                Grids.Add(new Tuple<int, int>(2, 15));
                Grids.Add(new Tuple<int, int>(3, 10));
                Grids.Add(new Tuple<int, int>(5, 6));
            }

            // TODO: Fix 87 && 10
            public Vector2 GetSize(float size = 138f, int hPadding = 100, int vPadding = 150, int vMargin = 87, int hMargin = 10)
            {
                var screenSize = UIUtils.ScreenRect.RestWidth(hPadding * 2).RestHeight(vPadding * 2).size;
                var tuple = GetNearestTuple(Mathf.FloorToInt(screenSize.x / size));
                //Debug.Log(tuple);

                return new Vector2(tuple.Item1 * size + hMargin, tuple.Item2 * size + vMargin);
            }

            private Tuple<int, int> GetNearestTuple(int widthFactor)
            {
                //Debug.Log(widthFactor);
                return Grids.Select(n => new { n, Distance = Mathf.Abs(n.Item1 - widthFactor) }).OrderBy(x => x.Distance).First().n;
            }
        }
    }
}