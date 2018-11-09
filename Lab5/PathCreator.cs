using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace Lab5
{
    public static class PathCreator
    {
        public static Path GetPolynom(List<Point> points)
        {
            var path = _GetPath();
            path.Data = _GetPathGeometry(points);
            return path;
        }

        private static Path _GetPath()
        {
            Path path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;
            return path;
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
