using UnityEngine;

namespace uzLib.Lite.Unity.Extensions
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
        public static float MultipleOf(float value, float multipleOf)
        {
            return Mathf.Round(value / multipleOf) * multipleOf;
        }
    }
}