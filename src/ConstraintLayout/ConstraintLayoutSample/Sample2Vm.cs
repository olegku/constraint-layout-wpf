using System.Windows.Controls;

namespace ConstraintLayoutSample
{
    public class Sample2Vm : SampleVmBase
    {
        private Orientation _stackOrientation;
        private double _spacing;

        public Sample2Vm() : base("Sample 2")
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