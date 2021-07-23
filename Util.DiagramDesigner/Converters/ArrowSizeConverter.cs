using System;
using System.Windows.Data;

namespace Util.DiagramDesigner
{
    public class ArrowSizeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ArrowSizeStyle arrowStyle)
            {
                return (int)arrowStyle;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
