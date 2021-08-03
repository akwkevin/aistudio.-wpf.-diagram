using AIStudio.Wpf.SFC.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.Models
{
    public class SFCNodeDesignerItem : DesignerItemBase
    {
        public SFCNodeDesignerItem()
        {

        }
        public SFCNodeDesignerItem(SFCNode item) : base(item)
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
            Kind = item.Kind;
            Expression = item.Expression;

            if (item is SFCActionNode actionNode)
            {
                LinkPoints = new List<LinkPoint> { actionNode.LinkPoint };
            }
            else if (item is SFCConditionNode sFCConditionNode)
            {
                LinkPoints = new List<LinkPoint>(sFCConditionNode.LinkPoint);
            }
            else if (item is Simulate_SolenoidViewModel simulate_SolenoidViewModel)
            {
                LinkPoints = new List<LinkPoint> { simulate_SolenoidViewModel.DILinkPoint, simulate_SolenoidViewModel.DOLinkPoint };
            }
            else if (item is Simulate_StartViewModel simulate_StartViewModel)
            {
                LinkPoints = new List<LinkPoint> { simulate_StartViewModel.LinkPoint };
            }
            else if (item is Simulate_TankViewModel simulate_TankViewModel)
            {
                LinkPoints = new List<LinkPoint> { simulate_TankViewModel.LinkPoint };
            }

        }

        [XmlArray]
        public List<ConnectorItem> Connectors { get; set; }

        [XmlAttribute]
        public SFCNodeKinds Kind { get; set; }

        [XmlAttribute]
        public string Expression { get; set; }

        [XmlIgnore]
        public List<LinkPoint> LinkPoints { get; set; } = new List<LinkPoint>();

        [JsonIgnore]
        [XmlElement("LinkPoint")]
        public string XmlLinkPoints
        {
            get
            {
                return SFCService.SerializeLinkPoint(LinkPoints);
            }
            set
            {
                LinkPoints = SFCService.DeserializeLinkPoint(value);
            }
        }

    }
}
