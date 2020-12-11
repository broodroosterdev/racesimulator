using System.Drawing;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace RaceSimulatorGUI
{
    public static class Renderer
    {
        public static BitmapSource DrawTrack(Track track)
        {
            Bitmap bitmap = ImageCache.GetEmpty(800, 450);
            return ImageCache.CreateBitmapSourceFromGdiBitmap(bitmap);
        }
    }
}