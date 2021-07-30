using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class SFCConditionNode : SFCNode
    {
        public SFCConditionNode() : base(SFCNodeKinds.Condition)
        {
        }

        public SFCConditionNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }
    }
}
