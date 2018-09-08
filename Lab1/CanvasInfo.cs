using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab1
{
    class CanvasInfo
    {
        public double Width { get; private set; }
        public double Heigth { get; private set; }
        public double Step { get; private set; }

        public double WidthCenter
        {
            get { return Width / 2; }
        }
        public double HeigthCenter
        {
            get { return Heigth / 2; }
        }

        public Point leftUp { get; private set; }
        public Point rigthUp { get; private set; }

        public CanvasInfo(double width, double heigth, double step)
        {
            Width = width;
            Heigth = heigth;
            Step = step;
        }

        public CanvasInfo(double width, double heigth, double step, Point left, Point rigth)
        {
            Width = width;
            Heigth = heigth;
            Step = step;
            leftUp = left;
            rigthUp = rigth;
        }

        public CanvasInfo CopyWithPoints(Point left, Point right)
        {
            return new CanvasInfo(Width, Heigth, Step, left, right);
        }
    }
}
