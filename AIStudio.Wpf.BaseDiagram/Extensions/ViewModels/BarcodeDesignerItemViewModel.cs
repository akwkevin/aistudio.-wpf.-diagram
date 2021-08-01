using AIStudio.Wpf.BaseDiagram.Services;
using System;
using Util.DiagramDesigner;
using ZXing;

namespace AIStudio.Wpf.BaseDiagram.Extensions.ViewModels
{
    public class BarcodeDesignerItemViewModel : DesignerItemViewModelBase
    {
        private IUIVisualizerService visualiserService;

        public BarcodeDesignerItemViewModel() : base()
        {

        }

        public BarcodeDesignerItemViewModel(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
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

            Format = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), (designerbase as DesignerItemBase).Reserve.ToString());
        }

        public void AutoSize()
        {
            ItemWidth = 140;
            ItemHeight = 140;
        }

        protected override void ExecuteEditCommand(object parameter)
        {
            EditData();
        }

        public override bool InitData()
        {
            if (string.IsNullOrEmpty(Icon))
                return EditData();
            return true;
        }

        public BarcodeFormat Format { get; set; } = BarcodeFormat.QR_CODE;

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

        public override bool EditData()
        {
            if (IsReadOnly == true) return false;

            BarcodeDesignerItemData data = new BarcodeDesignerItemData(this);
            if (visualiserService.ShowDialog(data) == true)
            {
                bool needauto = Text == null;
                Text = data.Text;
                Icon = data.Icon;
                Margin = data.Margin;
                if (needauto)
                {
                    AutoSize();
                }
                return true;
            }

            return false;
        }
    }
}
