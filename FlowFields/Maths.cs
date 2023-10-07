using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public class Maths
    {
        public static double CalculationDeviation = 0.0000001;

        public static double GetDistance(double d1, double d2)
        {
            return Math.Max(d1, d2) - Math.Min(d1, d2);
        }

        public static double GetDistance(Vector p1, Vector p2)
        {
            double res = Math.Sqrt(Math.Pow(GetDistance(p1.x, p2.x), 2) + Math.Pow(GetDistance(p1.y, p2.y), 2));
            if (GetDistance(res, res - Math.Round(res)) < CalculationDeviation) res = Math.Round(res);
            return res;
        }

        public static double GetAngle(Vector origin, Vector point)
        {
            double originalAngle = (Math.Atan2(origin.y - point.y, origin.x - point.x) / (2 * Math.PI) * 360 + 180) % 360;
            while (originalAngle >= 360) originalAngle -= 360;
            while (originalAngle < 0) originalAngle += 360;
            return originalAngle;
        }

        public static Vector Rotate(Vector origin, Vector point, double angle)
        {
            double radius = Maths.GetDistance(origin, point);
            double newAngle = GetAngle(origin, point) + angle;
            double newAngleRadians = newAngle / 360 * 2 * Math.PI - Math.PI;
            double x = Math.Cos(newAngleRadians) * radius + origin.x;
            double y = Math.Sin(newAngleRadians) * radius + origin.y;
            if (GetDistance(x, x - Math.Round(x)) < CalculationDeviation) x = Math.Round(x);
            if (GetDistance(y, y - Math.Round(y)) < CalculationDeviation) y = Math.Round(y);
            return new Vector(x, y);
        }
    }
}
