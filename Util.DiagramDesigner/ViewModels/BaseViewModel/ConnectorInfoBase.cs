using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public abstract class ConnectorInfoBase : BindableBase
    {
      
        public ConnectorInfoBase(ConnectorOrientation orientation)
        {
            this.Orientation = orientation;
            ColorViewModel = new ColorViewModel()
            {
                LineColor = new ColorObject() { Color = Color.FromArgb(0xAA, 0x00, 0x00, 0x80) },
                FillColor = new ColorObject() { Color = Colors.Lavender },
            };
        }

        private ConnectorOrientation _orientation;
        public ConnectorOrientation Orientation
        {
            get { return _orientation; }
            set
            {
                SetProperty(ref _orientation, value);
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

        public double _connectorValue;
        public double ConnectorValue
        {
            get
            {
                return _connectorValue;
            }
            set
            {
                SetProperty(ref _connectorValue, value);
            }
        }        
    }
}
