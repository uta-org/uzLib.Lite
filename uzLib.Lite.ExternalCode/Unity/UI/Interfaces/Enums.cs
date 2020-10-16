namespace UnityEngine.UI.Interfaces
{
    /// <summary>
    ///     The Form State (there are all the flags for all forms)
    /// </summary>
    public enum FormState
    {
        /// <summary>
        /// Has exceptions
        /// </summary>
        HasExceptions,

        /// <summary>
        ///     No state
        /// </summary>
        None,

        /// <summary>
        ///     Empty result
        /// </summary>
        EmptyResult,

        /// <summary>
        ///     Displaying information
        /// </summary>
        DisplayingInfo,

        /// <summary>
        ///     In editor while playing
        /// </summary>
        EditorWhilePlaying,

        /// <summary>
        ///     Waiting items
        /// </summary>
        WaitingItems,

        /// <summary>
        ///     The request has errors
        /// </summary>
        RequestHasErrors,

        /// <summary>
        ///     The waiting for worker
        /// </summary>
        WaitingForWorker,

        /// <summary>
        ///     Is inspecting?
        /// </summary>
        IsInspecting,

        /// <summary>
        ///     Is retrying?
        /// </summary>
        IsRetrying
    }
}