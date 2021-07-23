using System.Collections.Generic;
using System.Xml.Serialization;

namespace Util.DiagramDesigner
{
    public class ImageDesignerItem : DesignerItemBase
    {
        public ImageDesignerItem()
        {

        }
        public ImageDesignerItem(ImageItemViewModel item) : base(item)
        {
            Icon = item.Icon;
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
