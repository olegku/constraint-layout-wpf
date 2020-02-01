using System.ComponentModel;
using System.Windows;
using Kiwi;

namespace ConstraintLayout
{
    [TypeConverter(typeof(PropertyConstraintConverter))]
    public abstract class PropertyConstraint : Freezable
    {
        public abstract void AddToSolver(Solver solver, UIElement element, ConstraintProperty property);
    }
}