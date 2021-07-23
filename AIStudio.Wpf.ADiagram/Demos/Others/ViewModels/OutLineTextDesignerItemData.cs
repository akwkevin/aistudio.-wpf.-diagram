using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AIStudio.Wpf.ADiagram.Models;

namespace AIStudio.Wpf.ADiagram.Demos.Others
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
