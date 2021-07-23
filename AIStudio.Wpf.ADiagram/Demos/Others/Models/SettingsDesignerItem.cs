using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Wpf.ADiagram.Demos.Others
{
     public class SettingsDesignerItem : DesignerItemBase
    {
        public SettingsDesignerItem()
        {

        }
        public SettingsDesignerItem(SettingsDesignerItemViewModel item) : base(item)
        {
            this.Setting = item.Setting;
        }

        public string Setting { get; set; }
    }
}
