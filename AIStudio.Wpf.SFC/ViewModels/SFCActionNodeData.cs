using AIStudio.Wpf.BaseDiagram.Commands;
using AIStudio.Wpf.BaseDiagram.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    /// <summary>
    /// This is passed to the PopupWindow.xaml window, where a DataTemplate is used to provide the
    /// ContentControl with the look for this data. This class is also used to allow
    /// the popup to be cancelled without applying any changes to the calling ViewModel
    /// whos data will be updated if the PopupWindow.xaml window is closed successfully
    /// </summary>
    public class SFCActionNodeData : TitleBindableBase
    {
        public SFCActionNodeData(LinkPoint linkPoint, string expression)
        {
            Title = "输出动作";
  
            LinkPoint = linkPoint;
            Expression = expression;
        }

        private string _expression;
        public string Expression
        {
            get
            {
                return _expression;
            }
            set
            {
                SetProperty(ref _expression, value);
            }
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
