using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public abstract class ParticleHolder
    {
        public List<PointF> particles = new List<PointF>();

        public readonly int Width;
        public readonly int Height;

        public ParticleHolder(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public virtual void MoveParticles(FlowField flowField, double speed)
        {
            Vector size = new Vector(Width, Height);
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i] = (particles[i] + flowField.vectorField[(int)particles[i].X, (int)particles[i].Y].GetVector() * speed + size) % size;
            }
        }

        public abstract void RenderParticles(ref Bitmap bmp);
    }
}
