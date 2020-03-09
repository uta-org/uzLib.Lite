using System;
using System.Globalization;
using uzLib.Lite.Extensions;
using uzLib.Lite.ExternalCode.Extensions;

namespace UnityEngine.Utils
{
    extern alias SysDrawing;

    using _Color = SysDrawing::System.Drawing.Color;

    public static class ColorEntityExtensions
    {
        private const int ARGBAlphaShift = 24;
        private const int ARGBRedShift = 16;
        private const int ARGBGreenShift = 8;
        private const int ARGBBlueShift = 0;

        public static Color ToColor(this ColorNames name)
        {
            var entity = ColorEntity.m_Entities[name];

            return ToColor(entity.Hex.SanitizeHex());
        }

        private static string SanitizeHex(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            string hex = input.Replace("#", string.Empty);

            if (hex.Length == 6)
                hex = "ff" + hex;

            return hex.ToUpperInvariant();
        }

        public static uint ToUInt(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            string pseudoHex = input.SanitizeHex();

            if (!pseudoHex.IsHex())
                throw new ArgumentException("Is not hex!", nameof(input));

            return uint.Parse(pseudoHex);
        }

        public static Color ToColor(this string hex)
        {
            if (!uint.TryParse(hex.SanitizeHex(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value))
            {
                Debug.LogError($"Couldn't convert hex value: '{hex}'!");
                return default;
            }

            byte A = (byte)((value >> ARGBAlphaShift) & 0xFF);
            byte R = (byte)((value >> ARGBRedShift) & 0xFF);
            byte G = (byte)((value >> ARGBGreenShift) & 0xFF);
            byte B = (byte)((value >> ARGBBlueShift) & 0xFF);

            return new Color32(R, G, B, A);
        }

        public static uint ToUInt(this _Color c)
        {
            return (uint)(((c.A << 24) | (c.R << 16) | (c.G << 8) | c.B) & 0xffffffffL);
        }

        public static Color ToColor(this uint value)
        {
            byte A = (byte)((value >> ARGBAlphaShift) & 0xFF);
            byte R = (byte)((value >> ARGBRedShift) & 0xFF);
            byte G = (byte)((value >> ARGBGreenShift) & 0xFF);
            byte B = (byte)((value >> ARGBBlueShift) & 0xFF);

            return new Color32(R, G, B, A);
        }

        public static _Color ToSysColor(this uint value)
        {
            byte A = (byte)((value >> ARGBAlphaShift) & 0xFF);
            byte R = (byte)((value >> ARGBRedShift) & 0xFF);
            byte G = (byte)((value >> ARGBGreenShift) & 0xFF);
            byte B = (byte)((value >> ARGBBlueShift) & 0xFF);

            return _Color.FromArgb(A, R, G, B);
        }

        public static Color ToColor(this _Color sysColor)
        {
            return new Color32(sysColor.R, sysColor.G, sysColor.B, sysColor.A);
        }

        public static bool IsColorAvailable(this string colorName, out Color color)
        {
            bool isAvailable = Enum.TryParse<ColorNames>(colorName, true, out var name);
            color = name.ToColor();

            return isAvailable && color != default;
        }
    }
}