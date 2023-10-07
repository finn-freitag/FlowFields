using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class SimpleParticleHolder
    {
        public PointF[] particles;

        public readonly int Width;
        public readonly int Height;

        public SimpleParticleHolder(int particleCount, int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            particles = new PointF[particleCount];
            for (int i = 0; i < particleCount; i++)
            {
                particles[i] = new PointF((float)Random.Get(Width), (float)Random.Get(Height));
            }
        }

        public void MoveParticles(FlowField flowField, double speed)
        {
            for(int i = 0; i < particles.Length; i++)
            {
                particles[i] = particles[i] + flowField.vectorField[(int)particles[i].X, (int)particles[i].Y] * speed;
                particles[i].X = (particles[i].X + Width) % Width;
                particles[i].Y = (particles[i].Y + Height) % Height;
            }
        }

        public void RenderParticles(ref Bitmap bmp, double radius, Color color)
        {
            Graphics g = Graphics.FromImage(bmp);
            Brush b = new SolidBrush(color);
            float width = (float)(2 * radius);
            for(int i = 0; i < particles.Length; i++)
            {
                g.FillEllipse(b, (float)(particles[i].X - radius), (float)(particles[i].Y - radius), width, width);
            }
        }
    }
}
