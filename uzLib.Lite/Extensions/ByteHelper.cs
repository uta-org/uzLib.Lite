using System;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The ByteHelper class
    /// </summary>
    public static class ByteHelper
    {
        /// <summary>
        /// Rounds off the specified byte.
        /// </summary>
        /// <param name="@byte">The byte.</param>
        /// <param name="roundTo">The round to.</param>
        /// <returns></returns>
        public static byte RoundOff(this byte @byte, byte roundTo = 5)
        {
            return (byte)((byte)Math.Ceiling(@byte / (double)roundTo) * roundTo);
        }
    }
}