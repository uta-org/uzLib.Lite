using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using _System.Drawing;
using Unity.Controls;
using UnityEngine.Extensions;
using UnityEngine.Global;
using UnityEngine.UI.Interfaces;
using uzLib.Lite.ExternalCode.Core;
using uzLib.Lite.ExternalCode.Extensions;

#if !(!UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5)

using uzLib.Lite.Extensions;

#endif

namespace UnityEngine.UI
{
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

        public DockWindow(int id, Rect position, string title, GUIStyle style, params GUILayoutOption[] options)
            : this(id, position, new GUIContent(title), style, options)
        {
        }

        public DockWindow(int id, Rect position, Rect? dragPosition, string title, params GUILayoutOption[] options)
            : this(id, position, dragPosition, new GUIContent(title), null, options)
        {
        }

        public DockWindow(int id, Rect position, Rect? dragPosition, GUIContent content,
            params GUILayoutOption[] options)
            : this(id, position, dragPosition, content, null, options)
        {
        }

        public DockWindow(int id, Rect position, Rect? dragPosition, string title, GUIStyle style,
            params GUILayoutOption[] options)
            : this(id, position, dragPosition, new GUIContent(title), style, options)
        {
        }

        public DockWindow(int id, Rect position, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
            : this(id, position, null, content, style, options)
        {
        }

        public DockWindow(int id, Rect position, Rect? dragPosition, GUIContent content, GUIStyle style,
            params GUILayoutOption[] options)
            : this()
        {
            Init(id, position, dragPosition, content, style, options);
        }

        private Rect m_Position;

        public static bool IsHover { get; set; }
        public int Id { get; private set; }

        public Rect Position
        {
            get => m_Position;
            set
            {
                m_Position = value;
                UpdatePosition();
            }
        }

        //public Rect? DragPosition { get; private set; }
        public GUIContent Content { get; private set; }

        public GUIStyle Style { get; private set; }
        public GUILayoutOption[] Options { get; private set; }
        public bool DrawUI { get; set; }
        public bool IsEditor { get; set; }

        public Action EditorGUI { get; set; }

        public ICommonUI<T> Worker { get; set; }

        private UIDisplayer m_Display;
        private DockForm m_Form;
        private bool m_isEditor => Application.isEditor && !Application.isPlaying;

        public T Form
        {
            get
            {
                if (_form == null)
                    //if (Control.uwfDefaultController == null)
                    //{
                    //    var controller = new Application();
                    //    //controller.Resources = GdiImages;
                    //    controller.UpdatePaintClipRect();

                    //    Control.uwfDefaultController = controller;
                    //}

                    _form = new T();

                return _form;
            }
        }

        private void Init(int id, Rect position, Rect? dragPosition, GUIContent content, GUIStyle style,
            GUILayoutOption[] options)
        {
            Id = id;

            if (Camera.main == null)
                throw new NullReferenceException();

            //const float margin = 20f;

            //var mainCamera = Camera.main;

            //if (position != Rect.zero)
            //    position = position
            //        .SumTop(10)
            //        .SumLeft(10)
            //        .RestWidth(margin)
            //        .RestHeight(margin);

            //Position = position == Rect.zero
            //    ? new Rect(Vector2.one * 10f, new Vector2(
            //        mainCamera.pixelWidth - margin,
            //        mainCamera.pixelHeight - margin))
            //    : position;

            //if (dragPosition.HasValue && dragPosition.Value == Rect.zero)
            //    DragPosition = new Rect(0, 0, 600, 20);
            //else
            //    DragPosition = dragPosition;

            Content = content;
            Style = style;
            Options = options;

            //Debug.Log($"Is Editor?: {m_isEditor}");
            if (!m_isEditor)
            {
                m_Form = new DockForm
                {
                    Text = content.text,
                    //Location = new Point((int)position.x, (int)position.y),
                    //Size = new Size((int)position.width, (int)position.height)
                };

                m_Display = new UIDisplayer(position.SumTop(25).RestHeight(25), DrawWindow);
                m_Form.Controls.Add(m_Display);
            }

            DrawUI = true;
        }

        public Rect? DoDraw()
        {
            if (!DrawUI && !IsEditor)
                return null;

            //if (Style == null)
            //    Position = Options == null
            //        ? GUI.Window(Id, Position, DrawWindow, Content)
            //        : GUILayout.Window(Id, Position, DrawWindow, Content, Options);
            //else
            //    Position = Options == null
            //        ? GUI.Window(Id, Position, DrawWindow, Content, Style)
            //        : GUILayout.Window(Id, Position, DrawWindow, Content, Style, Options);

            //m_Form.RaiseOnPaint(null);

            //if (!m_Form.Visible)
            //{
            //    m_Form.Show();
            //    Debug.Log("Showing form!");
            //}

            return Position;
        }

        private void UpdatePosition()
        {
            if (m_Form == null) return;

            m_Form.Location = m_Position.position;
            m_Form.Size = m_Position.size;
        }

        //[Obsolete]
        //public void UpdatePosition(int nElements, float fixedSize, float horizontalPadding = 20, float verticalPadding = 70)
        //{
        //    var screenSize = UIUtils.ScreenRect.RestWidth(horizontalPadding * 2).RestHeight(verticalPadding).size;

        //    var widthFactor = Mathf.Floor(screenSize.x / fixedSize);
        //    var heightFactor = Mathf.Floor(screenSize.y / fixedSize);

        //    var hElements = fixedSize * 6;

        //    if (widthFactor < hElements)
        //        hElements = widthFactor;

        //    var vElements = Mathf.Ceil(nElements / hElements);

        //    if (heightFactor < vElements)
        //        vElements = heightFactor;

        //    var size = new Vector2(fixedSize * hElements, fixedSize * vElements);
        //    Position = new Rect(m_Position.position, size);

        //    //Debug.Log($"({hElements}, {vElements})");
        //    //Debug.Log(Position);
        //}

        protected virtual void DrawWindow()
        {
            //if (DragPosition.HasValue)
            //    GUI.DragWindow(DragPosition.Value);

            // TODO
            IsHover = Position.Contains(GlobalInput.MousePosition);

            Worker?.DrawInterface(string.Empty, null);
        }

        public DockWindow<T> Start(string title, ICommonUI<T> worker, Action editorGUI = null)
        {
            if (!IsStarted)
            {
                var m_pos = new Rect(0, 0, Screen.width, Screen.height);
                var m_dragPos = new Rect(0, 0, Screen.width, 20);

                Init(IdCounter, m_pos, m_dragPos, new GUIContent(title), null, null);

                EditorGUI = editorGUI ?? delegate { };

                ++IdCounter;

                Worker = worker;
                IsStarted = true;
            }

            return this;
        }

        public void Show(int nElements = 30, float size = 138)
        {
            // UpdatePosition(nElements, size);

            var _size = new Resolution().GetSize();
            Position = new Rect(new Vector2(Screen.width / 2f - _size.x / 2, Screen.height / 2f - _size.y / 2), _size);

            m_Form.Show();
        }

        public virtual void Update(bool enabled)
        {
            if (Instance != null)
                Instance.DrawUI = enabled;
        }

        internal class DockForm : Form
        {
            public DockForm()
            {
                FormBorderStyle = FormBorderStyle.FixedSingle;
                uwfMovable = false;

                Shown += _Shown;
                FormClosed += _Closed;
            }

            protected override void OnResize(EventArgs e)
            {
            }

            private void _Shown(object sender, EventArgs e)
            {
                DockBehaviour.IsShown = true;
            }

            private void _Closed(object sender, EventArgs e)
            {
                DockBehaviour.IsShown = false;
            }
        }

        internal class Resolution
        {
            // TODO
            private int nElements;

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

            public Vector2 GetSize(float size = 138f, int hPadding = 100, int vPadding = 150)
            {
                var screenSize = UIUtils.ScreenRect.RestWidth(hPadding * 2).RestHeight(vPadding * 2).size;
                var tuple = GetNearestTuple(Mathf.FloorToInt(screenSize.x / size));
                //Debug.Log(tuple);

                return new Vector2(tuple.Item1 * size, tuple.Item2 * size);
            }

            private Tuple<int, int> GetNearestTuple(int widthFactor)
            {
                //Debug.Log(widthFactor);
                return Grids.Select(n => new { n, Distance = Mathf.Abs(n.Item1 - widthFactor) }).OrderBy(x => x.Distance).First().n;
            }
        }
    }
}