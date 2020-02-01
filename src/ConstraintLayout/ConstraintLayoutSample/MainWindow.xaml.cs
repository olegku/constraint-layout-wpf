using System.Collections.ObjectModel;
using System.Windows;

namespace ConstraintLayoutSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class MainVm
    {
        public ObservableCollection<SampleVmBase> SampleVms { get; } = new ObservableCollection<SampleVmBase>
        {
            new Sample3Vm(),
            new Sample1Vm(),
            new Sample2Vm(),
        };
    }
}
