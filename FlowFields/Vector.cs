using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public struct Vector : VectorBase
    {
        public static readonly Vector Zero = new Vector(0, 0);

        public double x;
        public double y;

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector Normalize()
        {
            AngleVector vec = (AngleVector)this;
            vec.length = 1;
            return (Vector)vec;
        }

        public double GetLength()
        {
            return Maths.GetDistance(Vector.Zero, this);
        }

        public Vector GetVector()
        {
            return this;
        }

        public AngleVector GetAngleVector()
        {
            return (AngleVector)this;
        }

        public static explicit operator Vector(AngleVector vec)
        {
            return new Vector(Math.Cos(vec.angle / 360 * 2 * Math.PI - Math.PI) * vec.length, Math.Sin(vec.angle / 360 * 2 * Math.PI - Math.PI) * vec.length);
        }

        public static implicit operator PointF(Vector vec)
        {
            return new PointF((float)vec.x, (float)vec.y);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y);
        }

        public static Vector operator *(Vector a, double b)
        {
            return new Vector(a.x * b, a.y * b);
        }

        public static PointF operator %(PointF a, Vector b)
        {
            return new Vector(a.X % b.x, a.Y % b.y);
        }

        public static PointF operator +(PointF a, Vector b)
        {
            return new PointF(a.X + (float)b.x, a.Y + (float)b.y);
        }
    }
}
