using System;
using System.ComponentModel;

namespace Util.DiagramDesigner
{
    /// <summary>
    /// Simple service interface
    /// </summary>
    public interface IDiagramServiceProvider : INotifyPropertyChanged
    {
        IColorViewModel ColorViewModel { get; }
        IFontViewModel FontViewModel { get; }
        IDrawModeViewModel DrawModeViewModel { get; }
        IQuickThemeViewModel QuickThemeViewModel { get; }
        ILockObjectViewModel LockObjectViewModel { get; }
        SelectableDesignerItemViewModelBase SelectedItem { get; set; }
    }


    /// <summary>
    /// Simple service locator
    /// </summary>
    public class DiagramServiceProvider : BindableBase, IDiagramServiceProvider
    {
        public DiagramServiceProvider()
        {
            ColorViewModel = new ColorViewModel();
            FontViewModel = new FontViewModel();
            LockObjectViewModel = new LockObjectViewModel();
            _drawModeViewModel = new DrawModeViewModel();
            _quickThemeViewModel = new QuickThemeViewModel();
          
            _drawModeViewModel.PropertyChanged += ViewModel_PropertyChanged;
            _quickThemeViewModel.PropertyChanged += ViewModel_PropertyChanged;

            SetOldValue<IColorViewModel>(ColorViewModel, nameof(ColorViewModel));
            SetOldValue<IFontViewModel>(FontViewModel, nameof(FontViewModel));
            SetOldValue<ILockObjectViewModel>(LockObjectViewModel, nameof(LockObjectViewModel));
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(sender, e.PropertyName);
        }

        private IColorViewModel _colorViewModel;
        public IColorViewModel ColorViewModel
        {
            get
            {
                return _colorViewModel;
            }
            set
            {
                if (_colorViewModel != null)
                {
                    _colorViewModel.PropertyChanged -= ViewModel_PropertyChanged;
                }
                SetProperty(ref _colorViewModel, value);
                if (_colorViewModel != null)
                {
                    _colorViewModel.PropertyChanged += ViewModel_PropertyChanged;
                }
            }
        }

        private IFontViewModel _fontViewModel;
        public IFontViewModel FontViewModel
        {
            get
            {
                return _fontViewModel;
            }
            set
            {
                if (_fontViewModel != null)
                {
                    _fontViewModel.PropertyChanged -= ViewModel_PropertyChanged;
                }
                SetProperty(ref _fontViewModel, value);
                if (_fontViewModel != null)
                {
                    _fontViewModel.PropertyChanged += ViewModel_PropertyChanged;
                }
            }
        }

        private DrawModeViewModel _drawModeViewModel;
        public IDrawModeViewModel DrawModeViewModel
        {
            get { return _drawModeViewModel; }
        }

        private QuickThemeViewModel _quickThemeViewModel;
        public IQuickThemeViewModel QuickThemeViewModel
        {
            get { return _quickThemeViewModel; }
        }

        private ILockObjectViewModel _lockObjectViewModel;
        public ILockObjectViewModel LockObjectViewModel
        {
            get
            {
                return _lockObjectViewModel;
            }
            set
            {
                if (_lockObjectViewModel != null)
                {
                    _lockObjectViewModel.PropertyChanged -= ViewModel_PropertyChanged;
                }
                SetProperty(ref _lockObjectViewModel, value);
                if (_lockObjectViewModel != null)
                {
                    _lockObjectViewModel.PropertyChanged += ViewModel_PropertyChanged;
                }
            }
        }

        private SelectableDesignerItemViewModelBase _selectedItem;
        public SelectableDesignerItemViewModelBase SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != null)
                {
                    _selectedItem.PropertyChanged -= ViewModel_PropertyChanged;
                }
                if (SetProperty(ref _selectedItem, value))
                {
                    if (_selectedItem == null)
                    {
                        ColorViewModel = GetOldValue<ColorViewModel>(nameof(ColorViewModel));
                        FontViewModel = GetOldValue<FontViewModel>(nameof(FontViewModel));
                        LockObjectViewModel = GetOldValue<LockObjectViewModel>(nameof(LockObjectViewModel));
                    }
                    else
                    {
                        ColorViewModel = _selectedItem.ColorViewModel;
                        FontViewModel = _selectedItem.FontViewModel;
                        LockObjectViewModel = _selectedItem.LockObjectViewModel;
                    }
                }
                if (_selectedItem != null)
                {
                    _selectedItem.PropertyChanged += ViewModel_PropertyChanged;
                }
            }
        }
    }



    /// <summary>
    /// Simple service locator helper
    /// </summary>
    public class DiagramServicesProvider
    {
        private static Lazy<DiagramServicesProvider> instance = new Lazy<DiagramServicesProvider>(() => new DiagramServicesProvider());
        private IDiagramServiceProvider serviceProvider = new DiagramServiceProvider();

        public void SetNewServiceProvider(IDiagramServiceProvider provider)
        {
            serviceProvider = provider;
        }

        public IDiagramServiceProvider Provider
        {
            get { return serviceProvider; }
        }

        public static DiagramServicesProvider Instance
        {
            get { return instance.Value; }
        }
    }
}
