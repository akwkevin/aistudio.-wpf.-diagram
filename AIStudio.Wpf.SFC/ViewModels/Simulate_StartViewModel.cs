using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class Simulate_StartViewModel : SFCNode
    {
        public Simulate_StartViewModel() : base(SFCNodeKinds.Simulate_Start)
        {
            ItemWidth = 32;
            ItemHeight = 32;

            ExecuteAddLeftInput(null);
            ExecuteAddRightOutput(null);
        }

        public Simulate_StartViewModel(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }

        private LinkPoint linkPoint;
        public LinkPoint LinkPoint
        {
            get
            {
                return linkPoint;
            }
            set
            {
                SetProperty(ref linkPoint, value);
            }
        }

    }
}
