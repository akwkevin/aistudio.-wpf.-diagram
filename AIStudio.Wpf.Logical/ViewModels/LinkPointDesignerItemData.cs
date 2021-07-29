using AIStudio.Wpf.BaseDiagram.Models;

namespace AIStudio.Wpf.Logical.ViewModels
{
    /// <summary>
    /// This is passed to the PopupWindow.xaml window, where a DataTemplate is used to provide the
    /// ContentControl with the look for this data. This class is also used to allow
    /// the popup to be cancelled without applying any changes to the calling ViewModel
    /// whos data will be updated if the PopupWindow.xaml window is closed successfully
    /// </summary>
    public class LinkPointDesignerItemData : TitleBindableBase
    {
        public LinkPointDesignerItemData(LinkPoint linkPoint)
        {
            this.LinkPoint = linkPoint;           
        }

        private LinkPoint _linkPoint;
        public LinkPoint LinkPoint
        {
            get
            {
                return _linkPoint;
            }
            set
            {
                SetProperty(ref _linkPoint, value);
            }
        }
    }
}
