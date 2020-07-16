using System;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The DateHelper class
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Dates the time to unix timestamp.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(this DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }
    }
}