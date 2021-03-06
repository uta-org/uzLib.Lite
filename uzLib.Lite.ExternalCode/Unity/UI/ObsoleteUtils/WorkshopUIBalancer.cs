﻿using System;

namespace UnityEngine.ObsoleteUtils
{
    using UI;

    [Obsolete]
    public class WorkshopUIBalancer
    {
        private WorkshopUIBalancer()
        {
        }

        public WorkshopUIBalancer(object editorInstance, UIBalancedDraw balancedDraw)
        {
            EditorInstance = editorInstance;
            BalancedDraw = balancedDraw;
        }

        public object EditorInstance { get; }
        public UIBalancedDraw BalancedDraw { get; }
    }
}