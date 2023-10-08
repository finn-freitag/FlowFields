using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public struct AngleVector : VectorBase
    {
        public double angle;
        public double length;

        public AngleVector(double angle, double length)
        {
            this.angle = angle;
            this.length = length;
        }

        public AngleVector Normalize()
        {
            return new AngleVector(angle, 1);
        }

        public static AngleVector GetRandomNormalizedVector()
        {
            return new AngleVector(Random.Get(360), 1);
        }

        public Vector GetVector()
        {
            return (Vector)this;
        }

        public AngleVector GetAngleVector()
        {
            return this;
        }

        public static explicit operator AngleVector(double angle)
        {
            return new AngleVector(angle, 1);
        }

        public static explicit operator AngleVector(Vector vec)
        {
            return new AngleVector(Maths.GetAngle(Vector.Zero, vec), vec.GetLength());
        }
    }
}
