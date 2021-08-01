using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class SFCConditionNode : SFCNode
    {
        public SFCConditionNode() : base(SFCNodeKinds.Condition)
        {
            ColorViewModel.LineColor.Color = Colors.Black;
            ItemWidth = 30;
            ItemHeight = 30;           

            ExecuteAddTopInput(null);
            ExecuteAddBottomOutput(null);
        }

        public SFCConditionNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }

        protected override void Init()
        {
            base.Init();
        }

        private bool _showText;
        public override bool ShowText
        {
            get
            {
                return false;
            }
            set
            {
                SetProperty(ref _showText, value);
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

        private string _para;
        public string Para
        {
            get
            {
                return _para;
            }
            set
            {
                _para = value;
            }
        }

        private ObservableCollection<LinkPoint> _linkPoint = new ObservableCollection<LinkPoint>();
        public ObservableCollection<LinkPoint> LinkPoint
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
            SFCConditionNodeData data = new SFCConditionNodeData(LinkPoint, Expression);
            if (visualiserService.ShowDialog(data) == true)
            {
                this.LinkPoint = new ObservableCollection<LinkPoint>(data.LinkPoint.Select(p => SFCService.LinkPoint.FirstOrDefault(q => q.Name == p.Name)));
                this.Expression = data.Expression;
            }
        }
    }
}
