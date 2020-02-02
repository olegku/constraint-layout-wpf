using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Kiwi;
using MoreLinq.Extensions;

namespace ConstraintLayout.Constraints
{
    [ContentProperty(nameof(Items))]
    public class StackConstraint : ConstraintDefinition
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", 
            typeof(Orientation), 
            typeof(StackConstraint), 
            new FrameworkPropertyMetadata(
                Orientation.Vertical, 
                FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
            "Spacing", 
            typeof(double), 
            typeof(StackConstraint), 
            new FrameworkPropertyMetadata(
                default(double), 
                FrameworkPropertyMetadataOptions.AffectsArrange));

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(
            "Items",
            typeof(StackConstraintItemCollection),
            typeof(StackConstraint),
            new PropertyMetadata(default(StackConstraintItemCollection)));
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public StackConstraint()
        {
            SetValue(ItemsPropertyKey, new StackConstraintItemCollection());
        }

        #region Properties

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public double Spacing
        {
            get => (double)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public StackConstraintItemCollection Items => (StackConstraintItemCollection)GetValue(ItemsProperty);

        #endregion

        protected override Freezable CreateInstanceCore() => new SimpleConstraint();

        public override void ProvideConstraints(Solver solver)
        {
            var items = Items
                .Where(item => item.Element != null)
                .Select(item => new {item.Element, Vars = ConstraintCanvas.GetElementVars(item.Element) })
                .ToList();

            if (items.Count <= 1)
            {
                return;
            }

            items.Pairwise((a, b) => new {Prev = a, Next = b}).ForEach((x, i) =>
            {
                if (Orientation == Orientation.Horizontal)
                {
                    var prevRight = x.Prev.Vars.Right;
                    var nextLeft = x.Next.Vars.Left;
                    solver.AddConstraint(prevRight + Spacing == nextLeft);
                }
                else
                {
                    var prevBottom = x.Prev.Vars.Bottom;
                    var nextTop = x.Next.Vars.Top;
                    solver.AddConstraint(prevBottom + Spacing == nextTop);
                }
            });
        }
    }


    public class StackConstraintItemCollection : FreezableCollection<StackConstraintItem>
    {
    }


    public class StackConstraintItem : Freezable
    {
        public static readonly DependencyProperty ElementProperty = DependencyProperty.Register(
            "Element",
            typeof(UIElement),
            typeof(StackConstraintItem),
            new PropertyMetadata(default(UIElement)));

        public UIElement Element
        {
            get => (UIElement)GetValue(ElementProperty);
            set => SetValue(ElementProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new StackConstraintItem();
    }
}