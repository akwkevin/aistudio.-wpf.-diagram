using AIStudio.Wpf.ADiagram.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Converters
{
    public class RulerUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is PageUnit pageunit)
            {
                if (pageunit > PageUnit.km)
                {
                    return Unit.Inch;
                }
                else
                {
                    return Unit.Cm;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
