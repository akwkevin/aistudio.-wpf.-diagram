using AIStudio.Wpf.ADiagram.Services;
using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using ZXing;

namespace AIStudio.Wpf.ADiagram.Demos.Others
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
            ShowText = false;
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

        public override bool EditData()
        {
            if (IsReadOnly == true) return false;

            BarcodeDesignerItemData data = new BarcodeDesignerItemData(this);
            if (visualiserService.ShowDialog(data) == true)
            {
                bool needauto = Text == null;
                Text = data.Text;
                ShowText = false;
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
