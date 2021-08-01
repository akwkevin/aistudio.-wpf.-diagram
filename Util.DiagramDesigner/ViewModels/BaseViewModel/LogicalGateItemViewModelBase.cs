using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public class LogicalGateItemViewModelBase : DesignerItemViewModelBase
    {
        public SimpleCommand AddInputCommand { get; private set; }
        public SimpleCommand AddOutputCommand { get; private set; }


        public LogicalGateItemViewModelBase(LogicalType logicalType) : base()
        {
            this.LogicalType = logicalType;

            if (this.LogicalType == LogicalType.Input)
            {
                ClearConnectors();
                ExecuteAddOutput(null);
            }
            else if (this.LogicalType == LogicalType.Output)
            {
                ClearConnectors();
                ExecuteAddInput(null);
            }
            else if (this.LogicalType == LogicalType.Constant)
            {
                ClearConnectors();
                ExecuteAddOutput(null);
            }
            else if (this.LogicalType == LogicalType.Time)
            {
                ClearConnectors();
                ExecuteAddOutput(null);
            }
            else if (this.LogicalType == LogicalType.None)
            {
                ClearConnectors();
                ExecuteAddOutput(null);
            }
            else if (this.LogicalType == LogicalType.NOT)
            {
                ClearConnectors();
                ExecuteAddInput(null);
                ExecuteAddOutput(null);
            }
            else if (this.LogicalType == LogicalType.SEL)
            {
                ClearConnectors();
                ExecuteAddInput(null, 0);
                ExecuteAddInput(null, 1);
                ExecuteAddInput(null, 2);
                ExecuteAddOutput(null, 0);
            }
            else if (this.LogicalType >= LogicalType.ABS && this.LogicalType <= LogicalType.EXPT)
            {
                ClearConnectors();
                ExecuteAddInput(null);
                ExecuteAddOutput(null);
            }
            else
            {
                ClearConnectors();
                ExecuteAddInput(null);
                ExecuteAddInput(null);
                ExecuteAddOutput(null);
            }
            BuildMenuOptions();
        }

        public LogicalGateItemViewModelBase(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {
            BuildMenuOptions();
        }


        protected override void Init()
        {
            ShowRotate = false;
            ShowArrow = false;
            AddInputCommand = new SimpleCommand(para => ExecuteAddInput(para));
            AddOutputCommand = new SimpleCommand(para => ExecuteAddOutput(para));

            base.Init();
        }

        private void BuildMenuOptions()
        {
            bool enAddInput = false;
            bool enAddOutput = false;
            if (LogicalType >= LogicalType.ADD && LogicalType <= LogicalType.AVE)
            {
                enAddInput = true;
                enAddOutput = false;
            }
            else
            {
                enAddInput = false;
                enAddOutput = false;
            }

            menuOptions = new ObservableCollection<CinchMenuItem>();
            if (enAddInput == true)
            {
                CinchMenuItem menuItem = new CinchMenuItem();
                menuItem.Text = "添加输入";
                menuItem.Command = AddInputCommand;
                menuItem.CommandParameter = menuItem;
                menuOptions.Add(menuItem);
            }
            if (enAddOutput == true)
            {
                CinchMenuItem menuItem = new CinchMenuItem();
                menuItem.Text = "添加输出";
                menuItem.Command = AddOutputCommand;
                menuItem.CommandParameter = menuItem;
                menuOptions.Add(menuItem);
            }
        }

        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            LogicalGateDesignerItemBase designer = designerbase as LogicalGateDesignerItemBase;
            this.LogicalType = designer.LogicalType;
            this.OrderNumber = designer.OrderNumber;
            this.Value = designer.Value;
            this.IsEnabled = designer.IsEnabled;

            ClearConnectors();
            Input.Clear();
            Output.Clear();

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
        }

        private int _orderNumber;
        public int OrderNumber
        {
            get
            {
                return _orderNumber;
            }
            set
            {
                SetProperty(ref _orderNumber, value);
            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                SetProperty(ref _isEnabled, value);
            }
        }


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

        public LogicalType LogicalType { get; set; }


        public Dictionary<int, FullyCreatedConnectorInfo> Input { get; set; } = new Dictionary<int, FullyCreatedConnectorInfo>();
        public Dictionary<int, FullyCreatedConnectorInfo> Output { get; set; } = new Dictionary<int, FullyCreatedConnectorInfo>();

        public virtual void ExecuteAddInput(object parameter, int index = 0)
        {
            if (Input.Values.Count >= 2)
            {
                this.ItemHeight = this.ItemHeight * (Input.Values.Count + 1) / Input.Values.Count;
            }
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Left, true, ValueTypeInput.Count > index ? ValueTypeInput[index] : ValueTypeInput[0]);
            connector.XRatio = 0;
            Input.Add(Input.Count, connector);
            for (int i = 0; i < Input.Values.Count; i++)
            {
                Input[i].YRatio = (i + 1.0) / (Input.Values.Count + 1.0);
            }
            AddConnector(connector);
        }

        public virtual void ExecuteAddOutput(object parameter, int index = 0)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Right, true, ValueTypeOutput.Count > index ? ValueTypeOutput[index] : ValueTypeInput[0]);
            connector.XRatio = 1;
            Output.Add(Output.Count, connector);
            for (int i = 0; i < Output.Values.Count; i++)
            {
                Output[i].YRatio = (i + 1.0) / (Output.Values.Count + 1.0);
            }
            AddConnector(connector);
        }

        public List<ValueTypePoint> ValueTypeInput
        {
            get
            {
                if (LogicalType == LogicalType.NOT)
                {
                    return new List<ValueTypePoint>() { ValueTypePoint.Bool };
                }
                else if (LogicalType == LogicalType.AND || LogicalType == LogicalType.OR || LogicalType == LogicalType.XOR
                    || LogicalType == LogicalType.SHL || LogicalType == LogicalType.SHR || LogicalType == LogicalType.ROL || LogicalType == LogicalType.ROR)
                {
                    return new List<ValueTypePoint>() { ValueTypePoint.Int };
                }
                else if (LogicalType == LogicalType.SEL)
                {
                    return new List<ValueTypePoint>() { ValueTypePoint.Bool, ValueTypePoint.Real, ValueTypePoint.Real };
                }
                else
                {
                    return new List<ValueTypePoint>() { ValueTypePoint.Real };
                }
            }
        }

        public List<ValueTypePoint> ValueTypeOutput
        {
            get
            {
                if (LogicalType == LogicalType.GT || LogicalType == LogicalType.LT || LogicalType == LogicalType.GE || LogicalType == LogicalType.LE || LogicalType == LogicalType.EQ || LogicalType == LogicalType.NE
                    || LogicalType == LogicalType.NOT)
                {
                    return new List<ValueTypePoint>() { ValueTypePoint.Bool };
                }
                else if (LogicalType == LogicalType.AND || LogicalType == LogicalType.OR || LogicalType == LogicalType.XOR
                    || LogicalType == LogicalType.SHL || LogicalType == LogicalType.SHR || LogicalType == LogicalType.ROL || LogicalType == LogicalType.ROR)
                {
                    return new List<ValueTypePoint>() { ValueTypePoint.Int };
                }
                else
                {
                    return new List<ValueTypePoint>() { ValueTypePoint.Real };
                }
            }
        }
    }


}
