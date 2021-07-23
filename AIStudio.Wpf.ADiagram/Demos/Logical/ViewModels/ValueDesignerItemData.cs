using AIStudio.Wpf.ADiagram.Models;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Demos.Logical
{
    /// <summary>
    /// This is passed to the PopupWindow.xaml window, where a DataTemplate is used to provide the
    /// ContentControl with the look for this data. This class is also used to allow
    /// the popup to be cancelled without applying any changes to the calling ViewModel
    /// whos data will be updated if the PopupWindow.xaml window is closed successfully
    /// </summary>
    public class ValueDesignerItemData : TitleBindableBase
    {
        public ValueDesignerItemData(double value)
        {
            this.Value = value;           
        }

        private double _value;
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                SetProperty(ref _value, value);
            }
        }
    }
}
