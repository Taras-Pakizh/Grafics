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

            return geometry;
        }
    }
}
