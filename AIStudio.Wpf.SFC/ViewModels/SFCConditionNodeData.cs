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
    public class SFCConditionNodeData : TitleBindableBase
    {
        public SFCConditionNodeData(IEnumerable<LinkPoint> linkPoint, string expression)
        {
            Title = "转移条件";
            LinkPoint = new ObservableCollection<LinkPoint>(linkPoint);
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

        private ObservableCollection<LinkPoint> _linkPoint;
        public ObservableCollection<LinkPoint> LinkPoint
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

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                return this._addCommand ?? (this._addCommand = new DelegateCommand<object>(para => this.AddExecuted(para)));
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return this._deleteCommand ?? (this._deleteCommand = new DelegateCommand<object>(para => this.DeleteExecuted(para)));
            }
        }

      

        private void AddExecuted(object para)
        {
            LinkPoint.Add(new SFC.LinkPoint());
        }

        private void DeleteExecuted(object para)
        {
            LinkPoint.Remove(para as SFC.LinkPoint);
        }
    }
}
