using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Lab1
{
    class ArrowsCreator : IFiguresCreator
    {
        public PathGeometry GetFigures(CanvasInfo info)
        {
            PathGeometry geometry = new PathGeometry();
            double arrowThickness = info.Heigth / 75;
            double arrowLength = info.Width / 60;
            PathFigure arrowX = new PathFigure()
            {
                StartPoint = new Point(info.Width - arrowLength, info.HeigthCenter - arrowThickness)
            };
            PathFigure arrowY = new PathFigure()
            {
                StartPoint = new Point(info.WidthCenter - arrowThickness, arrowLength)
            };
            Point[] arrowXPoints = new Point[]
            {
                new Point(info.Width, info.HeigthCenter),
                new Point(info.Width - arrowLength, info.HeigthCenter + arrowThickness)
            };
            Point[] arrowYPoints = new Point[]
            {
                new Point(info.WidthCenter, 0),
                new Point(info.WidthCenter + arrowThickness, arrowLength)
            };
            arrowX.Segments.Add(new PolyLineSegment(arrowXPoints, true));
            arrowY.Segments.Add(new PolyLineSegment(arrowYPoints, true));
            geometry.Figures.Add(arrowX);
            geometry.Figures.Add(arrowY);

            return geometry;
        }
    }
}
