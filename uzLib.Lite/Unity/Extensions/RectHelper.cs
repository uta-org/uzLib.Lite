using UnityEngine;

namespace uzLib.Lite.Extensions
{
    public static class RectHelper
    {
        //#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

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

        //#endif
    }
}