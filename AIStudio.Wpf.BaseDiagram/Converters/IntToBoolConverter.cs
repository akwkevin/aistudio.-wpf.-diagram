using System;
using System.Windows.Data;

namespace AIStudio.Wpf.BaseDiagram.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (object.Equals(value, 0d))
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (object.Equals(value, false))
                return 0d;
            else
                return 1d;
        }
    }
}
