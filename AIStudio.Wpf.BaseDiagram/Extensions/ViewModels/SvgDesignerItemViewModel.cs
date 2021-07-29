using Util.DiagramDesigner;

namespace AIStudio.Wpf.BaseDiagram.Extensions.ViewModels
{
    public class SvgDesignerItemViewModel: MediaItemViewModel
    {
        protected override string Filter { get; set; } = "Svg|*.svg";

        public SvgDesignerItemViewModel() : base()
        {
        }

        public SvgDesignerItemViewModel(IDiagramViewModel parent, MediaDesignerItem designer) : base(parent, designer)
        {

        }
    }
}
