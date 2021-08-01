using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class Simulate_TankViewModel : SFCNode
    {
        public Simulate_TankViewModel() : base(SFCNodeKinds.Simulate_Tank)
        {
            ItemWidth = 50;
            ItemHeight = 120;

            ExecuteAddLeftInput(null);
            ExecuteAddLeftInput(null);
            ExecuteAddRightOutput(null);
            ExecuteAddRightOutput(null);
        }

        public Simulate_TankViewModel(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }

        public override void ExecuteAddLeftInput(object parameter)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Left, true);
            connector.XRatio = 0;
            connector.YRatio = (30 + Input.Count * 60) / ItemHeight;
            Input.Add(Input.Count, connector);

            AddConnector(connector);
        }

        public override void ExecuteAddRightOutput(object parameter)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Right, true);
            connector.XRatio = 1;
            connector.YRatio = (30 + Output.Count * 60) / ItemHeight;
            Output.Add(Output.Count, connector);


            AddConnector(connector);
        }


        /// <summary>
        /// 液位
        /// </summary>
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

        protected override void ExecuteEditCommand(object parameter)
        {
            Simulate_TankViewModelData data = new Simulate_TankViewModelData(LinkPoint);
            if (visualiserService.ShowDialog(data) == true)
            {
                this.LinkPoint = data.LinkPoint;
            }
        }
    }
}
