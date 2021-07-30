using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class SFCCOBeginNode : SFCNode
    {
        public SFCCOBeginNode() : base(SFCNodeKinds.COBegin)
        {
        }

        public SFCCOBeginNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }
    }
}
