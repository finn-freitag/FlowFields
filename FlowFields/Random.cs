using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public static class Random
    {
        private static System.Random r = new System.Random();

        public static void SetSeed(int seed)
        {
            r = new System.Random(seed);
        }

        public static double Get()
        {
            return r.NextDouble();
        }

        public static double Get(double max)
        {
            return Get() * max;
        }
    }
}
