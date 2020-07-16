#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5
extern alias SysDrawing;
// extern alias SysDrawingImaging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
using _System.Drawing;
#else

using SysDrawing::System.Drawing;

//using System.Drawing.Imaging;

#endif

#endif

namespace uzLib.Lite.Extensions
{
    extern alias SysDrawing;

    /// <summary>
    /// The BitmapHelper helper
    /// </summary>
    public static class BitmapHelper
    {
        /// <summary>
        /// The BMP stride
        /// </summary>
        private static int BmpStride = 0;

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        /// To the color.
        /// </summary>
        /// <param name="bmp">The BMP.</param>
        /// <returns></returns>/ *
#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
        public static IEnumerable<Color> ToColor(this SystemBitmap bmp)
        {
            SystemRectangle rect = new SystemRectangle(0, 0, bmp.Width, bmp.Height);
            SystemImaging.BitmapData bmpData = bmp.LockBits(rect, SystemImaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);
#else

        public static IEnumerable<Color> ToColor(this Bitmap bmp)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //SystemImaging.BitmapData bmpData = bmp.LockBits(rect, SystemImaging.ImageLockMode.ReadWrite,
            //    bmp.PixelFormat);

            // TODO
            dynamic bmpData = default;
#endif

            IntPtr ptr = bmpData.Scan0;

            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            BmpStride = bmpData.Stride;

            for (int column = 0; column < bmpData.Height; column++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    // Little endian
                    byte b = (byte)rgbValues[column * BmpStride + row * 4];
                    byte g = (byte)rgbValues[column * BmpStride + row * 4 + 1];
                    byte r = (byte)rgbValues[column * BmpStride + row * 4 + 2];

#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
                    yield return SystemColor.FromArgb(255, r, g, b);
#else
                    yield return Color.FromArgb(255, r, g, b);
#endif
                }
            }

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
        }

        /// <summary>
        /// Saves the bitmap.
        /// </summary>
        /// <param name="bmp">The BMP.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="path">The path.</param>
#if UNITY_2020 || UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
        public static void SaveBitmap(this SystemColor[] bmp, int width, int height, string path)
#else

        public static void SaveBitmap(this Color[] bmp, int width, int height, string path)
#endif
        {
            int stride = BmpStride;
            byte[] rgbValues = new byte[BmpStride * height];

            for (int column = 0; column < height; column++)
            {
                for (int row = 0; row < width; row++)
                {
                    int i = Pn(row, column, width);

                    // Little endian
                    rgbValues[column * BmpStride + row * 4] = bmp[i].B;
                    rgbValues[column * BmpStride + row * 4 + 1] = bmp[i].G;
                    rgbValues[column * BmpStride + row * 4 + 2] = bmp[i].R;
                    rgbValues[column * BmpStride + row * 4 + 3] = bmp[i].A;
                }
            }

            using (var ms = new MemoryStream(rgbValues))
            {
                Image image = Image.FromStream(ms);
                image.Save(path);
            }

            //unsafe
            //{
            //    fixed (byte* ptr = rgbValues)
            //    {
            //        using (Bitmap image = new Bitmap(width, height, width * 4,
            //                    PixelFormat.Format32bppArgb, new IntPtr(ptr)))
            //        {
            //            image.Save(path);
            //        }
            //    }
            //}
        }

#endif

        private static int Pn(int x, int y, int w)
        {
            return x + y * w;
        }
    }
}