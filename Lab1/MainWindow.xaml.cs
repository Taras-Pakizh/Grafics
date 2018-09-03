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

using ColorPickerWPF;
using ColorPickerWPF.Code;

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
            TransformPointChangeSize(e);
            Draw(e.NewSize.Height, e.NewSize.Width);
        }
        private void MyCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Height++;
        }
        //Clear
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rectancles.Clear();
            rectaclesPoints.Clear();
            Draw(HeightToDraw, WidthToDraw);
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (temp == new Point(-1, -1))
            {
                temp = e.GetPosition(MyCanvas);
            }
            else
            {
                Point point = e.GetPosition(MyCanvas);
                if (temp.X <= point.X)
                    DrawRectancles(temp, point, true);
                else DrawRectancles(point, temp, true);
                temp = new Point(-1, -1);
                Draw(HeightToDraw, WidthToDraw);
            }
        }
        //Color
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(MyPicker.Visibility == Visibility.Visible)
                MyPicker.Visibility = Visibility.Hidden;
            else MyPicker.Visibility = Visibility.Visible;
        }
        private void MyPicker_OnPickColor(Color _color)
        {
            color = _color;
            MyPicker.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Variables
        private double HeightToDraw;
        private double WidthToDraw;
        private int HorizontalDashes = 10;
        private int VerticalDashes = 20;
        private List<Path> rectancles = new List<Path>();
        private List<Point[]> rectaclesPoints = new List<Point[]>();
        private Point temp = new Point(-1, -1);
        private Color color;
        Random random = new Random();
        #endregion

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
            foreach (var item in rectancles)
                MyCanvas.Children.Add(item);
        }

        #region Figures
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

            labelX.BorderBrush = Brushes.White;
            labelY.BorderBrush = Brushes.White;

            labelX.Margin = new Thickness(WidthToDraw - 25, HeightToDraw / 2 - 25, 0, 0);
            labelY.Margin = new Thickness(WidthToDraw / 2 - 25, 25, 0, 0);

            return new List<Label>()
            {
                labelX,
                labelY
            };
        }
        #endregion

        //Logic
        private void DrawRectancles(Point leftUp, Point rightUp, bool New)
        {
            if(New == true)
                rectaclesPoints.Add(new Point[] { leftUp, rightUp });

            double Xdelta = Math.Abs(leftUp.X - rightUp.X);
            double Ydelta = Math.Abs(leftUp.Y - rightUp.Y);
            Point[] rectanclePoints = null;
            if(leftUp.Y >= rightUp.Y)
            {
                rectanclePoints = new Point[]
                {
                    rightUp,
                    new Point(rightUp.X + Ydelta, rightUp.Y + Xdelta),
                    new Point(leftUp.X + Ydelta, leftUp.Y + Xdelta),
                };
            }
            else
            {
                rectanclePoints = new Point[]
                {
                    rightUp,
                    new Point(rightUp.X - Ydelta, rightUp.Y + Xdelta),
                    new Point(leftUp.X - Ydelta, leftUp.Y + Xdelta),
                };
            }
            Point[] insideRectanclePoints = new Point[]
            {
                 new Point((rightUp.X + rectanclePoints[1].X) / 2, (rightUp.Y + rectanclePoints[1].Y) / 2),
                 new Point((rectanclePoints[1].X + rectanclePoints[2].X) / 2, (rectanclePoints[1].Y + rectanclePoints[2].Y) / 2),
                 new Point((rectanclePoints[2].X + leftUp.X) / 2, (rectanclePoints[2].Y + leftUp.Y) / 2),
            };

            PathFigure figure = new PathFigure();
            figure.StartPoint = leftUp;
            figure.Segments.Add(new PolyLineSegment(rectanclePoints, true));
            figure.IsClosed = true;

            PathFigure figure1 = new PathFigure();
            figure1.StartPoint = new Point() { X = (leftUp.X + rightUp.X) / 2, Y = (leftUp.Y + rightUp.Y) / 2};
            figure1.Segments.Add(new PolyLineSegment(insideRectanclePoints, true));
            figure1.IsClosed = true;

            var path1 = GetSolid();
            path1.Fill = new SolidColorBrush(color);

            var path2 = GetSolid();
            byte[] colors = new byte[4];
            random.NextBytes(colors);
            Color randomColor = new Color()
            {
                A = colors[0],
                B = colors[1],
                R = colors[2],
                G = colors[3]
            };
            path2.Fill = new SolidColorBrush(randomColor);

            var geo1 = new PathGeometry();
            var geo2 = new PathGeometry();
            geo1.Figures.Add(figure);
            geo2.Figures.Add(figure1);
            path1.Data = geo1;
            path2.Data = geo2;
            rectancles.Add(path1);
            rectancles.Add(path2);
        }
        private void TransformPointChangeSize(SizeChangedEventArgs args)
        {
            for(int i = 0; i < rectaclesPoints.Count; ++i)
            {
                for(int index = 0; index < rectaclesPoints[i].Length; ++index)
                {
                    rectaclesPoints[i][index] = new Point()
                    {
                        X = rectaclesPoints[i][index].X * (args.NewSize.Width / args.PreviousSize.Width),
                        Y = rectaclesPoints[i][index].Y * (args.NewSize.Height / args.PreviousSize.Height)
                    };
                }
            }
            TransformRectancle();
        }
        private void TransformRectancle()
        {
            rectancles.Clear();
            foreach(var item in rectaclesPoints)
            {
                DrawRectancles(item[0], item[1], false);
            }
        }
    }
}
