using System;
using System.Linq.Expressions;
using UnityEngine.Extensions;

namespace UnityEngine.Utils.DebugTools
{
    /// <summary>
    /// The DebugHelper class
    /// </summary>
    public static class DebugHelper
    {
        /// <summary>
        /// Debugs the parameters.
        /// </summary>
        /// <param name="params">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">params</exception>
        public static string DebugParams(params Expression<Func<object>>[] @params)
        {
            if (@params == null || @params.Length == 0)
                throw new ArgumentNullException(nameof(@params));

            string debugStr = string.Empty;
            int counter = 0;

            foreach (var param in @params)
            {
                string paramName = ExpressionHelper.GetMemberName(param);
                object value = param?.Compile().Invoke();

                debugStr += $"{paramName}: {value}";

                if (counter < @params.Length - 1)
                    debugStr += " || ";

                ++counter;
            }

            return debugStr;
        }

        /// <summary>
        /// Logs the specified parameters.
        /// </summary>
        /// <param name="params">The parameters.</param>
        public static void Log(params Expression<Func<object>>[] @params)
        {
            Debug.Log(DebugParams(@params));
        }

        /// <summary>
        /// Warnings the specified parameters.
        /// </summary>
        /// <param name="params">The parameters.</param>
        public static void Warning(params Expression<Func<object>>[] @params)
        {
            Debug.LogWarning(DebugParams(@params));
        }

        /// <summary>
        /// Errors the specified parameters.
        /// </summary>
        /// <param name="params">The parameters.</param>
        public static void Error(params Expression<Func<object>>[] @params)
        {
            Debug.LogError(DebugParams(@params));
        }
    }
}