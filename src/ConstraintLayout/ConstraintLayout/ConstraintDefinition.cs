using System.Windows;
using Kiwi;

namespace ConstraintLayout
{
    public abstract class ConstraintDefinition : Freezable
    {
        public abstract void ProvideConstraints(Solver solver);
    }
}
