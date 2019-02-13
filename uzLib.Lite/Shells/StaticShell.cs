namespace uzLib.Lite.Shells
{
    public static class StaticShell
    {
        public static GitShell MyShell { get; private set; }

        static StaticShell()
        {
            if (MyShell == null)
                MyShell = new GitShell();
        }
    }
}