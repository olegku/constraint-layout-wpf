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
    /// Interaction logic for Sample1.xaml
    /// </summary>
    public partial class Sample1 : UserControl
    {
        public Sample1()
        {
            InitializeComponent();
        }
    }

    public class Sample1Vm : SampleVmBase
    {
        private bool _useCanvasLayout;
        private int _spacing = 10;

        public Sample1Vm() : base(nameof(Sample1Vm))
        {
        }

        public bool UseCanvasLayout
        {
            get => _useCanvasLayout;
            set
            {
                if (value == _useCanvasLayout) return;
                _useCanvasLayout = value;
                OnPropertyChanged();
            }
        }

        public int Spacing
        {
            get => _spacing;
            set
            {
                if (value == _spacing) return;
                _spacing = value;
                OnPropertyChanged();
            }
        }
    }
}
