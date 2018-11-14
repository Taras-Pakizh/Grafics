using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Threading;

namespace Lab5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Point> _points = new List<Point>();
        private double _step = 0.005;
        private DispatcherTimer timer = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void _DrawElipses(List<Point> points, int radius, Brush brush)
        {
            foreach (var point in points)
            {
                var elipse = new Ellipse()
                {
                    Fill = brush,
                    Height = radius,
                    Width = radius
                };
                elipse.Margin = new Thickness(point.X - (elipse.Width / 2), point.Y - (elipse.Height / 2), 0, 0);
                MyCanvas.Children.Add(elipse);
            }
        }

        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var location = e.GetPosition(MyCanvas);
            _points.Add(location);
            if (_points.Count == 1)
                _DrawElipses(_points, 8, Brushes.Orange);

            if(_points.Count > 1 && ComboBox_Algorithm.SelectedItem != null)
            {
                _DrawBezie(_points);
            }
        }

        private int iterations;
        private int currentIteration;

        private void _DrawBezieStepByStep(object sender, EventArgs e)
        {
            if(currentIteration > iterations)
            {
                timer.Stop();
                return;
            }
            List<Point> StepPoints = new List<Point>();
            StepPoints.AddRange(_points.Take(currentIteration));
            _DrawBezie(StepPoints);
            ++currentIteration;
        }

        private void _DrawBezie(List<Point> points)
        {
            MyCanvas.Children.Clear();
            _DrawElipses(points, 8, Brushes.Orange);
            var item = (ComboBoxItem)ComboBox_Algorithm.SelectedItem;
            List<Point> bezie = new List<Point>();
            if (item != null && (string)item.Content == "Traditional")
                bezie = Bezie.Traditional(points, _step);
            else if ( item != null && (string)item.Content == "Recurtional")
                bezie = Bezie.Recurtion(points, _step);
            else bezie = Bezie.Traditional(points, _step);

            var path = PathCreator.GetPath();
            path.Data = Geometry.Parse(PathCreator.GetCurveInfo(bezie, points));
            MyCanvas.Children.Add(path);
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            _points.Clear();
            MyCanvas.Children.Clear();
        }

        private void Button_SetPoints_Click(object sender, RoutedEventArgs e)
        {
            Dialog_PointSet dialog = new Dialog_PointSet();
            dialog.Return += (result, checker) =>
            {
                _points = ConvertPointsToCanvas(result);
                dialog.Close();
                if (checker)
                {
                    timer = new DispatcherTimer();
                    timer.Tick += _DrawBezieStepByStep;
                    timer.Interval = new TimeSpan(10000000);
                    iterations = _points.Count;
                    currentIteration = 1;
                    timer.Start();
                }
                else
                    _DrawBezie(_points);
            };
            dialog.Show();
        }

        private List<Point> ConvertPointsToCanvas(List<Point> points)
        {
            double width = MyCanvas.ActualWidth / 2;
            double height = MyCanvas.ActualHeight / 2;

            if(points.Max(x=>x.X) > width)
            {
                double value = width / (points.Max(x => x.X) + 20);
                ModifyList(points, value);
            }
            if(points.Max(x=>x.Y) > height)
            {
                double value = height / (points.Max(x => x.Y) + 20);
                ModifyList(points, value);
            }

            ModifyList(points, new Point(width, height));

            return points;
        }

        private void ModifyList(List<Point> points, double value)
        {
            for (int i = 0; i < points.Count; ++i)
                points[i] = new Point()
                {
                    X = points[i].X * value,
                    Y = points[i].Y * value
                };
        }

        private void ModifyList(List<Point> points, Point center)
        {
            for (int i = 0; i < points.Count; ++i)
                points[i] = new Point()
                {
                    X = center.X + points[i].X,
                    Y = center.Y - points[i].Y
                };
        }

        private void Button_Tips_Click(object sender, RoutedEventArgs e)
        {
            TipWindow tip = new TipWindow();
            tip.Return += (points, checker) =>
            {
                _points = ConvertPointsToCanvas(points);
                _DrawBezie(_points);
                tip.Close();
            };
            tip.Show();
        }
    }
}
