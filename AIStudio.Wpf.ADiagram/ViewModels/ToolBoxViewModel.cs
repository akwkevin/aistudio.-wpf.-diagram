using AIStudio.Wpf.ADiagram.Models;
using Util.DiagramDesigner;
using Util.DiagramDesigner.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AIStudio.Wpf.ADiagram.Demos.Flowchart;
using AIStudio.Wpf.ADiagram.Demos.Logical;
using AIStudio.Wpf.ADiagram.Demos.Others;
using System.Windows.Input;
using AIStudio.Wpf.ADiagram.Commands;
using System.IO;
using Newtonsoft.Json;
using AIStudio.Wpf.ADiagram.Helpers;
using static AIStudio.Wpf.ADiagram.Helpers.NewNameHelper;

namespace AIStudio.Wpf.ADiagram.ViewModels
{
    public class ToolBoxViewModel : BindableBase
    {
        private IDiagramServiceProvider _service { get { return DiagramServicesProvider.Instance.Provider; } }
        public ToolBoxViewModel()
        {
            Init();
        }

        private ObservableCollection<ToolBoxCategory> _toolBoxCategory = new ObservableCollection<ToolBoxCategory>();
        public ObservableCollection<ToolBoxCategory> ToolBoxCategory
        {
            get
            {
                return _toolBoxCategory;
            }
            set
            {
                SetProperty(ref _toolBoxCategory, value);
            }
        }

        public ObservableCollection<ToolBoxCategory> TotalToolBoxCategory { get; set; }

        public ToolBoxCategory MyToolBoxCategory { get { return ToolBoxCategory[0]; } }

