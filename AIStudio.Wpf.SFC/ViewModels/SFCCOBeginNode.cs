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
            ItemWidth = 280;
            ItemHeight = 10;

            ExecuteAddTopInput(null);
            ExecuteAddBottomOutput(null);
            ExecuteAddBottomOutput(null);
        }

        public SFCCOBeginNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }

        public override void ExecuteAddTopInput(object parameter)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Top, true);
            connector.YRatio = 0;
            connector.XRatio = (40 + Input.Count * 200) / ItemWidth;
            Input.Add(Input.Count, connector);   
            
            AddConnector(connector);
        }

        public override void ExecuteAddBottomOutput(object parameter)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Bottom, true);
            connector.YRatio = 1;
            connector.XRatio = (40 + Output.Count * 200) / ItemWidth;
            Output.Add(Output.Count, connector);
          

            AddConnector(connector);
        }
    }
}
