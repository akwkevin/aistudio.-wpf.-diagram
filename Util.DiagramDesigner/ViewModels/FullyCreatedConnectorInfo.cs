using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Util.DiagramDesigner
{
    public class FullyCreatedConnectorInfo : ConnectorInfoBase
    {
       
        private List<CinchMenuItem> menuOptions;

        public FullyCreatedConnectorInfo(DesignerItemViewModelBase dataItem, ConnectorOrientation orientation, bool isInnerPoint=false, ValueTypePoint valueTypePoint = 0)
            : base(orientation)
        {
            this.DataItem = dataItem;
            this.IsInnerPoint = isInnerPoint;
            this.ValueTypePoint = valueTypePoint;

            menuOptions = new List<CinchMenuItem>();
            MenuItemCommand = new SimpleCommand(ExecuteMenuItemCommand);
            DeleteCommand = new SimpleCommand(ExecuteDeleteCommand);
            if (IsInnerPoint == true)
            {
                BuildMenuOptions();
            }
        }

        public void ExecuteMenuItemCommand(object arg)
        {
            Orientation = (ConnectorOrientation)arg;
        }

        public void ExecuteDeleteCommand(object arg)
        {
            DataItem.RemoveConnector(this);

        }

        private void BuildMenuOptions()
        {
            menuOptions.Clear();
            var orientation = new CinchMenuItem("方向");
            var top = new CinchMenuItem("上");
            top.Command = MenuItemCommand;
            top.CommandParameter = ConnectorOrientation.Top;
            var bottom = new CinchMenuItem("下");
            bottom.Command = MenuItemCommand;
            bottom.CommandParameter = ConnectorOrientation.Bottom;
            var left = new CinchMenuItem("左");
            left.Command = MenuItemCommand;
            left.CommandParameter = ConnectorOrientation.Left;
            var right = new CinchMenuItem("右");
            right.Command = MenuItemCommand;
            right.CommandParameter = ConnectorOrientation.Right;
            orientation.Children.Add(top);
            orientation.Children.Add(bottom);
            orientation.Children.Add(left);
            orientation.Children.Add(right);

            var delete = new CinchMenuItem("删除");
            delete.Command = DeleteCommand;

            menuOptions.Add(orientation);
            menuOptions.Add(delete);
        }

        public DesignerItemViewModelBase DataItem { get; private set; }

        private bool _showConnectors = false;
        public bool ShowConnectors
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

        private double _xRatio;
        public double XRatio
        {
            get { return _xRatio; }
            set
            {
                SetProperty(ref _xRatio, value);
            }
        }

        private double _yRatio;
        public double YRatio
        {
            get { return _yRatio; }
            set
            {
                SetProperty(ref _yRatio, value);
            }
        }

        public bool IsInnerPoint { get; set; }

        public ValueTypePoint _valueTypePoint;
        public ValueTypePoint ValueTypePoint
        {
            get { return _valueTypePoint; }
            set
            {
                SetProperty(ref _valueTypePoint, value);
            }
        }

        public SimpleCommand DeleteCommand { get; private set; }
        public SimpleCommand MenuItemCommand { get; private set; }

        public IEnumerable<CinchMenuItem> MenuOptions { get { return menuOptions; } }
    }


}
