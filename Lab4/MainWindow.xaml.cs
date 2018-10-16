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
using System.Drawing;
using System.IO;

namespace Lab4
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

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            string filepath = @"D:\LP\LP_5_semester\Compute Grafics\Labs\sticker1.jpg";
            Bitmap bitmap = null;
            using (Stream stream = File.Open(filepath, FileMode.Open))
            {
                var image = System.Drawing.Image.FromStream(stream);
                bitmap = new Bitmap(image);
            }
            MyImage.Source = Converter.ToBitmapSource(bitmap);
        }
    }
}
