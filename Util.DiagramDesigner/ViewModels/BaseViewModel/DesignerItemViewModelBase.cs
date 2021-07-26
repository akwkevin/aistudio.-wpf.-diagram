using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Reactive.Linq;

namespace Util.DiagramDesigner
{
    public abstract class DesignerItemViewModelBase : SelectableDesignerItemViewModelBase
    {
        public DesignerItemViewModelBase() : base()
        {
        }

        public DesignerItemViewModelBase(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }

        protected override void Init()
        {
            base.Init();

            connectors.Add(new FullyCreatedConnectorInfo(this, ConnectorOrientation.Top));
            connectors.Add(new FullyCreatedConnectorInfo(this, ConnectorOrientation.Bottom));
            connectors.Add(new FullyCreatedConnectorInfo(this, ConnectorOrientation.Left));
            connectors.Add(new FullyCreatedConnectorInfo(this, ConnectorOrientation.Right));
        }

        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            DesignerItemBase designer = designerbase as DesignerItemBase;

            this.Left = designer.Left;
            this.Top = designer.Top;
            this.Angle = designer.Angle;
            this.ScaleX = designer.ScaleX;
            this.ScaleY = designer.ScaleY;
            this.ItemWidth = designer.ItemWidth;
            this.ItemHeight = designer.ItemHeight;
            this.Icon = designer.Icon;

        }

        public FullyCreatedConnectorInfo TopConnector
        {
            get { return (connectors != null && connectors.Count >= 1) ? connectors[0] : null; }
        }

        public FullyCreatedConnectorInfo BottomConnector
        {
            get { return (connectors != null && connectors.Count >= 2) ? connectors[1] : null; }
        }

        public FullyCreatedConnectorInfo LeftConnector
        {
            get { return (connectors != null && connectors.Count >= 3) ? connectors[2] : null; }
        }

        public FullyCreatedConnectorInfo RightConnector
        {
            get { return (connectors != null && connectors.Count >= 4) ? connectors[3] : null; }
        }


