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
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
namespace TNM
{
    public class ViewModel {
        public ISeries[] Series { get; set; }  
            = new ISeries[] { 
                new LineSeries<double> {
                    Values = new double[] { 1, 2, 3, 4, 5, 6 },
                    Fill = null
            }
        };
    }
}
