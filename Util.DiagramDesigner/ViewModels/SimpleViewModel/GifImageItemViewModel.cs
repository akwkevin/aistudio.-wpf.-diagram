using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public class GifImageItemViewModel : DesignerItemViewModelBase
    {
        private IDisposable propertyChangedSubscription;
        private IDisposable connectorsChangedSubscription;
      

        public SimpleCommand AddItemCommand { get; private set; }
        public SimpleCommand ImageSwitchCommand { get; private set; }

        public GifImageItemViewModel() : base()
        {
        }

        public GifImageItemViewModel(IDiagramViewModel parent, MediaDesignerItem designer) : base(parent, designer) 
        {
          
        }


        protected override void Init()
        {
            AddItemCommand = new SimpleCommand(ExecuteAddItemCommand);
            ImageSwitchCommand = new SimpleCommand(ExecuteImageSwitchCommand);

            base.Init();

            ClearConnectors();
            //propertyChangedSubscription = WhenPropertyChanged.Where(o => o.ToString() == "Left" || o.ToString() == "Top" || o.ToString() == "ItemWidth" || o.ToString() == "ItemHeight").Subscribe(ChangeImageElement);
            connectorsChangedSubscription = WhenConnectorsChanged.Subscribe(OnConnectorsChanged);

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

        private bool _shouldInsertAnchor;
        public bool ShouldInsertAnchor
        {
            get { return _shouldInsertAnchor; }
            set
            {
                SetProperty(ref _shouldInsertAnchor, value);
            }
        }   

        private string dir = System.AppDomain.CurrentDomain.BaseDirectory + "Images\\Gifs";
        private void BuildMenuOptions()
        {
            if (Directory.Exists(dir))
            {
                menuOptions = new ObservableCollection<CinchMenuItem>();
                var equipmentImages = Directory.GetFiles(dir).Select(Path.GetFileName);
                foreach (var item in equipmentImages)
                {
                    CinchMenuItem menuItem = new CinchMenuItem();
                    menuItem.Text = item;
                    menuItem.Command = ImageSwitchCommand;
                    menuItem.CommandParameter = item;
                    menuOptions.Add(menuItem);
                }
            }
        }

        private void ExecuteAddItemCommand(object parameter)
        {
            FullyCreatedConnectorInfo connector = new FullyCreatedConnectorInfo(this, ConnectorOrientation.Top, true);
            MouseButtonEventArgs mosueArg = ((EventToCommandArgs)parameter).EventArgs as MouseButtonEventArgs;
            var position = mosueArg.GetPosition(((EventToCommandArgs)parameter).Sender as IInputElement);
            connector.XRatio = (position.X - connector.ConnectorWidth / 2) / connector.DataItem.ItemWidth;
            connector.YRatio = (position.Y - connector.ConnectorHeight / 2) / connector.DataItem.ItemHeight;
            AddConnector(connector);
        }

        private void ExecuteImageSwitchCommand(object parameter)
        {
            string image = parameter as string;
            string path = dir + @"\{0}";
            string filePath = string.Format(path, image);
            if (File.Exists(filePath))
            {
                Icon = filePath;
            }
        }

        private void OnConnectorsChanged(NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                if (args.NewItems.Count > 0)
                {
                    foreach (var item in args.NewItems)
                    {

                    }
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                if (args.OldItems.Count > 0)
                {
                    foreach (var item in args.OldItems)
                    {

                    }
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Reset)
            {

            }
        }

    }
}
