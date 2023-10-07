using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class TangentGravityPoint : IGravityPointFlowFieldItem
    {
        public Vector Position = new Vector(0, 0);

        public double Radius = 50;

        public bool isCounterClockwise = false;

        public ForceByDistance Gravity = (double distance, double radius) => { return 1 - distance / radius; };

        public Vector Modify(Vector flow, Vector Position)
        {
            double distance = Maths.GetDistance(this.Position, Position);
            if (distance <= Radius)
            {
                AngleVector direction = (AngleVector)(Position - this.Position);
                direction.angle += 90 + (isCounterClockwise ? 180 : 0);
                direction.length = 1;
                flow = flow.Normalize();
                double gravity = Gravity(distance, Radius);
                return (Vector)direction * gravity + flow * (1 - gravity);
            }
            return flow;
        }
    }
}
