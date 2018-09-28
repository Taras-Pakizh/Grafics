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

namespace Lab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Fractal fractal = null;
        int currentStage = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CanvasInfo info = new CanvasInfo(e.NewSize.Width, e.NewSize.Height, new Point(e.NewSize.Width / 2, e.NewSize.Height / 2));
            if (fractal != null && fractal.geometry != null)
                BuildFractal(info, 0, true);
        }

        private void BuildFractal(CanvasInfo info, int stage, bool top)
        {

            if (fractal == null)
                fractal = new Peano(info);
            int iterations = stage - fractal.Count;
            if(iterations > 0)
                fractal.CreateNextStages(iterations);

            fractal.info = info;
            Path path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;
            if (top)
            {
                path.Data = fractal.TopGeometry();
                currentStage = fractal.Count;
            }
            else
            {
                path.Data = fractal[stage - 1];
                currentStage = stage;
            }
            MyCanvas.Children.Clear();
            MyCanvas.Children.Add(path);
        }

        private int ReadIterations()
        {
            if(Int32.TryParse(InputBox.Text, out int result))
                if (result > 0 && result < 10)
                    return result;
            return -1;
        }

        //Events
        private void BuildBut_Click(object sender, RoutedEventArgs e)
        {
            int iterations = ReadIterations();
            if (iterations == -1) return;
            CanvasInfo info = new CanvasInfo(MyCanvas.ActualWidth, MyCanvas.ActualHeight, new Point(MyCanvas.ActualWidth / 2, MyCanvas.ActualHeight / 2));
            BuildFractal(info, iterations, false);
        }

        private void DeleteBut_Click(object sender, RoutedEventArgs e)
        {
            MyCanvas.Children.Clear();
        }

        private void AddIterateBut_Click(object sender, RoutedEventArgs e)
        {
            if (currentStage == 9) return;
            ++currentStage;
            InputBox.Text = currentStage.ToString();
            CanvasInfo info = new CanvasInfo(MyCanvas.ActualWidth, MyCanvas.ActualHeight, new Point(MyCanvas.ActualWidth / 2, MyCanvas.ActualHeight / 2));
            BuildFractal(info, currentStage, false);
        }

        private void RemoveIterateBut_Click(object sender, RoutedEventArgs e)
        {
            --currentStage;
            if (currentStage < 1) return;
            InputBox.Text = currentStage.ToString();
            CanvasInfo info = new CanvasInfo(MyCanvas.ActualWidth, MyCanvas.ActualHeight, new Point(MyCanvas.ActualWidth / 2, MyCanvas.ActualHeight / 2));
            BuildFractal(info, currentStage, false);
        }
    }
}
