namespace UnityEngine.Utils
{
#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5
    using _Color = System.Drawing.Color;
#else
    using _Color = _System.Drawing.Color;
#endif

    public partial class NamedColor
    {
        public ColorNames ColorName { get; }
        public uint Color { get; }
        public _Color SysColor => Color.ToSysColor();

        private NamedColor()
        {
        }

        public NamedColor(ColorNames colorName, uint color)
        {
            ColorName = colorName;
            Color = color;
        }

        public NamedColor(ColorNames colorName, _Color color)
        {
            ColorName = colorName;
            Color = ColorToUInt(color);
        }

        private uint ColorToUInt(_Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) |
                          (color.G << 8) | (color.B << 0));
        }

        public override string ToString()
        {
            return ColorName.ToString();
        }
    }
}