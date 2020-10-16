using System;

namespace UnityEngine.UI.Interfaces
{
    /// <summary>
    ///     The Common UI interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommonUI<T>
        where T : IUnityForm, new()
    {
        /// <summary>
        ///     Gets the window.
        /// </summary>
        /// <value>
        ///     The window.
        /// </value>
        DockWindow<T> Window { get; }

        /// <summary>
        ///     Draws the interface.
        /// </summary>
        /// <param name="editorInstance">The editor instance.</param>
        void DrawInterface(object editorInstance, Action callback);
    }
}