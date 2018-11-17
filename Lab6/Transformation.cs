using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab6
{
    static class Transformation
    {
        public static List<Point> MoveYX(List<Point> points, double step)
        {
            return points.Select(x => new Point()
            {
                X = x.X + step,
                Y = x.Y - step
            }).ToList();
        }

        public static List<Point> Scale(List<Point> points, Point FigureCenter, double step)
        {
            return points.Select(x => new Point()
            {
                X = ((x.X - FigureCenter.X) * step) + FigureCenter.X,
                Y = ((x.Y - FigureCenter.Y) * step) + FigureCenter.Y
            }).ToList();
        }

        public static List<Point> ReflectYX(List<Point> points, CanvasInfo info)
        {
            var newPoints = new List<Point>(points.Count);
            for(int i = 0; i < points.Count; ++i)
            {
                var point = _ConvertToAxis(points[i], info);
                point = new Point(point.Y, point.X);
                newPoints.Add(_ConvertToCanvas(point, info));
            }
            return newPoints;
        }

        private static Point _ConvertToAxis(Point point, CanvasInfo info)
        {
            return new Point()
            {
                X = point.X - info.WidthCenter,
                Y = info.HeigthCenter - point.Y
            };
        }

        private static Point _ConvertToCanvas(Point point, CanvasInfo info)
        {
            return new Point()
            {
                X = point.X + info.WidthCenter,
                Y = info.HeigthCenter - point.Y
            };
        }
    }
}
