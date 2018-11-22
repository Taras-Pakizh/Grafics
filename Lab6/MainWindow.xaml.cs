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
        private List<Point> _rectangle = new List<Point>();
        private RectangleDataContent content;

        private DispatcherTimer timer;
        private bool _IsMoving = false;
        
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //content = new RectangleDataContent(new List<Point>()
            //{
            //    new Point(600, 600),
            //    new Point(1000, 600),
            //    new Point(1000, 800),
            //    new Point(600, 800)
            //});
            _DrawDefault();
        }

        private void _DrawDefault()
        {
            MyCanvas.Children.Clear();
            _DrawAsix();
            if(content != null)
            {
                MyCanvas.Children.Add(PathCreator.GetRectangle(content.Rectancle));
                if (content.IsReflacting)
                    MyCanvas.Children.Add(PathCreator.GetRectangle(content.Reflacting));
            }
        }

        private void _DrawAsix()
        {
            var info = _GetCanvasInfo();
            MyCanvas.Children.Add(PathCreator.GetAxis(info));
            MyCanvas.Children.Add(PathCreator.GetYX(info));
        }

        private void _DrawElipse(Point point)
        {
            Ellipse ellipse = new Ellipse()
            {
                Width = 5,
                Height = 5,
                Fill = Brushes.Orange,
                Margin = new Thickness(point.X, point.Y, 0, 0)
            };
            MyCanvas.Children.Add(ellipse);
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
            content.Move(_GetCanvasInfo());
            _DrawDefault();
        }

        private void Button_Reflection_Click(object sender, RoutedEventArgs e)
        {
            if (content.IsReflacting)
                content.IsReflacting = false;
            else
                content.SetReflacting(_GetCanvasInfo());
            _DrawDefault();
        }

        private void Slider_Scale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (content == null) return;
            MyCanvas.Children.Clear();
            content.ScaleRectangle(e.NewValue, _GetCanvasInfo());
            _DrawDefault();
        }

        private CanvasInfo _GetCanvasInfo()
        {
            return new CanvasInfo(MyCanvas.ActualWidth, MyCanvas.ActualHeight);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (content == null)
                return;

            double xValue = e.NewSize.Width / e.PreviousSize.Width;
            double yValue = e.NewSize.Height / e.PreviousSize.Height;

            var rectangle = content.RealRectancle;
            for(int i = 0; i < rectangle.Count; ++i)
                rectangle[i] = new Point()
                {
                    X = rectangle[i].X * xValue,
                    Y = rectangle[i].Y * yValue
                };
            rectangle = RectangleCreator.Create(rectangle[0], rectangle[1]);
            CanvasInfo info = new CanvasInfo(e.NewSize.Width, e.NewSize.Height);
            content.SetNew(rectangle, info);

            _DrawDefault();
        }

        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _rectangle.Add(e.GetPosition(MyCanvas));
            _DrawElipse(_rectangle.Last());
            if(_rectangle.Count == 2)
            {
                content = new RectangleDataContent(RectangleCreator.Create(_rectangle.First(), _rectangle.Last()));
                _rectangle.Clear();
                _DrawDefault();
            }
        }
    }
}
