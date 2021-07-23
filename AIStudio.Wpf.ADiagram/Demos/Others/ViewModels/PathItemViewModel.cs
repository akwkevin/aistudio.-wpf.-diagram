using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AIStudio.Wpf.ADiagram.Demos.Others
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
