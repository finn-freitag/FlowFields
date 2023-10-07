using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class GravityPointFlowField : FlowField
    {
        public List<IGravityPointFlowFieldItem> Items = new List<IGravityPointFlowFieldItem>();

        public int DefaultAngle = -1;

        public GravityPointFlowField(int width, int height) : base(width, height)
        {

        }

        public override void RenderFlowVectors(int seed)
        {
            Random.SetSeed(seed);
            for(int y = 0; y < vectorField.GetLength(1); y++)
            {
                for(int x = 0; x < vectorField.GetLength(0); x++)
                {
                    if (DefaultAngle < 0) vectorField[x, y] = (Vector)AngleVector.GetRandomNormalizedVector();
                    else vectorField[x, y] = (Vector)new AngleVector(DefaultAngle % 360, 1);
                    Vector pos = new Vector(x, y);
                    for (int i = 0; i < Items.Count; i++)
                    {
                        vectorField[x, y] = Items[i].Modify(vectorField[x, y], pos).Normalize();
                    }
                }
            }
        }
    }
}
