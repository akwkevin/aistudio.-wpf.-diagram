using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIStudio.Wpf.ADiagram.Demos.Others
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
