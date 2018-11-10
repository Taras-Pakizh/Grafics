using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace Lab5
{
    public static class PathCreator
    {
        public static Path GetPolynom(List<Point> points)
        {
            var path = GetPath();
            path.Data = _GetPathGeometry(points);
            return path;
        }

        public static Path GetPath()
        {
            Path path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            return path;
        }

        public static string GetCurveInfo(List<Point> points, List<Point> polynom)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"M {polynom.First().X.ToString(CultureInfo.GetCultureInfo("en-US"))}, {polynom.First().Y.ToString(CultureInfo.GetCultureInfo("en-US"))} L ");
            foreach(var point in points)
            {
                builder.Append($"{point.X.ToString(CultureInfo.GetCultureInfo("en-US"))}, {point.Y.ToString(CultureInfo.GetCultureInfo("en-US"))} ");
            }
            builder.Append($"{polynom.Last().X.ToString(CultureInfo.GetCultureInfo("en-US"))}, {polynom.Last().Y.ToString(CultureInfo.GetCultureInfo("en-US"))}");
            return builder.ToString();
        }

        private static PathGeometry _GetPathGeometry(List<Point> points)
        {
            PathGeometry geometry = new PathGeometry();
            PathFigure polynom = new PathFigure()
            {
                StartPoint = points.First()
            };
            var segment = new PolyLineSegment(points.Skip(1), true);
            polynom.Segments.Add(segment);
            geometry.Figures.Add(polynom);
            return geometry;
        }
    }
}
