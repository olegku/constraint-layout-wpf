using System.Windows;
using ConstraintLayout.Utils;
using Kiwi;

namespace ConstraintLayout.PropertyConstraints
{
    public class LerpPropertyConstraint : PropertyConstraint
    {
        public static readonly DependencyProperty FromElementProperty = DependencyProperty.Register(
            "FromElement", 
            typeof(UIElement), 
            typeof(LerpPropertyConstraint), 
            new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty FromPropertyProperty = DependencyProperty.Register(
            "FromProperty",
            typeof(ConstraintProperty),
            typeof(LerpPropertyConstraint),
            new FrameworkPropertyMetadata(default(ConstraintProperty), FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ToElementProperty = DependencyProperty.Register(
            "ToElement",
            typeof(UIElement),
            typeof(LerpPropertyConstraint),
            new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ToPropertyProperty = DependencyProperty.Register(
            "ToProperty",
            typeof(ConstraintProperty),
            typeof(LerpPropertyConstraint),
            new FrameworkPropertyMetadata(default(ConstraintProperty), FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
            "Parameter", 
            typeof(double), 
            typeof(LerpPropertyConstraint),
            new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.AffectsArrange));

        public UIElement FromElement
        {
            get => (UIElement) GetValue(FromElementProperty);
            set => SetValue(FromElementProperty, value);
        }

        public ConstraintProperty FromProperty
        {
            get => (ConstraintProperty) GetValue(FromPropertyProperty);
            set => SetValue(FromPropertyProperty, value);
        }

        public UIElement ToElement
        {
            get => (UIElement) GetValue(ToElementProperty);
            set => SetValue(ToElementProperty, value);
        }

        public ConstraintProperty ToProperty
        {
            get => (ConstraintProperty) GetValue(ToPropertyProperty);
            set => SetValue(ToPropertyProperty, value);
        }

        public double Parameter
        {
            get => (double)GetValue(ParameterProperty);
            set => SetValue(ParameterProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new LerpPropertyConstraint();

        public override void AddToSolver(Solver solver, UIElement element, ConstraintProperty property)
        {
            var lhsVar = element.ApplyTo(ConstraintCanvas.GetElementVars)?[property];

            var fromVar = FromElement?.ApplyTo(ConstraintCanvas.GetElementVars)?[FromProperty];
            var toVar = ToElement?.ApplyTo(ConstraintCanvas.GetElementVars)?[ToProperty];
            var t = Parameter;

            if (fromVar is null && toVar is null)
            {
                // NOTE: nothing to add
            }
            else if (fromVar is null)
            {
                solver.AddConstraint(lhsVar == toVar);
            }
            else if (toVar is null)
            {
                solver.AddConstraint(lhsVar == fromVar);
            }
            else
            {
                solver.AddConstraint(lhsVar == fromVar * t + toVar * (1.0 - t));
            }
        }
    }
}