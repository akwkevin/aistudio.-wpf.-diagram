using AIStudio.Wpf.BaseDiagram;
using AIStudio.Wpf.BaseDiagram.Services;
using AIStudio.Wpf.Flowchart.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.Flowchart.ViewModels
{
    public class FlowNode : DesignerItemViewModelBase
    {
        protected IUIVisualizerService visualiserService;

        public FlowNode(NodeKinds kind) : base()
        {
            Kind = kind;
            Text = Kind.GetDescription();           
            ItemWidth = 80;
            ItemHeight = 40;

        }

        public FlowNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }

        protected override void Init()
        {
            base.Init();

            ShowRotate = false;
            ShowText = true;
            IsReadOnlyText = true;

            visualiserService = ApplicationServicesProvider.Instance.Provider.VisualizerService;
        }

        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            FlowNodeDesignerItem designer = designerbase as FlowNodeDesignerItem;
            this.Color = designer.Color;
            this.Kind = designer.Kind;
            this.StateImage = designer.StateImage;

            if (this is MiddleFlowNode middle)
            {
                middle.UserIds = designer.UserIds;
                middle.RoleIds = designer.RoleIds;
                middle.ActType = designer.ActType;
            }
        }

        private string _color;
        [Browsable(false)]
        public string Color
        {
            get { return _color; }
            set
            {
                SetProperty(ref _color, value);
            }
        } 

        [Browsable(false)]
        public NodeKinds Kind { get; set; }

        [Browsable(false)]
        public string StateImage { get; set; }

        #region 没有存起来，仅仅测试使用,实际这些代码应该都在服务端
        private int _status;
    
        public int Status
        {
            get { return _status; }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set
            {
                SetProperty(ref _remark, value);
            }
        }

        public List<string> PreStepId { get; set; }
        public string NextStepId { get; set; }
        public Dictionary<string, string> SelectNextStep { get; set; } = new Dictionary<string, string>();
        #endregion
    }

    public class StartFlowNode : FlowNode
    {
        public StartFlowNode() : base(NodeKinds.Start)
        {

        }

        public StartFlowNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class EndFlowNode : FlowNode
    {
        public EndFlowNode() : base(NodeKinds.End)
        {

        }

        public EndFlowNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class DecideFlowNode : FlowNode
    {
        public DecideFlowNode() : base(NodeKinds.Decide)
        {

        }

        public DecideFlowNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class COBeginFlowNode : FlowNode
    {
        public COBeginFlowNode() : base(NodeKinds.COBegin)
        {

        }

        public COBeginFlowNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }
    }

    public class COEndFlowNode : FlowNode
    {
        public COEndFlowNode() : base(NodeKinds.COEnd)
        {

        }

        public COEndFlowNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }
    }
}
