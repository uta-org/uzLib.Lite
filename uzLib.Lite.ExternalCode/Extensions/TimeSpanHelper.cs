using System;

namespace uzLib.Lite.ExternalCode.Extensions
{
    /// <summary>
    ///     The TimeSpan Helper
    /// </summary>
    public static class TimeSpanHelper
    {
        /// <summary>
        ///     Converts the seconds to date.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static string ConvertSecondsToDate(this float seconds)
        {
            return ((double)seconds).ConvertSecondsToDate();
        }

        /// <summary>
        ///     Converts the seconds to date.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static string ConvertSecondsToDate(this double seconds)
        {
            const string secondsStr = @"ss\.ff\s",
                         minutesStr = @"mm\m\ ",
                         hoursStr = @"hh\h\ ";

            string date = $"{secondsStr}";

            if (seconds >= 60)
                date = minutesStr + date;
            else if (seconds >= 3600)
                date = hoursStr + date;

            var t = TimeSpan.FromSeconds(Convert.ToDouble(seconds));

            if (t.Days > 0) return t.ToString($@"d\d\ {date}");

            return t.ToString(date);
        }
    }
}