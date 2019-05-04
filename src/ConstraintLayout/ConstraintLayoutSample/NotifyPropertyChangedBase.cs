using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConstraintLayoutSample.Annotations;

namespace ConstraintLayoutSample
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SampleVmBase : NotifyPropertyChangedBase
    {
        public SampleVmBase(string displayName)
        {
            DisplayName = displayName;
        }

        [Browsable(false)]
        public string DisplayName { get; }
    }
}