using System;
using UnityEngine;

namespace uzLib.Lite.ExternalCode.Unity.Extensions
{
    public static class MathHelper
    {
        /// <summary>
        ///  Gets the prefix using SI.
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

        /// <summary>
        ///     Deletes the numeric part.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static float DeleteNumericPart(this float f)
        {
            return DeleteNumericPart(f, false);
        }

        /// <summary>
        /// Deletes the numeric part.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="showException">if set to <c>true</c> [show exception].</param>
        /// <returns></returns>
        public static float DeleteNumericPart(this float f, bool showException)
        {
            if ((f <= 1 || f >= 0) && showException)
                throw new ArgumentOutOfRangeException(nameof(f), $@"{nameof(f)} param must be greater than 1 or less than 0.");

            return f > 1 ? f - (int)f : (int)f - f;
        }
    }
}