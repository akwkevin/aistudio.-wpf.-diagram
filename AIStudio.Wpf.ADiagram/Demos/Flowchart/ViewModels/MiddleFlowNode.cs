using AIStudio.Wpf.ADiagram.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Demos.Flowchart
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
    }
}
