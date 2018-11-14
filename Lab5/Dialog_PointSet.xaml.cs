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
    /// Логика взаимодействия для Dialog_PointSet.xaml
    /// </summary>
    public partial class Dialog_PointSet : Window
    {
        private List<Point> _result = new List<Point>();
        private Point _point;

        public event SetPointsEventHandler Return;

        public Dialog_PointSet()
        {
            InitializeComponent();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            if (!_GetPoint())
            {
                MessageBox.Show("Wrong input");
                return;
            }
            _result.Add(_point);
            var item = new ListBoxItem() { Content = _result.Count() + ". Point: " + _point.ToString() };
            if (_result.Count % 2 == 1)
                item.Background = Brushes.Orange;
            PointList.Items.Add(item);
        }

        private bool _GetPoint()
        {
            string x = Edit_X.Text;
            string y = Edit_Y.Text;

            if(Double.TryParse(x, out double pointX) && Double.TryParse(y, out double pointY))
            {
                _point = new Point(pointX, pointY);
                return true;
            }
            return false;
        }

        private void Button_Return_Click(object sender, RoutedEventArgs e)
        {
            bool check = false;
            if (CheckBox_StepByStep.IsChecked == true)
                check = true;
            Return?.Invoke(_result, check);
        }
    }
}
