using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Utils
{
    public class ColorEntity
    {
        internal static Dictionary<ColorNames, ColorEntity> m_Entities;

        static ColorEntity()
        {
            if (m_Entities == null)
            {
                m_Entities =
                    JsonConvert.DeserializeObject<Dictionary<ColorNames, ColorEntity>>(
                        Encoding.Default.GetString(uzLib.Lite.Properties.Resources.ColorNames));
            }
        }

        public string Name { get; set; }

        public string Hex { get; set; }

        public override string ToString()
        {
            return $"{{Name={Name}, Hex={Hex}}}";
        }
    }
}