using AIStudio.Wpf.BaseDiagram.Services;
using System;
using Util.DiagramDesigner;
using AIStudio.Wpf.BaseDiagram.Extensions.Models;

namespace AIStudio.Wpf.BaseDiagram.Extensions.ViewModels
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
