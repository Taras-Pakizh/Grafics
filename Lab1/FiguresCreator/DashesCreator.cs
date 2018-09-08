using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace Lab1
{
    class DashesCreator : IFiguresCreator
    {
        public PathGeometry GetFigures(CanvasInfo info)
        {
            PathGeometry geometry = new PathGeometry();
            int dashesXCount = (int)(info.HeigthCenter / info.Step);
            int dashesYCount = (int)(info.WidthCenter / info.Step);
            for(int i = 1; i < dashesXCount + 1; ++i)
            {
                PathFigure up = new PathFigure()
                {
                    StartPoint = new Point(0, info.HeigthCenter - (info.Step * i))
                };
                PathFigure down = new PathFigure()
                {
                    StartPoint = new Point(0, info.HeigthCenter + (info.Step * i))
                };
                up.Segments.Add(new LineSegment(new Point(info.Width, up.StartPoint.Y), true));
                down.Segments.Add(new LineSegment(new Point(info.Width, down.StartPoint.Y), true));
                geometry.Figures.Add(up);
                geometry.Figures.Add(down);
            }
            for(int i = 1; i < dashesYCount + 1; ++i)
            {
                PathFigure left = new PathFigure()
                {
                    StartPoint = new Point(info.WidthCenter - (info.Step * i), 0)
                };
                PathFigure rigth = new PathFigure()
                {
                    StartPoint = new Point(info.WidthCenter + (info.Step * i), 0)
                };
                left.Segments.Add(new LineSegment(new Point(left.StartPoint.X, info.Heigth), true));
                rigth.Segments.Add(new LineSegment(new Point(rigth.StartPoint.X, info.Heigth), true));
                geometry.Figures.Add(left);
                geometry.Figures.Add(rigth);
            }
            return geometry;
        }
    }
}
