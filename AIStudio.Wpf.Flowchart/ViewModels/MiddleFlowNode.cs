using AIStudio.Wpf.BaseDiagram.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.Flowchart.ViewModels
{
    public class MiddleFlowNode : FlowNode
    {
        public MiddleFlowNode() : base(NodeKinds.Middle)
        {

        }

        public MiddleFlowNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }

        private List<string> _userIds = new List<string>();
        [Browsable(true)]
        [StyleName("UserIdsStyle")]
        public List<string> UserIds
        {
            get { return _userIds; }
            set
            {
                SetProperty(ref _userIds, value);
            }
        }

        private List<string> _roleIds = new List<string>();
        [Browsable(true)]
        [StyleName("RoleIdsStyle")]
        public List<string> RoleIds
        {
            get { return _roleIds; }
            set
            {
                SetProperty(ref _roleIds, value);
            }
        }

        private string _actType;
        [Browsable(true)]
        [StyleName("ActTypeStyle")]
        public string ActType
        {
            get { return _actType; }
            set
            {
                SetProperty(ref _actType, value);
            }
        }

        protected override void ExecuteEditCommand(object param)
        {
            if (IsReadOnly == true) return;

            if (Status == 1)
            {
                MiddleFlowNodeData data = new MiddleFlowNodeData();
                if (visualiserService.ShowDialog(data) == true)
                {
                    FlowchartService.Approve(this, data.Status, data.Remark);
                }
            }
            else
            {
                MessageBox.Show("该节点不能进行审批！！！");
            }
        }


    }
}
