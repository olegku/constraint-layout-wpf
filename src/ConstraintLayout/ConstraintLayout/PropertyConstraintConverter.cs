using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace ConstraintLayout
{
    public class PropertyConstraintConverter : TypeConverter
    {
        private static readonly Regex SimpleConstraintRegex = new Regex(
            @"^\s*(?<Rel>==|>=|<=)?\s*(?<ElemProp>(?<Elem>\w+)\s*\.\s*(?<Prop>\w+))?\s*(?<ConstSign>\+|-)?\s*(?<Const>\d*)\s*?$");

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) ||
                   base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return ConvertFromStringValue(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        private static SimplePropertyConstraint ConvertFromStringValue(string value)
        {
            var match = SimpleConstraintRegex.Match(value);
            if (!match.Success) throw new FormatException("INCORRECT FORMAT!");

            var result = new SimplePropertyConstraint
            {
                Relation = ConstraintRelation.Equal,
                Constant = 0,
            };

            result.Relation = GetConstraintRelation(match.Groups["Rel"], result);

            if (match.Groups["ElemProp"].Success)
            {
                var elem = match.Groups["Elem"].Value;
                var prop = match.Groups["Prop"].Value;

                BindingOperations.SetBinding(result, SimplePropertyConstraint.ElementProperty, new Binding
                {
                    ElementName = elem,
                    Mode = BindingMode.OneTime
                });

                if (Enum.TryParse<ConstraintProperty>(prop, out var property))
                {
                    result.Property = property;
                }
                else
                {
                    throw new FormatException($"Unknown value '{prop}' for {typeof(ConstraintProperty).FullName}");
                }
            }

            var groupConst = match.Groups["Const"];
            if (groupConst.Success)
            {
                result.Constant = double.Parse(groupConst.Value);
            }

            var groupConstSign = match.Groups["ConstSign"];
            if (groupConstSign.Success &&
                groupConstSign.Value == "-")
            {
                result.Constant = -result.Constant;
            }

            return result;
        }

        private static ConstraintRelation GetConstraintRelation(Group groupRel, SimplePropertyConstraint result)
        {
            if (!groupRel.Success)
            {
                return ConstraintRelation.Equal;
            }

            switch (groupRel.Value)
            {
                case "==": return ConstraintRelation.Equal;
                case "<=": return ConstraintRelation.LessEqual;
                case ">=": return ConstraintRelation.GreaterEqual;
                default: throw new ArgumentOutOfRangeException(nameof(groupRel.Value), groupRel.Value, null);
            }
        }
    }
}