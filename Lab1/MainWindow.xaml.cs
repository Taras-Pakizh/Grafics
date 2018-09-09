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
        //Canvas
        private void MyCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw(GetStep());
        }
        private void MyCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Draw(GetStep());
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
                CanvasInfo info = new CanvasInfo(MyCanvas.ActualWidth, MyCanvas.ActualHeight, GetStep());
                if (temp.X <= point.X)
                    AddRectancle(PointConverter.ToCoor(temp, info), PointConverter.ToCoor(point, info), color);
                else AddRectancle(PointConverter.ToCoor(point, info), PointConverter.ToCoor(temp, info), color);
                temp = new Point(-1, -1);
                Draw(GetStep());
            }
        }
        //Slider
        private void SliderX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelX.Content = "Upper left point X: " + Math.Round(((Slider)sender).Value, 2);
        }
        private void SliderY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelY.Content = "Upper left point Y: " + Math.Round(((Slider)sender).Value, 2);
        }
        private void SliderX2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelX2.Content = "Upper right point X: " + Math.Round(((Slider)sender).Value, 2);
        }
        private void SliderY2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelY2.Content = "Upper right point Y: " + Math.Round(((Slider)sender).Value, 2);
        }
        //Menu
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (!IsPointsCorrect())
            {
                ErrorWindow window = new ErrorWindow();
                window.Show();
                return;
            }
            AddRectancle(
                new Point()
                {
                    X = SliderX.Value,
                    Y = SliderY.Value
                }, new Point()
                {
                    X = SliderX2.Value,
                    Y = SliderY2.Value
                },
                color
            );
            Draw(GetStep());
        }
        private void AuthorItem_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow window = new InfoWindow();
            window.Show();
        }
        private void UndoItem_Click(object sender, RoutedEventArgs e)
        {
            UndoRectancle();
            Draw(GetStep());
        }
        private void RedoItem_Click(object sender, RoutedEventArgs e)
        {
            RedoRectancle();
            Draw(GetStep());
        }
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            rectancleInfos.Clear();
            SavedRectancles.Clear();
            RemovedPaths.Clear();
            RemovedRectancles.Clear();
            Draw(GetStep());
        }
        private void ColorItem_Click(object sender, RoutedEventArgs e)
        {
            ColorPickWindow window = new ColorPickWindow();
            window.Pick += Window_Pick;
            window.Show();
        }
        #endregion


        #region Variables
        private Point temp = new Point(-1, -1);
        private Color color;
        private List<RectancleInfo> rectancleInfos = new List<RectancleInfo>();
        private List<Path> SavedRectancles = new List<Path>();
        private int maxStep = 50;
        private List<Path> RemovedPaths = new List<Path>();
        private List<RectancleInfo> RemovedRectancles = new List<RectancleInfo>();
        #endregion


        #region Logic
        private double GetStep()
        {
            double step = maxStep;
            foreach (var item in rectancleInfos)
            {
                var points = GetRectancleCoor(item);
                foreach(var point in points)
                {
                    if(Math.Abs(point.X) > MyCanvas.ActualWidth / step / 2)
                        step = MyCanvas.ActualWidth / (Math.Round(Math.Abs(point.X)) + 1) / 2;
                    if (Math.Abs(point.Y) > MyCanvas.ActualHeight / step / 2)
                        step = MyCanvas.ActualHeight / (Math.Round(Math.Abs(point.Y)) + 1) / 2;
                }
            }
            return step;
        }
        private Point[] GetRectancleCoor(RectancleInfo info)
        {
            double Xdelta = Math.Abs(info.leftUp.X - info.rightUp.X);
            double Ydelta = Math.Abs(info.leftUp.Y - info.rightUp.Y);
            Point[] rectanclePoints = null;
            if (info.leftUp.Y <= info.rightUp.Y)
            {
                rectanclePoints = new Point[]
                {
                    info.leftUp,
                    info.rightUp,
                    new Point(info.rightUp.X + Ydelta, info.rightUp.Y - Xdelta),
                    new Point(info.leftUp.X + Ydelta, info.leftUp.Y - Xdelta),
                };
            }
            else
            {
                rectanclePoints = new Point[]
                {
                    info.leftUp,
                    info.rightUp,
                    new Point(info.rightUp.X - Ydelta, info.rightUp.Y - Xdelta),
                    new Point(info.leftUp.X - Ydelta, info.leftUp.Y - Xdelta),
                };
            }
            return rectanclePoints;
        }
        private void Draw(double _step)
        {
            CanvasInfo info = new CanvasInfo(MyCanvas.ActualWidth, MyCanvas.ActualHeight, _step);
            GraficsCreator creator = new GraficsCreator(info);
            var lines = creator.DrawBackground();

            var rectancles = creator.DrawRectancles(rectancleInfos);
            int i = 0;
            foreach(var path in rectancles)
            {
                if(SavedRectancles.Count <= i)
                    SavedRectancles.Add(path);
                else
                    SavedRectancles[i].Data = path.Data;
                ++i;
            }

            var labels = creator.DrawLabels();
            MyCanvas.Children.Clear();
            foreach (var item in lines)
                MyCanvas.Children.Add(item);
            foreach (var item in SavedRectancles)
                MyCanvas.Children.Add(item);
            foreach (var item in labels)
                MyCanvas.Children.Add(item);
        }
        private void AddRectancle(Point left, Point right, Color _color)
        {
            rectancleInfos.Add(new RectancleInfo()
            {
                leftUp = left,
                rightUp = right,
                brush = new SolidColorBrush(_color)
            });
        }
        private void UndoRectancle()
        {
            if (rectancleInfos.Count < 1 || SavedRectancles.Count < 2) return;
            RemovedPaths.AddRange(SavedRectancles.Skip(SavedRectancles.Count - 2));
            SavedRectancles.RemoveRange(SavedRectancles.Count - 2, 2);
            RemovedRectancles.Add(rectancleInfos.Last());
            rectancleInfos.RemoveAt(rectancleInfos.Count - 1);
        }
        private void RedoRectancle()
        {
            if (RemovedRectancles.Count < 1 || RemovedPaths.Count < 2) return;
            SavedRectancles.AddRange(RemovedPaths.Skip(RemovedPaths.Count - 2));
            RemovedPaths.RemoveRange(RemovedPaths.Count - 2, 2);
            rectancleInfos.Add(RemovedRectancles.Last());
            RemovedRectancles.RemoveAt(RemovedRectancles.Count - 1);
        }

        private void Window_Pick(Color _color)
        {
            color = _color;
        }
        private bool IsPointsCorrect()
        {
            bool result = true;
            if (Math.Round(SliderX.Value, 2) == Math.Round(SliderX2.Value, 2) && 
                Math.Round(SliderY.Value, 2) == Math.Round(SliderY2.Value, 2))
                result = false;
            return result;
        }
        #endregion

    }
}
