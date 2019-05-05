using System.Windows;

namespace ConstraintLayout
{
    public static class Constraint
    {
        public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached(
            "Left", 
            typeof(double), 
            typeof(Constraint), 
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached(
            "Top", 
            typeof(double), 
            typeof(Constraint), 
            new PropertyMetadata(default(double)));


        public static void SetLeft(DependencyObject element, double value)
        {
            element.SetValue(LeftProperty, value);
        }

        public static double GetLeft(DependencyObject element)
        {
            return (double) element.GetValue(LeftProperty);
        }


        public static void SetTop(DependencyObject element, double value)
        {
            element.SetValue(TopProperty, value);
        }

        public static double GetTop(DependencyObject element)
        {
            return (double) element.GetValue(TopProperty);
        }
    }
}