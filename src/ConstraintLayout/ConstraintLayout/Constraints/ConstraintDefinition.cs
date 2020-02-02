using System.Windows;

namespace ConstraintLayout.Constraints
{
    public abstract class ConstraintDefinition : Freezable
    {
        public abstract void ProvideConstraints(Kiwi.Solver solver);
    }
}
