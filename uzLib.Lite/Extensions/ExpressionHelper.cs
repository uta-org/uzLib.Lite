using System;
using System.Linq.Expressions;

namespace UnityEngine.Extensions
{
    /// <summary>
    /// The ExpressionHelper class
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static string GetMemberName<T>(Expression<Func<T>> expression)
        {
            if (expression.Body is MemberExpression)
                return ((MemberExpression)expression.Body).Member.Name;

            var op = ((UnaryExpression)expression.Body).Operand;
            return ((MemberExpression)op).Member.Name;
        }
    }
}