using AIStudio.Wpf.ADiagram.Models;

namespace AIStudio.Wpf.ADiagram.Demos.Flowchart
{
    public class MiddleFlowNodeData : TitleBindableBase
    {
        public MiddleFlowNodeData()
        {
            Title = "审批";
        }

        private int _status = 100;
        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        private string _remark;
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                SetProperty(ref _remark, value);
            }
        }
    }
}
