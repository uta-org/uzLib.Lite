using System;

namespace uzLib.Lite.Extensions
{
    public static class EnumHelper
    {
        public static bool Has<T>(this T type, dynamic value)
            where T : struct, IConvertible
        {
            //var flags = ToFlags<T>();
            return (type & value) == value;
        }

        public static bool Is<T>(this T type, dynamic value)
            where T : struct, IConvertible
        {
            //var flags = ToFlags<T>();
            return type == value;
        }

        public static T Add<T>(this T type, dynamic value)
            where T : struct, IConvertible
        {
            //var flags = ToFlags<T>();
            return type | value;
        }

        public static T Remove<T>(this T type, dynamic value)
            where T : struct, IConvertible
        {
            //var flags = ToFlags<T>();
            return type & ~value;
        }

        //private static Dictionary<T, int> ToFlags<T>()
        //    where T : struct, IConvertible
        //{
        //    return Enum.GetValues(typeof(T))
        //        .Cast<T>()
        //        .ToDictionary(x => x, y => Convert.ToInt32(y));
        //}
    }
}