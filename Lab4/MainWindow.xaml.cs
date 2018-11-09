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
using Microsoft.Win32;

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

        private Bitmap _DefaultBitmap;
        private Bitmap bitmap;

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            string filepath = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                filepath = openFileDialog.FileName;

            if (filepath.Length == 0)
            {
                MessageBox.Show("File didn't open");
                return;
            }
            string[] files = new string[]
            {
                ".bmp", ".BMP",
                ".png", ".PNG",
                ".jpg", ".JPG",
                ".jpeg", ".JPEG"
            };
            for (int i = 0; i < files.Length + 1; ++i)
            {
                if (i == files.Length)
                {
                    MessageBox.Show("It is not image");
                    return;
                }
                if (filepath.EndsWith(files[i]))
                    break;
            }

            using (Stream stream = File.Open(filepath, FileMode.Open))
            {
                var image = System.Drawing.Image.FromStream(stream);
                bitmap = new Bitmap(image);
            }

            MyImage.Source = Converter.ToBitmapSource(bitmap);
            _DefaultBitmap = (Bitmap)bitmap.Clone();
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            bitmap = (Bitmap)_DefaultBitmap.Clone();

            if (bitmap == null)
                return;
            var hsv = GetHSVSliders();
            var cmyk = GetCMYKSliders();
            ConvertHSV(hsv);
            ConvertCMYK(cmyk);
            MyImage.Source = Converter.ToBitmapSource(bitmap);
        }

        private HSV GetHSVSliders()
        {
            HSV result = new HSV();
            result.H = HSV_H_Slider.Value;
            result.S = HSV_S_Slider.Value;
            result.V = HSV_V_Slider.Value;
            return result;
        }

        private CMYK GetCMYKSliders()
        {
            CMYK result = new CMYK();
            result.C = CMYK_C_Slider.Value;
            result.M = CMYK_M_Slider.Value;
            result.Y = CMYK_Y_Slider.Value;
            result.K = CMYK_K_Slider.Value;
            return result;
        }

        private void ConvertHSV(HSV _hsv)
        {
            if (_hsv.H == 1 && _hsv.S == 1 && _hsv.V == 1)
                return;

            for (int row = 0; row < bitmap.Height; ++row)
                for (int col = 0; col < bitmap.Width; ++col)
                {
                    var pixel = bitmap.GetPixel(row, col);
                    var hsv = Converter.RGB_to_HSV(pixel);
                    hsv = hsv * _hsv;
                    hsv.ToNormal();
                    pixel = Converter.HSV_to_RGB(hsv).ToColor();
                    bitmap.SetPixel(row, col, pixel);
                }
        }

        private void ConvertCMYK(CMYK _cmyk)
        {
            if (_cmyk.C == 1 && _cmyk.M == 1 && _cmyk.Y == 1 && _cmyk.K == 0)
                return;

            for (int row = 0; row < bitmap.Height; ++row)
                for (int col = 0; col < bitmap.Width; ++col)
                {
                    var pixel = bitmap.GetPixel(row, col);
                    var cmyk = Converter.RGB_to_CMYK(pixel);
                    cmyk = cmyk * _cmyk;
                    cmyk.ToNormal();
                    pixel = Converter.CMYK_to_RGB(cmyk).ToColor();
                    bitmap.SetPixel(row, col, pixel);
                }
        }
    }
}
