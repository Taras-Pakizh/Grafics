using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3
{
    class CanvasInfo
    {
        public double Width { get; private set; }
        public double Heigth { get; private set; }

        public Point FractalCenter { get; private set; }

        public CanvasInfo(double width, double heigth)
        {
            Width = width;
            Heigth = heigth;
        }

        public CanvasInfo(double width, double heigth, Point center)
        {
            Width = width;
            Heigth = heigth;
            FractalCenter = center;
        }

        public CanvasInfo CopyWithPoints(Point center)
        {
            return new CanvasInfo(Width, Heigth, center);
        }
    }
}
