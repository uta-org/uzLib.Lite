using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace uzLib.Lite.Unity.Extensions
{
    public static class TextureHelper
    {
        /// <summary>
        /// The texturemap
        /// </summary>
        private static Dictionary<string, Texture2D> Texturemap = new Dictionary<string, Texture2D>();

        /// <summary>
        /// The font
        /// </summary>
        private static Texture2D Font;

        /// <summary>
        /// Writes the letter to texture.
        /// </summary>
        /// <param name="chipName">Name of the chip.</param>
        /// <param name="fontWidth">Width of the font.</param>
        /// <param name="fontHeight">Height of the font.</param>
        /// <returns></returns>
        public static Color[] WriteLetterToTexture(this char chipName, int fontWidth = 12, int fontHeight = 18)
        {
            if (Font == null)
            {
                Debug.Log("Loaded chipfont!");
                Font = Resources.Load<Texture2D>("Textures/chipfont");
            }

            // Copy each letter to the texture
            int cur_id = (int)(chipName - '0');

            return Font.GetPixels(cur_id * fontWidth, 0, fontWidth, fontHeight);
        }

        // WIP: Vector2? offset = null
        /// <summary>
        /// Writes the text to texture.
        /// </summary>
        /// <param name="chipName">Name of the chip.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="fontWidth">Width of the font.</param>
        /// <param name="fontHeight">Height of the font.</param>
        /// <returns></returns>
        public static Texture2D WriteTextToTexture(this string chipName, Texture2D texture, int fontWidth = 12, int fontHeight = 18)
        {
            if (Font == null)
                Font = Resources.Load<Texture2D>("Textures/chipfont");

            // If texture already exists, don't create it again

            int offset = 5;
            int offset_y = Font.height;

            // Copy each letter to the texture
            for (int i = 0; i < chipName.Length; i++)
            {
                int cur_id = (int)(chipName[i] - '0');
                for (int y = 0; y < fontHeight; y++)
                {
                    for (int x = 0; x < fontWidth; x++)
                    {
                        Color tempColor = Font.GetPixel(cur_id * fontWidth + x, offset_y - y);
                        texture.SetPixel(offset + x, offset_y - y + 10, tempColor);
                    }
                }
                offset += fontWidth;
            }

            texture.Apply();

            return texture;
        }

        /// <summary>
        /// Texts to texture.
        /// </summary>
        /// <param name="chipName">Name of the chip.</param>
        /// <param name="fontWidth">Width of the font.</param>
        /// <param name="fontHeight">Height of the font.</param>
        /// <returns></returns>
        public static Texture2D TextToTexture(this string chipName, int fontWidth = 12, int fontHeight = 18)
        {
            if (Font == null)
                Font = Resources.Load<Texture2D>("Textures/chipfont");

            // If texture already exists, don't create it again
            if (!Texturemap.ContainsKey(chipName))
            {
                int textureWidth = 100;
                // Generate the texture
                var texture = new Texture2D(textureWidth, 100, TextureFormat.ARGB32, false);

                for (int y = 0; y < 100; y++)
                {
                    for (int x = 0; x < textureWidth; x++)
                    {
                        texture.SetPixel(x, y, Color.clear);
                    }
                }

                int offset = 5;
                int offset_y = Font.height;

                // Copy each letter to the texture
                for (int i = 0; i < chipName.Length; i++)
                {
                    int cur_id = (int)(chipName[i] - '0');
                    for (int y = 0; y < fontHeight; y++)
                    {
                        for (int x = 0; x < fontWidth; x++)
                        {
                            Color tempColor = Font.GetPixel(cur_id * fontWidth + x, offset_y - y);
                            texture.SetPixel(offset + x, offset_y - y + 10, tempColor);
                        }
                    }
                    offset += fontWidth;
                }

                // Apply all SetPixel calls
                texture.Apply();

                Texturemap[chipName] = texture;
            }

            return Texturemap[chipName];
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <param name="tex">The tex.</param>
        /// <returns></returns>
        public static Vector2 GetSize(this Texture2D tex)
        {
            return new Vector2(tex.width, tex.height);
        }

        /// <summary>
        /// Creates the texture.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static Texture2D CreateTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];

            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = color;

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }
    }
}