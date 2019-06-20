using System;

namespace UnityEngine.Extensions
{
    /// <summary>
    ///     The Bit Helper
    /// </summary>
    public static class BitHelper
    {
        /// <summary>
        ///     Gets the int from packed byte.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public static int GetIntFromPackedByte(this byte input, int start, int end)
        {
            var size = end - start;
            var shiftAmount = 8 - end;
            var mask = (1 << size) - 1;
            var result = (input >> shiftAmount) & mask;

            //Debug.Log("size: " + size + ", shiftAmount: " + shiftAmount + ", mask: " + mask + ", result: " + result);

            return result;
        }

        /// <summary>
        ///     Gets the int16 from bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static int GetInt16FromBytes(this byte[] bytes, int offset)
        {
            return BitConverter.ToInt16(new[] { bytes[offset], bytes[offset + 1] }, 0);
        }
    }
}