using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.Utils
{
    public class ColorEntity
    {
        internal static Dictionary<ColorNames, ColorEntity> m_Entities;

        public static List<string> ColorNames { get; }

        static ColorEntity()
        {
            if (m_Entities == null)
            {
                m_Entities =
                    JsonConvert.DeserializeObject<Dictionary<ColorNames, ColorEntity>>(
                        Encoding.Default.GetString(uzLib.Lite.Properties.Resources.ColorNames));

                ColorNames = m_Entities.Select(entity => entity.Value.Name).ToList();
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