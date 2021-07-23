using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Util.DiagramDesigner
{
    public class ColorBrushConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush brush = null;
            if (value is Color)
            {
                brush = new SolidColorBrush((Color)value);
            }
            else if (value is ColorObject colorObject)
            {
                if (colorObject.BrushType == BrushType.None)
                    brush = new SolidColorBrush(Colors.Transparent);
                else if (colorObject.BrushType == BrushType.SolidColorBrush)
                    brush = new SolidColorBrush(colorObject.Color);
                else if (colorObject.BrushType == BrushType.LinearGradientBrush)
                {
                    Point startPoint;
                    Point endPoint;
                    if (colorObject.LinearOrientation == LinearOrientation.LeftToRight)
                    {
                        startPoint = new Point(0, 0.5);
                        endPoint = new Point(1, 0.5);
                    }
                    else if (colorObject.LinearOrientation == LinearOrientation.LeftTopToRightBottom)
                    {
                        startPoint = new Point(0, 0);
                        endPoint = new Point(1, 1);
                    }
                    else if (colorObject.LinearOrientation == LinearOrientation.TopToBottom)
                    {
                        startPoint = new Point(0.5, 0);
                        endPoint = new Point(0.5, 1);
                    }
                    else if (colorObject.LinearOrientation == LinearOrientation.RightTopToLeftBottom)
                    {
                        startPoint = new Point(1, 0);
                        endPoint = new Point(0, 1);
                    }
                    else if (colorObject.LinearOrientation == LinearOrientation.RightToLeft)
                    {
                        startPoint = new Point(1, 0.5);
                        endPoint = new Point(0, 0.5);
                    }
                    else if (colorObject.LinearOrientation == LinearOrientation.RightBottomToLeftTop)
                    {
                        startPoint = new Point(1, 1);
                        endPoint = new Point(0, 0);
                    }
                    else if (colorObject.LinearOrientation == LinearOrientation.BottomToTop)
                    {
                        startPoint = new Point(0.5, 1);
                        endPoint = new Point(0.5, 0);
                    }
                    else if (colorObject.LinearOrientation == LinearOrientation.LeftBottomToRightTop)
                    {
                        startPoint = new Point(0, 1);
                        endPoint = new Point(1, 0);
                    }
                    else
                    {
                        startPoint = new Point(0, 0.5);
                        endPoint = new Point(1, 0.5);
                    }

                    LinearGradientBrush myBrush = new LinearGradientBrush();
                    myBrush.StartPoint = startPoint;
                    myBrush.EndPoint = endPoint;
                    if (colorObject.GradientStop != null)
                    {
                        foreach (var stop in colorObject.GradientStop)
                        {
                            myBrush.GradientStops.Add(new System.Windows.Media.GradientStop(stop.Color, stop.Offset));
                        }
                    }
                    brush = myBrush;

                    RotateTransform rotateTransform = new RotateTransform(colorObject.Angle, 0.5, 0.5);
                    myBrush.RelativeTransform = rotateTransform;
                }
                else if (colorObject.BrushType == BrushType.RadialGradientBrush)
                {
                    Point center;
                    Point gradientOrigin;
                    double radiusX;
                    double radiusY;

                    if (colorObject.RadialOrientation == RadialOrientation.LeftTop)
                    {
                        center = new Point(0, 0);
                        gradientOrigin = center;
                        radiusX = 1;
                        radiusY = 1;
                    }
                    else if (colorObject.RadialOrientation == RadialOrientation.RightTop)
                    {
                        center = new Point(1, 0);
                        gradientOrigin = center;
                        radiusX = 1;
                        radiusY = 1;
                    }
                    else if (colorObject.RadialOrientation == RadialOrientation.RightBottom)
                    {
                        center = new Point(1, 1);
                        gradientOrigin = center;
                        radiusX = 1;
                        radiusY = 1;
                    }
                    else if (colorObject.RadialOrientation == RadialOrientation.LeftBottom)
                    {
                        center = new Point(0, 1);
                        gradientOrigin = center;
                        radiusX = 1;
                        radiusY = 1;
                    }
                    else
                    {
                        center = new Point(0.5, 0.5);
                        gradientOrigin = center;
                        radiusX = 0.5;
                        radiusY = 0.5;
                    }

                    RadialGradientBrush myBrush = new RadialGradientBrush();
                    myBrush.Center = center;
                    myBrush.GradientOrigin = gradientOrigin;
                    myBrush.RadiusX = radiusX;
                    myBrush.RadiusY = radiusY;
                    if (colorObject.GradientStop != null)
                    {
                        foreach (var stop in colorObject.GradientStop)
                        {
                            myBrush.GradientStops.Add(new System.Windows.Media.GradientStop(stop.Color, stop.Offset));
                        }
                    }
                    brush = myBrush;

                    RotateTransform rotateTransform = new RotateTransform(colorObject.Angle, 0.5, 0.5);
                    myBrush.RelativeTransform = rotateTransform;
                }
                else if (colorObject.BrushType == BrushType.ImageBrush)
                {
                    ImageBrush myBrush = new ImageBrush();
                    myBrush.ImageSource = new BitmapImage(new Uri(colorObject.Image, UriKind.Absolute));
                    brush = myBrush;
                }
                else if (colorObject.BrushType == BrushType.DrawingBrush)
                {
                    DrawingBrush myBrush = new DrawingBrush();

                    GeometryDrawing backgroundSquare =
                        new GeometryDrawing(
                            Brushes.White,
                            null,
                            new RectangleGeometry(new Rect(0, 0, 100, 100)));

                    GeometryGroup aGeometryGroup = new GeometryGroup();
                    aGeometryGroup.Children.Add(new RectangleGeometry(new Rect(0, 0, 50, 50)));
                    aGeometryGroup.Children.Add(new RectangleGeometry(new Rect(50, 50, 50, 50)));

                    LinearGradientBrush checkerBrush = new LinearGradientBrush();
                    checkerBrush.GradientStops.Add(new System.Windows.Media.GradientStop(Colors.Black, 0.0));
                    checkerBrush.GradientStops.Add(new System.Windows.Media.GradientStop(Colors.Gray, 1.0));

                    GeometryDrawing checkers = new GeometryDrawing(checkerBrush, null, aGeometryGroup);

                    DrawingGroup checkersDrawingGroup = new DrawingGroup();
                    checkersDrawingGroup.Children.Add(backgroundSquare);
                    checkersDrawingGroup.Children.Add(checkers);

                    myBrush.Drawing = checkersDrawingGroup;
                    myBrush.Viewport = new Rect(0, 0, 0.25, 0.25);
                    myBrush.TileMode = TileMode.Tile;

                    brush = myBrush;
                }
                if (brush != null)
                {
                    brush.Opacity = colorObject.Opacity;
                }
            }
            else if (value is ObservableCollection<GradientStop> gradientStop)
            {
                LinearGradientBrush myBrush = new LinearGradientBrush();
                myBrush.StartPoint = new Point(0, 0.5);
                myBrush.EndPoint = new Point(1, 0.5);
                if (gradientStop != null)
                {
                    foreach (var stop in gradientStop)
                    {
                        myBrush.GradientStops.Add(new System.Windows.Media.GradientStop(stop.Color, stop.Offset));
                    }
                }
                brush = myBrush;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
