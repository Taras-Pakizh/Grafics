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
using System.Windows.Shapes;

namespace Lab1
{
    public delegate void PickColorEvent(Color color);

    public partial class ColorPickWindow : Window
    {
        public event PickColorEvent Pick;

        public ColorPickWindow()
        {
            InitializeComponent();
        }

        private void MyPicker_OnPickColor(Color color)
        {
            Pick?.Invoke(color);
            this.Close();
        }
    }
}
