using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab5
{
    public static class Bezie
    {
        public static List<Point> Traditional(List<Point> points, double step)
        {
            List<Point> result = new List<Point>();
            for(double t = step; t < 1; t += step)
            {
                result.Add(GetPoint(t, points));
            }
            return result;
        }

        private static Point GetPoint(double t, List<Point> points)
        {
            Point result = new Point(0, 0);
            double temp;
            for(int i = 0; i < points.Count; ++i)
            {
                temp = Bernshtein(points.Count - 1, t, i);
                result.X += temp * points[i].X;
                result.Y += temp * points[i].Y;
            }
            return result;
        }

        private static int Factorial(int value)
        {
            if (value < 0)
                throw new IndexOutOfRangeException();
            if (value == 0)
                return 1;
            return value * Factorial(value - 1);
        }

        private static double Bernshtein(int n, double t, int i)
        {
            return (Factorial(n) / (Factorial(i) * Factorial(n - i))) * Math.Pow(t, i) * Math.Pow(1 - t, n - i);
        }
    }
}
