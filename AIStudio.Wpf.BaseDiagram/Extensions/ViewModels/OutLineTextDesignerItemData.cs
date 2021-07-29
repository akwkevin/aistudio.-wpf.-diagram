using AIStudio.Wpf.BaseDiagram.Models;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.BaseDiagram.Extensions.ViewModels
{
    public class OutLineTextDesignerItemData : TitleBindableBase
    {
        public OutLineTextDesignerItemData()
        {

        }
        public OutLineTextDesignerItemData(OutLineTextDesignerItemViewModel item)
        {
            this.Title = "矢量文本";
            this.Text = item.Text;
            this.FontViewModel = CopyHelper.Mapper<FontViewModel, IFontViewModel>(item.FontViewModel);
        }

        private IFontViewModel _fontViewModel;
        public IFontViewModel FontViewModel
        {
            get
            {
                return _fontViewModel;
            }
            set
            {
                SetProperty(ref _fontViewModel, value);
            }
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                SetProperty(ref _text, value);
            }
        }

        

    }
}
