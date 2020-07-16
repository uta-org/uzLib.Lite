using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.Utils
{
    public class ColorEntity
    {
        public static Dictionary<ColorNames, ColorEntity> Entities { get; }

        public static List<string> ColorNames { get; }

        static ColorEntity()
        {
            if (Entities == null)
            {
#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5
                Entities =
                    JsonConvert.DeserializeObject<Dictionary<ColorNames, ColorEntity>>(
                        Encoding.Default.GetString(uzLib.Lite.ExternalCode.Properties.Resources.ColorNames));
#else
                Entities =
                    JsonConvert.DeserializeObject<Dictionary<ColorNames, ColorEntity>>(
                        Resources.Load<TextAsset>("ColorNames").text);
#endif

                ColorNames = Entities.Select(entity => entity.Value.Name).ToList();
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