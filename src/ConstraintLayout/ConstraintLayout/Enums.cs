using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintLayout
{
    public enum ConstraintProperty
    {
        Width,
        Height,

        Left,
        Center,
        Right,

        Top,
        Middle,
        Bottom
    }

    public enum ConstraintRelation
    {
        Equal,
        LessEqual,
        GreaterEqual
    }
}
