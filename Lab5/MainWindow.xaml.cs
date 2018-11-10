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

namespace Lab5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Point> _points = new List<Point>();
        private double _step = 0.005;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void _DrawPoints(List<Point> points, int radius, Brush brush)
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
                _DrawPoints(_points, 8, Brushes.Orange);

            if(_points.Count > 1 && ComboBox_Algorithm.SelectedItem != null)
            {
                MyCanvas.Children.Clear();
                _DrawPoints(_points, 8, Brushes.Orange);
                var item = (ComboBoxItem)ComboBox_Algorithm.SelectedItem;
                List<Point> bezie = new List<Point>();
                if ((string)item.Content == "Traditional")
                    bezie = Bezie.Traditional(_points, _step);
                else if ((string)item.Content == "Recurtional")
                    bezie = Bezie.Recurtion(_points, _step);
              
                var path = PathCreator.GetPath();
                path.Data = Geometry.Parse(PathCreator.GetCurveInfo(bezie, _points));
                MyCanvas.Children.Add(path);
            }
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            _points.Clear();
            MyCanvas.Children.Clear();
        }
    }
}
