using System;
using System.Collections.Generic;
using System.Drawing;

namespace RaceSimulatorGUI
{
    public class ImageCache
    {
        private Dictionary<string, Bitmap> _cache = new Dictionary<string, Bitmap>();

        public Bitmap GetImage(string key)
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

        public void Clear() => _cache.Clear();
    }
}