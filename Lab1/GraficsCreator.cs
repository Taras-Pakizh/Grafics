using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1
{
    class GraficsCreator
    {
        private CanvasInfo info;

        public GraficsCreator(CanvasInfo _info)
        {
            info = _info;
        }

        public IEnumerable<Path> DrawBackground()
        {
            IPathCreator creator = new SolidPathCreator();
            Path axis = creator.GetPath(Brushes.Aquamarine);
            Path arrows = creator.GetPath(Brushes.Aquamarine);
            creator = new DashedPathCreator();
            Path dashes = creator.GetPath(Brushes.Aquamarine);

            IFiguresCreator draw = new AxisCreator();
            axis.Data = draw.GetFigures(info);
            draw = new DashesCreator();
            dashes.Data = draw.GetFigures(info);
            draw = new ArrowsCreator();
            arrows.Data = draw.GetFigures(info);
            return new Path[]
            {
                axis,
                arrows,
                dashes
            };
        }

        public IEnumerable<Path> DrawRectancles(IEnumerable<RectancleInfo> rectancles)
        {
            RectancleCreator figureCreator = new RectancleCreator();
            IPathCreator pathCreator = new SolidPathCreator();
            List<Path> paths = new List<Path>();
            foreach(var item in rectancles)
            {
                Path path = pathCreator.GetPath(item.brush);
                path.Data = figureCreator.GetFigures(info.CopyWithPoints(
                    PointConverter.ToCanvas(item.leftUp, info), PointConverter.ToCanvas(item.rightUp, info)));
                paths.Add(path);
                Path newpath = pathCreator.GetPath(figureCreator.newRectancle.brush);
                newpath.Data = figureCreator.GetFigures(info.CopyWithPoints(
                    figureCreator.newRectancle.leftUp, figureCreator.newRectancle.rightUp));
                paths.Add(newpath);
                figureCreator.newRectancle = null;
            }
            return paths;
        }

        public IEnumerable<Label> DrawLabels()
        {
            LabelsCreator labelsCreator = new LabelsCreator();
            return labelsCreator.GetLabels(info);
        }
    }
}
