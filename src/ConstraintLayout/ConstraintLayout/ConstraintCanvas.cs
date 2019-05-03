using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Kiwi;

namespace ConstraintLayout
{

    public class ConstraintCanvas : Canvas
    {
        private static readonly DependencyPropertyKey ElementVarsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "ElementVars", 
            typeof(ElementVars), 
            typeof(ConstraintCanvas), 
            new PropertyMetadata(default(ElementVars)));
        internal static readonly DependencyProperty ElementVarsProperty = ElementVarsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty UseCanvasLayoutProperty = DependencyProperty.Register(
            "UseCanvasLayout",
            typeof(bool),
            typeof(ConstraintCanvas),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange));

        private int _childIdCounter;


        public ConstraintCanvas()
        {
            ConstraintDefinitions = new ConstraintDefinitionCollection();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ConstraintDefinitionCollection ConstraintDefinitions { get; }

        public bool UseCanvasLayout
        {
            get => (bool)GetValue(UseCanvasLayoutProperty);
            set => SetValue(UseCanvasLayoutProperty, value);
        }

        internal static ElementVars GetElementVars(DependencyObject element) => (ElementVars)element.GetValue(ElementVarsProperty);
        private static void SetElementVars(DependencyObject element, ElementVars value) => element.SetValue(ElementVarsProperty, value);

        
        #region Logical/Visual Tree

        internal new void AddLogicalChild(object child)
        {
            base.AddLogicalChild(child);
        }

        internal new void RemoveLogicalChild(object child)
        {
            base.RemoveLogicalChild(child);
        }


        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            visualAdded?.SetValue(ElementVarsPropertyKey, new ElementVars(_childIdCounter++));
            visualRemoved?.ClearValue(ElementVarsPropertyKey);
        }

        #endregion

        protected override Size MeasureOverride(Size constraint)
        {
            var availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (UIElement child in InternalChildren)
            {
                child?.Measure(availableSize);
            }
            return new Size();
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var solver = new Solver();

            foreach (UIElement child in InternalChildren)
            {
                var vars = GetElementVars(child);

                AddCommonConstraints(solver, vars, arrangeSize);
                AddCanvasConstraints(solver, vars, arrangeSize, child);

                // add constraints for element measured size
                solver.AddConstraint(vars.Width == child.DesiredSize.Width);
                solver.AddConstraint(vars.Height == child.DesiredSize.Height);
            }

            foreach (var constraintDefinition in ConstraintDefinitions)
            {
                constraintDefinition.ProvideConstraints(solver);
            }

            solver.UpdateVariables();

            foreach (UIElement child in InternalChildren)
            {
                if (UseCanvasLayout)
                {
                    ArrangeCanvasElement(arrangeSize, child);
                }
                else
                {
                    var vars = GetElementVars(child);

                    var left = vars.Left.Value;
                    var top = vars.Top.Value;
                    var width = vars.Width.Value;
                    var height = vars.Height.Value;

                    child.Arrange(new Rect(left, top, width, height));
                }
            }

            return arrangeSize;
        }

        private static void AddCommonConstraints(Solver solver, ElementVars vars, Size arrangeSize)
        {
            // weak constraints to keep element within panel bounds
            solver.AddConstraint(vars.Left >= 0 | Strength.Weak);
            solver.AddConstraint(vars.Top >= 0 | Strength.Weak);
            solver.AddConstraint(vars.Right <= arrangeSize.Width | Strength.Weak);
            solver.AddConstraint(vars.Bottom <= arrangeSize.Height | Strength.Weak);

            // strong constraints to make width and height non-negative
            solver.AddConstraint(vars.Width >= 0);
            solver.AddConstraint(vars.Height >= 0);

            // strong constraints to set relations for width/left/center/right and height/top/middle/bottom
            solver.AddConstraint(vars.Left + vars.Width == vars.Right);
            solver.AddConstraint(vars.Left + vars.Width / 2 == vars.Center);
            solver.AddConstraint(vars.Top + vars.Height == vars.Bottom);
            solver.AddConstraint(vars.Top + vars.Height / 2 == vars.Middle);
        }


        private static void AddCanvasConstraints(Solver solver, ElementVars vars, Size arrangeSize, UIElement child)
        {
            var leftValue = Canvas.GetLeft(child);
            var topValue = Canvas.GetTop(child);
            var rightValue = arrangeSize.Width - Canvas.GetRight(child);
            var bottomValue = arrangeSize.Height - Canvas.GetBottom(child);

            if (!double.IsNaN(leftValue))
            {
                solver.AddConstraint(vars.Left == leftValue | Strength.Medium);
            }
            else if (!double.IsNaN(rightValue))
            {
                solver.AddConstraint(vars.Right == rightValue | Strength.Medium);
            }
            else
            {
                solver.AddConstraint(vars.Left == 0 | Strength.Medium);
            }

            if (!double.IsNaN(topValue))
            {
                solver.AddConstraint(vars.Top == topValue | Strength.Medium);
            }
            else if (!double.IsNaN(bottomValue))
            {
                solver.AddConstraint(vars.Bottom == bottomValue | Strength.Medium);
            }
            else
            {
                solver.AddConstraint(vars.Top == 0 | Strength.Medium);
            }
        }

        private static void ArrangeCanvasElement(Size arrangeSize, UIElement child)
        {
            var x = 0.0;
            var y = 0.0;
            double left = GetLeft(child);
            if (!double.IsNaN(left))
            {
                x = left;
            }
            else
            {
                double right = GetRight(child);
                if (!double.IsNaN(right))
                {
                    x = arrangeSize.Width - child.DesiredSize.Width - right;
                }
            }

            double top = GetTop(child);
            if (!double.IsNaN(top))
            {
                y = top;
            }
            else
            {
                double bottom = GetBottom(child);
                if (!double.IsNaN(bottom))
                    y = arrangeSize.Height - child.DesiredSize.Height - bottom;
            }

            child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
        }
    }
}