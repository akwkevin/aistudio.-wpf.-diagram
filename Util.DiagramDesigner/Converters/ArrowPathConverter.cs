using System;
using System.Windows.Data;

namespace Util.DiagramDesigner
{

    public class ArrowPathConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ArrowPathStyle arrowStyle)
            {
                return ArrowPathData.Arrow[(int)arrowStyle];              
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
