using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static double DateTimeToUnixTimestamp(this DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                     new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}