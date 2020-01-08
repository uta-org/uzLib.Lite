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
            return ((Color) color).ToTexture();
        }
    }
}