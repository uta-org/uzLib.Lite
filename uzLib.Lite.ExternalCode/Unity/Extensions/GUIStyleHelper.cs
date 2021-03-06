﻿using System;
using UnityEngine;
using UnityEngine.Extensions;

namespace uzLib.Lite.ExternalCode.Extensions
{
    public static class GUIStyleHelper
    {
        public enum PaddingType
        {
            Left,
            Right,
            Top,
            Bottom
        }

        public static GUIStyle AddPadding(this GUIStyle style, int all)
            => style.AddPadding(all, all, all, all);

        public static GUIStyle AddPadding(this GUIStyle style, int value, PaddingType type)
        {
            int left;
            int right;
            int top;
            int bottom;

            switch (type)
            {
                case PaddingType.Top:
                    left = 0;
                    right = 0;
                    top = value;
                    bottom = 0;
                    break;

                case PaddingType.Bottom:
                    left = 0;
                    right = 0;
                    top = 0;
                    bottom = value;
                    break;

                case PaddingType.Left:
                    left = value;
                    right = 0;
                    top = 0;
                    bottom = 0;
                    break;

                case PaddingType.Right:
                    left = 0;
                    right = value;
                    top = 0;
                    bottom = 0;
                    break;

                default:
                    throw new ArgumentException(@"Undefined PaddingType passed!", nameof(type));
            }

            return AddPadding(style, new RectOffset(left, right, top, bottom));
        }

        public static GUIStyle AddPadding(this GUIStyle style, int horizontal, int vertical)
        {
            return AddPadding(style, new RectOffset(horizontal, horizontal, vertical, vertical));
        }

        public static GUIStyle AddPadding(this GUIStyle style, int left, int right, int top, int bottom)
        {
            return AddPadding(style, new RectOffset(left, right, top, bottom));
        }

        public static GUIStyle AddPadding(this GUIStyle style, RectOffset offset)
        {
            return new GUIStyle(style) { padding = offset };
        }

        public static GUIStyle GetStyle(this Color color)
        {
            return GetStyle(color, null);
        }

        public static GUIStyle GetStyle(this Color color, string styleName)
            => GetStyle(color, new GUIStyle(styleName));

        public static GUIStyle GetStyle(this Color color, GUIStyle other)
        {
            var style = other == null || string.IsNullOrEmpty(other.name) ? new GUIStyle() : new GUIStyle(other);

            style.normal = new GUIStyleState
            {
                background = color.ToTexture(16, 16)
            };

            return style;
        }
    }
}