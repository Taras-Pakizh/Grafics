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

namespace Lab5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Point> points = new List<Point>()
            {
                new Point(100, 200),
                new Point(200, 300),
                new Point(400, 300),
                new Point(300, 100)
            };
            double step = 0.001;

            _GetPolyline(points);
            _GetBezie(points, step);
        }

        private void _GetPolyline(List<Point> points)
        {
            MyCanvas.Children.Add(PathCreator.GetPolynom(points));
            _DrawPoints(points, 5);
        }

        private void _GetBezie(List<Point> points, double step)
        {
            //points = Bezie.Recurtion(points, step);
            points = Bezie.Traditional(points, step);
            _DrawPoints(points, 2);
        }

        private void _DrawPoints(List<Point> points, int radius)
        {
            foreach (var point in points)
            {
                var elipse = new Ellipse()
                {
                    Fill = Brushes.Black,
                    Height = radius,
                    Width = radius
                };
                elipse.Margin = new Thickness(point.X - (elipse.Width / 2), point.Y - (elipse.Height / 2), 0, 0);
                MyCanvas.Children.Add(elipse);
            }
        }
    }
}
