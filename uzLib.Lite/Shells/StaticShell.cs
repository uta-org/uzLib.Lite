namespace uzLib.Lite.Shells
{
    /// <summary>
    /// The StaticShell class
    /// </summary>
    public static class StaticShell
    {
        /// <summary>
        /// Gets my shell.
        /// </summary>
        /// <value>
        /// My shell.
        /// </value>
        public static GitShell MyShell { get; private set; }

        /// <summary>
        /// Initializes the <see cref="StaticShell"/> class.
        /// </summary>
        static StaticShell()
        {
            if (MyShell == null)
                MyShell = new GitShell();
        }
    }
}