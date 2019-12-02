using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine.Extensions;

namespace UnityEngine.Utils.TexturePackerTool
{
    extern alias TexLib;

    /// <summary>
    /// The Minified Node class
    /// </summary>
    public class UnityMinifiedNode
        : TexLib::UnityTexturePacker.Lib.IMinifiedNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityTexturePacker.Lib.MinifiedNode"/> class.
        /// </summary>
        public UnityMinifiedNode() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityTexturePacker.Lib.MinifiedNode"/> class.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public UnityMinifiedNode(Rect rect)
        {
            this.rect = rect;
        }

        /// <summary>
        /// The bounds
        /// </summary>
        [JsonIgnore]
        public TexLib::System.Drawing.Rectangle Bounds { get; set; }

        [JsonProperty("Bounds")]
        public string BoundsProxy
        {
            get => Bounds.ToString();
            set => Bounds = DeserializeBoundsProperty(value);
        }

        private TexLib::System.Drawing.Rectangle DeserializeBoundsProperty(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new NotImplementedException();

            var components = value.Split(',');
            return new TexLib::System.Drawing.Rectangle(
                GetInt(components[0]),
                GetInt(components[1]),
                GetInt(components[2]),
                GetInt(components[3]));
        }

        private int GetInt(string value)
        {
            return int.Parse(Regex.Match(value, @"\d+").Value);
        }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The rect
        /// </summary>
        private Rect rect;

        /// <summary>
        /// Gets the rectangle.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        public Rect Rectangle => rect == default ? rect = GetStringRect() : rect;

        /// <summary>
        /// Gets the string rect.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private Rect GetStringRect()
        {
            return Bounds.ToRect();
        }
    }
}