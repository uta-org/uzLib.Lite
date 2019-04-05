using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The ColorHelper class
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Posterizes the specified level.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        public static Color Posterize(this Color color, byte level)
        {
            byte r = 0,
                 g = 0,
                 b = 0;

            double value = color.R / 255.0;
            value *= level - 1;
            value = Math.Round(value);
            value /= level - 1;

            r = (byte)(value * 255);
            value = color.G / 255.0;
            value *= level - 1;
            value = Math.Round(value);
            value /= level - 1;

            g = (byte)(value * 255);
            value = color.B / 255.0;
            value *= level - 1;
            value = Math.Round(value);
            value /= level - 1;

            b = (byte)(value * 255);

            return Color.FromArgb(255, r, g, b);
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
        public static int ColorThreshold(this Color c1, Color c2)
        {
            return Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
        }

        /// <summary>
        /// Colors the similary perc.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static float ColorSimilaryPerc(this Color a, Color b)
        {
            return 1f - (a.ColorThreshold(b) / (256f * 3));
        }

        /// <summary>
        /// Rounds the color off.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="roundTo">The round to.</param>
        /// <returns></returns>
        public static Color RoundColorOff(this Color c, byte roundTo = 5)
        {
            return Color.FromArgb(255,
                c.R.RoundOff(roundTo),
                c.G.RoundOff(roundTo),
                c.B.RoundOff(roundTo));
        }
    }
}