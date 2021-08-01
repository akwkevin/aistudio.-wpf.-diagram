using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class SFCStartNode : SFCNode
    {
        public SFCStartNode() : base(SFCNodeKinds.Start)
        {
            ExecuteAddTopInput(null);
            ExecuteAddBottomOutput(null);
        }

        public SFCStartNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }
    }
}
