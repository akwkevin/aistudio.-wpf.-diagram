using AIStudio.Wpf.BaseDiagram.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.Logical.ViewModels
{
    /// <summary>
    /// This is passed to the PopupWindow.xaml window, where a DataTemplate is used to provide the
    /// ContentControl with the look for this data. This class is also used to allow
    /// the popup to be cancelled without applying any changes to the calling ViewModel
    /// whos data will be updated if the PopupWindow.xaml window is closed successfully
    /// </summary>
    public class LogicalGateItemData : TitleBindableBase
    {
        public LogicalGateItemData(IEnumerable<FullyCreatedConnectorInfo> inputvalues)
        {
            this.InputValues = new ObservableCollection<FullyCreatedConnectorInfo>(inputvalues);           
        }

        private ObservableCollection<FullyCreatedConnectorInfo> _inputValues;
        public ObservableCollection<FullyCreatedConnectorInfo> InputValues
        {
            get
            {
                return _inputValues;
            }
            set
            {
                SetProperty(ref _inputValues, value);
            }
        }
    }
}
