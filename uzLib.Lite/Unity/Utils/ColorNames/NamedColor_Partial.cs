using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine.Utils
{
    public partial class NamedColor
    {
        public ColorNames ColorName { get; }
        public uint Color { get; }

        private NamedColor() { }

        public NamedColor(ColorNames colorName, uint color)
        {
            ColorName = colorName;
            Color = color;
        }

        public override string ToString()
        {
            return ColorName.ToString();
        }
    }
}