using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Lab3
{
    class Peano : Fractal
    {
        //Vars
        private readonly Point center = new Point(0, 0);
        private readonly double defaulfSize = 1;

        //Constructor
        public Peano()
        {
            Stage = 1;
        }
        public Peano(CanvasInfo canvasInfo)
        {
            info = canvasInfo;
            Stage = 1;
        }
        public Peano(CanvasInfo canvasInfo, int stage)
        {
            info = canvasInfo;
            if (stage < 1)
                throw new Exception("Stage is less than 1");
            Stage = stage;
        }

        public override void CreateNextStages(int amoung)
        {
            if (amoung < 1) throw new IndexOutOfRangeException();
            var currentFractal = TopFractal();
            if (currentFractal.nextStage != null)
                currentFractal = currentFractal.nextStage;
            for (int i = 0; i < amoung; ++i)
            {
                currentFractal.geometry = CreateNextFractal(TopFractal());
                currentFractal.nextStage = new Peano(info, currentFractal.Stage + 1);
                currentFractal = currentFractal.nextStage;
            }
        }

        //Fractal
        public override PathGeometry CreateNextFractal(Fractal prevFractal)
        {
            if (prevFractal.Stage == 1 && prevFractal.geometry == null)
            {
                return CreateDeafaulFractal();
            }
            List<PathGeometry> newGeomeries = new List<PathGeometry>(4);
            for (int i = 0; i < newGeomeries.Capacity; ++i)
                newGeomeries.Add(prevFractal.geometry);
            newGeomeries[0] = RoundFractal(newGeomeries.First(), Sides.Right);
            newGeomeries[3] = RoundFractal(newGeomeries.Last(), Sides.Left);
            for (int i = 0; i < newGeomeries.Count; ++i)
                newGeomeries[i] = NextPositionFractal(newGeomeries[i], (Directions)i, prevFractal);
            return UnionGeometries(newGeomeries);
        }

        //Union geometry
        private PathGeometry UnionGeometries(List<PathGeometry> geometries)
        {
            List<List<Point>> newPoints = new List<List<Point>>(4);
            foreach(var item in geometries)
            {
                var Figure = item.Figures.First();
                var polyline = (PolyLineSegment)Figure.Segments.First();
                List<Point> subList = new List<Point>();
                subList.Add(Figure.StartPoint);
                subList.AddRange(polyline.Points.ToArray());
                newPoints.Add(subList);
            }
            newPoints.First().Reverse();
            newPoints.Last().Reverse();
            PathFigure figure = new PathFigure();
            figure.IsClosed = false;
            figure.StartPoint = newPoints.First().First();
            newPoints.First().RemoveAt(0);
            PolyLineSegment newPolyline = new PolyLineSegment();
            foreach (var list in newPoints)
                foreach (var point in list)
                    newPolyline.Points.Add(point);
            figure.Segments.Add(newPolyline);
            PathGeometry result = new PathGeometry();
            result.Figures.Add(figure);
            return result;
        }

        //Default fractal
        private PathGeometry CreateDeafaulFractal()
        {
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point()
            {
                X = center.X - defaulfSize,
                Y = center.Y - defaulfSize
            };
            PolyLineSegment polyLine = new PolyLineSegment();
            polyLine.Points.Add(new Point()
            {
                X = center.X - defaulfSize,
                Y = center.Y + defaulfSize
            });
            polyLine.Points.Add(new Point()
            {
                X = center.X + defaulfSize,
                Y = center.Y + defaulfSize
            });
            polyLine.Points.Add(new Point()
            {
                X = center.X + defaulfSize,
                Y = center.Y - defaulfSize
            });
            figure.Segments.Add(polyLine);
            figure.IsClosed = false;
            PathGeometry result = new PathGeometry();
            result.Figures.Add(figure);
            return result;
        }

        //Round
        private PathGeometry RoundFractal(PathGeometry pathGeometry, Sides side)
        {
            var figure = pathGeometry.Figures.First();
            var polyline = (PolyLineSegment)figure.Segments.First();
            var points = RoundPoints(polyline.Points.ToArray(), side);
            PathFigure newFigure = new PathFigure();
            newFigure.StartPoint = RoundPoints(new Point[] { figure.StartPoint }, side).First();
            newFigure.IsClosed = false;
            PolyLineSegment newPolyLine = new PolyLineSegment();
            foreach (var item in points)
                newPolyLine.Points.Add(item);
            newFigure.Segments.Add(newPolyLine);
            PathGeometry result = new PathGeometry();
            result.Figures.Add(newFigure);
            return result;
        }
        private Point[] RoundPoints(Point[] points, Sides side)
        {
            points = points.Select(point => new Point(point.Y, point.X)).ToArray();
            for (int i = 0; i < points.Count(); ++i)
            {
                if (side == Sides.Left)
                    points[i].X = -points[i].X;
                else points[i].Y = -points[i].Y;
            }
            return points;
        }
        private enum Sides
        {
            Left,
            Right
        }

        //New Positions
        private PathGeometry NextPositionFractal(PathGeometry pathGeometry, Directions direction, Fractal prevFractal)
        {
            var figure = pathGeometry.Figures.First();
            var changeX = Math.Pow(2, prevFractal.Stage);
            var changeY = changeX;
            var polyline = (PolyLineSegment)figure.Segments.First();

            if (direction == Directions.LeftDown || direction == Directions.RightDown)
                changeY = -changeY;
            if (direction == Directions.LeftDown || direction == Directions.LeftUp)
                changeX = -changeX;

            PolyLineSegment newPolyline = new PolyLineSegment();
            var points = polyline.Points.Select(point => new Point(point.X + changeX, point.Y + changeY)).ToArray();
            foreach (var item in points)
                newPolyline.Points.Add(item);
            PathFigure newFigure = new PathFigure();
            newFigure.IsClosed = false;
            newFigure.Segments.Add(newPolyline);
            newFigure.StartPoint = new Point(figure.StartPoint.X + changeX, figure.StartPoint.Y + changeY);
            PathGeometry result = new PathGeometry();
            result.Figures.Add(newFigure);
            return result;
        }
        private enum Directions
        {
            LeftDown = 0,
            LeftUp,
            RightUp,
            RightDown
        }

        //Scale Fractal
        public override PathGeometry ScaleToCanvas()
        {
            var figure = geometry.Figures.First();
            var polyline = (PolyLineSegment)figure.Segments.First();
            var DefaultSize = Math.Abs(figure.StartPoint.X);

            List<double> limits = new List<double>(4);
            limits.Add(info.FractalCenter.X);
            limits.Add(info.FractalCenter.Y);
            limits.Add(info.Width - info.FractalCenter.X);
            limits.Add(info.Heigth - info.FractalCenter.Y);

            double thickness = 25;
            var minLimit = limits.Min() - thickness;

            double coof = minLimit / DefaultSize;
            
            PathFigure newFigure = new PathFigure();
            newFigure.StartPoint = ConvertToCanvas(figure.StartPoint, coof);
            PolyLineSegment newPolyline = new PolyLineSegment();
            for(int i = 0; i < polyline.Points.Count; ++i)
            {
                newPolyline.Points.Add(ConvertToCanvas(polyline.Points[i], coof));
            }
            newFigure.Segments.Add(newPolyline);
            newFigure.IsClosed = false;
            PathGeometry result = new PathGeometry();
            result.Figures.Add(newFigure);
            return result;
        }
        private Point ConvertToCanvas(Point point, double coof)
        {
            return new Point()
            {
                X = point.X * coof + info.FractalCenter.X,
                Y = (-point.Y) * coof + info.FractalCenter.Y
            };
        }
    }
}
