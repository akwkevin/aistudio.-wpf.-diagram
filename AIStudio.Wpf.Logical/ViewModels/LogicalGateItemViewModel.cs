using AIStudio.Wpf.BaseDiagram;
using AIStudio.Wpf.BaseDiagram.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.Logical.ViewModels
{
    public class LogicalGateItemViewModel : LogicalGateItemViewModelBase
    {
        protected IUIVisualizerService visualiserService;
        public LogicalGateItemViewModel(LogicalType logicalType) : base(logicalType)
        {
            ColorViewModel.FillColor.Color = Colors.Orange;
        }

        public LogicalGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }

        protected override void Init()
        {
            base.Init();

            visualiserService = ApplicationServicesProvider.Instance.Provider.VisualizerService;
        }

        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            LogicalGateDesignerItemBase designer = designerbase as LogicalGateDesignerItemBase;
            this.Value = designer.Value;
        }


        protected override void ExecuteEditCommand(object parameter)
        {
            if (LogicalType == LogicalType.Constant)
            {
                ValueDesignerItemData data = new ValueDesignerItemData(Value);
                if (visualiserService.ShowDialog(data) == true)
                {
                    this.Value = data.Value;
                }
            }
            else
            {
                LogicalGateItemData data = new LogicalGateItemData(Input.Values);
                if (visualiserService.ShowDialog(data) == true)
                {

                }
            }
        }
    }

    public class AddGateItemViewModel : LogicalGateItemViewModel
    {
        public AddGateItemViewModel() : base(LogicalType.ADD)
        {
        }

        public AddGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {
        }
    }

    public class SubtractGateItemViewModel : LogicalGateItemViewModel
    {
        public SubtractGateItemViewModel() : base(LogicalType.SUB)
        {
        }

        public SubtractGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {
        }
    }

    public class MultiplyGateItemViewModel : LogicalGateItemViewModel
    {
        public MultiplyGateItemViewModel() : base(LogicalType.MUL)
        {
        }

        public MultiplyGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {
        }
    }

    public class DivideGateItemViewModel : LogicalGateItemViewModel
    {

        public DivideGateItemViewModel() : base(LogicalType.DIV)
        {
        }

        public DivideGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {
        }
    }

    public class AverageGateItemViewModel : LogicalGateItemViewModel
    {
        public AverageGateItemViewModel() : base(LogicalType.AVE)
        {

        }

        public AverageGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class MODGateItemViewModel : LogicalGateItemViewModel
    {
        public MODGateItemViewModel() : base(LogicalType.MOD)
        {

        }

        public MODGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class ANDGateItemViewModel : LogicalGateItemViewModel
    {
        public ANDGateItemViewModel() : base(LogicalType.AND)
        {

        }

        public ANDGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class ORGateItemViewModel : LogicalGateItemViewModel
    {
        public ORGateItemViewModel() : base(LogicalType.OR)
        {

        }

        public ORGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class XORGateItemViewModel : LogicalGateItemViewModel
    {
        public XORGateItemViewModel() : base(LogicalType.XOR)
        {

        }

        public XORGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class NOTGateItemViewModel : LogicalGateItemViewModel
    {
        public NOTGateItemViewModel() : base(LogicalType.NOT)
        {

        }

        public NOTGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class SHLGateItemViewModel : LogicalGateItemViewModel
    {
        public SHLGateItemViewModel() : base(LogicalType.SHL)
        {

        }

        public SHLGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class SHRGateItemViewModel : LogicalGateItemViewModel
    {
        public SHRGateItemViewModel() : base(LogicalType.SHR)
        {

        }

        public SHRGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class ROLGateItemViewModel : LogicalGateItemViewModel
    {
        public ROLGateItemViewModel() : base(LogicalType.ROL)
        {

        }

        public ROLGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class RORGateItemViewModel : LogicalGateItemViewModel
    {
        public RORGateItemViewModel() : base(LogicalType.ROR)
        {

        }

        public RORGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class SELGateItemViewModel : LogicalGateItemViewModel
    {
        public SELGateItemViewModel() : base(LogicalType.SEL)
        {
      
        }

        public SELGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class MAXGateItemViewModel : LogicalGateItemViewModel
    {
        public MAXGateItemViewModel() : base(LogicalType.MAX)
        {

        }

        public MAXGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class MINGateItemViewModel : LogicalGateItemViewModel
    {
        public MINGateItemViewModel() : base(LogicalType.MIN)
        {

        }

        public MINGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class LIMITGateItemViewModel : LogicalGateItemViewModel
    {
        public LIMITGateItemViewModel() : base(LogicalType.LIMIT)
        {

        }

        public LIMITGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class GTGateItemViewModel : LogicalGateItemViewModel
    {
        public GTGateItemViewModel() : base(LogicalType.GT)
        {

        }

        public GTGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class LTGateItemViewModel : LogicalGateItemViewModel
    {
        public LTGateItemViewModel() : base(LogicalType.GT)
        {

        }

        public LTGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class GEGateItemViewModel : LogicalGateItemViewModel
    {
        public GEGateItemViewModel() : base(LogicalType.GE)
        {

        }

        public GEGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class LEGateItemViewModel : LogicalGateItemViewModel
    {
        public LEGateItemViewModel() : base(LogicalType.LE)
        {

        }

        public LEGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class EQGateItemViewModel : LogicalGateItemViewModel
    {
        public EQGateItemViewModel() : base(LogicalType.LE)
        {

        }

        public EQGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class NEGateItemViewModel : LogicalGateItemViewModel
    {
        public NEGateItemViewModel() : base(LogicalType.NE)
        {

        }

        public NEGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class ABSGateItemViewModel : LogicalGateItemViewModel
    {
        public ABSGateItemViewModel() : base(LogicalType.ABS)
        {

        }

        public ABSGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class SQRTGateItemViewModel : LogicalGateItemViewModel
    {
        public SQRTGateItemViewModel() : base(LogicalType.SQRT)
        {

        }

        public SQRTGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class LNGateItemViewModel : LogicalGateItemViewModel
    {
        public LNGateItemViewModel() : base(LogicalType.LN)
        {

        }

        public LNGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class LOGGateItemViewModel : LogicalGateItemViewModel
    {
        public LOGGateItemViewModel() : base(LogicalType.LOG)
        {

        }

        public LOGGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class EXPGateItemViewModel : LogicalGateItemViewModel
    {
        public EXPGateItemViewModel() : base(LogicalType.EXP)
        {

        }

        public EXPGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class SINGateItemViewModel : LogicalGateItemViewModel
    {
        public SINGateItemViewModel() : base(LogicalType.SIN)
        {

        }

        public SINGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class COSGateItemViewModel : LogicalGateItemViewModel
    {
        public COSGateItemViewModel() : base(LogicalType.COS)
        {

        }

        public COSGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class TANGateItemViewModel : LogicalGateItemViewModel
    {
        public TANGateItemViewModel() : base(LogicalType.TAN)
        {

        }

        public TANGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class ASINGateItemViewModel : LogicalGateItemViewModel
    {
        public ASINGateItemViewModel() : base(LogicalType.ASIN)
        {

        }

        public ASINGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class ACOSGateItemViewModel : LogicalGateItemViewModel
    {
        public ACOSGateItemViewModel() : base(LogicalType.ACOS)
        {

        }

        public ACOSGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class ATANGateItemViewModel : LogicalGateItemViewModel
    {
        public ATANGateItemViewModel() : base(LogicalType.ATAN)
        {

        }

        public ATANGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class EXPTGateItemViewModel : LogicalGateItemViewModel
    {
        public EXPTGateItemViewModel() : base(LogicalType.EXPT)
        {

        }

        public EXPTGateItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class ConstantDesignerItemViewModel : LogicalGateItemViewModel
    {
        public ConstantDesignerItemViewModel() : base(LogicalType.Constant)
        {
            ItemHeight = 28;
        }

        public ConstantDesignerItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }

    }

    public class InputItemViewModel : LogicalGateItemViewModel
    {
        public InputItemViewModel() : base(LogicalType.Input)
        {
            ItemHeight = 28;
        }

        public InputItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }

        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            LogicalGateDesignerItemBase designer = designerbase as LogicalGateDesignerItemBase;
            this.Value = designer.Value;
            LinkPoint = LogicalService.LinkPoint.FirstOrDefault(p => p.Id.ToString() == designer.Icon);  //不想新增字段了，就用这个Icon保存自定义测点的Id吧。
            if (LinkPoint != null)
            {
                LinkPoint.Value = designer.Value;
            }
        }

        private LinkPoint _linkPoint;
        public LinkPoint LinkPoint
        {
            get { return _linkPoint; }
            set
            {
                if (SetProperty(ref _linkPoint, value))
                {
                    Icon = _linkPoint?.Id.ToString();//不想新增字段了，就用这个Icon保存自定义测点的Id吧。
                    Text = _linkPoint?.Name;
                }
            }
        }

        protected override void ExecuteEditCommand(object parameter)
        {
            LinkPointDesignerItemData data = new LinkPointDesignerItemData(LinkPoint);
            if (visualiserService.ShowDialog(data) == true)
            {
                this.LinkPoint = data.LinkPoint;
            }
        }
    }

    public class OutputItemViewModel : LogicalGateItemViewModel
    {
        public OutputItemViewModel() : base(LogicalType.Output)
        {
            ItemHeight = 28;
        }

        public OutputItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {

        }

        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            LogicalGateDesignerItemBase designer = designerbase as LogicalGateDesignerItemBase;
            this.Value = designer.Value;
            LinkPoint = LogicalService.LinkPoint.FirstOrDefault(p => p.Id.ToString() == designer.Icon);  //不想新增字段了，就用这个Icon保存自定义测点的Id吧。
            if (LinkPoint != null)
            {
                LinkPoint.Value = designer.Value;
            }
        }

        private LinkPoint _linkPoint;
        public LinkPoint LinkPoint
        {
            get { return _linkPoint; }
            set
            {
                if (SetProperty(ref _linkPoint, value))
                {
                    Icon = _linkPoint?.Id.ToString();//不想新增字段了，就用这个Icon保存自定义测点的Id吧。
                    Text = _linkPoint?.Name;
                }
            }
        }

        protected override void ExecuteEditCommand(object parameter)
        {
            LinkPointDesignerItemData data = new LinkPointDesignerItemData(LinkPoint);
            if (visualiserService.ShowDialog(data) == true)
            {
                this.LinkPoint = data.LinkPoint;
            }
        }
    }

    public class TimerDesignerItemViewModel : LogicalGateItemViewModel
    {
        private System.Timers.Timer readDataTimer = new System.Timers.Timer();
        public Action Do;

        public TimerDesignerItemViewModel() : base(LogicalType.Time)
        {
            ItemHeight = 32;
            ItemWidth = 32;
            Value = 1;
            Start();
            BuildMenuOptions();
        }

        public TimerDesignerItemViewModel(IDiagramViewModel parent, LogicalGateDesignerItemBase designer) : base(parent, designer)
        {
            BuildMenuOptions();
        }

        protected override void Init()
        {
            MenuItemCommand = new SimpleCommand(ExecuteMenuItemCommand);
            base.Init();

            readDataTimer.Elapsed += timeCycle;
            readDataTimer.Interval = 1000;
        }

        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            if (IsEnabled)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        private void Start()
        {
            IsEnabled = true;
            Output[0].ColorViewModel.FillColor.Color = Colors.Green;
            readDataTimer.Start();
        }

        private void Stop()
        {
            IsEnabled = false;
            Output[0].ColorViewModel.FillColor.Color = Colors.Red;
            readDataTimer.Stop();
        }

        private void ExecuteMenuItemCommand(object obj)
        {
            if ((obj as CinchMenuItem).IsChecked == true)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        public SimpleCommand MenuItemCommand { get; private set; }
        private void BuildMenuOptions()
        {
            menuOptions = new ObservableCollection<CinchMenuItem>();
            CinchMenuItem menuItem = new CinchMenuItem();
            menuItem.Text = "启动";
            menuItem.IsCheckable = true;
            menuItem.IsChecked = IsEnabled;
            menuItem.Command = MenuItemCommand;
            menuItem.CommandParameter = menuItem;
            menuOptions.Add(menuItem);
        }

        protected override void ExecuteEditCommand(object parameter)
        {
            ValueDesignerItemData data = new ValueDesignerItemData(Value);
            if (visualiserService.ShowDialog(data) == true)
            {
                this.Value = data.Value;
                readDataTimer.Interval = this.Value * 1000;
            }
        }

        private void timeCycle(object sender, EventArgs e)
        {
            Output.FirstOrDefault().Value.ConnectorValue += Value;
            if (Do != null)
            {
                Do();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            readDataTimer.Stop();
            readDataTimer.Dispose();
        }

    }
}
