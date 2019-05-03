using Kiwi;

namespace ConstraintLayout
{
    internal class ElementVars
    {
        public ElementVars(int elementId)
        {
            Id = elementId;

            Width = new Variable($"{elementId}.{nameof(Width)}");
            Height = new Variable($"{elementId}.{nameof(Height)}");

            Left = new Variable($"{elementId}.{nameof(Left)}");
            Center = new Variable($"{elementId}.{nameof(Center)}");
            Right = new Variable($"{elementId}.{nameof(Right)}");

            Top = new Variable($"{elementId}.{nameof(Top)}");
            Middle = new Variable($"{elementId}.{nameof(Middle)}");
            Bottom = new Variable($"{elementId}.{nameof(Bottom)}");
        }

        public int Id { get; }

        public Variable Width { get; }
        public Variable Height { get; }

        public Variable Left { get; }
        public Variable Center { get; }
        public Variable Right { get; }

        public Variable Top { get; }
        public Variable Middle { get; }
        public Variable Bottom { get; }

        public Variable this[ConstraintProperty property]
        {
            get
            {
                switch (property)
                {
                    case ConstraintProperty.Width: return Width;
                    case ConstraintProperty.Height: return Height;
                    case ConstraintProperty.Left: return Left;
                    case ConstraintProperty.Center: return Center;
                    case ConstraintProperty.Right: return Right;
                    case ConstraintProperty.Top: return Top;
                    case ConstraintProperty.Middle: return Middle;
                    case ConstraintProperty.Bottom: return Bottom;
                    default: return null;
                }
            }
        }
    }
}
