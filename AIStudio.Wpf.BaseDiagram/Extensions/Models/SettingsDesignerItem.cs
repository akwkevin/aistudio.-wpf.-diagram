using Util.DiagramDesigner;
using AIStudio.Wpf.BaseDiagram.Extensions.ViewModels;

namespace AIStudio.Wpf.BaseDiagram.Extensions.Models
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
