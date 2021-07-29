using System.Xml.Serialization;
using Util.DiagramDesigner;
using AIStudio.Wpf.BaseDiagram.Extensions.ViewModels;

namespace AIStudio.Wpf.BaseDiagram.Extensions.Models
{
    public class PersistDesignerItem : DesignerItemBase
    {       
        public PersistDesignerItem()
        {

        }
        public PersistDesignerItem(PersistDesignerItemViewModel item) : base(item)
        {
            this.HostUrl = item.HostUrl;
        }

        [XmlAttribute]
        public string HostUrl { get; set; }
    }
}
