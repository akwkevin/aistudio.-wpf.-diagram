using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.DiagramDesigner;

namespace Util.DiagramDesigner
{
    public class GroupDesignerItemViewModel : DesignerItemViewModelBase
    {
        public GroupDesignerItemViewModel() : base()
        {
            this.ClearConnectors();
            this.IsHitTestVisible = false;
        }

        protected override void ExecuteEditCommand(object param)
        {
        }
    }
}
