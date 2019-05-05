using System.Windows;
using Kiwi;

namespace ConstraintLayout
{
    public abstract class PropertyConstraint : Freezable
    {
        public abstract void AddToSolver(Solver solver, UIElement element, ConstraintProperty property);
    }
}