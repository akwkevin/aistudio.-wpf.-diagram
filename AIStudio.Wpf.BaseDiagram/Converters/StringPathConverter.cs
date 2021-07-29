using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AIStudio.Wpf.BaseDiagram.Converters
{
    public class StringPathConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                Geometry geo = Geometry.Parse(value as string);
                return geo;
                //GeometryConverter gc = new GeometryConverter();
                //return (Geometry)gc.ConvertFromString(value as string);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
