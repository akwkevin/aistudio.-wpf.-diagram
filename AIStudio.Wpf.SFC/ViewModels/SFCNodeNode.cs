using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class SFCNodeNode : SFCNode
    {
        public SFCNodeNode() : base(SFCNodeKinds.Node)
        {
            ExecuteAddTopInput(null);
            ExecuteAddBottomOutput(null);
            ExecuteAddActionOutput(null);
        }

        public SFCNodeNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }

    }
}
