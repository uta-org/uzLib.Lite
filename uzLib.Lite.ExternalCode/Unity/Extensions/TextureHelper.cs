using System.Collections.Generic;
using System.Linq;
using UnityEngine.Utils.TexturePackerTool;

namespace UnityEngine.Extensions
{
    public static class TextureHelper
    {
        /// <summary>
        ///     Creates the texture.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static Texture2D CreateTexture(int width, int height, Color color)
        {
            var pixels = new Color[width * height];

            for (var i = 0; i < pixels.Length; i++)
                pixels[i] = color;

            var texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }

        /// <summary>
        ///     To the texture.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Texture2D ToTexture(this Color color, int width, int height)
        {
            return CreateTexture(width, height, color);
        }

        /// <summary>
        ///     To the texture.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static Texture2D ToTexture(this Color color)
        {
            return CreateTexture(1, 1, color);
        }

        /// <summary>
        ///     To the texture.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static Texture2D ToTexture(this Color32 color)
        {
            return ((Color)color).ToTexture();
        }

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

        /// <summary>
        /// Gets the textures.
        /// </summary>
        /// <param name="_texture">The texture.</param>
        /// <param name="cropRects">The crop rects.</param>
        /// <returns></returns>
        public static IEnumerable<Texture2D> GetTextures(this Texture2D _texture, params Rect[] cropRects)
        {
            return GetTextures(_texture, cropRects.Select(rect => new UnityMinifiedNode(rect)).ToArray());
        }

        /// <summary>
        /// Gets the textures.
        /// </summary>
        /// <param name="_texture">The texture.</param>
        /// <param name="nodes">The nodes.</param>
        /// <returns></returns>
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
    }
}