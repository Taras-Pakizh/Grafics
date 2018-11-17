using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace Lab6
{
    static class PathCreator
    {
        #region Paths
        public static Path GetRectangle(List<Point> points)
        {
            var path = _GetDefaultPath();
            path.Data = _GetGeometryRectangle(points);
            return path;
        }
        public static Path GetAxis(CanvasInfo info)
        {
            var path = _GetDefaultPath();
            path.Data = _GetGeometryAxis(info);
            return path;
        }
        public static Path GetYX(CanvasInfo info)
        {
            var path = _GetDefaultPath();
            path.Data = _GetGeometryYX(info);
            return path;
        }
        #endregion

        private static Path _GetDefaultPath()
        {
            Path path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            return path;
        }

        private static PathGeometry _GetGeometryYX(CanvasInfo info)
        {
            PathGeometry geometry = new PathGeometry();
            Point startPoint = new Point();
            Point endPoint = new Point();
            if (info.Width > info.Heigth)
            {
                startPoint = new Point()
                {
                    X = info.WidthCenter - info.HeigthCenter,
                    Y = info.Heigth
                };
                endPoint = new Point()
                {
                    X = info.Width - startPoint.X,
                    Y = 0
                };
            }
            else
            {
                startPoint = new Point()
                {
                    X = 0,
                    Y = info.HeigthCenter + info.WidthCenter
                };
                endPoint = new Point()
                {
                    X = info.Width,
                    Y = info.HeigthCenter - (startPoint.Y - info.HeigthCenter)
                };
            }
            PathFigure YX = new PathFigure()
            {
                StartPoint = startPoint
            };
            YX.Segments.Add(new LineSegment(endPoint, true));
            geometry.Figures.Add(YX);
            return geometry;
        }

        private static PathGeometry _GetGeometryRectangle(List<Point> points)
        {
            PathGeometry geometry = new PathGeometry();
            PathFigure polynom = new PathFigure()
            {
                StartPoint = points.First()
            };
            polynom.IsClosed = true;
            var segment = new PolyLineSegment(points.Skip(1), true);
            polynom.Segments.Add(segment);
            geometry.Figures.Add(polynom);
            return geometry;
        }

        public static PathGeometry _GetGeometryAxis(CanvasInfo info)
        {
            PathGeometry geometry = new PathGeometry();
            PathFigure AxisX = new PathFigure()
            {
                StartPoint = new Point(0, info.HeigthCenter)
            };
            PathFigure AxisY = new PathFigure()
            {
                StartPoint = new Point(info.WidthCenter, 0)
            };
            AxisX.Segments.Add(new LineSegment(new Point(info.Width, info.HeigthCenter), true));
            AxisY.Segments.Add(new LineSegment(new Point(info.WidthCenter, info.Heigth), true));
            geometry.Figures.Add(AxisX);
            geometry.Figures.Add(AxisY);
            return geometry;
        }
    }
}
