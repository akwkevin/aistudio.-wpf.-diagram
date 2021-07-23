using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Demos.Others
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
