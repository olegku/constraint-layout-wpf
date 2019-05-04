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

namespace ConstraintLayoutSample
{
    /// <summary>
    /// Interaction logic for Sample2.xaml
    /// </summary>
    public partial class Sample2 : UserControl
    {
        public Sample2()
        {
            InitializeComponent();
        }
    }

    public class Sample2Vm : SampleVmBase
    {
        private Orientation _stackOrientation;
        private double _spacing;

        public Sample2Vm() : base(nameof(Sample2Vm))
        {
        }

        public Orientation StackOrientation
        {
            get => _stackOrientation;
            set
            {
                if (value == _stackOrientation) return;
                _stackOrientation = value;
                OnPropertyChanged();
            }
        }

        public double Spacing
        {
            get => _spacing;
            set
            {
                if (value.Equals(_spacing)) return;
                _spacing = value;
                OnPropertyChanged();
            }
        }
    }
}
