using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class SFCActionNode : SFCNode
    {
        public SFCActionNode() : base(SFCNodeKinds.Action)
        {
        }

        public SFCActionNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }
    }
}
