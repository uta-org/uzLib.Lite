namespace uzLib.Lite.ExternalCode.Extensions
{
    public static class NullableHelper
    {
        public static T GetValue<T>(this T? nul)
            where T : struct
        {
            return nul ?? default;
        }

        public static T GetValue<T>(this T? nul, T defValue)
            where T : struct
        {
            return nul ?? defValue;
        }
    }
}