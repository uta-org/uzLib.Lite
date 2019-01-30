using System;

namespace uzLib.Lite.Core
{
    public class Singleton<T>
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
    }
}