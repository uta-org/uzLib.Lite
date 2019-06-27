using System;

namespace UnityEngine.Utils
{
    public static class ColorEntityExtensions
    {
        private const int ARGBRedShift = 16;
        private const int ARGBGreenShift = 8;
        private const int ARGBBlueShift = 0;

        public static Color ToColor(this ColorNames name)
        {
            var entity = ColorEntity.m_Entities[name];
            string hex = entity.Hex.Replace("#", "0x");

            return ToColor(hex);
        }

        public static Color ToColor(this string hex)
        {
            uint value = uint.Parse(hex);
            byte R = (byte)((value >> ARGBRedShift) & 0xFF);
            byte G = (byte)((value >> ARGBGreenShift) & 0xFF);
            byte B = (byte)((value >> ARGBBlueShift) & 0xFF);

            return new Color32(R, G, B, 255);
        }

        public static bool IsColorAvailable(this string colorName, out Color color)
        {
            bool isAvailable = Enum.TryParse<ColorNames>(colorName, true, out var name);
            color = name.ToColor();

            return isAvailable;
        }
    }
}