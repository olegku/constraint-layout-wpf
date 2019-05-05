namespace ConstraintLayoutSample
{
    public class Sample1Vm : SampleVmBase
    {
        private bool _useCanvasLayout;
        private int _spacing = 10;

        public Sample1Vm() : base("Sample 1")
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