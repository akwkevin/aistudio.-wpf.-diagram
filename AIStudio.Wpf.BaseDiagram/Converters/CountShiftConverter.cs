using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AIStudio.Wpf.BaseDiagram.Controls;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.BaseDiagram.Converters
{
    public class CountShiftConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 2)
            {
                var diagram = values[0] as Util.DiagramDesigner.DiagramControl;
                var zoomValue = (double)values[1];
                var pageunit = (PageUnit)values[2];
                var unit = Unit.Cm;
                if (pageunit > PageUnit.km)
                {
                    unit = Unit.Inch;
                }
                Vector vector = System.Windows.Media.VisualTreeHelper.GetOffset(diagram);

                if (parameter?.ToString() == "Y")
                {
                    var value = 0 - (unit == Unit.Cm? DipHelper.DipToCm(vector.Y - 20) : DipHelper.DipToInch(vector.Y - 20))/ zoomValue;
                    return value;
                }
                else
                {
                    var value = 0 - (unit == Unit.Cm ? DipHelper.DipToCm(vector.X - 20) : DipHelper.DipToInch(vector.X - 20)) / zoomValue;
                    return value;
                }
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
