using System.Collections.Generic;
using System.Xml.Serialization;

namespace Util.DiagramDesigner
{
    public class MediaDesignerItem : DesignerItemBase
    {
        public MediaDesignerItem()
        {

        }

        public MediaDesignerItem(GifImageItemViewModel item) : base(item)
        {
            Connectors = new List<ConnectorItem>();
            foreach (var fullyCreatedConnectorInfo in item.Connectors)
            {
                ConnectorItem connector = new ConnectorItem()
                {
                    XRatio = fullyCreatedConnectorInfo.XRatio,
                    YRatio = fullyCreatedConnectorInfo.YRatio,
                    ConnectorWidth = fullyCreatedConnectorInfo.ConnectorWidth,
                    ConnectorHeight = fullyCreatedConnectorInfo.ConnectorHeight,
                    Orientation = fullyCreatedConnectorInfo.Orientation
                };
                Connectors.Add(connector);
            }
        }

        public MediaDesignerItem(MediaItemViewModel item) : base(item)
        {
            Connectors = new List<ConnectorItem>();
            foreach (var fullyCreatedConnectorInfo in item.Connectors)
            {
                ConnectorItem connector = new ConnectorItem()
                {
                    XRatio = fullyCreatedConnectorInfo.XRatio,
                    YRatio = fullyCreatedConnectorInfo.YRatio,
                    ConnectorWidth = fullyCreatedConnectorInfo.ConnectorWidth,
                    ConnectorHeight = fullyCreatedConnectorInfo.ConnectorHeight,
                    Orientation = fullyCreatedConnectorInfo.Orientation
                };
                Connectors.Add(connector);
            }
        }

        [XmlArray]
        public List<ConnectorItem> Connectors { get; set; }

    }
}
