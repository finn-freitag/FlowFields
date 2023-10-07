using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public abstract class FlowField
    {
        public readonly int Width;
        public readonly int Height;

        public Vector[,] vectorField { get; }

        public FlowField(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            vectorField = new Vector[width, height];
        }

        public void RenderFlowVectors()
        {
            RenderFlowVectors((int)(DateTime.UtcNow.Ticks / 2));
        }

        public abstract void RenderFlowVectors(int seed);

        public void RenderFlowVectorsToBitmap(ref Bitmap bmp, int size, Color color)
        {
            if (bmp.Width != vectorField.GetLength(0) || bmp.Height != vectorField.GetLength(1)) throw new ArgumentException("Bitmap size must match vectorField size!");
            Graphics g = Graphics.FromImage(bmp);
            Pen p = new Pen(new SolidBrush(color));
            for (int y = 0; y < bmp.Height; y += size)
            {
                for (int x = 0; x < bmp.Width; x += size)
                {
                    int vx = x + size / 2;
                    int vy = y + size / 2;
                    if (vx <= bmp.Width && vy <= bmp.Height)
                    {
                        AngleVector v = (AngleVector)vectorField[vx, vy];
                        v.length = size / 2;
                        Vector v2 = (Vector)v;
                        Vector pos = new Vector(x + 1, y + 1);
                        g.DrawLine(p, pos - v2, pos + v2);
                    }
                }
            }
            g.Flush();
            g.Dispose();
        }
    }
}
