using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1
{
    class DashedPathCreator : IPathCreator
    {
        public Path GetPath(Brush brush)
        {
            Path path = new Path();
            path.Stroke = brush;
            path.StrokeDashCap = PenLineCap.Round;
            path.StrokeDashArray = new DoubleCollection(new double[] { 20, 10 });
            return path;
        }
    }
}
