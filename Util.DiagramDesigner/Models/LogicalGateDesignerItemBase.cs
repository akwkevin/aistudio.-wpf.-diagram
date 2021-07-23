using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Util.DiagramDesigner
{
    public class LogicalGateDesignerItemBase : DesignerItemBase
    {
        public LogicalGateDesignerItemBase()
        {

        }
        public LogicalGateDesignerItemBase(LogicalGateItemViewModelBase item) : base(item)
        {
            this.Connectors = new List<ConnectorItem>();
            foreach (var fullyCreatedConnectorInfo in item.Connectors)
            {
                ConnectorItem connector = new ConnectorItem()
                {
                    XRatio = fullyCreatedConnectorInfo.XRatio,
                    YRatio = fullyCreatedConnectorInfo.YRatio,
                    ConnectorWidth = fullyCreatedConnectorInfo.ConnectorWidth,
                    ConnectorHeight = fullyCreatedConnectorInfo.ConnectorHeight,
                    Orientation = fullyCreatedConnectorInfo.Orientation,
                    IsInnerPoint = fullyCreatedConnectorInfo.IsInnerPoint,
                    ValueTypePoint = fullyCreatedConnectorInfo.ValueTypePoint,
                    ConnectorValue = fullyCreatedConnectorInfo.ConnectorValue
                };
                this.Connectors.Add(connector);
            }
            this.OrderNumber = item.OrderNumber;
            this.LogicalType = item.LogicalType;
            this.Value = item.Value;
            this.IsEnabled = item.IsEnabled;
        }

        [XmlArray]
        public List<ConnectorItem> Connectors { get; set; }

        [XmlAttribute]
        public int OrderNumber { get; set; }

        [XmlAttribute]
        public double Value { get; set; }

        [XmlAttribute]
        public LogicalType LogicalType { get; set; }

        [XmlAttribute]
        public bool IsEnabled { get; set; }

    }
}
