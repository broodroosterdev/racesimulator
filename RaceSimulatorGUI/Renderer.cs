using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Windows.Media.Imaging;
using Controller;
using Microsoft.Win32;
using Model;

namespace RaceSimulatorGUI
{
    public static class Renderer
    {
        #region graphics

        internal static string _straight = "straight.png";
        internal static string _start = "startgrid.png";
        internal static string _finish = "finish.png";
        internal static string _left = "leftcorner.png";
        internal static string _right = "rightcorner.png";
        internal static string _car = "car_red.png";

        #endregion

        public static Point GetCarOffset(SectionInfo info, Point origin, bool left, int Width, int Height)
        {
            Point offset;
            int Xoffset;
            if (left)
                Xoffset = Width / 4;
            else
                Xoffset = Width - ((Width / 4) * 2);

            //If the tile is vertical
            if (info.Direction % 2 == 0)
                return new Point(origin.X + Xoffset, origin.Y + Height / 2);
            else
                return new Point(origin.X + Height / 2, origin.Y + Xoffset);
        }

        public static Bitmap GetTile(SectionInfo info)
        {
            Bitmap image;
            switch (info.Type)
            {
                case SectionTypes.Straight:
                    image = ImageCache.GetImage(_straight);
                    break;
                case SectionTypes.LeftCorner:
                    image = ImageCache.GetImage(_left);
                    break;
                case SectionTypes.RightCorner:
                    image = ImageCache.GetImage(_right);
                    break;
                case SectionTypes.StartGrid:
                    image = ImageCache.GetImage(_start);
                    break;
                case SectionTypes.Finish:
                    image = ImageCache.GetImage(_finish);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            image.RotateFlip(DirectionToRotation(info.Direction));
            return image;
        }

        public static RotateFlipType DirectionToRotation(int direction)
        {
            switch (direction)
            {
                case 0:
                    return RotateFlipType.RotateNoneFlipNone;
                    break;
                case 1:
                    return RotateFlipType.Rotate90FlipNone;
                    break;
                case 2:
                    return RotateFlipType.Rotate180FlipNone;
                    break;
                case 3:
                    return RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static BitmapSource DrawTrack(Track track)
        {
            var (bounds, grid) = GridHelper.GenerateGrid(track.Sections);
            Bitmap bitmap = ImageCache.GetEmpty(1280, 720);
            Graphics g = Graphics.FromImage(bitmap);
            var tileHeight = 720 / (bounds.HighestX - bounds.LowestX);
            var tileWidth = tileHeight;
            var x = 0;
            var y = 0;
            foreach (var row in grid)
            {
                foreach (var tile in row)
                {
                    if (tile != null)
                    {
                        var origin = new Point(x * tileWidth, y * tileHeight);
                        g.DrawImage(new Bitmap(GetTile(tile), tileWidth, tileHeight), origin);
                        DrawCars(g, tile, origin, tileWidth, tileHeight);
                    }

                    x++;
                }

                x = 0;
                y++;
            }

            return ImageCache.CreateBitmapSourceFromGdiBitmap(bitmap);
        }

        public static void DrawCars(Graphics g, SectionInfo tile, Point origin, int tileWidth, int tileHeight)
        {
            var data = Data.CurrentRace.GetSectionData(tile.Section);

            if (data.Left != null)
            {
                var car = new Bitmap(ImageCache.GetImage(GetCar(data.Left)), tileWidth / 5, tileHeight / 3);
                car.RotateFlip(DirectionToRotation(tile.Direction));
                var offset = GetCarOffset(tile, origin, true, tileWidth, tileHeight);
                var opacity = data.Left.Equipment.IsBroken ? 0.5f : 1;
                g.DrawImage(ImageCache.SetImageOpacity(car, opacity), offset.X, offset.Y);
            }

            if (data.Right != null)
            {
                var car = new Bitmap(ImageCache.GetImage(GetCar(data.Right)), tileWidth / 5, tileHeight / 3);
                car.RotateFlip(DirectionToRotation(tile.Direction));
                var offset = GetCarOffset(tile, origin, false, tileWidth, tileHeight);
                var opacity = data.Right.Equipment.IsBroken ? 0.5f : 1;
                g.DrawImage(ImageCache.SetImageOpacity(car, opacity), offset.X, offset.Y);
            }
        }

        public static string GetCar(IParticipant participant)
        {
            return $"car_{participant.TeamColor.ToString().ToLower()}.png";
        }
    }
}