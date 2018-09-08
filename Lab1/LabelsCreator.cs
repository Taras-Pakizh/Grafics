using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lab1
{
    class LabelsCreator
    {
        private List<Label> labels;
        public IEnumerable<Label> Labels
        {
            get { return labels; }
        }

        public IEnumerable<Label> GetLabels(CanvasInfo info)
        {
            labels = new List<Label>();
            labels.AddRange(GetAxisLabels(info));
            labels.AddRange(GetXNumbersLabels(info));
            labels.AddRange(GetYNumbersLabels(info));
            return labels;
        }

        private IEnumerable<Label> GetAxisLabels(CanvasInfo info)
        {
            Label labelX = new Label();
            Label labelY = new Label();

            labelX.Content = "X";
            labelY.Content = "Y";

            labelX.BorderBrush = Brushes.White;
            labelY.BorderBrush = Brushes.White;

            labelX.Margin = new Thickness(info.Width - 25, info.Heigth / 2 - 25, 0, 0);
            labelY.Margin = new Thickness(info.Width / 2 - 25, 25, 0, 0);

            return new List<Label>()
            {
                labelX,
                labelY
            };
        }
        private IEnumerable<Label> GetXNumbersLabels(CanvasInfo info)
        {
            List<Label> result = new List<Label>();
            int dashesXCount = (int)(info.WidthCenter / info.Step);
            for(int i = 1; i < dashesXCount + 1; ++i)
            {
                result.Add(new Label()
                {
                    BorderBrush = Brushes.White,
                    Content = "-" + i,
                    Margin = new Thickness(info.WidthCenter - (info.Step * i), info.Heigth - 25, 0, 0)
                });
                result.Add(new Label()
                {
                    BorderBrush = Brushes.White,
                    Content = i,
                    Margin = new Thickness(info.WidthCenter + (info.Step * i), info.Heigth - 25, 0, 0)
                });
            }
            return result;
        }
        private IEnumerable<Label> GetYNumbersLabels(CanvasInfo info)
        {
            List<Label> result = new List<Label>();
            int dashesYCount = (int)(info.HeigthCenter / info.Step);
            for (int i = 1; i < dashesYCount + 1; ++i)
            {
                result.Add(new Label()
                {
                    BorderBrush = Brushes.White,
                    Content = i,
                    Margin = new Thickness(info.WidthCenter - 25, info.HeigthCenter - (info.Step * i), 0, 0)
                });
                result.Add(new Label()
                {
                    BorderBrush = Brushes.White,
                    Content = "-" + i,
                    Margin = new Thickness(info.WidthCenter - 25, info.HeigthCenter + (info.Step * i), 0, 0)
                });
            }
            return result;
        }
    }
}
