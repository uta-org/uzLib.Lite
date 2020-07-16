using System;
using System.Reflection;

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

        /// <summary>
        ///     Creates a derived class instance from a base class instance.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="baseObj">The base object.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static T FromBaseClassToDerivedClass<T>(this object baseObj)
            where T : new()
        {
            var derivedObj = new T(); //(T)Activator.CreateInstance(typeof(T));
            var t = baseObj.GetType();

            foreach (var fieldInf in t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                fieldInf.SetValue(derivedObj, fieldInf.GetValue(baseObj));

            foreach (var propInf in t.GetProperties(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                try
                {
                    propInf.SetValue(derivedObj, propInf.GetValue(baseObj));
                }
                catch
                {
                    // Some properties hasn't setter...
                }

            return derivedObj;
        }
    }
}