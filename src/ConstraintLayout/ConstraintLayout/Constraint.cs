using System.Windows;

namespace ConstraintLayout
{
    // TODO: add MiddleProperty and CenterProperty
    public static class Constraint
    {
        public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached(
            "Left", 
            typeof(PropertyConstraint), 
            typeof(Constraint), 
            new FrameworkPropertyMetadata(default(PropertyConstraint), FrameworkPropertyMetadataOptions.AffectsParentArrange));

        public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached(
            "Top", 
            typeof(PropertyConstraint), 
            typeof(Constraint),
            new FrameworkPropertyMetadata(default(PropertyConstraint), FrameworkPropertyMetadataOptions.AffectsParentArrange));

        public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached(
            "Right", 
            typeof(PropertyConstraint), 
            typeof(Constraint),
            new FrameworkPropertyMetadata(default(PropertyConstraint), FrameworkPropertyMetadataOptions.AffectsParentArrange));

        public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached(
            "Bottom", 
            typeof(PropertyConstraint), 
            typeof(Constraint),
            new FrameworkPropertyMetadata(default(PropertyConstraint), FrameworkPropertyMetadataOptions.AffectsParentArrange));

        public static PropertyConstraint GetLeft(DependencyObject element) => (PropertyConstraint) element.GetValue(LeftProperty);
        public static void SetLeft(DependencyObject element, PropertyConstraint value) => element.SetValue(LeftProperty, value);

        public static PropertyConstraint GetTop(DependencyObject element) => (PropertyConstraint) element.GetValue(TopProperty);
        public static void SetTop(DependencyObject element, PropertyConstraint value) => element.SetValue(TopProperty, value);

        public static PropertyConstraint GetRight(DependencyObject element) => (PropertyConstraint) element.GetValue(RightProperty);
        public static void SetRight(DependencyObject element, PropertyConstraint value) => element.SetValue(RightProperty, value);

        public static PropertyConstraint GetBottom(DependencyObject element) => (PropertyConstraint) element.GetValue(BottomProperty);
        public static void SetBottom(DependencyObject element, PropertyConstraint value) => element.SetValue(BottomProperty, value);
    }
}