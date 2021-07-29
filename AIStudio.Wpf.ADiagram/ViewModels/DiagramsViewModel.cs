using AIStudio.Wpf.BaseDiagram.Commands;
using AIStudio.Wpf.Flowchart;
using AIStudio.Wpf.BaseDiagram.Extensions.ViewModels;
using AIStudio.Wpf.BaseDiagram.Helpers;
using AIStudio.Wpf.ADiagram.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Util.DiagramDesigner;
using ZXing;

namespace AIStudio.Wpf.ADiagram.ViewModels
{
    public partial class DiagramsViewModel : BindableBase
    {
        private IDiagramServiceProvider _service { get { return DiagramServicesProvider.Instance.Provider; } }
       
        public DiagramsViewModel(string title, string status, DiagramType diagramType)
        {
            Title = title;
            Status = status;
            DiagramType = diagramType;

            DiagramViewModels = new ObservableCollection<IDiagramViewModel>()
            {
                new DiagramViewModel(){Name= "页-1", DiagramType = diagramType},
            };
            DiagramViewModel = DiagramViewModels.FirstOrDefault();

            Init();
        }
        public DiagramsViewModel(string filename)
        {
            FileName = filename;
            OpenFile(filename);
        }

        protected virtual void InitDiagramViewModel()
        {

        }

        protected virtual void Init()
        {
            InitDiagramViewModel();
        }

        public string FileName { get; set; }

        #region 属性

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        private bool _showGrid;
        public bool ShowGrid
        {
            get
            {
                return _showGrid;
            }
            set
            {
                if (SetProperty(ref _showGrid, value))
                {
                    foreach (var item in DiagramViewModels)
                    {
                        item.ShowGrid = _showGrid;
                    }
                }
            }
        }

        public DiagramType DiagramType { get; set; }

        private double OffsetX = 10;
        private double OffsetY = 10;

        private ObservableCollection<IDiagramViewModel> _diagramViewModels;
        public ObservableCollection<IDiagramViewModel> DiagramViewModels
        {
            get
            {
                return _diagramViewModels;
            }
            set
            {
                SetProperty(ref _diagramViewModels, value);
            }
        }

        private IDiagramViewModel _diagramViewModel;
        public IDiagramViewModel DiagramViewModel
        {
            get
            {
                return _diagramViewModel;
            }
            set
            {
                if (_diagramViewModel != value)
                {
                    if (_diagramViewModel != null)
                    {
                        _diagramViewModel.PropertyChanged -= DiagramViewModel_PropertyChanged;
                        _diagramViewModel.OutAddVerify -= AddVerify;
                    }
                    SetProperty(ref _diagramViewModel, value);
                    if (_diagramViewModel != null)
                    {
                        _diagramViewModel.PropertyChanged += DiagramViewModel_PropertyChanged;
                        _diagramViewModel.OutAddVerify += AddVerify;
                    }
                }
            }
        }
        #endregion

        #region 

        private void DiagramViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                _service.SelectedItem = DiagramViewModel.SelectedItems?.FirstOrDefault();
            }

            var property = sender.GetType().GetProperty(e.PropertyName);
            var attr = property.GetCustomAttributes(typeof(BrowsableAttribute), true);
            if (attr != null && attr.Length != 0 && (attr[0] as BrowsableAttribute).Browsable == false)
            {
                return;
            }

