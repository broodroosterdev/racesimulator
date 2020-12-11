using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Drawing.Brushes;
using FontFamily = System.Drawing.FontFamily;

namespace RaceSimulatorGUI
{
    public static class ImageCache
    {
        private static Dictionary<string, Bitmap> _cache = new Dictionary<string, Bitmap>();
        public static Bitmap GetImage(string key)
        {
            Bitmap bitmap;
            if (_cache.TryGetValue(key, out bitmap))
            {
                return bitmap;
            }
            else
            {
                bitmap = new Bitmap(key);
                _cache.Add(key, bitmap);
                return bitmap;
            }
        }

        public static Bitmap GetEmpty(int width, int height)
        {
            Bitmap bitmap;
            if (!_cache.TryGetValue("empty", out bitmap))
            {
                bitmap = new Bitmap(width, height);
                
                //Make background green
                var g = Graphics.FromImage(bitmap);
                var rect = new Rectangle(0, 0, width, height);
                g.FillRectangle(Brushes.SeaGreen, rect);
                _cache.Add("empty", bitmap);
            }
            return (Bitmap)bitmap.Clone();
        }

        public static void Clear() => _cache.Clear();
        
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}