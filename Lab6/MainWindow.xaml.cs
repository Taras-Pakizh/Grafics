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
using System.Windows.Threading;

namespace Lab6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RectangleDataContent content;

        private DispatcherTimer timer;
        private bool _IsMoving = false;
        private double _step = 1;
        
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            content = new RectangleDataContent(new List<Point>()
            {
                new Point(100, 100),
                new Point(500, 100),
                new Point(500, 300),
                new Point(100, 300)
            });
            _DrawDefault();
        }

        private void _DrawDefault()
        {
            MyCanvas.Children.Clear();
            _DrawAsix();
            MyCanvas.Children.Add(PathCreator.GetRectangle(content.Rectancle));
            if (content.IsReflacting)
                MyCanvas.Children.Add(PathCreator.GetRectangle(content.Reflacting));
        }

        private void _DrawAsix()
        {
            CanvasInfo info = new CanvasInfo(MyCanvas.ActualWidth, MyCanvas.ActualHeight);
            MyCanvas.Children.Add(PathCreator.GetAxis(info));
            MyCanvas.Children.Add(PathCreator.GetYX(info));
        }

        private void Button_MoveYX_Click(object sender, RoutedEventArgs e)
        {
            if (_IsMoving)
            {
                timer.Stop();
                _IsMoving = false;
            }
            else
            {
                _IsMoving = true;
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(10000);
                timer.Tick += Timer_Move;
                timer.Start();
            }
        }

        private void Timer_Move(object sender, EventArgs e)
        {
            content.Move(new CanvasInfo(MyCanvas.ActualWidth, MyCanvas.ActualHeight), _step);
            _DrawDefault();
        }

        private void Button_Reflection_Click(object sender, RoutedEventArgs e)
        {
            CanvasInfo info = new CanvasInfo(MyCanvas.ActualWidth, MyCanvas.ActualHeight);
            if (content.IsReflacting)
                content.IsReflacting = false;
            else
                content.SetReflacting(info);
            _DrawDefault();
        }

        private void Slider_Scale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (content == null) return;
            MyCanvas.Children.Clear();
            content.ScaleRectangle(e.NewValue);
            _DrawDefault();
        }
    }
}
