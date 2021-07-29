using Util.DiagramDesigner;

namespace AIStudio.Wpf.BaseDiagram.Extensions.ViewModels
{
    public class PathItemViewModel : DesignerItemViewModelBase
    {
        public PathItemViewModel() : base()
        {

        }

        public PathItemViewModel(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
           
        }

        protected override void Init()
        {
            base.Init();

            this.ShowConnectors = false;
        }
    }
}
