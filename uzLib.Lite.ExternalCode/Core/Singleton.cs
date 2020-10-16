using System;
using uzLib.Lite.ExternalCode.Core.Interfaces;

namespace uzLib.Lite.ExternalCode.Core
{
    /// <summary>
    /// The Singleton class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : IStarted
        where T : class
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static T _instance;

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
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