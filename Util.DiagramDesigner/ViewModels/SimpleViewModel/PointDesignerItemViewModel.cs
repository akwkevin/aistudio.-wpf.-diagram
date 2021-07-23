using System;
using System.Windows;

namespace Util.DiagramDesigner
{
    public class PointDesignerItemViewModel : DesignerItemViewModelBase
    {
        private bool _showConnectors = false;
        public new bool ShowConnectors
        {
            get
            {
                return _showConnectors;
            }
            set
            {
                SetProperty(ref _showConnectors, value);
            }
        }

        public Point CurrentLocation
        {
            get
            {
                return new Point() { X = Left + ItemWidth / 2, Y = Top + ItemHeight / 2 };
            }
        }

        public PointDesignerItemViewModel(Point location) : base()
        {
            Left = Math.Max(0, location.X - ItemWidth / 2);
            Top = Math.Max(0, location.Y - ItemHeight / 2);
        }

        protected override void Init()
        {
            base.Init();

            this.ClearConnectors();
            this.AddConnector(new FullyCreatedConnectorInfo(this, ConnectorOrientation.None, true));

            ItemWidth = 5;
            ItemHeight = 5;
        }

      
    }
}
