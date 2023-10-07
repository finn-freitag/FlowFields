using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class GravityPoint : IGravityPointFlowFieldItem
    {
        public Vector Position = new Vector(0, 0);

        public double Radius = 50;

        public ForceByDistance Gravity = (double distance, double radius) => { return 1 - distance / radius; };

        public Vector Modify(Vector flow, Vector Position)
        {
            double distance = Maths.GetDistance(this.Position, Position);
            if (distance <= Radius)
            {
                Vector direction = Position - this.Position;
                direction = direction.Normalize();
                flow = flow.Normalize();
                double gravity = Gravity(distance, Radius);
                return direction * gravity + flow * (1 - gravity);
            }
            return flow;
        }
    }
}
