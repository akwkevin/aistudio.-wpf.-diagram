using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AIStudio.Wpf.ADiagram.Converters
{
    public class BoolVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = value as bool?;
            var visible = parameter as string;

            if (visibility != null && visible != null)
            {
                if (visibility == true && visible == "true")
                {
                    return Visibility.Visible;
                }
                if (visibility == false && visible == "false")
                {
                    return Visibility.Visible;
                }
            }
            else if (visibility != null)
            {
                if (visibility == true)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}
