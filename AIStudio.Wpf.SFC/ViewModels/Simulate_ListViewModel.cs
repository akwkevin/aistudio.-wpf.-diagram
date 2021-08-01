using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class Simulate_ListViewModel : SFCNode
    {
        public Simulate_ListViewModel() : base(SFCNodeKinds.Simulate_List)
        {
            ItemWidth = 170;
            ItemHeight = 260;
        }

        public Simulate_ListViewModel(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }
    
    }
}
