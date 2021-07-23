using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Util.DiagramDesigner
{
    /// <summary>
    /// Interaction logic for ConnectorContainer.xaml
    /// </summary>
    public partial class ConnectorContainer : ItemsControl
    {
        private Canvas rootCanvas;
        public ConnectorContainer()
        {
            InitializeComponent();
            ((INotifyCollectionChanged)Items).CollectionChanged += ConnectorContainer_CollectionChanged;
        }

        void ConnectorContainer_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach(var item in e.NewItems)
                {
                    FullyCreatedConnectorInfo vm = item as FullyCreatedConnectorInfo;
                    var connector = ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;

                    Canvas.SetLeft(connector, vm.DataItem.ItemWidth * vm.XRatio);
                    Canvas.SetTop(connector, vm.DataItem.ItemHeight * vm.YRatio);
                }
                SetConnectorLocation();
            }
        }

         void ConnectorContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetConnectorLocation();
        }

         private void rootCanvas_Loaded(object sender, RoutedEventArgs e)
         {
             rootCanvas = sender as Canvas;
             SetConnectorLocation();
             SizeChanged += ConnectorContainer_SizeChanged;
         }

         private void SetConnectorLocation()
         {
            foreach (var connector in rootCanvas.Children.OfType<ContentPresenter>())
            {
                var vm = connector.DataContext as FullyCreatedConnectorInfo;
                if (vm != null)
                {
                    Canvas.SetLeft(connector, vm.DataItem.ItemWidth * vm.XRatio);
                    Canvas.SetTop(connector, vm.DataItem.ItemHeight * vm.YRatio);
                }
            }
        }
    }
}
