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
using Color = System.Drawing.Color;

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

            for(int row = 0; row < bitmap.Height; ++row)
                for(int col = 0; col < bitmap.Width; ++col)
                {
                    var pixel = bitmap.GetPixel(row, col);
                    var cmyk = Converter.RGB_to_CMYK(pixel);
                    cmyk.M = 1;
                    pixel = Converter.CMYK_to_RGB(cmyk).ToColor();
                    bitmap.SetPixel(row, col, pixel);
                }

            MyImage.Source = Converter.ToBitmapSource(bitmap);
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Color color = Color.FromArgb(125, 29, 107);
            MessageBox.Show(color.GetHue() + " " + color.GetSaturation() + " " + color.GetBrightness());
        }
    }
}
