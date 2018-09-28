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

        public MainWindow()
        {
            InitializeComponent();
            Do();
        }

        public void Do()
        {
            fractal = new Peano();
            fractal.CreateNextStages(7);
        }

        private void MyCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CanvasInfo info = new CanvasInfo(e.NewSize.Width, e.NewSize.Height, new Point(e.NewSize.Width / 2, e.NewSize.Height / 2));
            fractal.info = info;
            Path path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            path.Data = fractal.TopGeometry();
            MyCanvas.Children.Clear();
            MyCanvas.Children.Add(path);
        }
    }
}
