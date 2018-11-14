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

namespace Lab5
{
    /// <summary>
    /// Логика взаимодействия для TipWindow.xaml
    /// </summary>
    public partial class TipWindow : Window
    {
        private List<List<Point>> Tips = new List<List<Point>>()
        {
            new List<Point>()
            {
                new Point(0, 0),
                new Point(300, 300)
            },
            new List<Point>()
            {
                new Point(-300, -300),
                new Point(-300, 300),
                new Point(300, 300),
                new Point(300, -300)
            },
            new List<Point>()
            {
                new Point(-300, -300),
                new Point(300, 300),
                new Point(-300, 300),
                new Point(300, -300)
            }
        };

        public event SetPointsEventHandler Return;

        public TipWindow()
        {
            InitializeComponent();
        }

        private void TryTip_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_Tips.SelectedIndex == -1)
                return;
            Return?.Invoke(Tips[ListBox_Tips.SelectedIndex], false);
        }
    }
}
