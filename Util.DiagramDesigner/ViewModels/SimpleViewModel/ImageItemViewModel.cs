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
    public class ImageItemViewModel : DesignerItemViewModelBase
    {

        private static readonly string filter = "图片|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
        public ImageItemViewModel() : base()
        {
        }

        public ImageItemViewModel(IDiagramViewModel parent, ImageDesignerItem designer) : base(parent, designer)
        {

        }

        protected override void Init()
        {
            base.Init();

            this.PropertyChanged += ImageItemViewModel_PropertyChanged;

            BuildMenuOptions();
        }

        private void ImageItemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ItemWidth) || e.PropertyName == nameof(ItemHeight) || e.PropertyName == nameof(ResizeMargin) || e.PropertyName == nameof(ClipMode))
            {
                RaisePropertyChanged(nameof(Object));
            }
            else if (e.PropertyName == nameof(IsSelected))
            {
                if (IsSelected == false)
                {
                    EndResize();
                }
            }
        }

        private string _suffix;
        public string Suffix
        {
            get { return _suffix; }
            set
            {
                SetProperty(ref _suffix, filter.Contains(value) ? value : ".txt");  
            }
        }

        public ImageItemViewModel Object
        {
            get { return this; }
        }

        public double ImageWidth { get; set; }
        public double ImageHeight { get; set; }

        private bool _resizeMode;
        public bool ResizeMode
        {
            get { return _resizeMode; }
            set
            {
                SetProperty(ref _resizeMode, value);
            }
        }

        //显示的时候是真实的Marigin,不显示的时候是按比例换算的Marigin
        private Thickness _resizeMargin = new Thickness(0);
        public Thickness ResizeMargin
        {
            get { return _resizeMargin; }
            set
            {
                SetProperty(ref _resizeMargin, value);
            }
        }

        private ClipMode _clipMode;
        public ClipMode ClipMode
        {
            get { return _clipMode; }
            set
            {
                SetProperty(ref _clipMode, value);
            }
        }


        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

            ImageDesignerItem designer = designerbase as ImageDesignerItem;

            this.Icon = designer.Icon;
            Suffix = Path.GetExtension(this.Icon).ToLower();
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

        protected override void ExecuteEditCommand(object param)
        {
            if (IsReadOnly == true) return;

            if (ResizeMode == true)
            {
                EndResize();
                return;
            }

            System.Diagnostics.Process.Start(Icon);

        }

        public void InitWidthAndHeight()
        {
            try
            {
                using (FileStream fs = new FileStream(this.Icon, FileMode.Open, FileAccess.Read))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                    this.ImageWidth = image.Width;
                    this.ImageHeight = image.Height;
                }
            }
            catch
            {
                this.Suffix = ".txt";
                this.ImageWidth = 32;
                this.ImageHeight = 32;
            }
        }
        public void AutoSize()
        {
            this.ItemWidth = this.ImageWidth;
            this.ItemHeight = this.ImageHeight;
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
            openFile.Filter = filter;

            if (openFile.ShowDialog() == true)
            {
                bool needauto = Icon == null;
                Icon = openFile.FileName;
                Suffix = Path.GetExtension(Icon).ToLower();
                InitWidthAndHeight();
                if (needauto)
                {
                    AutoSize();
                }
                return true;
            }

            return false;
        }

        public void StartResize()
        {
            if (ResizeMode == true) return;

            ResizeMode = true;
            if (ResizeMargin == new Thickness(0, 0, 0, 0))
            {
                ResizeMargin = new Thickness(ItemWidth * 0.1, ItemHeight * 0.1, ItemWidth * 0.1, ItemHeight * 0.1);
            }
            else
            {
                var margin = ResizeMargin;
                double xradio = ItemWidth / ImageWidth;
                double yradio = ItemHeight / ImageHeight;
                ResizeMargin = new Thickness(margin.Left * xradio, margin.Top * yradio, margin.Right * xradio, margin.Bottom * yradio);

                ItemWidth = ItemWidth + ResizeMargin.Left + ResizeMargin.Right;
                ItemHeight = ItemHeight + ResizeMargin.Top + ResizeMargin.Bottom;
                Left = Left - ResizeMargin.Left;
                Top = Top - ResizeMargin.Top;
            }
        }

        public void EndResize()
        {
            if (ResizeMode == false)
                return;

            ResizeMode = false;
            var margin = ResizeMargin;
            double xradio = ItemWidth / ImageWidth;
            double yradio = ItemHeight / ImageHeight;

            ResizeMargin = new Thickness(margin.Left / xradio, margin.Top / yradio, margin.Right / xradio, margin.Bottom / yradio);
            ItemWidth = ItemWidth - margin.Left - margin.Right;
            ItemHeight = ItemHeight - margin.Top - margin.Bottom;
            Left = Left + margin.Left;
            Top = Top + margin.Top;
        }

        public void Reset()
        {
            if (ResizeMode == true)
            {
                ResizeMargin = new Thickness(0, 0, 0, 0);
            }

            if (ResizeMargin == new Thickness(0, 0, 0, 0))
            {
                ResizeMode = false;
                return;
            }

            var margin = ResizeMargin;
            ResizeMargin = new Thickness(0, 0, 0, 0);
            ItemWidth = ItemWidth + margin.Left + margin.Right;
            ItemHeight = ItemHeight + margin.Top + margin.Bottom;
            Left = Left - margin.Left;
            Top = Top - margin.Top;
        }

    }


    public enum ClipMode
    {
        RectangleGeometry,
        EllipseGeometry
    }
}
