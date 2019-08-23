#if !UNITY_2018 && !UNITY_2017 && !UNITY_5

extern alias SysDrawing;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using SysDrawing::System.Drawing;

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

#if !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        /// To the color.
        /// </summary>
        /// <param name="bmp">The BMP.</param>
        /// <returns></returns>
        public static IEnumerable<SysDrawing::System.Drawing.Color> ToColor(this SysDrawing::System.Drawing.Bitmap bmp)
        {
            SysDrawing::System.Drawing.Rectangle rect = new SysDrawing::System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            SysDrawing::System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, SysDrawing::System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

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

                    yield return SysDrawing::System.Drawing.Color.FromArgb(255, r, g, b);
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
        public static void SaveBitmap(this SysDrawing::System.Drawing.Color[] bmp, int width, int height, string path)
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