using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public class DiagramViewModel : BindableBase, IDiagramViewModel
    {
        #region 属性
        private PageSizeType _pageSizeType = PageSizeType.A4;
        public PageSizeType PageSizeType
        {
            get
            {
                return _pageSizeType;
            }
            set
            {
                SetProperty(ref _pageSizeType, value);
                RaisePropertyChanged(nameof(PageSize));
            }
        }

        private Size _pageSize = new Size(1000, 600);

        public Size PageSize
        {
            get
            {
                if (PageSizeOrientation == PageSizeOrientation.Vertical)
                {
                    return GetPageSize();
                }
                else
                {
                    return new Size(GetPageSize().Height, GetPageSize().Width);
                }
            }
            set
            {
                SetProperty(ref _pageSize, value);
            }
        }

        public Size GetPageSize()
        {
            switch (PageSizeType)
            {
                case PageSizeType.Letter: return new Size(612, 792);
                case PageSizeType.Folio: return new Size(612, 936);
                case PageSizeType.Legal: return new Size(612, 1008);
                case PageSizeType.Executive: return new Size(522, 756);
                case PageSizeType.Statement: return new Size(396, 612);
                case PageSizeType.Envelope: return new Size(297, 684);
                case PageSizeType.MonarchEnvelope: return new Size(279, 540);
                case PageSizeType.Tabloid: return new Size(792, 1224);
                case PageSizeType.LetterSmall: return new Size(612, 792);
                case PageSizeType.CSheet: return new Size(1224, 1584);
                case PageSizeType.DSheet: return new Size(1584, 2448);
                case PageSizeType.ESheet: return new Size(2448, 3168);
                case PageSizeType.A3: return new Size(842, 1191);
                case PageSizeType.A4: return new Size(595, 842);
                case PageSizeType.A5: return new Size(420, 595);
                case PageSizeType.B4: return new Size(709, 1003);
                case PageSizeType.B5: return new Size(516, 729);
                case PageSizeType.DLEnvelope: return new Size(312, 624);
                case PageSizeType.C5Envelope: return new Size(459, 649);
                case PageSizeType.Quarto: return new Size(609, 780);
                case PageSizeType.C6Quarto: return new Size(323, 459);
                case PageSizeType.B5Quarto: return new Size(499, 709);
                case PageSizeType.ItalyQuarto: return new Size(312, 652);
                case PageSizeType.A4small: return new Size(595, 842);
                case PageSizeType.GermanStdFanfold: return new Size(612, 864);
                case PageSizeType.GermanLegalFanfold: return new Size(576, 936);
                case PageSizeType.PRC16K: return new Size(414, 609);
                case PageSizeType.PRC32K: return new Size(275, 428);
                default: return _pageSize;
            }
        }

        private PageSizeOrientation _pageSizeOrientation;
        public PageSizeOrientation PageSizeOrientation
        {
            get
            {
                return _pageSizeOrientation;
            }
            set
            {
                SetProperty(ref _pageSizeOrientation, value);
                RaisePropertyChanged(nameof(PageSize));
            }
        }

        private PageUnit _pageUnit = PageUnit.cm;
        [Browsable(false)]
        public PageUnit PageUnit
        {
            get
            {
                return _pageUnit;
            }
            set
            {
                if (value != PageUnit.cm && value != PageUnit.inch)
                {
                    return;
                }
                SetProperty(ref _pageUnit, value);
            }
        }

        private Size _gridCellSize = new Size(50, 50);
        public Size GridCellSize
        {
            get
            {
                return _gridCellSize;
            }
            set
            {
                SetProperty(ref _gridCellSize, value);
            }
        }

        private Color _pageBackground = Colors.White;
        public Color PageBackground
        {
            get
            {
                return _pageBackground;
            }
            set
            {
                SetProperty(ref _pageBackground, value);
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
                SetProperty(ref _showGrid, value);
            }
        }

        private Color _gridColor = Colors.LightGray;
        public Color GridColor
        {
            get
            {
                return _gridColor;
            }
            set
            {
                SetProperty(ref _gridColor, value);
            }
        }

        private double _gridMargin = 28d;
        public double GridMargin
        {
            get
            {
                return _gridMargin;
            }
            set
            {
                SetProperty(ref _gridMargin, value);
            }
        }

        private double _zoomValue = 1;
        [Browsable(false)]
        public double ZoomValue
        {
            get
            {
                return _zoomValue;
            }
            set
            {
                SetProperty(ref _zoomValue, value);
            }
        }

        private string _name;
        [Browsable(false)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        private DiagramType _diagramType;
        [Browsable(false)]
        public DiagramType DiagramType
        {
            get
            {
                return _diagramType;
            }
            set
            {
                SetProperty(ref _diagramType, value);
            }
        }

        private CellHorizontalAlignment _cellHorizontalAlignment;
        [Browsable(false)]
        public CellHorizontalAlignment CellHorizontalAlignment
        {
            get
            {
                return _cellHorizontalAlignment;
            }
            set
            {
                SetProperty(ref _cellHorizontalAlignment, value);
            }
        }

        private CellVerticalAlignment _cellVerticalAlignment;
        [Browsable(false)]
        public CellVerticalAlignment CellVerticalAlignment
        {
            get
            {
                return _cellVerticalAlignment;
            }
            set
            {
                SetProperty(ref _cellVerticalAlignment, value);
            }
        }


        private bool _isEditName;
        [Browsable(false)]
        public bool IsEditName
        {
            get
            {
                return _isEditName;
            }
            set
            {
                SetProperty(ref _isEditName, value);
            }
        }

        private Point _currentPoint;
        [Browsable(false)]
        public Point CurrentPoint
        {
            get
            {
                return _currentPoint;
            }
            set
            {
                SetProperty(ref _currentPoint, value);
            }
        }

        private Color _currentColor;
        [Browsable(false)]
        public Color CurrentColor
        {
            get
            {
                return _currentColor;
            }
            set
            {
                SetProperty(ref _currentColor, value);
            }
        }
        #endregion

        private DoCommandManager DoCommandManager = new DoCommandManager();
        public DiagramViewModel()
        {
            CreateNewDiagramCommand = new SimpleCommand(ExecuteCreateNewDiagramCommand);
            AddItemCommand = new SimpleCommand(ExecuteAddItemCommand);
            DirectAddItemCommand = new SimpleCommand(ExecuteDirectAddItemCommand);
            RemoveItemCommand = new SimpleCommand(ExecuteRemoveItemCommand);
            DirectRemoveItemCommand = new SimpleCommand(ExecuteDirectRemoveItemCommand);
            ClearSelectedItemsCommand = new SimpleCommand(ExecuteClearSelectedItemsCommand);

            AlignTopCommand = new SimpleCommand(ExecuteAlignTopCommand);
            AlignVerticalCentersCommand = new SimpleCommand(ExecuteAlignVerticalCentersCommand);
            AlignBottomCommand = new SimpleCommand(ExecuteAlignBottomCommand);
            AlignLeftCommand = new SimpleCommand(ExecuteAlignLeftCommand);
            AlignHorizontalCentersCommand = new SimpleCommand(ExecuteAlignHorizontalCentersCommand);
            AlignRightCommand = new SimpleCommand(ExecuteAlignRightCommand);
            BringForwardCommand = new SimpleCommand(ExecuteBringForwardCommand);
            BringToFrontCommand = new SimpleCommand(ExecuteBringToFrontCommand);
            SendBackwardCommand = new SimpleCommand(ExecuteSendBackwardCommand);
            SendToBackCommand = new SimpleCommand(ExecuteSendToBackCommand);
            DistributeHorizontalCommand = new SimpleCommand(ExecuteDistributeHorizontalCommand);
            DistributeVerticalCommand = new SimpleCommand(ExecuteDistributeVerticalCommand);
            SelectAllCommand = new SimpleCommand(ExecuteSelectAllCommand);
            Mediator.Instance.Register(this);

            Items.CollectionChanged += Items_CollectionChanged;
        }

        #region UnDo ReDo

        private void Do(object sender, string propertyName, object newvalue)
        {
            sender.SetPropertyValue(propertyName, newvalue);
        }

        private void UnDo(object sender, string propertyName, object oldvalue)
        {
            sender.SetPropertyValue(propertyName, oldvalue);
        }

        private bool _undoing;
        private void UndoExecuted(object para)
        {
            _undoing = true;
            DoCommandManager.UnDo();
            _undoing = false;
        }
        private void RedoExecuted(object para)
        {
            _undoing = true;
            DoCommandManager.ReDo();
            _undoing = false;
        }
        private bool Undo_Enabled(object para)
        {
            return DoCommandManager.CanUnDo;
        }
        private bool Redo_Enabled(object para)
        {
            return DoCommandManager.CanReDo;
        }

        #endregion

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<SelectableDesignerItemViewModelBase>())
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                    item.Dispose();
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<SelectableDesignerItemViewModelBase>())
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }

            RaisePropertyChanged("Items");
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(sender, e.PropertyName);

            if (_undoing == true) return;

            //连续改变，需要特殊处理，不单独触发属性改变ReDo
            if (sender is DesignerItemViewModelBase designer)
            {
                if (designer.BeginDo) return;
            }

            if (e is ValuePropertyChangedEventArgs valuePropertyChangedEventArgs)
            {
                var property = sender.GetType().GetProperty(e.PropertyName);
                var attr = property.GetCustomAttributes(typeof(CanDoAttribute), true);
                if (attr != null && attr.Length != 0)
                {
                    DoCommandManager.DoNewCommand(sender.ToString() + e.PropertyName, () => Do(sender, e.PropertyName, valuePropertyChangedEventArgs.NewValue), () => UnDo(sender, e.PropertyName, valuePropertyChangedEventArgs.OldValue), null, false);
                }
            }
        }

        [MediatorMessageSink("DoneDrawingMessage")]
        public void OnDoneDrawingMessage(bool dummy)
        {
            foreach (var item in Items.OfType<DesignerItemViewModelBase>())
            {
                item.ShowConnectors = false;
            }
        }

        public SimpleCommand CreateNewDiagramCommand { get; private set; }
        public SimpleCommand DirectAddItemCommand { get; private set; }
        public SimpleCommand AddItemCommand { get; private set; }
        public SimpleCommand DirectRemoveItemCommand { get; private set; }
        public SimpleCommand RemoveItemCommand { get; private set; }
        public SimpleCommand ClearSelectedItemsCommand { get; private set; }
        public SimpleCommand AlignTopCommand { get; private set; }
        public SimpleCommand AlignVerticalCentersCommand { get; private set; }
        public SimpleCommand AlignBottomCommand { get; private set; }
        public SimpleCommand AlignLeftCommand { get; private set; }
        public SimpleCommand AlignHorizontalCentersCommand { get; private set; }
        public SimpleCommand AlignRightCommand { get; private set; }
        public SimpleCommand BringForwardCommand { get; private set; }
        public SimpleCommand BringToFrontCommand { get; private set; }
        public SimpleCommand SendBackwardCommand { get; private set; }
        public SimpleCommand SendToBackCommand { get; private set; }

        public SimpleCommand DistributeHorizontalCommand { get; private set; }
        public SimpleCommand DistributeVerticalCommand { get; private set; }
        public SimpleCommand SelectAllCommand { get; private set; }

        private SimpleCommand _undoCommand;
        public SimpleCommand UndoCommand
        {
            get
            {
                return this._undoCommand ?? (this._undoCommand = new SimpleCommand(Undo_Enabled, this.UndoExecuted));
            }
        }

        private SimpleCommand _redoCommand;
        public SimpleCommand RedoCommand
        {
            get
            {
                return this._redoCommand ?? (this._redoCommand = new SimpleCommand(Redo_Enabled, this.RedoExecuted));
            }
        }


        public ObservableCollection<SelectableDesignerItemViewModelBase> Items { get; set; } = new ObservableCollection<SelectableDesignerItemViewModelBase>();

        public List<SelectableDesignerItemViewModelBase> SelectedItems
        {
            get { return Items.Where(x => x.IsSelected).ToList(); }
        }


        private SelectionService selectionService;
        public SelectionService SelectionService
        {
            get
            {
                if (selectionService == null)
                    selectionService = new SelectionService(this);

                return selectionService;
            }
        }

        public Func<SelectableDesignerItemViewModelBase, bool> OutAddVerify { get; set; }

        public bool AddVerify(SelectableDesignerItemViewModelBase item)
        {
            if (item.InitData() == false)
                return false;

            if (OutAddVerify != null && OutAddVerify(item) != true)
                return false;

            return true;
        }

        private void ExecuteCreateNewDiagramCommand(object parameter)
        {
            this.Items.Clear();
        }

        private void ExecuteDirectAddItemCommand(object parameter)
        {
            if (parameter is SelectableDesignerItemViewModelBase ite)
            {
                if (AddVerify(ite) != true) return;

                ClearSelectedItems();
                Add(ite);
            }
            else if (parameter is List<SelectableDesignerItemViewModelBase> items)
            {
                if (items.Select(p => AddVerify(p)).Any() != true) return;

                ClearSelectedItems();
                foreach (var item in items)
                {
                    Add(item);
                }
            }
        }

        private void Add(SelectableDesignerItemViewModelBase item)
        {
            item.Parent = this;
            item.ZIndex = Items.Count;
            item.Id = Guid.NewGuid();
            //item.LineColor = this.LineColor;
            //item.FillColor = this.FillColor;
            var logical = item as LogicalGateItemViewModelBase;
            if (logical != null && logical.LogicalType > 0)
            {
                logical.OrderNumber = Items.OfType<LogicalGateItemViewModelBase>().Count(p => (int)p.LogicalType > 0) + 1;
            }

            var designerItemViewModelBase = item as DesignerItemViewModelBase;
            if (designerItemViewModelBase != null)
            {
                designerItemViewModelBase.SetCellAlignment();
            }
            Items.Add(item);
            item.IsSelected = true;
        }

        private void ExecuteAddItemCommand(object parameter)
        {
            if (parameter is SelectableDesignerItemViewModelBase ite)
            {
                if (AddVerify(ite) != true) return;

                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    ClearSelectedItems();
                    Add(ite);
                },
                () =>
                {
                    Items.Remove(ite);
                });
            }
            else if (parameter is List<SelectableDesignerItemViewModelBase> items)
            {
                if (items.Select(p => AddVerify(p)).Any() != true) return;

                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    ClearSelectedItems();
                    foreach (var item in items)
                    {
                        Add(item);
                    }
                },
                () =>
                {
                    items.ForEach(item => Items.Remove(item));
                });
            }
        }

        private void ExecuteDirectRemoveItemCommand(object parameter)
        {
            if (parameter is SelectableDesignerItemViewModelBase ite)
            {
                ite.IsSelected = false;
                Items.Remove(ite);
                if (ite.OutTextItem != null)
                {
                    Items.Remove(ite.OutTextItem);
                }
            }
            else if (parameter is List<SelectableDesignerItemViewModelBase> items)
            {
                foreach (var item in items)
                {
                    item.IsSelected = false;
                    Items.Remove(item);
                    if (item.OutTextItem != null)
                    {
                        Items.Remove(item.OutTextItem);
                    }
                }
            }
        }
        private void ExecuteRemoveItemCommand(object parameter)
        {
            if (parameter is SelectableDesignerItemViewModelBase ite)
            {
                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    ite.IsSelected = false;
                    Items.Remove(ite);
                    if (ite.OutTextItem != null)
                    {
                        Items.Remove(ite.OutTextItem);
                    }

                },
                () =>
                {
                    Items.Add(ite);
                });
            }
            else if (parameter is List<SelectableDesignerItemViewModelBase> items)
            {
                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    foreach (var item in items)
                    {
                        item.IsSelected = false;
                        Items.Remove(item);
                        if (item.OutTextItem != null)
                        {
                            Items.Remove(item.OutTextItem);
                        }
                    }

                },
                () =>
                {
                    foreach (var item in items)
                    {
                        Items.Add(item);
                        if (item.OutTextItem != null)
                        {
                            Items.Add(item.OutTextItem);
                        }
                    }

                });
            }
        }
        private void ExecuteClearSelectedItemsCommand(object parameter)
        {
            ClearSelectedItems();
        }

        public void ClearSelectedItems()
        {
            foreach (var item in this.Items.ToList())
            {
                item.IsSelected = false;
            }
        }

        #region 布局
        private void ExecuteAlignTopCommand(object parameter)
        {
            var selectedItems = this.SelectedItems.OfType<DesignerItemViewModelBase>();
            var guid = Guid.NewGuid();

            if (selectedItems.Count() > 1)
            {
                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    double top = selectedItems.OrderBy(p => p.Top).Select(p => p.Top).FirstOrDefault();

                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.SetOldValue(item.Top, nameof(item.Top), guid.ToString());
                        item.Top = top;
                    }
                },
                 () =>
                 {
                     foreach (DesignerItemViewModelBase item in selectedItems)
                     {
                         item.Top = item.GetOldValue<double>(nameof(item.Top), guid.ToString());
                     }
                 },
                 () =>
                 {
                     foreach (DesignerItemViewModelBase item in selectedItems)
                     {
                         item.ClearOldValue<double>(nameof(item.Top), guid.ToString());
                     }
                 });
            }
        }

        private void ExecuteAlignVerticalCentersCommand(object parameter)
        {
            var selectedItems = this.SelectedItems.OfType<DesignerItemViewModelBase>();
            var guid = Guid.NewGuid();

            if (selectedItems.Count() > 1)
            {
                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    double mid = selectedItems.Select(p => p.Top + p.ItemHeight / 2).Average();

                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.SetOldValue(item.Top, nameof(item.Top), guid.ToString());
                        item.Top = mid - item.ItemHeight / 2;
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.Top = item.GetOldValue<double>(nameof(item.Top), guid.ToString());
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.ClearOldValue<double>(nameof(item.Top), guid.ToString());
                    }
                });
            }
        }

        private void ExecuteAlignBottomCommand(object parameter)
        {
            var selectedItems = this.SelectedItems.OfType<DesignerItemViewModelBase>();
            var guid = Guid.NewGuid();

            if (selectedItems.Count() > 1)
            {
                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    double top = selectedItems.OrderBy(p => p.Top + p.ItemHeight).Select(p => p.Top + p.ItemHeight).LastOrDefault();

                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.SetOldValue(item.Top, nameof(item.Top), guid.ToString());
                        item.Top = top - item.ItemHeight;
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.Top = item.GetOldValue<double>(nameof(item.Top), guid.ToString());
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.ClearOldValue<double>(nameof(item.Top), guid.ToString());
                    }
                });
            }
        }

        private void ExecuteAlignLeftCommand(object parameter)
        {
            var selectedItems = this.SelectedItems.OfType<DesignerItemViewModelBase>();
            var guid = Guid.NewGuid();

            if (selectedItems.Count() > 1)
            {
                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    double left = selectedItems.OrderBy(p => p.Left).Select(p => p.Left).FirstOrDefault();

                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.SetOldValue(item.Left, nameof(item.Left), guid.ToString());
                        item.Left = left;
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.Left = item.GetOldValue<double>(nameof(item.Left), guid.ToString());
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.ClearOldValue<double>(nameof(item.Left), guid.ToString());
                    }
                });
            }
        }

        private void ExecuteAlignHorizontalCentersCommand(object parameter)
        {
            var selectedItems = this.SelectedItems.OfType<DesignerItemViewModelBase>();
            var guid = Guid.NewGuid();

            if (selectedItems.Count() > 1)
            {
                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    double mid = selectedItems.Select(p => p.Left + p.ItemWidth / 2).Average();

                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.SetOldValue(item.Left, nameof(item.Left), guid.ToString());
                        item.Left = mid - item.ItemWidth / 2;
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.Left = item.GetOldValue<double>(nameof(item.Left), guid.ToString());
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.ClearOldValue<double>(nameof(item.Left), guid.ToString());
                    }
                });
            }
        }

        private void ExecuteAlignRightCommand(object parameter)
        {
            var selectedItems = this.SelectedItems.OfType<DesignerItemViewModelBase>();
            var guid = Guid.NewGuid();

            if (selectedItems.Count() > 1)
            {
                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    double right = selectedItems.OrderBy(p => p.Left + p.ItemWidth).Select(p => p.Left + p.ItemWidth).LastOrDefault();

                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.SetOldValue(item.Left, nameof(item.Left), guid.ToString());
                        item.Left = right - item.ItemWidth;
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.Left = item.GetOldValue<double>(nameof(item.Left), guid.ToString());
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        item.ClearOldValue<double>(nameof(item.Left), guid.ToString());
                    }
                });
            }
        }
        #endregion

        private void ExecuteBringForwardCommand(object parameter)
        {
            List<SelectableDesignerItemViewModelBase> ordered = SelectedItems.OrderByDescending(p => p.ZIndex).ToList();

            List<SelectableDesignerItemViewModelBase> changeditems = new List<SelectableDesignerItemViewModelBase>();
            var guid = Guid.NewGuid();

            DoCommandManager.DoNewCommand(this.ToString(),
            () =>
            {
                int count = this.Items.Count;
                for (int i = 0; i < ordered.Count; i++)
                {
                    var item = ordered[i];
                    int currentIndex = item.ZIndex;
                    int newIndex = Math.Min(count - 1 - i, currentIndex + 1);
                    if (currentIndex != newIndex)
                    {
                        item.SetOldValue<int>(item.ZIndex, nameof(item.ZIndex), guid.ToString());
                        item.ZIndex = newIndex;
                        changeditems.Add(item);

                        IEnumerable<SelectableDesignerItemViewModelBase> it = this.Items.Where(p => p.ZIndex == newIndex);

                        foreach (var elm in it)
                        {
                            if (elm != item)
                            {
                                elm.SetOldValue<int>(elm.ZIndex, nameof(elm.ZIndex), guid.ToString());
                                elm.ZIndex = currentIndex;
                                changeditems.Add(elm);
                                break;
                            }
                        }
                    }
                }
            },
            () =>
            {
                foreach (var item in changeditems)
                {
                    item.ZIndex = item.GetOldValue<int>(nameof(item.ZIndex), guid.ToString());
                }
            },
            () =>
            {
                foreach (var item in changeditems)
                {
                    item.ClearOldValue<double>(nameof(item.ZIndex), guid.ToString());
                }
            });
        }
        private void ExecuteBringToFrontCommand(object parameter)
        {
            List<SelectableDesignerItemViewModelBase> selectionSorted = SelectedItems.OrderByDescending(p => p.ZIndex).ToList();
            List<SelectableDesignerItemViewModelBase> childrenSorted = Items.OrderByDescending(p => p.ZIndex).ToList();

            List<SelectableDesignerItemViewModelBase> changeditems = new List<SelectableDesignerItemViewModelBase>();
            var guid = Guid.NewGuid();

            DoCommandManager.DoNewCommand(this.ToString(),
            () =>
            {
                int i = childrenSorted.Count - 1;
                int j = childrenSorted.Count - selectionSorted.Count - 1;

                foreach (SelectableDesignerItemViewModelBase item in childrenSorted)
                {
                    item.SetOldValue<int>(item.ZIndex, nameof(item.ZIndex), guid.ToString());
                    if (selectionSorted.Contains(item))
                    {
                        item.ZIndex = i--;
                    }
                    else
                    {
                        item.ZIndex = j--;
                    }
                    changeditems.Add(item);
                }
            },
            () =>
            {
                foreach (var item in changeditems)
                {
                    item.ZIndex = item.GetOldValue<int>(nameof(item.ZIndex), guid.ToString());
                }
            },
            () =>
            {
                foreach (var item in changeditems)
                {
                    item.ClearOldValue<double>(nameof(item.ZIndex), guid.ToString());
                }
            });
        }
        private void ExecuteSendBackwardCommand(object parameter)
        {
            List<SelectableDesignerItemViewModelBase> ordered = this.SelectedItems.OrderBy(p => p.ZIndex).ToList();
            int count = this.Items.Count;

            List<SelectableDesignerItemViewModelBase> changeditems = new List<SelectableDesignerItemViewModelBase>();
            var guid = Guid.NewGuid();

            DoCommandManager.DoNewCommand(this.ToString(),
            () =>
            {
                for (int i = 0; i < ordered.Count; i++)
                {
                    var item = ordered[i];
                    int currentIndex = item.ZIndex;
                    int newIndex = Math.Max(i, currentIndex - 1);
                    if (currentIndex != newIndex)
                    {
                        item.SetOldValue<int>(item.ZIndex, nameof(item.ZIndex), guid.ToString());
                        item.ZIndex = newIndex;
                        changeditems.Add(item);
                        IEnumerable<SelectableDesignerItemViewModelBase> it = this.Items.Where(p => p.ZIndex == newIndex);

                        foreach (var elm in it)
                        {
                            if (elm != ordered[i])
                            {
                                elm.SetOldValue<int>(elm.ZIndex, nameof(elm.ZIndex), guid.ToString());
                                elm.ZIndex = currentIndex;
                                changeditems.Add(elm);

                                break;
                            }
                        }
                    }
                }
            },
            () =>
            {
                foreach (var item in changeditems)
                {
                    item.ZIndex = item.GetOldValue<int>(nameof(item.ZIndex), guid.ToString());
                }
            },
            () =>
            {
                foreach (var item in changeditems)
                {
                    item.ClearOldValue<double>(nameof(item.ZIndex), guid.ToString());
                }
            });
        }
        private void ExecuteSendToBackCommand(object parameter)
        {
            List<SelectableDesignerItemViewModelBase> selectionSorted = SelectedItems.OrderByDescending(p => p.ZIndex).ToList();
            List<SelectableDesignerItemViewModelBase> childrenSorted = Items.OrderByDescending(p => p.ZIndex).ToList();

            List<SelectableDesignerItemViewModelBase> changeditems = new List<SelectableDesignerItemViewModelBase>();
            var guid = Guid.NewGuid();

            DoCommandManager.DoNewCommand(this.ToString(),
            () =>
            {
                int i = childrenSorted.Count - 1;
                int j = selectionSorted.Count - 1;

                foreach (SelectableDesignerItemViewModelBase item in childrenSorted)
                {
                    item.SetOldValue<int>(item.ZIndex, nameof(item.ZIndex), guid.ToString());
                    if (selectionSorted.Contains(item))
                    {
                        item.ZIndex = j--;
                    }
                    else
                    {
                        item.ZIndex = i--;
                    }
                    changeditems.Add(item);
                }
            },
            () =>
            {
                foreach (var item in changeditems)
                {
                    item.ZIndex = item.GetOldValue<int>(nameof(item.ZIndex), guid.ToString());
                }
            },
            () =>
            {
                foreach (var item in changeditems)
                {
                    item.ClearOldValue<double>(nameof(item.ZIndex), guid.ToString());
                }
            });
        }
        private void ExecuteDistributeHorizontalCommand(object parameter)
        {
            var selectedItems = from item in this.SelectedItems.OfType<DesignerItemViewModelBase>()
                                where item.ParentId == Guid.Empty
                                orderby item.Left
                                select item;
            var guid = Guid.NewGuid();

            if (selectedItems.Count() > 1)
            {
                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    double left = Double.MaxValue;
                    double right = Double.MinValue;
                    double sumWidth = 0;
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        left = Math.Min(left, item.Left);
                        right = Math.Max(right, item.Left + item.ItemWidth);
                        sumWidth += item.ItemWidth;
                    }

                    double distance = Math.Max(0, (right - left - sumWidth) / (selectedItems.Count() - 1));
                    double offset = selectedItems.First().Left;

                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        double delta = offset - item.Left;
                        foreach (DesignerItemViewModelBase di in SelectionService.GetGroupMembers(item))
                        {
                            di.SetOldValue(di.Left, nameof(di.Left), guid.ToString());
                            di.Left += delta;
                        }
                        offset = offset + item.ItemWidth + distance;
                    }

                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        foreach (DesignerItemViewModelBase di in SelectionService.GetGroupMembers(item))
                        {
                            di.Left = di.GetOldValue<double>(nameof(di.Left), guid.ToString());
                        }
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        foreach (DesignerItemViewModelBase di in SelectionService.GetGroupMembers(item))
                        {
                            di.ClearOldValue<double>(nameof(di.Left), guid.ToString());
                        }
                    }
                });
            }
        }

        private void ExecuteDistributeVerticalCommand(object parameter)
        {
            var selectedItems = from item in this.SelectedItems.OfType<DesignerItemViewModelBase>()
                                where item.ParentId == Guid.Empty
                                orderby item.Top
                                select item;
            var guid = Guid.NewGuid();

            if (selectedItems.Count() > 1)
            {
                DoCommandManager.DoNewCommand(this.ToString(),
                () =>
                {
                    double top = Double.MaxValue;
                    double bottom = Double.MinValue;
                    double sumHeight = 0;
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        top = Math.Min(top, item.Top);
                        bottom = Math.Max(bottom, item.Top + item.ItemHeight);
                        sumHeight += item.ItemHeight;
                    }

                    double distance = Math.Max(0, (bottom - top - sumHeight) / (selectedItems.Count() - 1));
                    double offset = selectedItems.First().Top;

                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        double delta = offset - item.Top;
                        foreach (DesignerItemViewModelBase di in SelectionService.GetGroupMembers(item))
                        {
                            di.SetOldValue(di.Top, nameof(di.Top), guid.ToString());
                            di.Top += +delta;
                        }
                        offset = offset + item.ItemHeight + distance;
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        foreach (DesignerItemViewModelBase di in SelectionService.GetGroupMembers(item))
                        {
                            di.Top = di.GetOldValue<double>(nameof(di.Top), guid.ToString());
                        }
                    }
                },
                () =>
                {
                    foreach (DesignerItemViewModelBase item in selectedItems)
                    {
                        foreach (DesignerItemViewModelBase di in SelectionService.GetGroupMembers(item))
                        {
                            di.ClearOldValue<double>(nameof(di.Top), guid.ToString());
                        }
                    }
                });
            }
        }

        private void ExecuteSelectAllCommand(object parameter)
        {
            foreach (var item in Items)
            {
                item.IsSelected = true;
            }
        }

        public bool BelongToSameGroup(IGroupable item1, IGroupable item2)
        {
            IGroupable root1 = SelectionService.GetGroupRoot(item1);
            IGroupable root2 = SelectionService.GetGroupRoot(item2);

            return (root1.Id == root2.Id);
        }

        public void UpdateZIndex()
        {
            List<SelectableDesignerItemViewModelBase> ordered = Items.OrderBy(p => p.ZIndex).ToList();


            for (int i = 0; i < ordered.Count; i++)
            {
                ordered[i].ZIndex = i;
            }
        }

        public Rect GetBoundingRectangle(IEnumerable<DesignerItemViewModelBase> items)
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;

            foreach (DesignerItemViewModelBase item in items)
            {
                x1 = Math.Min(item.Left, x1);
                y1 = Math.Min(item.Top, y1);

                x2 = Math.Max(item.Left + item.ItemWidth, x2);
                y2 = Math.Max(item.Top + item.ItemHeight, y2);
            }

            return new Rect(new Point(x1, y1), new Point(x2, y2));
        }


    }




}
