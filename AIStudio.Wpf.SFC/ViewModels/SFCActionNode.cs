using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class SFCActionNode : SFCNode
    {
        public SFCActionNode() : base(SFCNodeKinds.Action)
        {
            FontViewModel.FontSize = 10;
            ItemWidth = 60;
            ItemHeight = 48;

            ExecuteAddLeftInput(null);
        }

        public SFCActionNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
            
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

        private LinkPoint _linkPoint;
        public LinkPoint LinkPoint
        {
            get
            {
                return _linkPoint;
            }
            set
            {
                SetProperty(ref _linkPoint, value);
            }
        }

        protected override void ExecuteEditCommand(object parameter)
        {
            SFCActionNodeData data = new SFCActionNodeData(LinkPoint, Expression);
            if (visualiserService.ShowDialog(data) == true)
            {
                this.LinkPoint = data.LinkPoint;
                this.Expression = data.Expression;
            }
        }
    }
}
