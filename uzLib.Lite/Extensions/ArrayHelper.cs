using System;

namespace uzLib.Lite.Extensions
{
    public static class ArrayHelper
    {
        public static T Try<T>(this T[] args, int index, T alternative)
        {
            try
            {
                return args[index];
            }
            catch
            {
                return alternative;
            }
        }

        public static T Try<T>(this T[] args, int index, T alternative, Func<T, T> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            try
            {
                return alternative?.Equals(default) == false ? transform(args[index]) : args[index];
            }
            catch
            {
                return alternative?.Equals(default) == false
                    ? transform(alternative)
                    : default;
            }
        }
    }
}