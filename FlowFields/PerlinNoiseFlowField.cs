using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class PerlinNoiseFlowField : FlowField // https://github.com/samhooke/PerlinNoise/blob/master/Noise.cs
    {
        public double persistence = 1;
        public double frequency = 0.01;
        private double amplitude = 360;
        public int octaves = 1;
        private int randomSeed = 123;

        public PerlinNoiseFlowField(int width, int height) : base(width, height)
        {
            
        }

        public override void RenderFlowVectors(int seed)
        {
            randomSeed = seed;
            for(int y = 0; y < Height; y++)
            {
                for(int x = 0; x < Width; x++)
                {
                    vectorField[x, y] = (Vector)new AngleVector(Get2D(x, y), 1);
                }
            }
        }

        private double Get2D(double x, double y)
        {
            double total = 0;
            double tempAmplitude = amplitude;
            double tempFrequency = frequency;
            for (int i = 0; i < octaves; i++)
            {
                //tempFrequency = Math.Pow(2,i);
                //double tempAmplitude = Math.Pow(persistence,i);
                total += Gradient2D(x * tempFrequency, y * tempFrequency, randomSeed) * tempAmplitude;
                tempAmplitude *= persistence;
                tempFrequency *= 2;
            }
            return total;
        }

        private double Gradient2D(double x, double y, int seed)
        {
            // Calculate integer coordinates
            int integerX = (x > 0.0 ? (int)x : (int)x - 1);
            int integerY = (y > 0.0 ? (int)y : (int)y - 1);
            // Calculate remainder of coordinates
            double fractionalX = x - (double)integerX;
            double fractionalY = y - (double)integerY;
            // Get values for corners
            double v1 = Smooth2D(integerX, integerY, seed);
            double v2 = Smooth2D(integerX + 1, integerY, seed);
            double v3 = Smooth2D(integerX, integerY + 1, seed);
            double v4 = Smooth2D(integerX + 1, integerY + 1, seed);
            // Interpolate X
            double i1 = InterpolateCosine(v1, v2, fractionalX);
            double i2 = InterpolateCosine(v3, v4, fractionalX);
            // Interpolate Y
            return InterpolateCosine(i1, i2, fractionalY);
        }

        private double Smooth2D(int x, int y, int seed)
        {
            // 4/16 + 4/8 + 1/4 = 1

            // -- +- -+ ++
            double corners = (PerlinNoise2D(x - 1, y - 1, seed) + PerlinNoise2D(x + 1, y - 1, seed) + PerlinNoise2D(x - 1, y + 1, seed) + PerlinNoise2D(x + 1, y + 1, seed)) / 16;
            // -0 +0 0- 0+
            double sides = (PerlinNoise2D(x - 1, y, seed) + PerlinNoise2D(x + 1, y, seed) + PerlinNoise2D(x, y - 1, seed) + PerlinNoise2D(x, y + 1, seed)) / 8;
            // 00
            double center = PerlinNoise2D(x, y, seed) / 4;
            return corners + sides + center;
        }

        private double InterpolateCosine(double a, double b, double x)
        {
            double ft = x * Math.PI;
            double f = (1 - Math.Cos(ft)) * 0.5;
            return a * (1 - f) + b * f;
        }

        private double PerlinNoise2D(int x, int y, int seed)
        {
            int n = (x * 1619 + y * 31337 * 1013 * seed) & 0x7fffffff;
            n = (n << 13) ^ n;
            return (1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
        }
    }
}
