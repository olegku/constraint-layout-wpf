using System;
using System.Windows;
using ConstraintLayout.Utils;
using Kiwi;

namespace ConstraintLayout.PropertyConstraints
{
    public class SimplePropertyConstraint : PropertyConstraint
    {
        public static readonly DependencyProperty RelationProperty = DependencyProperty.Register(
            "Relation",
            typeof(ConstraintRelation),
            typeof(SimplePropertyConstraint),
            new FrameworkPropertyMetadata(ConstraintRelation.Equal, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ElementProperty = DependencyProperty.Register(
            "Element", 
            typeof(UIElement), 
            typeof(SimplePropertyConstraint), 
            new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(
            "Property", 
            typeof(ConstraintProperty), 
            typeof(SimplePropertyConstraint),
            new FrameworkPropertyMetadata(default(ConstraintProperty), FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ConstantProperty = DependencyProperty.Register(
            "Constant",
            typeof(double),
            typeof(SimplePropertyConstraint),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public ConstraintRelation Relation
        {
            get => (ConstraintRelation)GetValue(RelationProperty);
            set => SetValue(RelationProperty, value);
        }

        public UIElement Element
        {
            get => (UIElement) GetValue(ElementProperty);
            set => SetValue(ElementProperty, value);
        }

        public ConstraintProperty Property
        {
            get => (ConstraintProperty) GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        public double Constant
        {
            get => (double) GetValue(ConstantProperty);
            set => SetValue(ConstantProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new SimplePropertyConstraint();

        public override void AddToSolver(Solver solver, UIElement element, ConstraintProperty property)
        {
            var lhsVar = element.ApplyTo(ConstraintCanvas.GetElementVars)?[property];
            var rhsVar = Element?.ApplyTo(ConstraintCanvas.GetElementVars)?[Property];
            var op = GetRelationalOperator(Relation);

            solver.AddConstraint(rhsVar is null
                ? new Kiwi.Constraint(lhsVar - Constant, op)
                : new Kiwi.Constraint(lhsVar - rhsVar - Constant, op));
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