using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class DisappearingParticleHolder : SmoothParticleHolder
    {
        public Brush Background = new SolidBrush(Color.FromArgb(25, 255, 255, 255));

        public DisappearingParticleHolder(int particleCount, int Width, int Height) : base(particleCount, Width, Height)
        {

        }

        public override void RenderParticles(ref Bitmap bmp)
        {
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Background, 0, 0, bmp.Width, bmp.Height);
            g.Flush();
            g.Dispose();
            base.RenderParticles(ref bmp);
        }
    }
}
