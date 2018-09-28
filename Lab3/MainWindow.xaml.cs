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
        public MainWindow()
        {
            InitializeComponent();
            Do();
        }

        public void Do()
        {
            CanvasInfo info = new CanvasInfo(800, 400, new Point(400, 200));
            Path path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            Fractal fractal = new Peano(info);
            fractal.CreateNextStages(3);
            path.Data = fractal.TopGeometry();
            MyCanvas.Children.Add(path);
        }
    }
}