        public ToolBoxCategory SvgToolBoxCategory { get { return ToolBoxCategory[2]; } }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return this._deleteCommand ?? (this._deleteCommand = new DelegateCommand<ToolBoxCategory>(para => this.Delete(para)));
            }
        }

        private ICommand _addItemCommand;
        public ICommand AddItemCommand
        {
            get
            {
                return this._addItemCommand ?? (this._addItemCommand = new DelegateCommand(() => this.AddMyItem()));
            }
        }

        private ICommand _importItemCommand;
        public ICommand ImportItemCommand
        {
            get
            {
                return this._importItemCommand ?? (this._importItemCommand = new DelegateCommand(() => this.ImportMyItem()));
            }
        }

        private ICommand _deleteItemCommand;
        public ICommand DeleteItemCommand
        {
            get
            {
                return this._deleteItemCommand ?? (this._deleteItemCommand = new DelegateCommand<ToolBoxData>(para => this.DeleteMyItem(para)));
            }
        }

        public void Init()
        {
            ToolBoxCategory.Add(new MineToolBoxCategory() { Header = "我的形状", ToolBoxItems = new ObservableCollection<ToolBoxData>(), IsExpanded = true });

            List<ToolBoxData> toolBoxItems = new List<ToolBoxData>();
            toolBoxItems.Add(new ImageToolBoxData("../Images/Setting.png", typeof(SettingsDesignerItemViewModel)));
            toolBoxItems.Add(new ImageToolBoxData("../Images/Persist.png", typeof(PersistDesignerItemViewModel)));

            ToolBoxCategory.Add(new ToolBoxCategory() { Header = "WPF-Diagram-Designer", ToolBoxItems = new ObservableCollection<ToolBoxData>(toolBoxItems), IsExpanded = true });

            ToolBoxCategory.Add(new ToolBoxCategory() { Header = "Svg", ToolBoxItems = new ObservableCollection<ToolBoxData>() });

            List<PathToolBoxData> pathToolBoxItems = new List<PathToolBoxData>();
            pathToolBoxItems.Add(new PathToolBoxData("M 0,20 L 30 0 L 60,20 L 30,40 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0,0 H 60 V 40 C 30,30 30,50 0,40 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 10,0 L 60 0 L 50,40 L 0,40 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 10,20 A 20,20 0 1 1 50,20 A 20,20 0 1 1 10,20", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0, 3 C 30, -7 30, 13 60, 3 V 37 C 30, 47 30, 27 0, 37 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 50,0 V 40 M 10,0 V 40 M 0 0 H 60 V 40 H 0 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 5,0 H 60 A 40,40 0 0 0 60,40 H 5 A 40,40 0 0 1 5,0 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0,10 H 60 M 10,0 V 40 M 0,0 H 60 V 40 H 0 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 30,40 A 20,20 0 1 1 30,0 A 20,20 0 0 1 43,35 H 50 L 50,40 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("F 1 M 57,40 H 3 A 4,20 0 1 1 3,0 H 57 A 4,20.1 0 1 1 56,0", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0 10 L 60,0 V 40 H 0 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0 10 L 10,0 H 60 V 40 H 0 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0,0 H 40 A 20,20 0 0 1 40,40 H 0 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 20,40 A 20,20 0 0 1 20,0 H 40 A 20,20 0 0 1 40,40 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0,20 A 40,40 0 0 1 15,0 H 55 A 60,60 0 0 1 55,40 H 15 A 40,40, 0 0 1 0,20 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0 10 L 10,0 H 50 L 60,10 V 40 H 0 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0,20 L 10,0  H 50 L 60,20 L 50,40 H10 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0 0 H 60 L 50 40 H 10 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 0 0 H 60 V 20 L 30, 40 L 0, 20 Z", typeof(PathItemViewModel)));
            pathToolBoxItems.Add(new PathToolBoxData("M 9,2 11,7 17,7 12,10 14,15 9,12 4,15 6,10 1,7 7,7 Z", typeof(PathItemViewModel)));
            ToolBoxCategory.Add(new ToolBoxCategory() { Header = "Path", ToolBoxItems = new ObservableCollection<ToolBoxData>(pathToolBoxItems) });

            List<ToolBoxData> logicalChartToolBoxItems = new List<ToolBoxData>();
            logicalChartToolBoxItems.Add(new TextToolBoxData("0", typeof(ConstantDesignerItemViewModel), 32, 20));
            logicalChartToolBoxItems.Add(new TextToolBoxData("In", typeof(InputItemViewModel), 32, 20));
            logicalChartToolBoxItems.Add(new TextToolBoxData("Out", typeof(OutputItemViewModel), 32, 20));
            logicalChartToolBoxItems.Add(new EllipseTextToolBoxData("1s", typeof(TimerDesignerItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("ADD", typeof(AddGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("SUB", typeof(SubtractGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("MUL", typeof(MultiplyGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("DIV", typeof(DivideGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("AVE", typeof(AverageGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("MOD", typeof(MODGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("AND", typeof(ANDGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("OR", typeof(ORGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("XOR", typeof(XORGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("NOT", typeof(NOTGateItemViewModel)));


            logicalChartToolBoxItems.Add(new TextToolBoxData("SHL", typeof(SHLGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("SHR", typeof(SHRGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("ROL", typeof(ROLGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("ROR", typeof(RORGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("SEL", typeof(SELGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("MAX", typeof(MAXGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("MIN", typeof(MINGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("LIMIT", typeof(LIMITGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("GT", typeof(GTGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("LT", typeof(LTGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("GE", typeof(GEGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("LE", typeof(LEGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("EQ", typeof(EQGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("NE", typeof(NEGateItemViewModel)));


            logicalChartToolBoxItems.Add(new TextToolBoxData("ABS", typeof(ABSGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("SQRT", typeof(SQRTGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("LN", typeof(LNGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("LOG", typeof(LOGGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("EXP", typeof(EXPGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("SIN", typeof(SINGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("COS", typeof(COSGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("TAN", typeof(TANGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("ASIN", typeof(ASINGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("ACOS", typeof(ACOSGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("ATAN", typeof(ATANGateItemViewModel)));
            logicalChartToolBoxItems.Add(new TextToolBoxData("EXPT", typeof(EXPTGateItemViewModel)));

            ToolBoxCategory.Add(new ToolBoxCategory() { Header = "逻辑图", ToolBoxItems = new ObservableCollection<ToolBoxData>(logicalChartToolBoxItems) });

            List<ToolBoxData> flowchartToolBoxItems = new List<ToolBoxData>();
            flowchartToolBoxItems.Add(new FlowchartToolBoxData(NodeKinds.Start, typeof(StartFlowNode)));
            flowchartToolBoxItems.Add(new FlowchartToolBoxData(NodeKinds.End, typeof(EndFlowNode)));
            flowchartToolBoxItems.Add(new FlowchartToolBoxData(NodeKinds.Middle, typeof(MiddleFlowNode)));
            flowchartToolBoxItems.Add(new FlowchartToolBoxData(NodeKinds.Decide, typeof(DecideFlowNode)));
            flowchartToolBoxItems.Add(new FlowchartToolBoxData(NodeKinds.COBegin, typeof(COBeginFlowNode)));
            flowchartToolBoxItems.Add(new FlowchartToolBoxData(NodeKinds.COEnd, typeof(COEndFlowNode)));
            ToolBoxCategory.Add(new ToolBoxCategory() { Header = "流程图", ToolBoxItems = new ObservableCollection<ToolBoxData>(flowchartToolBoxItems) });

            List<ToolBoxData> mediaToolBoxItems = new List<ToolBoxData>();
            mediaToolBoxItems.Add(new MediaToolBoxData("../Images/GIF.png", typeof(GifImageItemViewModel)));
            mediaToolBoxItems.Add(new MediaToolBoxData("../Images/VIDEO.png", typeof(VideoItemViewModel)));
            mediaToolBoxItems.Add(new MediaToolBoxData("../Images/SVG.png", typeof(SvgDesignerItemViewModel)));
            ToolBoxCategory.Add(new ToolBoxCategory() { Header = "媒体", ToolBoxItems = new ObservableCollection<ToolBoxData>(mediaToolBoxItems) });

            LoadMyItems();
            LoadSvgItems();
        }

        private void Delete(ToolBoxCategory para)
        {
            para.IsChecked = false;
        }

        private string _svg = System.AppDomain.CurrentDomain.BaseDirectory + "Images\\Svgs";
        private void LoadSvgItems()
        {
            if (Directory.Exists(_svg))
            {
                var files = Directory.GetFiles(_svg);
                foreach (var filename in files.OrderBy(p => p, new NaturalStringComparer()).Where(p => p.ToLower().EndsWith(".svg")))
                {
                    SvgToolBoxCategory.ToolBoxItems.Add(new SvgToolBoxData(filename, typeof(SvgDesignerItemViewModel)));
                }
            }
        }

        private string _custom = System.AppDomain.CurrentDomain.BaseDirectory + "DesignItems\\Customs";

        private void LoadMyItems()
        {
            if (Directory.Exists(_custom))
            {
                var files = Directory.GetFiles(_custom);
                foreach (var filename in files.Where(p => p.ToLower().EndsWith(".json")))
                {
                    var itemBase = ReadMyItem(filename);
                    if (itemBase != null)
                    {
                        MyToolBoxCategory.ToolBoxItems.Add(new DesignerItemToolBoxData(itemBase, filename, TypeHelper.GetType(itemBase.ItemTypeName)));
                    }
                }
            }
        }

        private DesignerItemBase ReadMyItem(string filename)
        {
            try
            {
                var xmlobject = JsonConvert.DeserializeObject<DiagramDocument>(File.ReadAllText(filename));
                var diagramItemData = xmlobject.DiagramItems[0].AllDesignerItems[0];

                return diagramItemData;
            }
            catch
            {

            }

            return null;
        }

        private void AddMyItem()
        {
            if (_service.SelectedItem is DesignerItemViewModelBase designer)
            {
                DiagramDocument diagramDocument = new DiagramDocument();
                diagramDocument.DiagramItems = new List<DiagramItem>();
                DiagramItem diagramItem = new DiagramItem();
                diagramItem.AddItems(new List<DesignerItemViewModelBase> { designer });
                diagramDocument.DiagramItems.Add(diagramItem);
                string newname = NewNameHelper.GetNewName(MyToolBoxCategory.ToolBoxItems.OfType<DesignerItemToolBoxData>().Select(p => Path.GetFileNameWithoutExtension(p.FileName)),"");
                var filename = $"{_custom}\\{newname}.json";
                File.WriteAllText(filename, JsonConvert.SerializeObject(diagramDocument));

                var itemBase = ReadMyItem(filename);
                if (itemBase != null)
                {
                    MyToolBoxCategory.ToolBoxItems.Add(new DesignerItemToolBoxData(itemBase, filename, TypeHelper.GetType(itemBase.ItemTypeName)));
                }
            }
        }

        private void ImportMyItem()
        {

        }

        private void DeleteMyItem(ToolBoxData toolBox)
        {
            MyToolBoxCategory.ToolBoxItems.Remove(toolBox);
            if (toolBox is DesignerItemToolBoxData designer)
            {
                File.Delete(designer.FileName);
            }
        }
    }

    public class ToolBoxCategory : BindableBase
    {
        public string Header { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                SetProperty(ref _isExpanded, value);
            }
        }

        private bool _isChecked = true;
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                SetProperty(ref _isChecked, value);
            }
        }

        private ObservableCollection<ToolBoxData> _toolBoxItems;
        public ObservableCollection<ToolBoxData> ToolBoxItems
        {
            get
            {
                return _toolBoxItems;
            }
            set
            {
                SetProperty(ref _toolBoxItems, value);
            }
        }
    }

    public class MineToolBoxCategory : ToolBoxCategory
    {

    }
}
