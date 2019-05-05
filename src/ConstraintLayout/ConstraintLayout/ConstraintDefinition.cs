using System.Windows;

namespace ConstraintLayout
{
    public abstract class ConstraintDefinition : Freezable
    {
        public abstract void ProvideConstraints(Kiwi.Solver solver);
    }
}
