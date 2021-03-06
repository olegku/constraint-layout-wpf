﻿using System.Windows;
using ConstraintLayout.PropertyConstraints;

namespace ConstraintLayout
{
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

        public static readonly DependencyProperty CenterProperty = DependencyProperty.RegisterAttached(
            "Center", 
            typeof(PropertyConstraint), 
            typeof(Constraint),
            new FrameworkPropertyMetadata(default(PropertyConstraint), FrameworkPropertyMetadataOptions.AffectsParentArrange));

        public static readonly DependencyProperty MiddleProperty = DependencyProperty.RegisterAttached(
            "Middle", 
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

        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
            "Width", 
            typeof(PropertyConstraint), 
            typeof(Constraint),
            new FrameworkPropertyMetadata(default(PropertyConstraint), FrameworkPropertyMetadataOptions.AffectsParentArrange));

        public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
            "Height", 
            typeof(PropertyConstraint), 
            typeof(Constraint),
            new FrameworkPropertyMetadata(default(PropertyConstraint), FrameworkPropertyMetadataOptions.AffectsParentArrange));


        public static PropertyConstraint GetLeft(DependencyObject element) => (PropertyConstraint) element.GetValue(LeftProperty);
        public static void SetLeft(DependencyObject element, PropertyConstraint value) => element.SetValue(LeftProperty, value);

        public static PropertyConstraint GetTop(DependencyObject element) => (PropertyConstraint) element.GetValue(TopProperty);
        public static void SetTop(DependencyObject element, PropertyConstraint value) => element.SetValue(TopProperty, value);

        public static void SetCenter(DependencyObject element, PropertyConstraint value) => element.SetValue(CenterProperty, value);
        public static PropertyConstraint GetCenter(DependencyObject element) => (PropertyConstraint)element.GetValue(CenterProperty);

        public static void SetMiddle(DependencyObject element, PropertyConstraint value) => element.SetValue(MiddleProperty, value);
        public static PropertyConstraint GetMiddle(DependencyObject element) => (PropertyConstraint)element.GetValue(MiddleProperty);

        public static PropertyConstraint GetRight(DependencyObject element) => (PropertyConstraint) element.GetValue(RightProperty);
        public static void SetRight(DependencyObject element, PropertyConstraint value) => element.SetValue(RightProperty, value);

        public static PropertyConstraint GetBottom(DependencyObject element) => (PropertyConstraint) element.GetValue(BottomProperty);
        public static void SetBottom(DependencyObject element, PropertyConstraint value) => element.SetValue(BottomProperty, value);

        public static void SetWidth(DependencyObject element, PropertyConstraint value) => element.SetValue(WidthProperty, value);
        public static PropertyConstraint GetWidth(DependencyObject element) => (PropertyConstraint)element.GetValue(WidthProperty);

        public static void SetHeight(DependencyObject element, PropertyConstraint value) => element.SetValue(HeightProperty, value);
        public static PropertyConstraint GetHeight(DependencyObject element) => (PropertyConstraint)element.GetValue(HeightProperty);
    }
}