using AIStudio.Wpf.ADiagram.Services;
using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace AIStudio.Wpf.ADiagram.Demos.Others
{
    public class SettingsDesignerItemViewModel : DesignerItemViewModelBase
    {
        private IUIVisualizerService visualiserService;

        public SettingsDesignerItemViewModel(IDiagramViewModel parent, SettingsDesignerItem designer) : base(parent, designer)
        {
           
        }

        public SettingsDesignerItemViewModel() : base()
        {

        }

        protected override void Init()
        {
            base.Init();

            visualiserService = ApplicationServicesProvider.Instance.Provider.VisualizerService;
            this.ShowConnectors = false;
        }

       protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            SettingsDesignerItem designer = designerbase as SettingsDesignerItem;
            this.Setting = designer.Setting;
        }

        public String Setting{ get; set; }

        protected override void ExecuteEditCommand(object parameter)
        {
            SettingsDesignerItemData data = new SettingsDesignerItemData(Setting);
            if (visualiserService.ShowDialog(data) == true)
            {
                this.Setting = data.Setting1;
            }
        }


    }
}
