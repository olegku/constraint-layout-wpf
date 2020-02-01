using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Kiwi;

namespace ConstraintLayout.Markup
{
    public class NoLessExtension : MarkupExtension
    {
        public NoLessExtension()
        {
        }

        public NoLessExtension(double constant)
        {
            Constant = constant;
        }

        public NoLessExtension(string elementName, ConstraintProperty property)
        {
            ElementName = elementName;
            Property = property;
        }

        public NoLessExtension(string elementName, ConstraintProperty property, double constant)
        {
            ElementName = elementName;
            Property = property;
            Constant = constant;
        }

        #region Properties

        public RelationalOperator Operator { get; set; }

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
                Relation = ConstraintRelation.GreaterEqual,
                Property = Property,
                Constant = Constant,
            };

            if (Element != null)
            {
                constraint.Element = Element;
            }
            else if (ElementName != null)
            {
                BindingOperations.SetBinding(constraint, SimplePropertyConstraint.ElementProperty, new Binding()
                {
                    ElementName = ElementName,
                    Mode = BindingMode.OneTime
                });
            }
            else
            {
                throw new InvalidOperationException($"'{nameof(Element)}' or '{nameof(ElementName)}' must be set");
            }

            return constraint;
        }
    }
}
