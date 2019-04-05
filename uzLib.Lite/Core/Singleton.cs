﻿using System;

namespace uzLib.Lite.Core
{
    /// <summary>
    /// The Singleton class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T>
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
    }
}