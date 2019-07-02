namespace UnityEngine.Utils
{
    extern alias SysDrawing;
    using _Color = SysDrawing::System.Drawing.Color;

    public partial class NamedColor
    {
        public ColorNames ColorName { get; }
        public uint Color { get; }
        public _Color SysColor  => Color.ToSysColor();

        private NamedColor() { }

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