        private string _icon;
        [CanDo]
        public string Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                SetProperty(ref _icon, value);
            }
        }

        private double _itemWidth = 65;
        [Browsable(true)]
        [CanDo]
        public double ItemWidth
        {
            get
            {
                return _itemWidth;
            }
            set
            {
                if (value <= 0) return;
                SetProperty(ref _itemWidth, value);
            }
        }

        private double _itemHeight = 65;
        [Browsable(true)]
        [CanDo]
        public double ItemHeight
        {
            get
            {
                return _itemHeight;
            }
            set
            {
                if (value <= 0) return;
                SetProperty(ref _itemHeight, value);
            }
        }

        [CanDo]
        public Point ItemWidthHeight
        {
            get
            {
                return new Point(ItemWidth, ItemHeight);
            }
            set
            {
                ItemWidth = value.X;
                ItemHeight = value.Y;
            }
        }

        private bool _showConnectors = false;
        [Browsable(false)]
        public bool ShowConnectors
        {
            get
            {
                return _showConnectors;
            }
            set
            {
                if (SetProperty(ref _showConnectors, value))
                {
                    foreach (var connector in connectors)
                    {
                        connector.ShowConnectors = value;
                    }
                }
            }
        }

        private bool _showResize = true;
        [Browsable(false)]
        public bool ShowResize
        {
            get
            {
                return _showResize;
            }
            set
            {
                SetProperty(ref _showResize, value);
            }
        }

        public bool ShowRotate { get; set; } = true;

        private double _left;
        [Browsable(true)]
        [CanDo]
        public double Left
        {
            get
            {
                return _left;
            }
            set
            {
                SetProperty(ref _left, value);
            }
        }

        private double _top;
        [Browsable(true)]
        [CanDo]
        public double Top
        {
            get
            {
                return _top;
            }
            set
            {
                SetProperty(ref _top, value);
            }
        }        

        [CanDo]
        public Point TopLeft
        {
            get
            {
                return new Point(Left, Top);
            }
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        private double _angle;
        [CanDo]
        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                SetProperty(ref _angle, value);
            }
        }

        private double _scaleX = 1;
        [CanDo]
        public double ScaleX
        {
            get
            {
                return _scaleX;
            }
            set
            {
                SetProperty(ref _scaleX, value);
            }
        }

        private double _scaleY = 1;
        [CanDo]
        public double ScaleY
        {
            get
            {
                return _scaleY;
            }
            set
            {
                SetProperty(ref _scaleY, value);
            }
        }

        private double _margin;

        public double Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                SetProperty(ref _margin, value);
            }
        }

        private ObservableCollection<FullyCreatedConnectorInfo> connectors = new ObservableCollection<FullyCreatedConnectorInfo>();
        public IEnumerable<FullyCreatedConnectorInfo> Connectors { get { return connectors; } }

        protected ObservableCollection<CinchMenuItem> menuOptions;
        public IEnumerable<CinchMenuItem> MenuOptions { get { return menuOptions; } }

        public bool ShowMenuOptions
        {
            get
            {
                if (MenuOptions == null || MenuOptions.Count() == 0)
                    return false;
                else
                    return true;
            }
        }

        public bool BeginDo { get; set; }

        public IObservable<NotifyCollectionChangedEventArgs> WhenConnectorsChanged
        {
            get
            {
                return Observable
                    .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                        h => this.connectors.CollectionChanged += h,
                        h => this.connectors.CollectionChanged -= h)
                    .Select(x => x.EventArgs);
            }
        }

        public void AddConnector(FullyCreatedConnectorInfo connector)
        {
            if (!connectors.Contains(connector))
            {
                connectors.Add(connector);
            }
        }

        public void RemoveConnector(FullyCreatedConnectorInfo connector)
        {
            if (connectors.Contains(connector))
            {
                connectors.Remove(connector);
            }
        }

        public void ClearConnectors()
        {
            connectors.Clear();
        }

        public void SetCellAlignment()
        {
            if (!(this is TextDesignerItemViewModel))
            {
                if (Parent.CellHorizontalAlignment == CellHorizontalAlignment.Center)
                {
                    this.Left = (int)(this.Left / Parent.GridCellSize.Width) * Parent.GridCellSize.Width + Parent.GridMargin + (Parent.GridCellSize.Width > this.ItemWidth ? (Parent.GridCellSize.Width - this.ItemWidth) / 2 : 0);
                }
                else if(Parent.CellHorizontalAlignment == CellHorizontalAlignment.Left)
                {
                    this.Left = (int)(this.Left / Parent.GridCellSize.Width) * Parent.GridCellSize.Width + Parent.GridMargin;
                }
                else if (Parent.CellHorizontalAlignment == CellHorizontalAlignment.Right)
                {
                    this.Left = (int)(this.Left / Parent.GridCellSize.Width) * Parent.GridCellSize.Width + Parent.GridMargin + (Parent.GridCellSize.Width > this.ItemWidth ? (Parent.GridCellSize.Width - this.ItemWidth) : 0);
                }

                if (Parent.CellVerticalAlignment == CellVerticalAlignment.Center)
                {                  
                    this.Top = (int)(this.Top / Parent.GridCellSize.Height) * Parent.GridCellSize.Height + Parent.GridMargin + (Parent.GridCellSize.Height > this.ItemHeight ? (Parent.GridCellSize.Height - this.ItemHeight) / 2 : 0);
                }
                else if (Parent.CellVerticalAlignment == CellVerticalAlignment.Top)
                {                    
                    this.Top = (int)(this.Top / Parent.GridCellSize.Height) * Parent.GridCellSize.Height + Parent.GridMargin;
                }
                else if (Parent.CellVerticalAlignment == CellVerticalAlignment.Bottom)
                {                   
                    this.Top = (int)(this.Top / Parent.GridCellSize.Height) * Parent.GridCellSize.Height + Parent.GridMargin + (Parent.GridCellSize.Height > this.ItemHeight ? (Parent.GridCellSize.Height - this.ItemHeight) : 0);
                }
            }          
        }

        public void RaiseTopLeft()
        {
            this.RaisePropertyChanged(nameof(TopLeft), new Point(GetOldValue<double>(nameof(Left)), GetOldValue<double>(nameof(Top))), TopLeft);
        }

        public void RaiseItemWidthHeight()
        {
            this.RaisePropertyChanged(nameof(ItemWidthHeight), new Point(GetOldValue<double>(nameof(ItemWidth)), GetOldValue<double>(nameof(ItemHeight))), ItemWidthHeight);
        }

        public void RaiseAngle()
        {
            this.RaisePropertyChanged(nameof(Angle), GetOldValue<double>(nameof(Angle)), Angle);
        }
    }
}
