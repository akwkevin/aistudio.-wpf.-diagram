using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public class ClipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string para = parameter?.ToString();
            try
            {
                if (value is ImageItemViewModel imageItemViewModel)
                {
                    double xradio = imageItemViewModel.ItemWidth / imageItemViewModel.ImageWidth;
                    double yradio = imageItemViewModel.ItemHeight / imageItemViewModel.ImageHeight;
                    if (para == "Clip")
                    {
                        if (imageItemViewModel.ClipMode == ClipMode.RectangleGeometry)
                        {
                            RectangleGeometry rectangle = new RectangleGeometry();
                            if (imageItemViewModel.ResizeMode == false)
                            {
                                rectangle.Rect = new System.Windows.Rect(imageItemViewModel.ResizeMargin.Left * xradio, imageItemViewModel.ResizeMargin.Top * yradio, imageItemViewModel.ItemWidth, imageItemViewModel.ItemHeight);
                            }
                            else
                            {
                                rectangle.Rect = new System.Windows.Rect(imageItemViewModel.ResizeMargin.Left, imageItemViewModel.ResizeMargin.Top , imageItemViewModel.ItemWidth - (imageItemViewModel.ResizeMargin.Left + imageItemViewModel.ResizeMargin.Right), imageItemViewModel.ItemHeight - (imageItemViewModel.ResizeMargin.Top + imageItemViewModel.ResizeMargin.Bottom));
                            }
                            return rectangle;
                        }
                        else if (imageItemViewModel.ClipMode == ClipMode.EllipseGeometry)
                        {
                            EllipseGeometry ellipse = new EllipseGeometry();
                            if (imageItemViewModel.ResizeMode == false)
                            {                               
                                ellipse.Center = new Point(imageItemViewModel.ResizeMargin.Left * xradio + imageItemViewModel.ItemWidth / 2, imageItemViewModel.ResizeMargin.Top * yradio + imageItemViewModel.ItemHeight / 2);
                                ellipse.RadiusX = imageItemViewModel.ItemWidth / 2;
                                ellipse.RadiusY = imageItemViewModel.ItemHeight / 2;
                            }
                            else
                            {
                                ellipse.Center = new Point(imageItemViewModel.ResizeMargin.Left + (imageItemViewModel.ItemWidth - imageItemViewModel.ResizeMargin.Left - imageItemViewModel.ResizeMargin.Right) / 2, imageItemViewModel.ResizeMargin.Top + (imageItemViewModel.ItemHeight - imageItemViewModel.ResizeMargin.Top  - imageItemViewModel.ResizeMargin.Bottom)/ 2);
                                ellipse.RadiusX = (imageItemViewModel.ItemWidth - imageItemViewModel.ResizeMargin.Left - imageItemViewModel.ResizeMargin.Right) / 2;
                                ellipse.RadiusY = (imageItemViewModel.ItemHeight - imageItemViewModel.ResizeMargin.Top - imageItemViewModel.ResizeMargin.Bottom) / 2;
                            }
                            return ellipse;
                        }
                    }
                    else if (para == "Margin")
                    {
                        if (imageItemViewModel.ResizeMode == false)
                        {
                            return new Thickness(-imageItemViewModel.ResizeMargin.Left * xradio, -imageItemViewModel.ResizeMargin.Top * yradio, -imageItemViewModel.ResizeMargin.Right * xradio, -imageItemViewModel.ResizeMargin.Bottom * yradio);
                        }
                        else
                        {
                            return new Thickness();
                        }
                    }
                }
            }
            catch { }

            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
