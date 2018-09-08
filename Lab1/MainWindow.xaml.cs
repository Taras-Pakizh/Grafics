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
            VerticalDashes = 22;
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
            ColorPickWindow window = new ColorPickWindow();
            window.Pick += Window_Pick;
            window.Show();
        }
        private void Window_Pick(Color _color)
        {
            color = _color;
        }
        //Slider
        private void SliderX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelX.Content = "Верхня ліва X: " + ((Slider)sender).Value;
        }
        private void SliderY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelY.Content = "Верхня ліва Y: " + ((Slider)sender).Value;
        }
        private void SliderX2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelX2.Content = "Верхня права X: " + ((Slider)sender).Value;
        }
        private void SliderY2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelY2.Content = "Верхня права Y: " + ((Slider)sender).Value;
        }
        //Add
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TransformCoorPoints(SliderX.Value, SliderY.Value, SliderX2.Value, SliderY2.Value);
        }
        #endregion

        #region Variables
        private Point temp = new Point(-1, -1);
        private Color color;
        Random random = new Random();
        #endregion

        
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

            double step = WidthToDraw / VerticalDashes;
            HorizontalDashes = (int)Math.Round(HeightToDraw / step);

            figure.StartPoint = new Point(0, (HorizontalDashes / 2) * step);
            LineSegment line = new LineSegment();
            line.Point = new Point(WidthToDraw, (HorizontalDashes / 2) * step);
            figure.Segments.Add(line);
            return figure;
        }
        private List<PathFigure> GetHorizontalLines()
        {
            //if (HorizontalDashes % 2 != 0)
            //    throw new Exception("Wrong");
            List<PathFigure> figures = new List<PathFigure>();
            double step = WidthToDraw / VerticalDashes;
            HorizontalDashes = (int)Math.Round(HeightToDraw / step);
            for (int i = 1; i < HorizontalDashes; ++i)
            {
                if (i == (HorizontalDashes / 2)) continue;
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
            double step = WidthToDraw / VerticalDashes;
            for (int i = 1; i < VerticalDashes; ++i)
            {
                if (i == (VerticalDashes / 2)) continue;
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
        private void TransformCoorPoints(double x1, double y1, double x2, double y2)
        {
            if (x1 == x2 && y1 == y2) return;

            if (VerticalDashes / 2 < Math.Abs(x1))
                VerticalDashes = 2 * (int)Math.Abs(x1);
            if (VerticalDashes / 2 < Math.Abs(x2))
                VerticalDashes = 2 * (int)Math.Abs(x2);
            if (VerticalDashes / 4 <= Math.Abs(y1))
                VerticalDashes = (int)(Math.Abs(y1) * 4) + 1;
            if (VerticalDashes / 4 <= Math.Abs(y2))
                VerticalDashes = (int)(Math.Abs(y2) * 4) + 1;
            if (VerticalDashes % 2 != 0)
                ++VerticalDashes;
            double step = WidthToDraw / VerticalDashes;
            HorizontalDashes = (int)Math.Round(HeightToDraw / step);

            x1 += VerticalDashes / 2;
            y1 = (-y1) + (HorizontalDashes / 2);
            x2 += VerticalDashes / 2;
            y2 = (-y2) + (HorizontalDashes / 2);
            double delta = step;
            Point pointLeft = new Point()
            {
                X = (x1 * delta),
                Y = (y1 * delta)
            };
            Point pointRight = new Point()
            {
                X = (x2 * delta),
                Y = (y2 * delta)
            };
            if (pointLeft.X <= pointRight.X)
                DrawRectancles(pointLeft, pointRight, true);
            else DrawRectancles(pointRight, pointLeft, true);
            Draw(HeightToDraw, WidthToDraw);
        }
    }
}
