using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class FlowDirection : IGravityPointFlowFieldItem
    {
        public Vector Position = new Vector(0, 0);
        public AngleVector Flow = new AngleVector(0, 0.5);

        public ForceByDistance Force = (double distance, double radius) => { return 1 - distance / radius; };

        public Vector Modify(Vector flow, Vector Position)
        {
            double distance = Maths.GetDistance(this.Position, Position);
            if (distance <= Flow.length)
            {
                Vector direction = (Vector)Flow.Normalize();
                flow = flow.Normalize();
                double gravity = Force(distance, Flow.length);
                return direction * gravity - flow * (1 - gravity);
            }
            return flow;
        }
    }
}
