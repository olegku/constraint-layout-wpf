using System.Windows;
using ConstraintLayout.PropertyConstraints;

namespace ConstraintLayoutSample.Behaviors
{
    public class SimpleConstraintMouseDragElementBehavior : AbstractMouseDragHandlerBehavior
    {
        public static readonly DependencyProperty XConstraintProperty = DependencyProperty.Register(
            "XConstraint", 
            typeof(SimplePropertyConstraint), 
            typeof(SimpleConstraintMouseDragElementBehavior), 
            new PropertyMetadata(default(SimplePropertyConstraint)));

        public static readonly DependencyProperty YConstraintProperty = DependencyProperty.Register(
            "YConstraint",
            typeof(SimplePropertyConstraint),
            typeof(SimpleConstraintMouseDragElementBehavior),
            new PropertyMetadata(default(SimplePropertyConstraint)));

        public SimplePropertyConstraint XConstraint
        {
            get => (SimplePropertyConstraint) GetValue(XConstraintProperty);
            set => SetValue(XConstraintProperty, value);
        }

        public SimplePropertyConstraint YConstraint
        {
            get => (SimplePropertyConstraint) GetValue(YConstraintProperty);
            set => SetValue(YConstraintProperty, value);
        }

        protected override void ApplyTranslation(double x, double y)
        {
            if (XConstraint != null) XConstraint.Constant += x;
            if (YConstraint != null) YConstraint.Constant += y;
        }
    }
}