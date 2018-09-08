using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Lab1
{
    class AxisCreator : IFiguresCreator
    {
        public PathGeometry GetFigures(CanvasInfo info)
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
