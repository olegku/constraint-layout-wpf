using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ConstraintLayoutSample.Controls
{
    public class LayoutInfo
    {
        private static readonly HashSet<FrameworkElement> TrackedElements = new HashSet<FrameworkElement>();
        private static readonly FrameworkElement LayoutUpdateSubscriptionTargetElement = new FrameworkElement
        {
            Name = nameof(LayoutUpdateSubscriptionTargetElement)
        };

        #region Dependency Properties

        private static readonly DependencyPropertyKey LayoutSlotPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "LayoutSlot", 
            typeof(Rect), 
            typeof(LayoutInfo), 
            new PropertyMetadata(default(Rect)));

        public static readonly DependencyProperty LayoutSlotProperty = LayoutSlotPropertyKey.DependencyProperty;

        public static readonly DependencyProperty TrackLayoutSlotProperty = DependencyProperty.RegisterAttached(
            "TrackLayoutSlot",
            typeof(bool),
            typeof(LayoutInfo),
            new PropertyMetadata(false, TrackLayoutSlotChanged));

        #endregion

        static LayoutInfo()
        {
            LayoutUpdateSubscriptionTargetElement.LayoutUpdated += LayoutUpdated;
        }

        public static Rect GetLayoutSlot(DependencyObject element) => (Rect) element.GetValue(LayoutSlotProperty);
        private static void SetLayoutSlot(DependencyObject element, Rect value) => element.SetValue(LayoutSlotPropertyKey, value);

        public static bool GetTrackLayoutSlot(DependencyObject element) => (bool)element.GetValue(TrackLayoutSlotProperty);
        public static void SetTrackLayoutSlot(DependencyObject element, bool value) => element.SetValue(TrackLayoutSlotProperty, value);

        private static void TrackLayoutSlotChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if ((bool)e.NewValue)
                {
                    TrackedElements.Add(element);
                }
                else
                {
                    TrackedElements.Remove(element);
                }
            }
        }

        private static void LayoutUpdated(object sender, EventArgs e)
        {
            foreach (var element in TrackedElements)
            {
                SetLayoutSlot(element, LayoutInformation.GetLayoutSlot(element));
            }
        }
    }
}
