using Util.DiagramDesigner;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public class TextDesignerItemViewModel : DesignerItemViewModelBase
    {

        public TextDesignerItemViewModel(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }

        public TextDesignerItemViewModel()
        {

        }

        protected override void Init()
        {
            base.Init();

            this.ItemWidth = 150;
            this.ClearConnectors();
        }

        protected override void ExecuteEditCommand(object param)
        {
            if (IsReadOnly == true) return;
        }

        private string _watermark = "请输入文本";
        public string Watermark
        {
            get
            {
                return _watermark;
            }
            set
            {
                SetProperty(ref _watermark, value);
            }
        }

        //固定在DataTemplate中处理
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
    }
}
