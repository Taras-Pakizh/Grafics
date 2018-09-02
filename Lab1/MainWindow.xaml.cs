using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Events
        private void MyCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw(e.NewSize.Height, e.NewSize.Width);
        }
        private void MyCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Height++;
        }
        #endregion

        //Vars
        private double HeightToDraw;
        private double WidthToDraw;
        private int HorizontalDashes = 10;
        private int VerticalDashes = 20;
        private List<Path> rectancles = new List<Path>();

        //Path
        private Path GetSolid()
        {
            Path lines = new Path();
            lines.Stroke = Brushes.Aquamarine;
            lines.StrokeThickness = 2;
            return lines;
        }
        private Path GetDashed()
        {
            Path dashes = new Path();
            dashes.Stroke = Brushes.Aquamarine;
            dashes.StrokeDashCap = PenLineCap.Round;
            dashes.StrokeDashArray = new DoubleCollection(new double[] { 20, 10 });
            return dashes;
        }

        //Background
        private void Draw(double _Height, double _Width)
        {
            HeightToDraw = _Height;
            WidthToDraw = _Width;

            Path lines = GetSolid();
            Path dashes = GetDashed();
            PathGeometry pathGeoLines = new PathGeometry();
            PathGeometry pathGeoDashes = new PathGeometry();

            pathGeoLines.Figures.Add(GetHorixontalCoorLine());
            pathGeoLines.Figures.Add(GetVerticalCoorLine());
            pathGeoLines.Figures.Add(GetXArrow());
            pathGeoLines.Figures.Add(GetYArrow());

            var horizontalLines = GetHorizontalLines();
            var verticalLines = GetVerticalLines();
            foreach (var figure in horizontalLines)
                pathGeoDashes.Figures.Add(figure);
            foreach (var figure in verticalLines)
                pathGeoDashes.Figures.Add(figure);

            lines.Data = pathGeoLines;
            dashes.Data = pathGeoDashes;

            MyCanvas.Children.Clear();
            MyCanvas.Children.Add(lines);
            MyCanvas.Children.Add(dashes);
            var labels = GetLabels();
            foreach (var label in labels)
                MyCanvas.Children.Add(label);
            DrawRectancle(new Point(50, 50), new Point(100, 100));
            foreach (var item in rectancles)
                MyCanvas.Children.Add(item);
        }
        private PathFigure GetHorixontalCoorLine()
        {
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(WidthToDraw / 2, 0);
            LineSegment line = new LineSegment();
            line.Point = new Point(WidthToDraw / 2, HeightToDraw);
            figure.Segments.Add(line);
            return figure;
        }
        private PathFigure GetVerticalCoorLine()
        {
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(0, HeightToDraw / 2);
            LineSegment line = new LineSegment();
            line.Point = new Point(WidthToDraw, HeightToDraw / 2);
            figure.Segments.Add(line);
            return figure;
        }
        private List<PathFigure> GetHorizontalLines()
        {
            if (HorizontalDashes % 2 != 0)
                throw new Exception("Wrong");
            List<PathFigure> figures = new List<PathFigure>();
            double step = (HeightToDraw / 2) / ((HorizontalDashes / 2) + 1);
            for(int i = 1; i < HorizontalDashes + 2; ++i)
            {
                if (i == (HorizontalDashes / 2) + 1) continue;
                PathFigure figure = new PathFigure();
                figure.StartPoint = new Point(0, i * step);
                LineSegment line = new LineSegment();
                line.Point = new Point(WidthToDraw, i * step);
                figure.Segments.Add(line);
                figures.Add(figure);
            }
            return figures;
        }
        private List<PathFigure> GetVerticalLines()
        {
            if (VerticalDashes % 2 != 0)
                throw new Exception("Wrong");
            List<PathFigure> figures = new List<PathFigure>();
            double step = (WidthToDraw / 2) / ((VerticalDashes / 2) + 1);
            for (int i = 1; i < VerticalDashes + 2; ++i)
            {
                if (i == (VerticalDashes / 2) + 1) continue;
                PathFigure figure = new PathFigure();
                figure.StartPoint = new Point(i * step, 0);
                LineSegment line = new LineSegment();
                line.Point = new Point(i * step, HeightToDraw);
                figure.Segments.Add(line);
                figures.Add(figure);
            }
            return figures;
        }
        private PathFigure GetXArrow()
        {
            PathFigure figure = new PathFigure();
            double arrowHeight = HeightToDraw / 75;
            double arrowWidth = WidthToDraw / 60;
            figure.StartPoint = new Point(WidthToDraw - arrowWidth, HeightToDraw / 2 - arrowHeight);
            Point[] points = new Point[]
            {
                new Point(WidthToDraw, HeightToDraw / 2),
                new Point(WidthToDraw - arrowWidth, HeightToDraw / 2 + arrowHeight)
            };
            PolyLineSegment segment = new PolyLineSegment(points, true);
            figure.Segments.Add(segment);
            return figure;
        }
        private PathFigure GetYArrow()
        {
            PathFigure figure = new PathFigure();
            double arrowHeight = WidthToDraw / 60;
            double arrowWidth = HeightToDraw / 75;
            figure.StartPoint = new Point(WidthToDraw / 2 - arrowWidth, arrowHeight);
            Point[] points = new Point[]
            {
                new Point(WidthToDraw / 2, 0),
                new Point(WidthToDraw / 2 + arrowWidth, arrowHeight)
            };
            PolyLineSegment segment = new PolyLineSegment(points, true);
            figure.Segments.Add(segment);
            return figure;
        }
        private List<Label> GetLabels()
        {
            Label labelX = new Label();
            Label labelY = new Label();

            labelX.Content = "X";
            labelY.Content = "Y";

            labelX.Margin = new Thickness(WidthToDraw - 25, HeightToDraw / 2 - 25, 0, 0);
            labelY.Margin = new Thickness(WidthToDraw / 2 - 25, 25, 0, 0);

            return new List<Label>()
            {
                labelX,
                labelY
            };
        }

        //Logic
        private void DrawRectancle(Point leftUp, Point rightUp)
        {
            double Xdelta = Math.Abs(leftUp.X - rightUp.X);
            double Ydelta = Math.Abs(leftUp.Y = rightUp.Y);
            Point[] rectanclePoints = new Point[]
            {
                rightUp,
                new Point(rightUp.X - Xdelta, rightUp.Y + Ydelta),
                new Point(leftUp.X - Xdelta, rightUp.Y + Ydelta)
            };
            PathFigure figure = new PathFigure();
            figure.StartPoint = leftUp;
            figure.Segments.Add(new PolyLineSegment(rectanclePoints, true));
            figure.IsClosed = true;

            //-------Shit
            var path = GetSolid();
            var geo = new PathGeometry();
            geo.Figures.Add(figure);
            path.Data = geo;
            rectancles.Add(path);
            //Draw(HeightToDraw, WidthToDraw);
        }
    }
}
