using System;
using System.Windows;
using Kiwi;

namespace ConstraintLayout
{
    public abstract class ConstraintDefinition : Freezable
    {
        public abstract void ProvideConstraints(Solver solver);
    }


    public class ConstraintDefinitionCollection : FreezableCollection<ConstraintDefinition>
    {
    }


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

        public static readonly DependencyProperty ConstantProperty = DependencyProperty.Register(
            "Constant",
            typeof(double),
            typeof(SimpleConstraint),
            new PropertyMetadata(0.0, ConstantPropertyChangedCallback));

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

        public double Constant
        {
            get => (double)GetValue(ConstantProperty);
            set => SetValue(ConstantProperty, value);
        }

        public ConstraintRelation Relation
        {
            get => (ConstraintRelation)GetValue(RelationProperty);
            set => SetValue(RelationProperty, value);
        }
        
        private static void ConstantPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //TODO: Parent.Invalidate();
        }

        protected override Freezable CreateInstanceCore() => new SimpleConstraint();

        public override void ProvideConstraints(Solver solver)
        {
            var vars1 = ConstraintCanvas.GetElementVars(Element1);
            var vars2 = ConstraintCanvas.GetElementVars(Element2);
            if (vars1 == null || vars2 == null) return;

            var var1 = vars1[Property1];
            var var2 = vars2[Property2];
            if (ReferenceEquals(var1, null) || ReferenceEquals(var2, null)) return;

            solver.AddConstraint(new Constraint(var1 - var2 + Constant, GetRelationalOperator(Relation)));
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
