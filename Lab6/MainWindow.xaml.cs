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

namespace Lab6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Point> _points = new List<Point>()
        {
            new Point(100, 100),
            new Point(500, 100),
            new Point(500, 300),
            new Point(100, 300)
        };

        public MainWindow()
        {
            InitializeComponent();
            MyCanvas.Children.Add(PathCreator.GetRectangle(_points));
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            CanvasInfo info = new CanvasInfo(MyCanvas.ActualWidth, MyCanvas.ActualHeight);

            MyCanvas.Children.Add(PathCreator.GetRectangle(Transformation.MoveYX(_points, -100)));
            MyCanvas.Children.Add(PathCreator.GetAxis(info));
            MyCanvas.Children.Add(PathCreator.GetYX(info));
        }

        private bool _IsMoving = false;

        private void Button_MoveYX_Click(object sender, RoutedEventArgs e)
        {

        }

        private bool _IsReflecting = false;

        private void Button_Reflection_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Slider_Scale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Point center = new Point()
            {
                X = _points.Average(x=>x.X),
                Y = _points.Average(x=>x.Y)
            };
            MyCanvas.Children.Clear();
            var points = Transformation.Scale(_points, center, e.NewValue);
            MyCanvas.Children.Add(PathCreator.GetRectangle(points));
        }
    }
}
