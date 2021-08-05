using AIStudio.Wpf.BaseDiagram;
using AIStudio.Wpf.BaseDiagram.Services;
using AIStudio.Wpf.SFC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class SFCNode : DesignerItemViewModelBase
    {
        protected IUIVisualizerService visualiserService;

        public SFCNode(SFCNodeKinds kind) : base()
        {
            ColorViewModel.FillColor.Color = Colors.Blue;
            Kind = kind;
            ItemWidth = 80;
            ItemHeight = 40;
        }

        public SFCNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }

        protected override void Init()
        {
            IsInnerConnector = true;
            ShowRotate = false;
            ShowArrow = false;
            ShowText = true;
            IsReadOnlyText = true;

            base.Init();

            visualiserService = ApplicationServicesProvider.Instance.Provider.VisualizerService;
        }

        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            SFCNodeDesignerItem designer = designerbase as SFCNodeDesignerItem;
            this.Kind = designer.Kind;
            this.Expression = designer.Expression;

            foreach (var connector in designer.Connectors)
            {
                FullyCreatedConnectorInfo fullyCreatedConnectorInfo = new FullyCreatedConnectorInfo(this, connector.Orientation, true);
                fullyCreatedConnectorInfo.XRatio = connector.XRatio;
                fullyCreatedConnectorInfo.YRatio = connector.YRatio;
                fullyCreatedConnectorInfo.ConnectorWidth = connector.ConnectorWidth;
                fullyCreatedConnectorInfo.ConnectorHeight = connector.ConnectorHeight;
                fullyCreatedConnectorInfo.Orientation = connector.Orientation;
                fullyCreatedConnectorInfo.IsInnerPoint = connector.IsInnerPoint;
                fullyCreatedConnectorInfo.ValueTypePoint = connector.ValueTypePoint;
                fullyCreatedConnectorInfo.ConnectorValue = connector.ConnectorValue;

                if (fullyCreatedConnectorInfo.Orientation == ConnectorOrientation.Left)
                {
                    Input.Add(Input.Count, fullyCreatedConnectorInfo);
                }
                else if (fullyCreatedConnectorInfo.Orientation == ConnectorOrientation.Right)
                {
                    Output.Add(Output.Count, fullyCreatedConnectorInfo);
                }
                AddConnector(fullyCreatedConnectorInfo);
            }

            if (this is SFCActionNode actionNode)
            {
                actionNode.LinkPoint = designer.LinkPoints.FirstOrDefault();
            }
            else if (this is SFCConditionNode sFCConditionNode)
            {
                sFCConditionNode.LinkPoint = new System.Collections.ObjectModel.ObservableCollection<LinkPoint>(designer.LinkPoints);
            }
            else if (this is Simulate_SolenoidViewModel simulate_SolenoidViewModel)
            {
                simulate_SolenoidViewModel.DILinkPoint = designer.LinkPoints.FirstOrDefault();
                simulate_SolenoidViewModel.DOLinkPoint = designer.LinkPoints.LastOrDefault();
            }
            else if (this is Simulate_StartViewModel simulate_StartViewModel)
            {
                simulate_StartViewModel.LinkPoint = designer.LinkPoints.FirstOrDefault();
            }
            else if (this is Simulate_TankViewModel simulate_TankViewModel)
            {
                simulate_TankViewModel.LinkPoint = designer.LinkPoints.FirstOrDefault();
            }
        }

        protected override void InitConnector()
        {
            ClearConnectors();
        }

        public Dictionary<int, FullyCreatedConnectorInfo> Input { get; set; } = new Dictionary<int, FullyCreatedConnectorInfo>();
        public Dictionary<int, FullyCreatedConnectorInfo> Output { get; set; } = new Dictionary<int, FullyCreatedConnectorInfo>();
        public Dictionary<int, FullyCreatedConnectorInfo> Action { get; set; } = new Dictionary<int, FullyCreatedConnectorInfo>();

        public virtual void ExecuteAddLeftInput(object parameter)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Left, true);
            connector.XRatio = 0;
            Input.Add(Input.Count, connector);
            for (int i = 0; i < Input.Values.Count; i++)
            {
                Input[i].YRatio = (i + 1.0) / (Input.Values.Count + 1.0);
            }
            AddConnector(connector);
        }

        public virtual void ExecuteAddTopInput(object parameter)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Top, true);
            connector.YRatio = 0;
            Input.Add(Input.Count, connector);
            for (int i = 0; i < Input.Values.Count; i++)
            {
                Input[i].XRatio = (i + 1.0) / (Input.Values.Count + 1.0);
                if (Output.ContainsKey(i))
                {
                    Output[i].XRatio = Input[i].XRatio;
                }
            }
            AddConnector(connector);
        }

        public virtual void ExecuteAddRightOutput(object parameter)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Right, true);
            connector.XRatio = 1;
            Output.Add(Output.Count, connector);
            for (int i = 0; i < Output.Values.Count; i++)
            {
                Output[i].YRatio = (i + 1.0) / (Output.Values.Count + 1.0);
            }
            AddConnector(connector);
        }

        public virtual void ExecuteAddBottomOutput(object parameter)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Bottom, true);
            connector.YRatio = 1;
            Output.Add(Output.Count, connector);
            for (int i = 0; i < Output.Values.Count; i++)
            {
                Output[i].XRatio = (i + 1.0) / (Output.Values.Count + 1.0);
                if (Input.ContainsKey(i))
                {
                    Input[i].XRatio = Output[i].XRatio;
                }
            }

            AddConnector(connector);
        }

        public virtual void ExecuteAddActionOutput(object parameter)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Right, true);
            connector.XRatio = 1;
            Action.Add(Action.Count, connector);
            Action[Action.Count - 1].YRatio = 0.5;
            AddConnector(connector);
        }

        [Browsable(false)]
        public SFCNodeKinds Kind { get; set; }

        private double _value;
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                SetProperty(ref _value, value);
            }
        }

        private string _expression;
        public string Expression
        {
            get
            {
                return _expression;
            }
            set
            {
                SetProperty(ref _expression, value);
            }
        }

        #region 暂不保存
        private int _status;

        public int Status
        {
            get { return _status; }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        public List<SFCNode> NextNode { get; set; } = new List<SFCNode>();
        public List<SFCNode> PreNode { get; set; } = new List<SFCNode>();
        #endregion
    }
}
