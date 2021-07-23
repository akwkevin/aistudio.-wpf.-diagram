using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Util.DiagramDesigner
{
    public class ConectorValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length < 1)
            {
                throw new NotImplementedException();
            }
            if (values[0] is double && values[1] is ValueTypePoint)
            {
                double connectorValue = (double)values[0];
                ValueTypePoint valueTypePoint = (ValueTypePoint)values[1];                

                if (valueTypePoint == ValueTypePoint.Bool)
                {
                    return (connectorValue == 0) ? "F" : "T";
                }
                else if (valueTypePoint == ValueTypePoint.Int)
                {
                    return connectorValue.ToString("0");
                }
                else
                {
                    return connectorValue.ToString("f3");
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
