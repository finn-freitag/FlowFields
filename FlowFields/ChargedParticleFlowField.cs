using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class ChargedParticleFlowField : FlowField
    {
        public readonly double E0 = 8.854187817 * Math.Pow(10, -12);
        public readonly double Er = 1;

        public List<ChargedParticle> ChargedParticles = new List<ChargedParticle>();
        public double Speed = 0.5;

        public ChargedParticleFlowField(int width, int height) : base(width, height)
        {

        }

        public override void RenderFlowVectors(int seed)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector vector = new Vector(x, y);
                    Vector v = new Vector(x, y);
                    for (int i = 0; i < ChargedParticles.Count; i++)
                    {
                        ChargedParticle cp = ChargedParticles[i];
                        vector += (Vector)new AngleVector(Maths.GetAngle(v, cp.Location),
                            cp.Charge / (4 * Math.PI * E0 * Er) * (1 / Math.Pow(Maths.GetDistance(new Vector(x, y), cp.Location), 2))) * (cp.Negative ? -1 : 1);
                    }
                    vector = vector.Normalize();
                    if (vector.x == double.NaN || vector.y == double.NaN) throw new Exception();
                    vectorField[x, y] = vector * Speed;
                }
            }
        }
    }
}
