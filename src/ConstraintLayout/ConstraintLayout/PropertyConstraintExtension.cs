using System;
using System.Windows;
using System.Windows.Markup;

namespace ConstraintLayout
{
    public class PropertyConstraintExtension : MarkupExtension
    {
        private readonly ConstraintRelation _relation;

        public PropertyConstraintExtension(ConstraintRelation relation)
        {
            _relation = relation;
        }

        #region Properties

        public UIElement Element { get; set; }
        public ConstraintProperty Property { get; set; }
        public double Constant { get; set; }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new SimplePropertyConstraint
            {
                Relation = _relation,
                Element = Element,
                Property = Property,
                Constant = Constant,
            };
        }
    }
}