            Status = "*";
        }

        #endregion

        protected virtual bool AddVerify(SelectableDesignerItemViewModelBase arg)
        {
            return true;
        }

        public void ReDoExecuted()
        {
            DiagramViewModel.RedoCommand.Execute(null);
        }

        public void UnDoExecuted()
        {
            DiagramViewModel.UndoCommand.Execute(null);
        }

        public void SelectedAllExecuted()
        {
            DiagramViewModel.SelectAllCommand.Execute(null);
        }

        private void OpenFile(string filename)
        {
            try
            {
                DiagramDocument diagramDocument = null;

                if (filename.ToLower().EndsWith(".xml"))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(DiagramDocument));
                    FileInfo fileInfo = new FileInfo(filename);

                    using (TextReader reader = fileInfo.OpenText())
                    {
                        diagramDocument = (DiagramDocument)serializer.Deserialize(reader);
                    }
                }
                else
                {
                    diagramDocument = JsonConvert.DeserializeObject<DiagramDocument>(File.ReadAllText(filename));
                }

                Title = diagramDocument.Title;
                DiagramType = diagramDocument.DiagramType;

                List<DiagramViewModel> viewModels = new List<DiagramViewModel>();
                foreach (var diagramitem in diagramDocument.DiagramItems)
                {
                    var viewModel = new DiagramViewModel();
                    viewModel.Name = diagramitem.Name;
                    viewModel.DiagramType = diagramitem.DiagramType;

                    foreach (DesignerItemBase diagramItemData in diagramitem.AllDesignerItems)
                    {
                        Type type = TypeHelper.GetType(diagramItemData.ItemTypeName);

                        DesignerItemViewModelBase itemBase = (DesignerItemViewModelBase)Activator.CreateInstance(type, viewModel, diagramItemData);
                        viewModel.Items.Add(itemBase);
                    }

                    foreach (var connection in diagramitem.Connections)
                    {
                        connection.SourceType = System.Type.GetType(connection.SourceTypeName);
                        connection.SinkType = System.Type.GetType(connection.SinkTypeName);
                        DesignerItemViewModelBase sourceItem = GetConnectorDataItem(viewModel, connection.SourceId, connection.SourceType);
                        ConnectorOrientation sourceConnectorOrientation = connection.SourceOrientation;
                        FullyCreatedConnectorInfo sourceConnectorInfo = GetFullConnectorInfo(connection.Id, sourceItem, sourceConnectorOrientation, connection.SourceXRatio, connection.SourceYRatio, connection.SourceInnerPoint);

                        DesignerItemViewModelBase sinkItem = GetConnectorDataItem(viewModel, connection.SinkId, connection.SinkType);
                        ConnectorOrientation sinkConnectorOrientation = connection.SinkOrientation;
                        FullyCreatedConnectorInfo sinkConnectorInfo = GetFullConnectorInfo(connection.Id, sinkItem, sinkConnectorOrientation, connection.SinkXRatio, connection.SinkYRatio, connection.SinkInnerPoint);

                        ConnectorViewModel connectionVM = new ConnectorViewModel(viewModel, sourceConnectorInfo, sinkConnectorInfo, connection);
                        viewModel.Items.Add(connectionVM);
                    }

                    viewModels.Add(viewModel);
                }
                DiagramViewModels = new ObservableCollection<IDiagramViewModel>(viewModels);
                DiagramViewModel = DiagramViewModels.FirstOrDefault();
            }
            catch (System.IO.FileNotFoundException fnfe)
            {
                throw new FileNotFoundException("The system document could not be found ", fnfe);
            }
            catch (System.IO.DirectoryNotFoundException dnfe)
            {
                throw new DirectoryNotFoundException("A required directory was nt found", dnfe);
            }
            catch (System.IO.IOException ioe)
            {
                throw new IOException("A file system error occurred", ioe);
            }
            catch (System.UnauthorizedAccessException uae)
            {
                throw new UnauthorizedAccessException("The requested file system access wasnot granted", uae);
            }
            catch (System.Security.SecurityException se)
            {
                throw new System.Security.SecurityException("The security policy prevents access to a file system resource", se);
            }
            catch (System.Exception e)
            {
                throw new System.Exception(
                    string.Format("The database format vc  invalid \r\n Exception:{0} \r\n InnerException:{1}", e.Message, e.InnerException.Message));
            }
        }

        public bool SaveFile(bool isSaveAs = false)
        {
            string filter = "Files (*.xml)|*.xml|Files (*.json)|*.json|All Files (*.*)|*.*";

            if (string.IsNullOrEmpty(FileName) || isSaveAs == true)
            {
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.Filter = filter;
                if (saveFile.ShowDialog() == true)
                {
                    FileName = saveFile.FileName;
                    Title = Path.GetFileNameWithoutExtension(FileName);
                }
                else
                {
                    return false;
                }
            }

            var para = Path.GetExtension(FileName);

            DiagramDocument diagramDocument = new DiagramDocument();
            diagramDocument.DiagramItems = new List<DiagramItem>();
            diagramDocument.Title = Title;
            diagramDocument.DiagramType = DiagramType;

            foreach (var viewModel in DiagramViewModels)
            {
                DiagramItem diagramItem = new DiagramItem();
                diagramItem.Name = viewModel.Name;
                diagramItem.DiagramType = viewModel.DiagramType;

                diagramItem.AddItems(DiagramViewModel.Items);            

                foreach (var connectionVM in DiagramViewModel.Items.OfType<ConnectorViewModel>())
                {

                    FullyCreatedConnectorInfo sinkConnector = connectionVM.SinkConnectorInfo as FullyCreatedConnectorInfo;

                    ConnectionItem connection = new ConnectionItem(
                        connectionVM.SourceConnectorInfo.DataItem.Id,
                        connectionVM.SourceConnectorInfo.Orientation,
                        DiagramItem.GetTypeOfDiagramItem(connectionVM.SourceConnectorInfo.DataItem),
                        GetXRatioFromConnector(connectionVM.SourceConnectorInfo),
                        GetYRatioFromConnector(connectionVM.SourceConnectorInfo),
                        connectionVM.SourceConnectorInfo.IsInnerPoint,
                        sinkConnector.DataItem.Id,
                        sinkConnector.Orientation,
                        DiagramItem.GetTypeOfDiagramItem(sinkConnector.DataItem),
                        GetXRatioFromConnector(sinkConnector),
                        GetYRatioFromConnector(sinkConnector),
                        sinkConnector.IsInnerPoint,
                        connectionVM);

                    diagramItem.ConnectionIds.Add(connectionVM.Id);
                    diagramItem.Connections.Add(connection);
                }

                diagramDocument.DiagramItems.Add(diagramItem);
            }

            if (para == ".xml")
            {
                FileInfo file = new FileInfo(FileName);
                diagramDocument.Save(file);
            }
            else
            {
                File.WriteAllText(FileName, JsonConvert.SerializeObject(diagramDocument));
            }
            Status = "";

            return true;
        }


        public void Paste()
        {
            if (Clipboard.ContainsData(DataFormats.Xaml))
            {
                String clipboardData = Clipboard.GetData(DataFormats.Xaml) as String;

                if (String.IsNullOrEmpty(clipboardData))
                    return;
                try
                {
                    List<SelectableDesignerItemViewModelBase> items = new List<SelectableDesignerItemViewModelBase>();
                    DiagramItem copyitem = XmlSerializeHelper.DESerializer<DiagramItem>(clipboardData);


                    Dictionary<Guid, Guid> mappingOldToNewIDs = new Dictionary<Guid, Guid>();

                    foreach (var diagramItemData in copyitem.AllDesignerItems)
                    {
                        DesignerItemViewModelBase newItem = null;

                        Guid newID = Guid.NewGuid();
                        mappingOldToNewIDs.Add(diagramItemData.Id, newID);

                        diagramItemData.Id = newID;
                        diagramItemData.Left += OffsetX;

                        diagramItemData.Top += OffsetY;
                        Type type = TypeHelper.GetType(diagramItemData.ItemTypeName);

                        DesignerItemViewModelBase itemBase = (DesignerItemViewModelBase)Activator.CreateInstance(type, DiagramViewModel, diagramItemData);
                        newItem = itemBase;

                        if (newItem != null)
                        {
                            if (newItem.ParentId != Guid.Empty)
                            {
                                newItem.ParentId = mappingOldToNewIDs[newItem.ParentId];
                            }
                            items.Add(newItem);
                        }
                    }


                    OffsetX += 10;
                    OffsetY += 10;

                    foreach (var connection in copyitem.Connections)
                    {
                        Guid newID = Guid.NewGuid();

                        connection.SourceId = mappingOldToNewIDs[connection.SourceId];
                        connection.SinkId = mappingOldToNewIDs[connection.SinkId];

                        connection.SourceType = System.Type.GetType(connection.SourceTypeName);
                        connection.SinkType = System.Type.GetType(connection.SinkTypeName);
                        DesignerItemViewModelBase sourceItem = GetConnectorDataItem(DiagramViewModel, connection.SourceId, connection.SourceType);
                        ConnectorOrientation sourceConnectorOrientation = connection.SourceOrientation;
                        FullyCreatedConnectorInfo sourceConnectorInfo = GetFullConnectorInfo(connection.Id, sourceItem, sourceConnectorOrientation, connection.SourceXRatio, connection.SourceYRatio, connection.SourceInnerPoint);

                        DesignerItemViewModelBase sinkItem = GetConnectorDataItem(DiagramViewModel, connection.SinkId, connection.SinkType);
                        ConnectorOrientation sinkConnectorOrientation = connection.SinkOrientation;
                        FullyCreatedConnectorInfo sinkConnectorInfo = GetFullConnectorInfo(connection.Id, sinkItem, sinkConnectorOrientation, connection.SinkXRatio, connection.SinkYRatio, connection.SinkInnerPoint);

                        ConnectorViewModel connectionVM = new ConnectorViewModel(DiagramViewModel, sourceConnectorInfo, sinkConnectorInfo, connection);
                        items.Add(connectionVM);
                    }

                    DiagramViewModel.DirectAddItemCommand.Execute(items);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void CopyCurrentSelection()
        {
            IEnumerable<SelectableDesignerItemViewModelBase> selectedDesignerItems =
            DiagramViewModel.SelectedItems.OfType<SelectableDesignerItemViewModelBase>();

            List<ConnectorViewModel> selectedConnections =
                DiagramViewModel.SelectedItems.OfType<ConnectorViewModel>().ToList();

            foreach (ConnectorViewModel connection in DiagramViewModel.Items.OfType<ConnectorViewModel>())
            {
                if (!selectedConnections.Contains(connection))
                {
                    DesignerItemViewModelBase sourceItem = (from item in selectedDesignerItems.OfType<DesignerItemViewModelBase>()
                                                            where item.Id == connection.SourceConnectorInfo.DataItem.Id
                                                            select item).FirstOrDefault();

                    DesignerItemViewModelBase sinkItem = (from item in DiagramViewModel.SelectedItems.OfType<DesignerItemViewModelBase>()
                                                          where item.Id == ((connection.SinkConnectorInfo as FullyCreatedConnectorInfo).DataItem).Id
                                                          select item).FirstOrDefault();

                    if (sourceItem != null &&
                        sinkItem != null &&
                        DiagramViewModel.BelongToSameGroup(sourceItem, sinkItem))
                    {
                        selectedConnections.Add(connection);
                    }
                }
            }

            DiagramItem copyitem = new DiagramItem();
            copyitem.AddItems(selectedDesignerItems);       

            foreach (var connectionVM in selectedConnections.OfType<ConnectorViewModel>())
            {
                FullyCreatedConnectorInfo sinkConnector = connectionVM.SinkConnectorInfo as FullyCreatedConnectorInfo;

                ConnectionItem connection = new ConnectionItem(
                   connectionVM.SourceConnectorInfo.DataItem.Id,
                   connectionVM.SourceConnectorInfo.Orientation,
                   DiagramItem.GetTypeOfDiagramItem(connectionVM.SourceConnectorInfo.DataItem),
                   GetXRatioFromConnector(connectionVM.SourceConnectorInfo),
                   GetYRatioFromConnector(connectionVM.SourceConnectorInfo),
                   connectionVM.SourceConnectorInfo.IsInnerPoint,
                   sinkConnector.DataItem.Id,
                   sinkConnector.Orientation,
                   DiagramItem.GetTypeOfDiagramItem(sinkConnector.DataItem),
                   GetXRatioFromConnector(sinkConnector),
                   GetYRatioFromConnector(sinkConnector),
                   sinkConnector.IsInnerPoint,
                   connectionVM);

                copyitem.ConnectionIds.Add(connectionVM.Id);
                copyitem.Connections.Add(connection);
            }
            string xml = XmlSerializeHelper.XmlSerialize<DiagramItem>(copyitem);

            OffsetX = 10;
            OffsetY = 10;
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, xml);
        }

        public void DeleteCurrentSelection()
        {
            List<SelectableDesignerItemViewModelBase> itemsToRemove = DiagramViewModel.SelectedItems.OfType<SelectableDesignerItemViewModelBase>().ToList();
            List<SelectableDesignerItemViewModelBase> connectionsToAlsoRemove = new List<SelectableDesignerItemViewModelBase>();

            foreach (var connector in DiagramViewModel.Items.OfType<ConnectorViewModel>())
            {
                if (ItemsToDeleteHasConnector(itemsToRemove, connector.SourceConnectorInfo))
                {
                    connectionsToAlsoRemove.Add(connector);
                }

                if (ItemsToDeleteHasConnector(itemsToRemove, connector.SinkConnectorInfo))
                {
                    connectionsToAlsoRemove.Add(connector);
                }

            }
            itemsToRemove.AddRange(connectionsToAlsoRemove);

            DiagramViewModel.RemoveItemCommand.Execute(itemsToRemove);
        }

        public void CutCurrentSelection()
        {
            CopyCurrentSelection();
            OffsetX = 0;
            OffsetY = 0;
            DeleteCurrentSelection();
        }

        private double GetXRatioFromConnector(FullyCreatedConnectorInfo info)
        {
            if (info.IsInnerPoint)
            {
                return info.XRatio;
            }
            else
            {
                switch (info.Orientation)
                {
                    case ConnectorOrientation.Top:
                        return 0.5;
                    case ConnectorOrientation.Left:
                        return 0;
                    case ConnectorOrientation.Bottom:
                        return 0.5;
                    case ConnectorOrientation.Right:
                        return 1;
                    default: return info.XRatio;
                }
            }
        }

        private double GetYRatioFromConnector(FullyCreatedConnectorInfo info)
        {
            if (info.IsInnerPoint)
            {
                return info.YRatio;
            }
            else
            {
                switch (info.Orientation)
                {
                    case ConnectorOrientation.Top:
                        return 0;
                    case ConnectorOrientation.Left:
                        return 0.5;
                    case ConnectorOrientation.Bottom:
                        return 1;
                    case ConnectorOrientation.Right:
                        return 0.5;
                    default: return info.YRatio;
                }
            }
        }

       

        private DesignerItemViewModelBase GetConnectorDataItem(IDiagramViewModel diagramViewModel, Guid conectorDataItemId, Type connectorDataItemType)
        {
            DesignerItemViewModelBase dataItem = diagramViewModel.Items.OfType<DesignerItemViewModelBase>().Single(x => x.Id == conectorDataItemId);
            return dataItem;
        }

        private FullyCreatedConnectorInfo GetFullConnectorInfo(Guid connectorId, DesignerItemViewModelBase dataItem, ConnectorOrientation connectorOrientation, double xRatio, double yRatio, bool isInnerPoint)
        {
            if (isInnerPoint)
            {
                return dataItem.Connectors.Where(p => p.XRatio == xRatio && p.YRatio == yRatio).FirstOrDefault();
            }
            else
            {
                switch (connectorOrientation)
                {
                    case ConnectorOrientation.Top:
                        return dataItem.TopConnector;
                    case ConnectorOrientation.Left:
                        return dataItem.LeftConnector;
                    case ConnectorOrientation.Right:
                        return dataItem.RightConnector;
                    case ConnectorOrientation.Bottom:
                        return dataItem.BottomConnector;

                    default:
                        throw new InvalidOperationException(
                            string.Format("Found invalid persisted Connector Orientation for Connector Id: {0}", connectorId));
                }
            }
        }

        private ConnectorViewModel GetSourceItem(FullyCreatedConnectorInfo sinkConnector)
        {
            foreach (var connector in DiagramViewModel.Items.OfType<ConnectorViewModel>())
            {
                if (connector.SinkConnectorInfo == sinkConnector)
                {
                    return connector;
                }
            }
            return null;
        }

        private bool ItemsToDeleteHasConnector(List<SelectableDesignerItemViewModelBase> itemsToRemove, ConnectorInfoBase connector)
        {
            if (connector is FullyCreatedConnectorInfo fully)
            {
                return itemsToRemove.Contains(fully.DataItem);
            }

            return false;
        }

        #region 布局
        public void AlignTopExecuted(object para)
        {
            DiagramViewModel.AlignTopCommand.Execute(null);
        }
        public void AlignVerticalCentersExecuted(object para)
        {
            DiagramViewModel.AlignVerticalCentersCommand.Execute(null);
        }
        public void AlignBottomExecuted(object para)
        {
            DiagramViewModel.AlignBottomCommand.Execute(null);
        }
        public void AlignLeftExecuted(object para)
        {
            DiagramViewModel.AlignLeftCommand.Execute(null);
        }
        public void AlignHorizontalCentersExecuted(object para)
        {
            DiagramViewModel.AlignHorizontalCentersCommand.Execute(null);
        }
        public void AlignRightExecuted(object para)
        {
            DiagramViewModel.AlignRightCommand.Execute(null);
        }
        public void BringForwardExecuted(object para)
        {
            DiagramViewModel.BringForwardCommand.Execute(null);
        }
        public void BringToFrontExecuted(object para)
        {
            DiagramViewModel.BringToFrontCommand.Execute(null);
        }
        public void SendBackwardExecuted(object para)
        {
            DiagramViewModel.SendBackwardCommand.Execute(null);
        }
        public void SendToBackExecuted(object para)
        {
            DiagramViewModel.SendToBackCommand.Execute(null);
        }
        public void DistributeHorizontalExecuted(object para)
        {
            DiagramViewModel.DistributeHorizontalCommand.Execute(null);
        }
        public void DistributeVerticalExecuted(object para)
        {
            DiagramViewModel.DistributeVerticalCommand.Execute(null);
        }
        public void SelectAllExecuted(object para)
        {
            DiagramViewModel.SelectAllCommand.Execute(null);
        }
        public void GroupExecuted(object para)
        {
            var items = from item in DiagramViewModel.SelectedItems.OfType<DesignerItemViewModelBase>()
                        where item.ParentId == Guid.Empty
                        select item;

            Rect rect = DiagramViewModel.GetBoundingRectangle(items);

            GroupDesignerItemViewModel groupItem = new GroupDesignerItemViewModel();
            groupItem.IsGroup = true;
            groupItem.ItemWidth = rect.Width;
            groupItem.ItemHeight = rect.Height;
            groupItem.Left = rect.Left;
            groupItem.Top = rect.Top;
            groupItem.ZIndex = DiagramViewModel.Items.Count;

            DiagramViewModel.DirectAddItemCommand.Execute(groupItem);

            foreach (DesignerItemViewModelBase item in items)
                item.ParentId = groupItem.Id;

            DiagramViewModel.ClearSelectedItemsCommand.Execute(null);
            //groupItem.IsSelected = true;
            DiagramViewModel.SelectionService.AddToSelection(groupItem);
        }

        public void UngroupExecuted(object para)
        {
            var groups = (from item in DiagramViewModel.SelectedItems.OfType<DesignerItemViewModelBase>()
                          where item.IsGroup && item.ParentId == Guid.Empty
                          select item).ToArray();

            foreach (DesignerItemViewModelBase groupRoot in groups)
            {
                var children = from child in DiagramViewModel.SelectedItems.OfType<DesignerItemViewModelBase>()
                               where child.ParentId == groupRoot.Id
                               select child;

                foreach (DesignerItemViewModelBase child in children)
                    child.ParentId = Guid.Empty;

                DiagramViewModel.RemoveItemCommand.Execute(groupRoot);
                DiagramViewModel.UpdateZIndex();
            }
        }
        #endregion

        #region 主题
        public void SetPropertyValue(SelectableDesignerItemViewModelBase selectable, string propertyName)
        {
            foreach (var item in DiagramViewModel.SelectedItems)
            {
                if (item != selectable)
                {
                    CopyHelper.CopyPropertyValue(selectable, item, propertyName);
                }
            }
        }

        public void SetFont(IFontViewModel fontViewModel, string propertyName)
        {
            foreach (var item in DiagramViewModel.SelectedItems)
            {
                if (item.FontViewModel != fontViewModel)
                {
                    CopyHelper.CopyPropertyValue(fontViewModel, item.FontViewModel, propertyName);
                }
            }
        }

        public void SetColor(IColorViewModel colorViewModel, string propertyName)
        {
            foreach (var item in DiagramViewModel.SelectedItems)
            {
                if (item.ColorViewModel != colorViewModel)
                {
                    CopyHelper.CopyPropertyValue(colorViewModel, item.ColorViewModel, propertyName);
                }
            }
        }

        public void SetQuickItem(IQuickThemeViewModel quickThemeViewModel, string propertyName)
        {
            if (propertyName == nameof(QuickTheme) && quickThemeViewModel.QuickTheme != null)
            {
                foreach (var item in DiagramViewModel.SelectedItems)
                {
                    SetFont(quickThemeViewModel.QuickTheme.FontViewModel, "FontColor");
                    SetColor(quickThemeViewModel.QuickTheme.ColorViewModel, "FillColor");
                    SetColor(quickThemeViewModel.QuickTheme.ColorViewModel, "LineColor");
                    SetColor(quickThemeViewModel.QuickTheme.ColorViewModel, "LineWidth");
                }
                quickThemeViewModel.QuickTheme = null;
            }
        }

        public void CenterExecuted(object para)
        {
            foreach (var item in DiagramViewModel.SelectedItems.OfType<DesignerItemViewModelBase>())
            {
                item.Left = (DiagramViewModel.PageSize.Width - item.ItemWidth) / 2;
                item.Top = (DiagramViewModel.PageSize.Height - item.ItemHeight) / 2;
            }
        }

        public void SameWidthExecuted(object para)
        {
            if (para is DesignerItemViewModelBase designerItem)
            {
                foreach (var item in DiagramViewModel.SelectedItems.OfType<DesignerItemViewModelBase>())
                {
                    item.ItemWidth = designerItem.ItemWidth;
                }
            }
        }

        public void SameHeightExecuted(object para)
        {
            if (para is DesignerItemViewModelBase designerItem)
            {
                foreach (var item in DiagramViewModel.SelectedItems.OfType<DesignerItemViewModelBase>())
                {
                    item.ItemHeight = designerItem.ItemHeight;
                }
            }
        }

        public void SameAngleExecuted(object para)
        {
            if (para is DesignerItemViewModelBase designerItem)
            {
                foreach (var item in DiagramViewModel.SelectedItems.OfType<DesignerItemViewModelBase>())
                {
                    item.Angle = designerItem.Angle;
                }
            }
        }

        public void SameSizeExecuted(object para)
        {
            if (para is DesignerItemViewModelBase designerItem)
            {
                foreach (var item in DiagramViewModel.SelectedItems.OfType<DesignerItemViewModelBase>())
                {
                    item.ItemWidth = designerItem.ItemWidth;
                    item.ItemHeight = designerItem.ItemHeight;
                }
            }
        }

        public void LockAction(LockObject lockObject, string propertyName)
        {
            foreach (var item in DiagramViewModel.SelectedItems)
            {
                item.LockObjectViewModel.SetValue(lockObject);
            }
        }

        public virtual void AddPageExecuted(object para)
        {
            int index = 0;
            if (para is DiagramViewModel oldpage)
            {
                index = DiagramViewModels.IndexOf(oldpage) + 1;
            }
            else
            {
                index = DiagramViewModels.Count;
            }
            var page = new DiagramViewModel() { Name = NewNameHelper.GetNewName(DiagramViewModels.Select(p => p.Name), "页-"), DiagramType = DiagramType };
            DiagramViewModels.Insert(index, page);
            DiagramViewModel = page;
            InitDiagramViewModel();
        }

        public void AddCopyPageExecuted(object para)
        {
            if (DiagramViewModel != null)
            {
                var viewModel = DiagramViewModel;
                DiagramItem diagramItem = new DiagramItem();
                diagramItem.Name = viewModel.Name;
                diagramItem.DiagramType = viewModel.DiagramType;

                diagramItem.AddItems(DiagramViewModel.Items);

                foreach (var connectionVM in DiagramViewModel.Items.OfType<ConnectorViewModel>())
                {
                    FullyCreatedConnectorInfo sinkConnector = connectionVM.SinkConnectorInfo as FullyCreatedConnectorInfo;

                    ConnectionItem connection = new ConnectionItem(
                        connectionVM.SourceConnectorInfo.DataItem.Id,
                        connectionVM.SourceConnectorInfo.Orientation,
                        DiagramItem.GetTypeOfDiagramItem(connectionVM.SourceConnectorInfo.DataItem),
                        GetXRatioFromConnector(connectionVM.SourceConnectorInfo),
                        GetYRatioFromConnector(connectionVM.SourceConnectorInfo),
                        connectionVM.SourceConnectorInfo.IsInnerPoint,
                        sinkConnector.DataItem.Id,
                        sinkConnector.Orientation,
                        DiagramItem.GetTypeOfDiagramItem(sinkConnector.DataItem),
                        GetXRatioFromConnector(sinkConnector),
                        GetYRatioFromConnector(sinkConnector),
                        sinkConnector.IsInnerPoint,
                        connectionVM);

                    diagramItem.ConnectionIds.Add(connectionVM.Id);
                    diagramItem.Connections.Add(connection);
                }

                viewModel = new DiagramViewModel();
                viewModel.Name = NewNameHelper.GetNewName(DiagramViewModels.Select(p => p.Name), "页-");
                viewModel.DiagramType = diagramItem.DiagramType;

                foreach (DesignerItemBase diagramItemData in diagramItem.AllDesignerItems)
                {
                    Type type = TypeHelper.GetType(diagramItemData.ItemTypeName);

                    DesignerItemViewModelBase itemBase = (DesignerItemViewModelBase)Activator.CreateInstance(type, viewModel, diagramItemData);
                    viewModel.Items.Add(itemBase);
                }

                foreach (var connection in diagramItem.Connections)
                {
                    connection.SourceType = System.Type.GetType(connection.SourceTypeName);
                    connection.SinkType = System.Type.GetType(connection.SinkTypeName);
                    DesignerItemViewModelBase sourceItem = GetConnectorDataItem(viewModel, connection.SourceId, connection.SourceType);
                    ConnectorOrientation sourceConnectorOrientation = connection.SourceOrientation;
                    FullyCreatedConnectorInfo sourceConnectorInfo = GetFullConnectorInfo(connection.Id, sourceItem, sourceConnectorOrientation, connection.SourceXRatio, connection.SourceYRatio, connection.SourceInnerPoint);

                    DesignerItemViewModelBase sinkItem = GetConnectorDataItem(viewModel, connection.SinkId, connection.SinkType);
                    ConnectorOrientation sinkConnectorOrientation = connection.SinkOrientation;
                    FullyCreatedConnectorInfo sinkConnectorInfo = GetFullConnectorInfo(connection.Id, sinkItem, sinkConnectorOrientation, connection.SinkXRatio, connection.SinkYRatio, connection.SinkInnerPoint);

                    ConnectorViewModel connectionVM = new ConnectorViewModel(viewModel, sourceConnectorInfo, sinkConnectorInfo, connection);
                    viewModel.Items.Add(connectionVM);
                }

                DiagramViewModels.Add(viewModel);
                DiagramViewModel = viewModel;
                InitDiagramViewModel();
            }
        }

        public void DeletePageExecuted(object para)
        {
            if (para is DiagramViewModel oldpage)
            {
                int index = DiagramViewModels.IndexOf(oldpage) - 1;
                DiagramViewModels.Remove(oldpage);
                if (index > 0)
                {
                    DiagramViewModel = DiagramViewModels[index];
                }
                else
                {
                    DiagramViewModel = DiagramViewModels.FirstOrDefault();
                }
            }

        }

        public void RenamePageExecuted(object para)
        {
            if (para is DiagramViewModel oldpage)
            {
                oldpage.IsEditName = true;
            }
        }

        public void EndRenamePageExecuted(object para)
        {
            if (para is DiagramViewModel oldpage)
            {
                oldpage.IsEditName = false;
            }
        }


        public void AddImageExecuted(object para)
        {
            ImageItemViewModel itemBase = new ImageItemViewModel();
            DiagramViewModel.DirectAddItemCommand.Execute(itemBase);
            if (itemBase.Parent != null)
            {
                _service.DrawModeViewModel.CursorMode = CursorMode.Move;
            }
        }

        public void EditImageExecuted(object para)
        {
            ImageItemViewModel itemBase = para as ImageItemViewModel;
            if (itemBase != null)
            {
                itemBase.EditData();
            }
        }

        public void ResizeImageExecuted(object para)
        {
            ImageItemViewModel itemBase = para as ImageItemViewModel;
            if (itemBase != null)
            {
                itemBase.StartResize();
            }
        }

        public void ResetImageExecuted(object para)
        {
            ImageItemViewModel itemBase = para as ImageItemViewModel;
            if (itemBase != null)
            {
                itemBase.Reset();
            }
        }

        public void AddVideoExecuted(object para)
        {
            VideoItemViewModel itemBase = new VideoItemViewModel();
            DiagramViewModel.DirectAddItemCommand.Execute(itemBase);
            if (itemBase.Parent != null)
            {
                _service.DrawModeViewModel.CursorMode = CursorMode.Move;
            }
        }

        public void AddOutLineTextExecuted(object para)
        {
            OutLineTextDesignerItemViewModel itemBase = new OutLineTextDesignerItemViewModel();
            DiagramViewModel.DirectAddItemCommand.Execute(itemBase);
            if (itemBase.Parent != null)
            {
                _service.DrawModeViewModel.CursorMode = CursorMode.Move;
            }
        }

        public void AddBarcodeExecuted(object para)
        {
            BarcodeDesignerItemViewModel itemBase = new BarcodeDesignerItemViewModel() { Format = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), para.ToString()), Text="AIStudio.Wpf.ADiagram" };
            DiagramViewModel.DirectAddItemCommand.Execute(itemBase);
            if (itemBase.Parent != null)
            {
                _service.DrawModeViewModel.CursorMode = CursorMode.Move;
            }
        }
        #endregion

       
        private Size MeasureString(OutLineTextDesignerItemViewModel itemBase)
        {
            var formattedText = new FormattedText(
                itemBase.Text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily(itemBase.FontViewModel.FontFamily), itemBase.FontViewModel.FontStyle, itemBase.FontViewModel.FontWeight, itemBase.FontViewModel.FontStretch),
                itemBase.FontViewModel.FontSize,
                Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }
    }
}
