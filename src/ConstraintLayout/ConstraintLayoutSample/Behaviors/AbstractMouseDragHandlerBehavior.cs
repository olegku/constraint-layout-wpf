using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace ConstraintLayoutSample.Behaviors
{
    public abstract class AbstractMouseDragHandlerBehavior : Behavior<FrameworkElement>
    {
        private bool _settingPosition;
        private Point _relativePosition;

        #region Dependency Properties

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", 
            typeof(double),
            typeof(AbstractMouseDragHandlerBehavior),
            new PropertyMetadata(double.NaN, OnXChanged));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", 
            typeof(double),
            typeof(AbstractMouseDragHandlerBehavior),
            new PropertyMetadata(double.NaN, OnYChanged));

        public static readonly DependencyProperty ConstrainToParentBoundsProperty = DependencyProperty.Register(
            "ConstrainToParentBounds", 
            typeof(bool), 
            typeof(AbstractMouseDragHandlerBehavior), 
            new PropertyMetadata(false, OnConstrainToParentBoundsChanged));

        #endregion

        #region Events

        public event MouseEventHandler DragStarted;
        public event MouseEventHandler Dragged;
        public event MouseEventHandler DragEnded;

        #endregion

        #region Properties

        public double X
        {
            get => (double)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public double Y
        {
            get => (double)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public bool ConstrainToParentBounds
        {
            get => (bool)GetValue(ConstrainToParentBoundsProperty);
            set => SetValue(ConstrainToParentBoundsProperty, value);
        }

        #endregion

        #region Property Changed Handlers

        private static void OnXChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var dragBehavior = (AbstractMouseDragHandlerBehavior)sender;
            dragBehavior.UpdatePosition(new Point((double)args.NewValue, dragBehavior.Y));
        }

        private static void OnYChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var dragBehavior = (AbstractMouseDragHandlerBehavior)sender;
            dragBehavior.UpdatePosition(new Point(dragBehavior.X, (double)args.NewValue));
        }

        private static void OnConstrainToParentBoundsChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var dragBehavior = (AbstractMouseDragHandlerBehavior)sender;
            dragBehavior.UpdatePosition(new Point(dragBehavior.X, dragBehavior.Y));
        }

        #endregion

        protected UIElement GetRootElement()
        {
            DependencyObject child = AssociatedObject;
            DependencyObject parent = child;
            while (parent != null)
            {
                child = parent;
                parent = VisualTreeHelper.GetParent(child);
            }

            return child as UIElement;
        }

        private void UpdatePosition(Point point)
        {
            if (!_settingPosition && AssociatedObject != null)
            {
                GeneralTransform elementToRoot = AssociatedObject.TransformToVisual(GetRootElement());
                Point translation = GetTransformOffset(elementToRoot);
                double xChange = double.IsNaN(point.X) ? 0 : point.X - translation.X;
                double yChange = double.IsNaN(point.Y) ? 0 : point.Y - translation.Y;
                ApplyTranslation(xChange, yChange);
            }
        }

        protected abstract void ApplyTranslation(double x, double y);

        private void UpdatePosition()
        {
            GeneralTransform elementToRoot = AssociatedObject.TransformToVisual(GetRootElement());
            Point translation = GetTransformOffset(elementToRoot);
            X = translation.X;
            Y = translation.Y;
        }

        protected override void OnAttached()
        {
            AssociatedObject.AddHandler(
                UIElement.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler(OnMouseLeftButtonDown),
                handledEventsToo: false);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.RemoveHandler(
                UIElement.MouseLeftButtonDownEvent, 
                new MouseButtonEventHandler(OnMouseLeftButtonDown));
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var positionInElementCoordinates = e.GetPosition(AssociatedObject);
            StartDrag(positionInElementCoordinates);
            OnDragStarted(e);
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AssociatedObject.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var newPositionInElementCoordinates = e.GetPosition(AssociatedObject);
            Drag(newPositionInElementCoordinates);
            OnDrag(e);
        }

        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            EndDrag();
            OnDragEnded(e);
        }

        private void OnDragStarted(MouseButtonEventArgs e)
        {
            DragStarted?.Invoke(this, e);
        }
        
        private void OnDrag(MouseEventArgs e)
        {
            Dragged?.Invoke(this, e);
        }

        private void OnDragEnded(MouseEventArgs e)
        {
            DragEnded?.Invoke(this, e);
        }

        internal void StartDrag(Point positionInElementCoordinates)
        {
            _relativePosition = positionInElementCoordinates;

            AssociatedObject.CaptureMouse();

            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.LostMouseCapture += OnLostMouseCapture;
            AssociatedObject.AddHandler(
                UIElement.MouseLeftButtonUpEvent,
                new MouseButtonEventHandler(OnMouseLeftButtonUp),
                handledEventsToo: false);
        }

        internal void Drag(Point newPositionInElementCoordinates)
        {
            var relativeXDiff = newPositionInElementCoordinates.X - _relativePosition.X;
            var relativeYDiff = newPositionInElementCoordinates.Y - _relativePosition.Y;

            var elementToRoot = AssociatedObject.TransformToVisual(GetRootElement());
            var relativeDifferenceInRootCoordinates = TransformAsVector(elementToRoot, relativeXDiff, relativeYDiff);

            _settingPosition = true;
            ApplyTranslation(relativeDifferenceInRootCoordinates.X, relativeDifferenceInRootCoordinates.Y);
            UpdatePosition();
            _settingPosition = false;
        }

        internal void EndDrag()
        {
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.LostMouseCapture -= OnLostMouseCapture;
            AssociatedObject.RemoveHandler(
                UIElement.MouseLeftButtonUpEvent,
                new MouseButtonEventHandler(OnMouseLeftButtonUp));
        }
        
        private static Point GetTransformOffset(GeneralTransform transform)
        {
            return transform.Transform(new Point(0, 0));
        }

        protected static Point TransformAsVector(GeneralTransform transform, double x, double y)
        {
            var origin = transform.Transform(new Point(0, 0));
            var transformedPoint = transform.Transform(new Point(x, y));
            return new Point(transformedPoint.X - origin.X, transformedPoint.Y - origin.Y);
        }
    }
}
