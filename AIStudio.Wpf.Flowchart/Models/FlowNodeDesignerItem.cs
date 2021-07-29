using AIStudio.Wpf.Flowchart.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.Flowchart.Models
{
    public class FlowNodeDesignerItem : DesignerItemBase
    {
        public FlowNodeDesignerItem()
        {

        }
        public FlowNodeDesignerItem(FlowNode item) : base(item)
        {
            if (item is MiddleFlowNode middleFlow)
            {
                UserIds = middleFlow.UserIds;
                RoleIds = middleFlow.RoleIds;
                ActType = middleFlow.ActType;
            }
            Color = item.Color;
            Kind = item.Kind;
            StateImage = item.StateImage;
        }

        [XmlArray]
        public List<string> UserIds { get; set; }

        [XmlArray]
        public List<string> RoleIds { get; set; }

        [XmlAttribute]
        public string ActType { get; set; }

        [XmlAttribute]
        public string Color { get; set; }

        [XmlAttribute]
        public NodeKinds Kind { get; set; }

        [XmlAttribute]
        public string StateImage { get; set; }
    }
}
