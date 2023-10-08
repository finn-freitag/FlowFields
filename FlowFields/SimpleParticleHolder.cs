using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class SimpleParticleHolder : ParticleHolder
    {
        public double Radius = 3;
        public Brush Brush = Brushes.Red;


        public SimpleParticleHolder(int particleCount, int Width, int Height) : base(Width, Height)
        {
            particles = new List<PointF>();
            for (int i = 0; i < particleCount; i++)
            {
                particles.Add(new PointF((float)Random.Get(Width), (float)Random.Get(Height)));
            }
        }

        public override void RenderParticles(ref Bitmap bmp)
        {
            Graphics g = Graphics.FromImage(bmp);
            float width = (float)(2 * Radius);
            for(int i = 0; i < particles.Count; i++)
            {
                g.FillEllipse(Brush, (float)(particles[i].X - Radius), (float)(particles[i].Y - Radius), width, width);
            }
            g.Flush();
            g.Dispose();
        }
    }
}
