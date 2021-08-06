using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Util.DiagramDesigner
{
    public class ConnectionDataConverter : IMultiValueConverter
    {
        static ConnectionDataConverter()
        {
            Instance = new ConnectionDataConverter();
        }

        public static ConnectionDataConverter Instance
        {
            get;
            private set;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<PointInfoBase> points = (List<PointInfoBase>)values[0];
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = points[0];
            if (values[1]?.ToString() == DrawMode.RadiusConnectingLine.ToString())
            {
                for (var i = 0; i < points.Count - 1; i++)
                {
                    int current = i, last = i - 1, next = i + 1, next2 = i + 2;
                    if (last == -1)
                    {
                        last = 0;
                    }
                    if (next == points.Count)
                    {
                        next = points.Count - 1;
                    }
                    if (next2 == points.Count)
                    {
                        next2 = points.Count - 1;
                    }
                    var bzs = SegmentHelper.GetBezierSegment(points[current], points[last], points[next], points[next2]);
                    figure.Segments.Add(bzs);
                }
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {

                    LineSegment arc = new LineSegment(points[i], true);
                    figure.Segments.Add(arc);
                }
            }

            pathGeometry.Figures.Add(figure);
            return pathGeometry;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
