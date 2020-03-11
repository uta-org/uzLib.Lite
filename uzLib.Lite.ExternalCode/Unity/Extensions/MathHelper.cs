namespace uzLib.Lite.ExternalCode.Unity.Extensions
{
    public static class MathHelper
    {
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
    }
}