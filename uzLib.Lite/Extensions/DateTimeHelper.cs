using System;

namespace UnityEngine.Extensions
{
    public static class DateTimeHelper
    {
        /// <summary>
        ///     Determines whether [has passed from] [the specified date].
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="expireDate">The expire date.</param>
        /// <param name="hours">The hours.</param>
        /// <returns>
        ///     <c>true</c> if [has passed from] [the specified from date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPassedFrom(DateTime fromDate, DateTime expireDate, double hours)
        {
            return expireDate - fromDate > TimeSpan.FromHours(hours);
        }

        /// <summary>
        ///     Determines whether [has passed from now] [the specified from date].
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="hours">The hours.</param>
        /// <returns>
        ///     <c>true</c> if [has passed from now] [the specified from date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPassedFromNow(this DateTime fromDate, double hours)
        {
            return HasPassedFrom(fromDate, DateTime.Now, hours);
        }
    }
}