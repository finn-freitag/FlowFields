using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public static class HSLBrush
    {
        public static Brush CreateBrush(FlowField flowField)
        {
            Bitmap bmp = new Bitmap(flowField.Width, flowField.Height);
            for(int y = 0; y < flowField.Height; y++)
            {
                for(int x = 0; x < flowField.Width; x++)
                {
                    bmp.SetPixel(x, y, ToRGB((flowField.vectorField[x, y]).GetAngleVector().angle, 100, 50));
                }
            }
            return new TextureBrush(bmp);
        }

        private static Color ToRGB(double hue, double saturation, double lightness, byte alpha = 255)
        {
            double red, green, blue;

            hue = (hue + 360) % 360;

            double h = hue / 360.0;
            double s = saturation / 100.0;
            double l = lightness / 100.0;

            if (Math.Abs(s - 0.0) < double.Epsilon)
            {
                red = l;
                green = l;
                blue = l;
            }
            else
            {
                double var2;

                if (l < 0.5)
                {
                    var2 = l * (1.0 + s);
                }
                else
                {
                    var2 = l + s - s * l;
                }

                var var1 = 2.0 * l - var2;

                red = hue2Rgb(var1, var2, h + 1.0 / 3.0);
                green = hue2Rgb(var1, var2, h);
                blue = hue2Rgb(var1, var2, h - 1.0 / 3.0);
            }

            // --

            var nRed = Convert.ToInt32(red * 255.0);
            var nGreen = Convert.ToInt32(green * 255.0);
            var nBlue = Convert.ToInt32(blue * 255.0);
            return System.Drawing.Color.FromArgb((byte)alpha, (byte)nRed, (byte)nGreen, (byte)nBlue);
        }

        private static double hue2Rgb(
            double v1,
            double v2,
            double vH)
        {
            if (vH < 0.0)
            {
                vH += 1.0;
            }
            if (vH > 1.0)
            {
                vH -= 1.0;
            }
            if (6.0 * vH < 1.0)
            {
                return v1 + (v2 - v1) * 6.0 * vH;
            }
            if (2.0 * vH < 1.0)
            {
                return v2;
            }
            if (3.0 * vH < 2.0)
            {
                return v1 + (v2 - v1) * (2.0 / 3.0 - vH) * 6.0;
            }

            return v1;
        }
    }
}
