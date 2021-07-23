using System;
using System.Windows.Data;

namespace Util.DiagramDesigner
{

    public class LineDashConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is LineDashStyle arrowStyle)
            {
                return StrokeDashArray.Dash[(int)arrowStyle];
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
