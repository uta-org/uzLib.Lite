using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine.Utils.TexturePackerTool;
using uzLib.Lite.ExternalCode.Extensions;

namespace UnityEngine.Extensions
{
    using Global.IMGUI;

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
        /// Gets the center.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">texture</exception>
        public static Vector2 GetCenter(this Texture2D texture)
        {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            return new Vector2(texture.width / 2f, texture.height / 2f);
        }

        // TODO: Implement using the Manipulator
        // public static Texture2D GetTextureAsync(this Sprite sprite) { }
        // public static Texture2D GetTexturesAsync(this Texture2D _texture, params Rect[] cropRects) { }
        // public static Texture2D GetTexturesAsync(this Texture2D _texture, params Minified[] nodes) { }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <returns></returns>
        public static Texture2D GetTexture(this Sprite sprite)
        {
            // assume "sprite" is your Sprite object
            var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            croppedTexture.name = sprite.name;

            var texture = sprite.texture.isReadable ? sprite.texture : sprite.texture.DuplicateTexture();
            var rect = sprite.textureRect;
            var pixels = texture.GetPixels((int)rect.x,
                (int)(sprite.texture.height - rect.yMax),
                (int)rect.width,
                (int)rect.height);

            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            return croppedTexture;
        }

        public static IEnumerable<Texture2D> GetTextures(this Texture2D _texture, params Rect[] cropRects)
        {
            return GetTextures(_texture, cropRects.Select(rect => new UnityMinifiedNode(rect)).ToArray());
        }

        public static IEnumerable<Texture2D> GetTextures(this Texture2D _texture, params UnityMinifiedNode[] nodes)
        {
            var texture = _texture.isReadable ? _texture : _texture.DuplicateTexture();

            foreach (var node in nodes)
            {
                var rect = node.Rectangle;
                var croppedTexture = new Texture2D((int)rect.width, (int)rect.height);

                if (!string.IsNullOrEmpty(node.Name))
                    croppedTexture.name = node.Name;

                // var rect = sprite.textureRect;
                var pixels = texture.GetPixels((int)rect.x,
                    (int)(texture.height - rect.yMax),
                    (int)rect.width,
                    (int)rect.height);

                croppedTexture.SetPixels(pixels);
                croppedTexture.Apply();

                yield return croppedTexture;
            }
        }

        /// <summary>
        /// Duplicates the texture.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Texture2D DuplicateTexture(this Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);

            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);

            return readableText;
        }

        /// <summary>
        /// Creates the texture.
        /// </summary>
        /// <param name="colors">The colors.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Texture2D CreateTexture(this NativeArray<Color32> colors, int width, int height)
        {
            return CreateTexture(colors.ToArray(), width, height);
        }

        /// <summary>
        /// Creates the texture.
        /// </summary>
        /// <param name="colors">The colors.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Texture2D CreateTexture(this Color32[] colors, int width, int height)
        {
            var texture = new Texture2D(width, height);

            texture.SetPixels32(colors);
            texture.Apply();

            return texture;
        }

        /// <summary>
        /// Debugs the textures.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <exception cref="System.ArgumentNullException">list</exception>
        public static void DebugTextures(this List<Texture2D> list)
        {
            if (list.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(list));

            GUILayout.BeginHorizontal();
            {
                foreach (var texture in list)
                    GlobalGUILayout.DrawTexture(texture);
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        ///     Returns a scaled copy of given texture.
        /// </summary>
        /// <param name="tex">Source texure to scale</param>
        /// <param name="width">Destination texture width</param>
        /// <param name="height">Destination texture height</param>
        /// <param name="mode">Filtering mode</param>
        public static Texture2D Scaled(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
        {
            Rect texR = new Rect(0, 0, width, height);
            _gpu_scale(src, width, height, mode);

            //Get rendered data back to a new texture
            Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true);
            result.Resize(width, height);
            result.ReadPixels(texR, 0, 0, true);

            return result;
        }

        /// <summary>
        /// Scales the texture data of the given texture.
        /// </summary>
        /// <param name="tex">Texure to scale</param>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <param name="mode">Filtering mode</param>
        public static void Scale(Texture2D tex, int width, int height, FilterMode mode = FilterMode.Trilinear)
        {
            Rect texR = new Rect(0, 0, width, height);
            _gpu_scale(tex, width, height, mode);

            // Update new texture
            tex.Resize(width, height);
            tex.ReadPixels(texR, 0, 0, true);
            tex.Apply(true);        //Remove this if you hate us applying textures for you :)
        }

        // Internal unility that renders the source texture into the RTT - the scaling method itself.
        private static void _gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
        {
            //We need the source texture in VRAM because we render with it
            src.filterMode = fmode;
            src.Apply(true);

            //Using RTT for best quality and performance. Thanks, Unity 5
            RenderTexture rtt = new RenderTexture(width, height, 32);

            //Set the RTT in order to render to it
            Graphics.SetRenderTarget(rtt);

            //Setup 2D matrix in range 0..1, so nobody needs to care about sized
            GL.LoadPixelMatrix(0, 1, 1, 0);

            //Then clear & draw the texture to fill the entire RTT.
            GL.Clear(true, true, new Color(0, 0, 0, 0));
            Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
        }

        /// <summary>
        /// Rotates the texture.
        /// </summary>
        /// <param name="originTexture">The origin texture.</param>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static Texture2D RotateTexture(Texture2D originTexture, int angle)
        {
            var result = RotateImageMatrix(
                originTexture.GetPixels32(), originTexture.width, originTexture.height, angle);

            var resultTexture = new Texture2D(originTexture.width, originTexture.height);

            resultTexture.SetPixels32(result);
            resultTexture.Apply();

            return resultTexture;
        }

        /// <summary>
        /// Rotates the image matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static Color32[] RotateImageMatrix(Color32[] matrix, int width, int height, int angle)
        {
            Color32[] pix1 = new Color32[matrix.Length];

            int x = 0;
            int y = 0;

            Color32[] pix3 = rotateSquare(
                matrix, width, height, Math.PI / 180 * (double)angle);

            for (int j = 0; j < height; j++)
                for (var i = 0; i < width; i++)
                    pix1[x + i + width * (j + y)] = pix3[i + j * width];

            return pix3;
        }

        /// <summary>
        /// Rotates the square.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="phi">The phi.</param>
        /// <returns></returns>
        private static Color32[] rotateSquare(Color32[] arr, int width, int height, double phi)
        {
            int x;
            int y;
            int i;
            int j;
            double sn = Math.Sin(phi);
            double cs = Math.Cos(phi);
            Color32[] arr2 = new Color32[arr.Length];

            int xc = width / 2;
            int yc = height / 2;

            for (j = 0; j < height; j++)
                for (i = 0; i < width; i++)
                {
                    arr2[j * width + i] = new Color32(0, 0, 0, 0);
                    x = (int)(cs * (i - xc) + sn * (j - yc) + xc);
                    y = (int)(-sn * (i - xc) + cs * (j - yc) + yc);
                    if (x > -1 && x < width && y > -1 && y < height)
                        arr2[j * width + i] = arr[y * width + x];
                }

            return arr2;
        }
    }
}