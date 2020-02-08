using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ConstraintLayoutSample.Converters
{
    public class RectPointConverter : IValueConverter
    {
        public Point RelativePosition { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Rect rect)
            {
                return rect.Location + new Vector(
                           RelativePosition.X * rect.Width,
                           RelativePosition.Y * rect.Height);
            }

            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}