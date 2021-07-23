using AIStudio.Wpf.ADiagram.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Demos.Flowchart
{
    public class FlowNode : DesignerItemViewModelBase
    {
        public FlowNode(NodeKinds kind) : base()
        {
            Kind = kind;
            Text = Kind.GetDescription();
            ShowText = true;
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
        [Browsable(true)]
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
