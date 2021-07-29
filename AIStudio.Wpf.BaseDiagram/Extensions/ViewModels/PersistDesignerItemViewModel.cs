using AIStudio.Wpf.BaseDiagram.Services;
using Util.DiagramDesigner;
using AIStudio.Wpf.BaseDiagram.Extensions.Models;

namespace AIStudio.Wpf.BaseDiagram.Extensions.ViewModels
{
    public class PersistDesignerItemViewModel : DesignerItemViewModelBase
    {
        private IUIVisualizerService visualiserService;

        public PersistDesignerItemViewModel(IDiagramViewModel parent, PersistDesignerItem designer) : base(parent, designer)
        {

        }

        public PersistDesignerItemViewModel() : base()
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

            PersistDesignerItem designer = designerbase as PersistDesignerItem;
            this.HostUrl = designer.HostUrl;
        }


        public string HostUrl { get; set; }

        protected override void ExecuteEditCommand(object parameter)
        {
            PersistDesignerItemData data = new PersistDesignerItemData(HostUrl);
            if (visualiserService.ShowDialog(data) == true)
            {
                this.HostUrl = data.HostUrl;
            }
        }



    }
}
