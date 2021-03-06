﻿#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using uzLib.Lite.ExternalCode.Extensions;
using UnityEditor;

#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
using UnityEngine.Extensions;
#endif

namespace UnityEngine.UI.Menus
{
    using static RadioMenuComponent;

    [InitializeOnLoad]
    [AttributeUsage(AttributeTargets.Method)]
    public class RadioMenuAttribute : Attribute
    {
        private static readonly Dictionary<string, HashSet<RadioMenuAttribute>> m_menus =
            new Dictionary<string, HashSet<RadioMenuAttribute>>();

        static RadioMenuAttribute()
        {
#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
            var attrs = typeof(Editor).Assembly.GetCustomAttributes<RadioMenuAttribute>();

            // Delaying until first editor tick so that the menu
            // will be populated before setting check state, and
            // re-apply correct action
            EditorApplication.delayCall += () =>
            {
                foreach (var attr in attrs)
                    RadioMenuInvoker.PerformAction(attr, false);
            };
#endif
        }

        private RadioMenuAttribute()
        {
        }

        public RadioMenuAttribute(string name, string path)
        {
            Path = path;

            m_menus.AddOnce(name, new HashSet<RadioMenuAttribute>());
            m_menus[name].Add(this);
        }

        public string Path { get; }
        public bool Enabled { get; set; }

        public override string ToString()
        {
            return $"Path: '{Path}' | Enabled?: {Enabled}";
        }

        public static HashSet<RadioMenuAttribute> GetAttributes(string path)
        {
            var attr = GetAttributeFromPath(path);

            return m_menus.Values.FirstOrDefault(hashset => hashset.Contains(attr));
        }

        public static RadioMenuAttribute GetAttributeFromPath(string path)
        {
            return m_menus
                .Values
                .SelectMany(x => x)
                .FirstOrDefault(_attr => _attr.Path == path);
        }

        public static bool IsDefault(string name)
        {
            return (m_menus
                    .GetValue(name)?
                    .All(attr => (!attr?.Enabled ?? false) == false))
                .GetValue(true);
        }
    }
}

#endif