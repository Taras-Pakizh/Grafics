using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab5
{
    static class ExtentionPoint
    {
        public static Point Add(this Point left, Point right)
        {
            return new Point()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static Point Multiple(this Point point, double value)
        {
            return new Point()
            {
                X = point.X * value,
                Y = point.Y * value
            };
        }
    }
}
