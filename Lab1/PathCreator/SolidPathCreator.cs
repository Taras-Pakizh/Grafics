using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1
{
    class SolidPathCreator : IPathCreator
    {
        public Path GetPath(Brush brush)
        {
            Path path = new Path();
            path.Stroke = Brushes.Aquamarine;
            path.Fill = brush;
            path.StrokeThickness = 2;
            return path;
        }
    }
}
