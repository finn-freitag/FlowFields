using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class SmoothParticleHolder : SimpleParticleHolder
    {
        public SmoothParticleHolder(int particleCount, int Width, int Height) : base(particleCount, Width, Height)
        {
            Radius = 1;
            Brush = new SolidBrush(Color.FromArgb(1, 255, 0, 0));
        }

        public override void MoveParticles(FlowField flowField, double speed)
        {
            Vector size = new Vector(Width, Height);
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i] = particles[i] + flowField.vectorField[(int)particles[i].X, (int)particles[i].Y] * speed;
                if (particles[i].X < 0 || particles[i].Y < 0 || particles[i].X >= Width || particles[i].Y >= Height)
                    particles[i] = new PointF((float)Random.Get(Width), (float)Random.Get(Height));
            }
        }
    }
}
