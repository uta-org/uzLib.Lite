using System.Reflection;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The ReflectionHelper class
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <param name="asm">The asm.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        public static void InvokeStaticMethod(this Assembly asm, string className, string methodName, params object[] parameters)
        {
            asm.GetType(className).GetMethod(methodName).Invoke(null, parameters);
        }

        /// <summary>
        /// Determines whether the specified class name has method.
        /// </summary>
        /// <param name="asm">The asm.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>
        ///   <c>true</c> if the specified class name has method; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasMethod(this Assembly asm, string className, string methodName)
        {
            return asm.GetType(className).GetMethod(methodName) != null;
        }
    }
}