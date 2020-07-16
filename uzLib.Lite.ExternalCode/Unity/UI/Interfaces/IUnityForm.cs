namespace UnityEngine.UI.Interfaces
{
    using ObsoleteUtils;

    /// <summary>
    ///     The Unity Form interface
    /// </summary>
    public interface IUnityForm
    {
        /// <summary>
        ///     Initializates the component.
        /// </summary>
        void InitializateComponent();

        /// <summary>
        ///     Shows the specified win rect.
        /// </summary>
        /// <param name="winRect">The win rect.</param>
        /// <param name="editorInstance">The editor instance.</param>
        /// <returns></returns>
        FormState Show(Rect winRect, WorkshopUIBalancer editorInstance);
    }
}