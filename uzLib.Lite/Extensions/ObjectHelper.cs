using System;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The ObjectHelper class
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Determines whether this instance is casteable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if the specified input is casteable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCasteable<T>(this object input)
        {
            try
            {
                Convert.ChangeType(input, typeof(T));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}