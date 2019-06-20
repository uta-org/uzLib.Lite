using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using uzLib.Lite.Extensions;

namespace UnityEngine.Extensions
{
    /// <summary>
    /// The MathHelper class
    /// </summary>
    public static class MathHelper
    {
        /// <summary>Get the multiples the of.</summary>
        /// <param name="value">The value.</param>
        /// <param name="multipleOf">The multiple to round off.</param>
        /// <returns></returns>
        public static float MultipleOf(this float value, float multipleOf)
        {
            return Mathf.Round(value / multipleOf) * multipleOf;
        }

        /// <summary>
        /// Returns the number with the greatest absolute value
        /// </summary>
        /// <param name="nums">The nums.</param>
        /// <returns></returns>
        public static float MaxAbs(params float[] nums)
        {
            float result = 0;

            for (int i = 0; i < nums.Length; i++)
            {
                if (Mathf.Abs(nums[i]) > Mathf.Abs(result))
                {
                    result = nums[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Betweens the inclusive.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static bool BetweenInclusive(this float v, float min, float max)
        {
            return v >= min && v <= max;
        }

        /// <summary>
        /// Betweens the exclusive.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static bool BetweenExclusive(this float v, float min, float max)
        {
            return v > min && v < max;
        }

        /// <summary>
        /// Rounds to int.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static int RoundToInt(this float f)
        {
            return Mathf.RoundToInt(f);
        }

        /// <summary>
        /// Sets the y.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="yPos">The y position.</param>
        public static void SetY(this Transform t, float yPos)
        {
            Vector3 pos = t.position;

            pos.y = yPos;
            t.position = pos;
        }

        /// <summary>
        /// Distances the specified position.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="pos">The position.</param>
        /// <returns></returns>
        public static float Distance(this Transform t, Vector3 pos)
        {
            return Vector3.Distance(t.position, pos);
        }

        /// <summary>
        /// Creates the rotation around axes.
        /// </summary>
        /// <param name="degrees">The degrees.</param>
        /// <returns></returns>
        public static Quaternion CreateRotationAroundAxes(Vector3 degrees)
        {
            Quaternion rotation = Quaternion.identity;

            if (degrees.x != 0)
                rotation *= Quaternion.AngleAxis(degrees.x, Vector3.right);

            if (degrees.y != 0)
                rotation *= Quaternion.AngleAxis(degrees.y, Vector3.up);

            if (degrees.z != 0)
                rotation *= Quaternion.AngleAxis(degrees.z, Vector3.forward);

            return rotation;
        }

        /// <summary>
        /// Transforms the direction.
        /// </summary>
        /// <param name="rot">The rot.</param>
        /// <param name="dir">The dir.</param>
        /// <returns></returns>
        public static Vector3 TransformDirection(this Quaternion rot, Vector3 dir)
        {
            return rot * dir;
        }

        /// <summary>
        /// Transforms the rotation from local space to world space.
        /// </summary>
        public static Quaternion TransformRotation(this Transform tr, Quaternion rot)
        {
            Vector3 localForward = rot * Vector3.forward;
            Vector3 localUp = rot * Vector3.up;

            return Quaternion.LookRotation(tr.TransformDirection(localForward), tr.TransformDirection(localUp));
        }

        /// <summary>
        /// Sets the global scale.
        /// </summary>
        /// <param name="tr">The tr.</param>
        /// <param name="globalScale">The global scale.</param>
        public static void SetGlobalScale(this Transform tr, Vector3 globalScale)
        {
            Vector3 parentGlobalScale = tr.parent != null ? tr.parent.lossyScale : Vector3.one;
            tr.localScale = Vector3.Scale(globalScale, parentGlobalScale.Inverted());
        }

        /// <summary>
        /// Clamps the direction.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="referenceVec">The reference vec.</param>
        /// <param name="maxAngle">The maximum angle.</param>
        /// <returns></returns>
        public static Vector3 ClampDirection(Vector3 dir, Vector3 referenceVec, float maxAngle)
        {
            float angle = Vector3.Angle(dir, referenceVec);
            if (angle > maxAngle)
            {
                // needs to be clamped

                return Vector3.RotateTowards(dir, referenceVec, (angle - maxAngle) * Mathf.Deg2Rad, 0f);
                //	Vector3.Lerp( dir, referenceVec, );
            }

            return dir;
        }

        /// <summary>
        /// Get the nearest the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T NearestValue<T>(this IEnumerable<T> list, dynamic value)
        {
            if (!list.IsNullOrEmpty())
                return list.Aggregate((x, y) => Math.Abs(x - value) < Math.Abs(y - value) ? x : y);

            return default;
        }

        /// <summary>
        /// Gets the negative value.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static float GetNegativeValue(this float f)
        {
            return f < 0 ? f : -f;
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetPrefix(this long value)
        {
            if (value >= 100000000000) return (value / 1000000000).ToString("#,0") + " G";

            if (value >= 10000000000) return (value / 1000000000D).ToString("0.#") + " G";

            if (value >= 100000000) return (value / 1000000).ToString("#,0") + " M";

            if (value >= 10000000) return (value / 1000000D).ToString("0.#") + " M";

            if (value >= 100000) return (value / 1000).ToString("#,0") + " K";

            if (value >= 10000) return (value / 1000D).ToString("0.#") + " K";

            return value.ToString("#,0");
        }

        public static int Round(this float f)
        {
            return (int)f;
        }

        public static bool IsNumeric(this object Expression)
        {
            var isNum = int.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo,
                out var retNum);
            return isNum;
        }

        public static int? TryParseAsInt(this object Expression)
        {
            var isNum = int.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo,
                out var retNum);
            return isNum ? (int?)retNum : null;
        }
    }
}