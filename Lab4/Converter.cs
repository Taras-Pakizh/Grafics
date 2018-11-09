﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows;
using System.ComponentModel;

namespace Lab4
{
    static class Converter
    {
        public static BitmapSource ToBitmapSource(Bitmap bitmap)
        {

            BitmapSource bitSrc = null;

            var hBitmap = bitmap.GetHbitmap();


            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }

            return bitSrc;
        }

        public static HSV RGB_to_HSV(System.Drawing.Color color)
        {
            RGB rgb = new RGB(color.R, color.G, color.B);
            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
            v = Math.Max(Math.Max(rgb.R, rgb.G), rgb.B);
            delta = v - min;

            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            if (s == 0)
                h = 0.0;

            else
            {
                if (rgb.R == v)
                    h = (rgb.G - rgb.B) / delta;
                else if (rgb.G == v)
                    h = 2 + (rgb.B - rgb.R) / delta;
                else if (rgb.B == v)
                    h = 4 + (rgb.R - rgb.G) / delta;

                h *= 60;

                if (h < 0.0)
                    h = h + 360;
            }

            return new HSV(h, s, (v / 255));
        }

        public static RGB HSV_to_RGB(HSV hsv)
        {
            double r = 0, g = 0, b = 0;

            if (hsv.S == 0)
            {
                r = hsv.V;
                g = hsv.V;
                b = hsv.V;
            }
            else
            {
                int i;
                double f, p, q, t;

                if (hsv.H == 360)
                    hsv.H = 0;
                else
                    hsv.H = hsv.H / 60;

                i = (int)Math.Truncate(hsv.H);
                f = hsv.H - i;

                p = hsv.V * (1.0 - hsv.S);
                q = hsv.V * (1.0 - (hsv.S * f));
                t = hsv.V * (1.0 - (hsv.S * (1.0 - f)));

                switch (i)
                {
                    case 0:
                        r = hsv.V;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = hsv.V;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = hsv.V;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = hsv.V;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = hsv.V;
                        break;

                    default:
                        r = hsv.V;
                        g = p;
                        b = q;
                        break;
                }

            }

            return new RGB((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        public static CMYK RGB_to_CMYK(System.Drawing.Color color)
        {
            RGB rgb = new RGB(color.R, color.G, color.B);

            double dr = (double)rgb.R / 255;
            double dg = (double)rgb.G / 255;
            double db = (double)rgb.B / 255;
            double k = 1 - Math.Max(Math.Max(dr, dg), db);
            double c = (1 - dr - k) / (1 - k);
            double m = (1 - dg - k) / (1 - k);
            double y = (1 - db - k) / (1 - k);

            return new CMYK(c, m, y, k);
        }

        public static RGB CMYK_to_RGB(CMYK cmyk)
        {
            byte r = (byte)(255 * (1 - cmyk.C) * (1 - cmyk.K));
            byte g = (byte)(255 * (1 - cmyk.M) * (1 - cmyk.K));
            byte b = (byte)(255 * (1 - cmyk.Y) * (1 - cmyk.K));

            return new RGB(r, g, b);
        }
    }

    class HSV
    {
        public double H { get; set; }
        public double S { get; set; }
        public double V { get; set; }

        public HSV() { }
        public HSV(double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }
        public override string ToString()
        {
            return H + " " + S + " " + V;
        }

        public static HSV operator *(HSV first, HSV second)
        {
            return new HSV()
            {
                H = first.H * second.H,
                S = first.S * second.S,
                V = first.V * second.V
            };
        }

        public void ToNormal()
        {
            if (H < 0)
            {
                while (H < 0)
                    H += 360;
            }
            if (H > 360)
            {
                while (H > 360)
                    H -= 360;
            }
            if (S < 0) S = 0;
            if (S > 1) S = 1;
            if (V < 0) V = 0;
            if (V > 1) V = 1;
        }
    }

    class RGB
    {
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public RGB() { }
        public RGB(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }
        public override string ToString()
        {
            return R + " " + G + " " + B;
        }
        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb((int)R, (int)G, (int)B);
        }
    }

    class CMYK
    {
        public double C { get; set; }
        public double M { get; set; }
        public double Y { get; set; }
        public double K { get; set; }

        public CMYK() { }
        public CMYK(double c, double m, double y, double k)
        {
            C = c;
            M = m;
            Y = y;
            K = k;
        }
        public override string ToString()
        {
            return C + " " + M + " " + Y + " " + K;
        }

        public static CMYK operator *(CMYK first, CMYK second)
        {
            return new CMYK()
            {
                C = first.C * second.C,
                M = first.M * second.M,
                Y = first.Y * second.Y,
                K = first.K * second.K
            };
        }

        public void ToNormal()
        {
            if (C < 0) C = 0;
            if (C > 1) C = 1;
            if (M < 0) M = 0;
            if (M > 1) M = 1;
            if (Y < 0) Y = 0;
            if (Y > 1) Y = 1;
            if (K < 0) K = 0;
            if (K > 1) K = 1;
        }
    }
}
