using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using uzLib.Lite.Extensions;

namespace uzLib.Lite.Unity.Extensions
{
    /// <summary>
    /// The ColorHelper class
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Gets the orange.
        /// </summary>
        /// <value>
        /// The orange.
        /// </value>
        public static Color Orange { get { return Color.Lerp(Color.yellow, Color.red, 0.5f); } }

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

        /// <summary>
        /// Gets the color of the similar.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="cs">The cs.</param>
        /// <returns></returns>
        public static Color GetSimilarColor(this Color c1, IEnumerable<Color> cs)
        {
            return cs.OrderBy(x => x.ColorThreshold(c1)).FirstOrDefault();
        }

        /// <summary>
        /// Colors the threshold.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns></returns>
        public static float ColorThreshold(this Color c1, Color c2)
        {
            return (Mathf.Abs(c1.r - c2.r) + Mathf.Abs(c1.g - c2.g) + Mathf.Abs(c1.b - c2.b));
        }

        /// <summary>
        /// Colors the similary perc.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static float ColorSimilaryPerc(this Color a, Color b)
        {
            return 1f - (a.ColorThreshold(b) / 3);
        }

        /// <summary>
        /// Rounds the color off.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="roundTo">The round to.</param>
        /// <returns></returns>
        public static Color RoundColorOff(this Color c, float roundTo = 5)
        {
            return new Color(
                c.r.MultipleOf(roundTo),
                c.g.MultipleOf(roundTo),
                c.b.MultipleOf(roundTo),
                1f);
        }
    }
}