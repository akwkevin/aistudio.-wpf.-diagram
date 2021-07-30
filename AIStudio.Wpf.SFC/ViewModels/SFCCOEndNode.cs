using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class SFCCOEndNode : SFCNode
    {
        public SFCCOEndNode() : base(SFCNodeKinds.COEnd)
        {
        }

        public SFCCOEndNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }
    }
}
