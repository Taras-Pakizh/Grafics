using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab1
{
    static class PointConverter
    {
        public static Point ToCanvas(Point point, CanvasInfo info)
        {
            return new Point()
            {
                X = info.WidthCenter + (info.Step * point.X),
                Y = info.HeigthCenter + (info.Step * point.Y)
            };
        }

        public static Point ToCoor(Point point, CanvasInfo info)
        {
            return new Point()
            {
                X = (point.X - info.WidthCenter) / info.Step,
                Y = (info.HeigthCenter - point.Y) / info.Step
            };
        }
    }
}
