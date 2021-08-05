using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public class PointInfoBase : BindableBase
    {
        public PointInfoBase()
        {
            ColorViewModel = new ColorViewModel()
            {
                LineColor = new ColorObject() { Color = Color.FromArgb(0xAA, 0x00, 0x00, 0x80) },
                FillColor = new ColorObject() { Color = Colors.Lavender },
            };
        }

        public PointInfoBase(Point point) : this()
        {
            X = point.X;
            Y = point.Y;
        }


        private double _x;
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                if(SetProperty(ref _x, value))
                {
                    RaisePropertyChanged(nameof(Left));
                }
            }
        }

        private double _y;
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (SetProperty(ref _y, value))
                {
                    RaisePropertyChanged(nameof(Top));
                }
            }
        }

        public double Left
        {
            get
            {
                return X - ConnectorWidth / 2;
            }
        }

        public double Top
        {
            get
            {
                return Y - ConnectorHeight / 2;
            }
        }

        private double connectorWidth = 8;
        public double ConnectorWidth
        {
            get { return connectorWidth; }
            set { connectorWidth = value; }
        }

        private double connectorHeight = 8;
        public double ConnectorHeight
        {
            get { return connectorHeight; }
            set { connectorHeight = value; }
        }

        private IColorViewModel _colorViewModel;
        public IColorViewModel ColorViewModel
        {
            get
            {
                return _colorViewModel;
            }
            set
            {
                SetProperty(ref _colorViewModel, value);
            }
        }

        public static implicit operator PointInfoBase(Point point)
        {
            return new PointInfoBase(point);
        }

        public static implicit operator Point(PointInfoBase pointInfoBase)
        {
            return new Point(pointInfoBase.X, pointInfoBase.Y);
        }

        public static List<PointInfoBase> ToList(List<Point> lst)
        {
            return lst.Select(p => (PointInfoBase)p).ToList();
        }
    }
}
