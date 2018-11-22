using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab6
{
    static class RectangleCreator
    {
        public static List<Point> Create(Point first, Point second)
        {
            Point swap;
            if(first.Y > second.Y)
            {
                swap = first;
                first = second;
                second = swap;
            }

            Point diff = new Point()
            {
                X = second.X - first.X,
                Y = second.Y - first.Y
            };

            if (diff.X < 0)
            {
                diff.X *= -1;
                diff.Y *= -1;
            }

            List<Point> result = new List<Point>();
            result.Add(first);
            result.Add(second);
            result.Add(new Point()
            {
                X = (2 * second.X - diff.Y) / 2,
                Y = (2 * second.Y + diff.X) / 2
            });
            result.Add(new Point()
            {
                X = (2 * first.X - diff.Y) / 2,
                Y = (2 * first.Y + diff.X) / 2
            });

            return result;
        }
    }
}
