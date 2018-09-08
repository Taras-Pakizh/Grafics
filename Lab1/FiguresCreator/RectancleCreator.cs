using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Lab1
{
    class RectancleCreator : IFiguresCreator
    {
        public RectancleInfo newRectancle { get; set; }

        public PathGeometry GetFigures(CanvasInfo info)
        {
            PathGeometry geometry = new PathGeometry();
            PathFigure rectancle = new PathFigure()
            {
                StartPoint = info.leftUp
            };
            double Xdelta = Math.Abs(info.leftUp.X - info.rigthUp.X);
            double Ydelta = Math.Abs(info.leftUp.Y - info.rigthUp.Y);
            Point[] rectanclePoints = null;
            if (info.leftUp.Y >= info.rigthUp.Y)
            {
                rectanclePoints = new Point[]
                {
                    info.rigthUp,
                    new Point(info.rigthUp.X + Ydelta, info.rigthUp.Y + Xdelta),
                    new Point(info.leftUp.X + Ydelta, info.leftUp.Y + Xdelta),
                };
            }
            else
            {
                rectanclePoints = new Point[]
                {
                    info.rigthUp,
                    new Point(info.rigthUp.X - Ydelta, info.rigthUp.Y + Xdelta),
                    new Point(info.leftUp.X - Ydelta, info.leftUp.Y + Xdelta),
                };
            }
            rectancle.Segments.Add(new PolyLineSegment(rectanclePoints, true));
            rectancle.IsClosed = true;
            geometry.Figures.Add(rectancle);

            List<Point> newRect = new List<Point>();
            newRect.Add(info.leftUp);
            newRect.AddRange(rectanclePoints);
            if(newRectancle == null)
                SetNewRectancle(newRect);

            return geometry;
        }

        private void SetNewRectancle(IList<Point> points)
        {
            Random random = new Random();
            byte[] colors = new byte[4];
            random.NextBytes(colors);
            Color randomColor = new Color()
            {
                A = colors[0],
                B = colors[1],
                R = colors[2],
                G = colors[3]
            };
            Point left, right;
            if(points[0].Y <= points[1].Y)
            {
                left = new Point()
                {
                    X = (points[3].X + points[0].X) / 2,
                    Y = (points[3].Y + points[0].Y) / 2
                };
                right = new Point()
                {
                    X = (points[0].X + points[1].X) / 2,
                    Y = (points[0].Y + points[1].Y) / 2
                };
            }
            else
            {
                left = new Point()
                {
                    X = (points[0].X + points[1].X) / 2,
                    Y = (points[0].Y + points[1].Y) / 2
                };
                right = new Point()
                {
                    X = (points[1].X + points[2].X) / 2,
                    Y = (points[1].Y + points[2].Y) / 2
                };
            }
            newRectancle = new RectancleInfo()
            {
                leftUp = left,
                rightUp = right,
                brush = new SolidColorBrush(randomColor)
            };
        }
    }
}
