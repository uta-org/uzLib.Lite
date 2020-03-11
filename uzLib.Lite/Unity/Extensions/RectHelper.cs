﻿namespace UnityEngine.Extensions
{
    public static class RectHelper
    {
        public static int GetLength(this Rect rect)
        {
            return Mathf.RoundToInt(rect.width * rect.width);
        }

        public static Rect ResetPosition(this Rect rect)
        {
            return new Rect(Vector2.zero, rect.size);
        }

        public static Rect SumTop(this Rect rect, float topMargin)
        {
            return new Rect(rect.position.x, rect.position.y + topMargin, rect.width, rect.height);
        }

        public static Rect RestWidth(this Rect rect, float width)
        {
            return new Rect(rect.position.x, rect.position.y, rect.width - width, rect.height);
        }
    }
}