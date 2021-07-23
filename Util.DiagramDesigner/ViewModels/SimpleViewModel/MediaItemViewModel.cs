using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Util.DiagramDesigner
{
    public class MediaItemViewModel : DesignerItemViewModelBase
    {
        protected virtual string Filter { get; set; } = "媒体·|*.*";

        public MediaItemViewModel() : base()
        {
 
        }

        public MediaItemViewModel(IDiagramViewModel parent, MediaDesignerItem designer) : base(parent, designer)
        {

        }

        protected override void Init()
        {
            base.Init();

            BuildMenuOptions();
        }      


        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            MediaDesignerItem designer = designerbase as MediaDesignerItem;

            this.Icon = designer.Icon;

            foreach (var connector in designer.Connectors)
            {
                FullyCreatedConnectorInfo fullyCreatedConnectorInfo = new FullyCreatedConnectorInfo(this, connector.Orientation, true);
                fullyCreatedConnectorInfo.XRatio = connector.XRatio;
                fullyCreatedConnectorInfo.YRatio = connector.YRatio;
                fullyCreatedConnectorInfo.ConnectorWidth = connector.ConnectorWidth;
                fullyCreatedConnectorInfo.ConnectorHeight = connector.ConnectorHeight;
                AddConnector(fullyCreatedConnectorInfo);
            }
        }

        private void BuildMenuOptions()
        {
            menuOptions = new ObservableCollection<CinchMenuItem>();
            CinchMenuItem menuItem = new CinchMenuItem();
            menuItem.Text = "更换";
            menuItem.Command = MenuItemCommand;
            menuItem.CommandParameter = menuItem;
            menuOptions.Add(menuItem);
        }

        private SimpleCommand _menuItemCommand;
        public SimpleCommand MenuItemCommand
        {
            get
            {
                return this._menuItemCommand ?? (this._menuItemCommand = new SimpleCommand(ExecuteMenuItemCommand));
            }
        }

        private void ExecuteMenuItemCommand(object obj)
        {
            EditData();
        }

        public override bool InitData()
        {
            if (string.IsNullOrEmpty(Icon))
                return EditData();
            return true;
        }
        public override bool EditData()
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            openFile.Filter = Filter;

            if (openFile.ShowDialog() == true)
            {
                Icon = openFile.FileName;
                return true;
            }

            return false;
        }

        protected override void ExecuteEditCommand(object param)
        {
        
        }
    }
}
