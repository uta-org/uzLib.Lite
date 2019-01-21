using System;

namespace uzLib.Lite.Core
{
    using Interfaces;

    public class Singleton<T> : IStarted
        where T : class
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Activator.CreateInstance<T>();

                return _instance;
            }
            protected set
            {
                _instance = value;
            }
        }

        public bool IsStarted { get; set; }
    }
}