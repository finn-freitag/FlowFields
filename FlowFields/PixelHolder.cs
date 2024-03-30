using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public static class PixelHolder
    {
        public static Bitmap MovePixels(Bitmap bmp, FlowField flowField, double speed = 1)
        {
            if (bmp.Width != flowField.Width || bmp.Height != flowField.Height) throw new ArgumentException("Bitmap and flow field have to match in size!");
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    PointF oldLoc = new Vector(x, y) - flowField.vectorField[x, y].GetVector() * speed;
                    Point p = new Point(Math.Max(0, Math.Min((int)oldLoc.X, bmp.Width - 1)), Math.Max(0, Math.Min((int)oldLoc.Y, bmp.Height - 1)));
                    newBmp.SetPixel(x, y, bmp.GetPixel(p.X, p.Y));
                }
            }
            return newBmp;
        }
    }
}
