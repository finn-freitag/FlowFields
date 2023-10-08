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
        public int randomSeed = 123;
        public bool Animate = false;
        public double VariationSpeed = 10;
        private double zCoord = 1;

        public PerlinNoiseFlowField(int width, int height) : base(width, height)
        {
            randomSeed = (int)(DateTime.UtcNow.Ticks / 2);
        }

        public override void RenderFlowVectors(int seed)
        {
            if (!Animate)
            {
                randomSeed = seed;
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        vectorField[x, y] = new AngleVector(Get2D(x, y), 1);
                    }
                }
            }
            else
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        vectorField[x, y] = new AngleVector(Get3D(x, y, zCoord), 1);
                    }
                }
                zCoord += VariationSpeed;
                zCoord = zCoord % (double.MaxValue - 2 * VariationSpeed);
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

        private double Get3D(double x, double y, double z)
        {
            double total = 0;
            double tempAmplitude = amplitude;
            double tempFrequency = frequency;
            for (int i = 0; i < octaves; i++)
            {
                //tempFrequency = Math.Pow(2,i);
                //double tempAmplitude = Math.Pow(persistence,i);
                total += Gradient3D(x * tempFrequency, y * tempFrequency, z * tempFrequency, randomSeed) * tempAmplitude;
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

        private double Gradient3D(double x, double y, double z, int seed)
        {
            // Calculate integer coordinates
            int integerX = (x > 0.0 ? (int)x : (int)x - 1);
            int integerY = (y > 0.0 ? (int)y : (int)y - 1);
            int integerZ = (z > 0.0 ? (int)z : (int)z - 1);
            // Calculate remainder of coordinates
            double fractionalX = x - (double)integerX;
            double fractionalY = y - (double)integerY;
            double fractionalZ = z - (double)integerZ;
            // Get values for corners
            double v1 = Smooth3D(integerX, integerY, integerZ, seed);
            double v2 = Smooth3D(integerX + 1, integerY, integerZ, seed);
            double v3 = Smooth3D(integerX, integerY + 1, integerZ, seed);
            double v4 = Smooth3D(integerX + 1, integerY + 1, integerZ, seed);
            double v5 = Smooth3D(integerX, integerY, integerZ + 1, seed);
            double v6 = Smooth3D(integerX + 1, integerY, integerZ + 1, seed);
            double v7 = Smooth3D(integerX, integerY + 1, integerZ + 1, seed);
            double v8 = Smooth3D(integerX + 1, integerY + 1, integerZ + 1, seed);
            // Interpolate X
            double ii1 = InterpolateCosine(v1, v2, fractionalX);
            double ii2 = InterpolateCosine(v3, v4, fractionalX);
            double ii3 = InterpolateCosine(v5, v6, fractionalX);
            double ii4 = InterpolateCosine(v7, v8, fractionalX);
            // Interpolate Y
            double i1 = InterpolateCosine(ii1, ii2, fractionalY);
            double i2 = InterpolateCosine(ii3, ii4, fractionalY);
            // Interpolate Z
            return InterpolateCosine(i1, i2, fractionalZ);
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

        private double Smooth3D(int x, int y, int z, int seed)
        {
            // 12/48 + 8/32 + 6/16 + 1/8 = 1

            // ++0 -+0 0++ 0+-   +-0 --0 0-+ 0--   +0+ +0- -0+ -0-
            double edges = 0;
            edges += PerlinNoise3D(x + 1, y + 1, z, seed) + PerlinNoise3D(x - 1, y + 1, z, seed) + PerlinNoise3D(x, y + 1, z + 1, seed) + PerlinNoise3D(x, y + 1, z - 1, seed);
            edges += PerlinNoise3D(x + 1, y - 1, z, seed) + PerlinNoise3D(x - 1, y - 1, z, seed) + PerlinNoise3D(x, y - 1, z + 1, seed) + PerlinNoise3D(x, y - 1, z - 1, seed);
            edges += PerlinNoise3D(x + 1, y, z + 1, seed) + PerlinNoise3D(x + 1, y, z - 1, seed) + PerlinNoise3D(x - 1, y, z + 1, seed) + PerlinNoise3D(x - 1, y, z - 1, seed);
            edges /= 48;
            // --- --+ -+- -++ +-- +-+ ++- +++
            double corners = 0;
            corners += PerlinNoise3D(x - 1, y - 1, z - 1, seed) + PerlinNoise3D(x - 1, y - 1, z + 1, seed) + PerlinNoise3D(x - 1, y + 1, z - 1, seed) + PerlinNoise3D(x - 1, y + 1, z + 1, seed);
            corners += PerlinNoise3D(x + 1, y - 1, z - 1, seed) + PerlinNoise3D(x + 1, y - 1, z + 1, seed) + PerlinNoise3D(x + 1, y + 1, z - 1, seed) + PerlinNoise3D(x + 1, y + 1, z + 1, seed);
            corners /= 32;
            // +00 -00 0+0 0-0 00+ 00-
            double sides = 0;
            corners += PerlinNoise3D(x - 1, y, z, seed) + PerlinNoise3D(x - 1, y, z, seed) + PerlinNoise3D(x, y + 1, z, seed);
            corners += PerlinNoise3D(x, y - 1, z, seed) + PerlinNoise3D(x, y, z + 1, seed) + PerlinNoise3D(x, y, z - 1, seed);
            corners /= 16;
            // 000
            double center = PerlinNoise3D(x, y, z, seed) / 8;

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

        private double PerlinNoise3D(int x, int y, int z, int seed)
        {
            int n = (x * 1619 + y * 31337 + z * 52591 * 1013 * seed) & 0x7fffffff;
            n = (n << 13) ^ n;
            return (1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
        }
    }
}
