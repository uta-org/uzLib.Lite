using System;
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

        public static bool IsHover { get; set; }
        public int Id { get; private set; }
        public Rect Position { get; private set; }
        public Rect? DragPosition { get; private set; }
        public GUIContent Content { get; private set; }
        public GUIStyle Style { get; private set; }
        public GUILayoutOption[] Options { get; private set; }
        public bool DrawUI { get; set; }
        public bool IsEditor { get; set; }

        public Action EditorGUI { get; set; }

        public ICommonUI<T> Worker { get; set; }

        private UIDisplayer m_Display;
        private Form m_Form;
        private bool m_isEditor;

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

            const float margin = 20f;

            var mainCamera = Camera.main;

            if (position != Rect.zero)
                position = position
                    .SumTop(10)
                    .SumLeft(10)
                    .RestWidth(margin)
                    .RestHeight(margin);

            Position = position == Rect.zero
                ? new Rect(Vector2.one * 10f, new Vector2(
                    mainCamera.pixelWidth - margin,
                    mainCamera.pixelHeight - margin))
                : position;

            if (dragPosition.HasValue && dragPosition.Value == Rect.zero)
                DragPosition = new Rect(0, 0, 600, 20);
            else
                DragPosition = dragPosition;

            Content = content;
            Style = style;
            Options = options;

            Debug.Log($"Is Editor?: {m_isEditor}");
            if (!m_isEditor)
            {
                m_Form = new Form
                {
                    Text = content.text,
                    Location = new Point((int)position.x, (int)position.y),
                    Size = new Size((int)position.width, (int)position.height)
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
                m_isEditor = editorGUI != null;

                ++IdCounter;

                Worker = worker;
                IsStarted = true;
            }

            return this;
        }

        public void Show()
        {
            m_Form.Show();
        }

        public virtual void Update(bool enabled)
        {
            if (Instance != null)
                Instance.DrawUI = enabled;
        }
    }
}