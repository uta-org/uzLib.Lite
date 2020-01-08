using System;

namespace uzLib.Lite.ExternalCode.Extensions
{
    public static class BitHelper
    {
        /// <summary>
        ///     Converts to hexadecimal.
        /// </summary>
        /// <param name="byte">The byte.</param>
        /// <returns></returns>
        public static string ConvertToHex(this byte @byte)
        {
            return "0x" + BitConverter.ToString(new[] {@byte});
        }

        /// <summary>
        ///     Converts to hexadecimal.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string ConvertToHex(this byte[] bytes)
        {
            return "0x" + BitConverter.ToString(bytes).Replace("-", " 0x");
        }
    }
}