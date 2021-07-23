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

namespace AIStudio.Wpf.ADiagram.Demos.Others
{
    public class OutLineTextDesignerItemViewModel : TextDesignerItemViewModel
    {
        private IUIVisualizerService visualiserService;

        public OutLineTextDesignerItemViewModel(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }

        public OutLineTextDesignerItemViewModel() : base()
        {

        }

        protected override void Init()
        {
            base.Init();

            visualiserService = ApplicationServicesProvider.Instance.Provider.VisualizerService;

            FontViewModel.FontFamily = "Arial";
            FontViewModel.FontSize = 36;
        }

        public void AutoSize()
        {
            var size = MeasureString();
            ItemWidth = size.Width;
            ItemHeight = size.Height;
        }

        private Size MeasureString()
        {
            var formattedText = new FormattedText(
                Text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily(FontViewModel.FontFamily), FontViewModel.FontStyle, FontViewModel.FontWeight, FontViewModel.FontStretch),
                FontViewModel.FontSize,
                Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }

        protected override void ExecuteEditCommand(object parameter)
        {
            EditData();
        }

        public override bool InitData()
        {
            if (string.IsNullOrEmpty(Text))
                return EditData();
            return true;
        }

        public override bool EditData()
        {
            if (IsReadOnly == true) return false;

            OutLineTextDesignerItemData data = new OutLineTextDesignerItemData(this);
            if (visualiserService.ShowDialog(data) == true)
            {
                Text = data.Text;
                FontViewModel = CopyHelper.Mapper<FontViewModel, IFontViewModel>(data.FontViewModel);
                AutoSize();
                return true;
            }

            return false;
        }
    }
}
