using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ConstraintLayoutSample.Behaviors
{
    public class MouseDragElementBehavior : MouseDragHandlerBehavior
    {
        private Transform _cachedRenderTransform;

        private Rect ElementBounds
        {
            get
            {
                // TODO: this one is weird
                Rect layoutRect = GetLayoutRect(AssociatedObject);
                return new Rect(new Point(0, 0), new Size(layoutRect.Width, layoutRect.Height));
            }
        }

        private FrameworkElement ParentElement => AssociatedObject.Parent as FrameworkElement;

        //private Point ActualPosition
        //{
        //    get
        //    {
        //        GeneralTransform elementToRoot = AssociatedObject.TransformToVisual(RootElement);
        //        Point translation = GetTransformOffset(elementToRoot);
        //        return new Point(translation.X, translation.Y);
        //    }
        //}

        private Transform RenderTransform
        {
            get
            {
                if (_cachedRenderTransform == null || !ReferenceEquals(_cachedRenderTransform, AssociatedObject.RenderTransform))
                {
                    Transform clonedTransform = AssociatedObject.RenderTransform.CloneCurrentValue();
                    RenderTransform = clonedTransform;
                }
                return _cachedRenderTransform;
            }
            set
            {
                if (_cachedRenderTransform != value)
                {
                    _cachedRenderTransform = value;
                    AssociatedObject.RenderTransform = value;
                }
            }
        }

        protected override void ApplyTranslation(double x, double y)
        {
            if (ParentElement != null)
            {
                GeneralTransform rootToParent = GetRootElement().TransformToVisual(ParentElement);
                Point transformedPoint = TransformAsVector(rootToParent, x, y);
                x = transformedPoint.X;
                y = transformedPoint.Y;

                if (ConstrainToParentBounds)
                {
                    FrameworkElement parentElement = ParentElement;
                    Rect parentBounds = new Rect(0, 0, parentElement.ActualWidth, parentElement.ActualHeight);

                    GeneralTransform objectToParent = AssociatedObject.TransformToVisual(parentElement);
                    Rect objectBoundingBox = ElementBounds;
                    objectBoundingBox = objectToParent.TransformBounds(objectBoundingBox);

                    Rect endPosition = objectBoundingBox;
                    endPosition.X += x;
                    endPosition.Y += y;

                    if (!RectContainsRect(parentBounds, endPosition))
                    {
                        if (endPosition.X < parentBounds.Left)
                        {
                            double diff = endPosition.X - parentBounds.Left;
                            x -= diff;
                        }
                        else if (endPosition.Right > parentBounds.Right)
                        {
                            double diff = endPosition.Right - parentBounds.Right;
                            x -= diff;
                        }

                        if (endPosition.Y < parentBounds.Top)
                        {
                            double diff = endPosition.Y - parentBounds.Top;
                            y -= diff;
                        }
                        else if (endPosition.Bottom > parentBounds.Bottom)
                        {
                            double diff = endPosition.Bottom - parentBounds.Bottom;
                            y -= diff;
                        }
                    }
                }

                ApplyTranslationTransform(x, y);
            }
        }

        internal void ApplyTranslationTransform(double x, double y)
        {
            Transform renderTransform = RenderTransform;
            // todo jekelly: what if its frozen?
            TranslateTransform translateTransform = renderTransform as TranslateTransform;

            if (translateTransform == null)
            {
                TransformGroup renderTransformGroup = renderTransform as TransformGroup;
                MatrixTransform renderMatrixTransform = renderTransform as MatrixTransform;
                if (renderTransformGroup != null)
                {
                    if (renderTransformGroup.Children.Count > 0)
                    {
                        translateTransform = renderTransformGroup.Children[renderTransformGroup.Children.Count - 1] as TranslateTransform;
                    }
                    if (translateTransform == null)
                    {
                        translateTransform = new TranslateTransform();
                        renderTransformGroup.Children.Add(translateTransform);
                    }
                }
                else if (renderMatrixTransform != null)
                {
                    Matrix matrix = renderMatrixTransform.Matrix;
                    matrix.OffsetX += x;
                    matrix.OffsetY += y;
                    MatrixTransform matrixTransform = new MatrixTransform();
                    matrixTransform.Matrix = matrix;
                    RenderTransform = matrixTransform;
                    return;
                }
                else
                {
                    TransformGroup transformGroup = new TransformGroup();
                    translateTransform = new TranslateTransform();
                    // this will break multi-step animations that target the render transform
                    if (renderTransform != null)
                    {
                        transformGroup.Children.Add(renderTransform);
                    }
                    transformGroup.Children.Add(translateTransform);
                    RenderTransform = transformGroup;
                }
            }

            Debug.Assert(translateTransform != null, "TranslateTransform should not be null by this point.");
            translateTransform.X += x;
            translateTransform.Y += y;
        }

        private static bool RectContainsRect(Rect rect1, Rect rect2)
        {
            return !rect1.IsEmpty && 
                   !rect2.IsEmpty && 
                   rect1.X <= rect2.X && 
                   rect1.Y <= rect2.Y && 
                   rect1.X + rect1.Width >= rect2.X + rect2.Width && 
                   rect1.Y + rect1.Height >= rect2.Y + rect2.Height;
        }

        internal static Rect GetLayoutRect(FrameworkElement element)
        {
            double actualWidth = element.ActualWidth;
            double actualHeight = element.ActualHeight;

            // Use RenderSize here because that works for SL Image and MediaElement - the other uses fo ActualWidth/Height are correct even for these element types
            if (element is Image || element is MediaElement)
            {
                if (element.Parent is Canvas)
                {
                    actualWidth = double.IsNaN(element.Width) ? actualWidth : element.Width;
                    actualHeight = double.IsNaN(element.Height) ? actualHeight : element.Height;
                }
                else
                {
                    actualWidth = element.RenderSize.Width;
                    actualHeight = element.RenderSize.Height;
                }
            }

            actualWidth = element.Visibility == Visibility.Collapsed ? 0 : actualWidth;
            actualHeight = element.Visibility == Visibility.Collapsed ? 0 : actualHeight;
            Thickness margin = element.Margin;

            Rect slotRect = LayoutInformation.GetLayoutSlot(element);

            double left = 0.0;
            double top = 0.0;

            switch (element.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    left = slotRect.Left + margin.Left;
                    break;

                case HorizontalAlignment.Center:
                    left = (slotRect.Left + margin.Left + slotRect.Right - margin.Right) / 2.0 - actualWidth / 2.0;
                    break;

                case HorizontalAlignment.Right:
                    left = slotRect.Right - margin.Right - actualWidth;
                    break;

                case HorizontalAlignment.Stretch:
                    left = Math.Max(
                        slotRect.Left + margin.Left,
                        (slotRect.Left + margin.Left + slotRect.Right - margin.Right) / 2.0 - actualWidth / 2.0);
                    break;
            }

            switch (element.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    top = slotRect.Top + margin.Top;
                    break;

                case VerticalAlignment.Center:
                    top = (slotRect.Top + margin.Top + slotRect.Bottom - margin.Bottom) / 2.0 - actualHeight / 2.0;
                    break;

                case VerticalAlignment.Bottom:
                    top = slotRect.Bottom - margin.Bottom - actualHeight;
                    break;

                case VerticalAlignment.Stretch:
                    top = Math.Max(slotRect.Top + margin.Top, (slotRect.Top + margin.Top + slotRect.Bottom - margin.Bottom) / 2.0 - actualHeight / 2.0);
                    break;
            }

            return new Rect(left, top, actualWidth, actualHeight);
        }
    }
}