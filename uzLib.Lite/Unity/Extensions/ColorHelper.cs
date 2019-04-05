using UnityEngine;

namespace uzLib.Lite.Unity.Extensions
{
    /// <summary>
    /// The ColorHelper class
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Ares the colors similar.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns></returns>
        public static bool AreColorsSimilar(Color c1, Color c2, float tolerance = .005f)
        {
            return Mathf.Abs(c1.r - c2.r) <= tolerance ^
                   Mathf.Abs(c1.g - c2.g) <= tolerance ^
                   Mathf.Abs(c1.b - c2.b) <= tolerance;
        }

        /// <summary>Gets the distance between colors.</summary>
        /// <param name="c1">The color 1.</param>
        /// <param name="c2">The color 2.</param>
        /// <returns></returns>
        public static float GetColorDistance(Color c1, Color c2)
        {
            //return Mathf.Abs((new Vector3(c1.r, c1.g, c1.b) - new Vector3(c2.r, c2.g, c2.b)).magnitude);
            return (Mathf.Abs(c1.r - c2.r) +
                   Mathf.Abs(c1.g - c2.g) +
                   Mathf.Abs(c1.b - c2.b)) / 3;
        }

        /// <summary>Gets a random color.</summary>
        /// <returns></returns>
        public static Color GetRandomColor(bool isTransparent)
        {
            return new Color(Random.value, Random.value, Random.value, isTransparent ? 0 : 1);
        }
    }
}