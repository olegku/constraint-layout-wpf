using System;
using System.Windows;
using ConstraintLayout.Utils;
using Kiwi;

namespace ConstraintLayout
{
    public class SimpleConstraint : ConstraintDefinition
    {
        public static readonly DependencyProperty Element1Property = DependencyProperty.Register(
            "Element1",
            typeof(UIElement),
            typeof(SimpleConstraint),
            new PropertyMetadata((UIElement)null));

        public static readonly DependencyProperty Element2Property = DependencyProperty.Register(
            "Element2",
            typeof(UIElement),
            typeof(SimpleConstraint),
            new PropertyMetadata((UIElement)null));

        public static readonly DependencyProperty Property1Property = DependencyProperty.Register(
            "Property1", 
            typeof(ConstraintProperty), 
            typeof(SimpleConstraint), 
            new PropertyMetadata(default(ConstraintProperty)));

        public static readonly DependencyProperty Property2Property = DependencyProperty.Register(
            "Property2", 
            typeof(ConstraintProperty), 
            typeof(SimpleConstraint), 
            new PropertyMetadata(default(ConstraintProperty)));

        public static readonly DependencyProperty Constant1Property = DependencyProperty.Register(
            "Constant1",
            typeof(double),
            typeof(SimpleConstraint),
            new PropertyMetadata(0.0));

        public static readonly DependencyProperty Constant2Property = DependencyProperty.Register(
            "Constant2",
            typeof(double),
            typeof(SimpleConstraint),
            new PropertyMetadata(0.0));

        public static readonly DependencyProperty RelationProperty = DependencyProperty.Register(
            "Relation", 
            typeof(ConstraintRelation), 
            typeof(SimpleConstraint), 
            new PropertyMetadata(ConstraintRelation.Equal));


        public UIElement Element1
        {
            get => (UIElement)GetValue(Element1Property);
            set => SetValue(Element1Property, value);
        }

        public UIElement Element2
        {
            get => (UIElement)GetValue(Element2Property);
            set => SetValue(Element2Property, value);
        }

        public ConstraintProperty Property1
        {
            get => (ConstraintProperty)GetValue(Property1Property);
            set => SetValue(Property1Property, value);
        }

        public ConstraintProperty Property2
        {
            get => (ConstraintProperty)GetValue(Property2Property);
            set => SetValue(Property2Property, value);
        }

        public double Constant1
        {
            get => (double)GetValue(Constant1Property);
            set => SetValue(Constant1Property, value);
        }

        public double Constant2
        {
            get => (double)GetValue(Constant2Property);
            set => SetValue(Constant2Property, value);
        }

        public ConstraintRelation Relation
        {
            get => (ConstraintRelation)GetValue(RelationProperty);
            set => SetValue(RelationProperty, value);
        }
        
        protected override Freezable CreateInstanceCore() => new SimpleConstraint();

        public override void ProvideConstraints(Solver solver)
        {
            var var1 = Element1?.ApplyTo(ConstraintCanvas.GetElementVars)?[Property1];
            var var2 = Element2?.ApplyTo(ConstraintCanvas.GetElementVars)?[Property2];

            var var1NotNull = !ReferenceEquals(var1, null);
            var var2NotNull = !ReferenceEquals(var2, null);

            var op = GetRelationalOperator(Relation);

            if (var1NotNull && var2NotNull)
            {
                solver.AddConstraint(new Kiwi.Constraint(var1 + Constant1 - var2 - Constant2, op));
            }
            else if (var1NotNull)
            {
                solver.AddConstraint(new Kiwi.Constraint(var1 + Constant1 - Constant2, op));
            }
            else if (var2NotNull)
            {
                solver.AddConstraint(new Kiwi.Constraint(Constant1 - var2 - Constant2, op));
            }
        }

        private RelationalOperator GetRelationalOperator(ConstraintRelation relation)
        {
            switch (relation)
            {
                case ConstraintRelation.Equal: return RelationalOperator.OP_EQ;
                case ConstraintRelation.LessEqual: return RelationalOperator.OP_LE;
                case ConstraintRelation.GreaterEqual: return RelationalOperator.OP_GE;
                default: throw new ArgumentOutOfRangeException(nameof(relation), relation, null);
            }
        }
    }
}