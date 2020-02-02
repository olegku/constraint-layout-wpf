using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using ConstraintLayout.PropertyConstraints;

namespace ConstraintLayout.Markup
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

        [ConstructorArgument("elementName")]
        public string ElementName { get; set; }

        [ConstructorArgument("property")]
        public ConstraintProperty Property { get; set; }

        [ConstructorArgument("constant")]
        public double Constant { get; set; }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var constraint = new SimplePropertyConstraint
            {
                Relation = _relation,
                Property = Property,
                Constant = Constant,
            };

            if (Element != null)
            {
                constraint.Element = Element;
            }
            else if (ElementName != null)
            {
                BindingOperations.SetBinding(constraint, SimplePropertyConstraint.ElementProperty, new Binding
                {
                    ElementName = ElementName,
                    Mode = BindingMode.OneTime
                });
            }

            return constraint;
        }
    }
}