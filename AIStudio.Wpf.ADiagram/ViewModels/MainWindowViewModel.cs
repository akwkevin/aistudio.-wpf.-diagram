using AIStudio.Wpf.ADiagram.Commands;
using AIStudio.Wpf.ADiagram.Demos.Flowchart;
using AIStudio.Wpf.ADiagram.Demos.Logical;
using AIStudio.Wpf.ADiagram.Enums;
using AIStudio.Wpf.ADiagram.Helpers;
using AIStudio.Wpf.ADiagram.Views;
using ControlzEx.Theming;
using Dragablz;
using Fluent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IDiagramServiceProvider _service { get { return DiagramServicesProvider.Instance.Provider; } }
        private string _history = System.AppDomain.CurrentDomain.BaseDirectory + "history.json";

        public MainWindowViewModel()
        {
            ToolBoxViewModel = new ToolBoxViewModel();

            ConnectorViewModel.PathFinder = new OrthogonalPathFinder();
            DiagramsViewModels = new ObservableCollection<DiagramsViewModel>();
            DiagramsViewModels.Add(new DiagramsViewModel(this, "新建-1", "*", DiagramType.Normal));
            DiagramsViewModel = DiagramsViewModels.FirstOrDefault();

            StandardColor = GenerateStandardGradients();

            if (File.Exists(_history))
            {
                HistoryList = JsonConvert.DeserializeObject<ObservableCollection<string>>(File.ReadAllText(_history));
            }
            else
            {
                HistoryList = new ObservableCollection<string>();
            }
            this.PropertyChanged += MainWindowViewModel_PropertyChanged;
            _service.PropertyChanged += Provider_PropertyChanged;
        }

        #region 属性
        public ToolBoxViewModel ToolBoxViewModel { get; private set; }

        private ObservableCollection<DiagramsViewModel> _diagramsViewModels;
        public ObservableCollection<DiagramsViewModel> DiagramsViewModels
        {
            get
            {
                return _diagramsViewModels;
            }
            set
            {
                SetProperty(ref _diagramsViewModels, value);
            }
        }

        private DiagramsViewModel _diagramsViewModel;
        public DiagramsViewModel DiagramsViewModel
        {
            get
            {
                return _diagramsViewModel;
            }
            set
            {
                SetProperty(ref _diagramsViewModel, value);
            }
        }

        private ColorType _colorObject;
        public ColorType ColorType
        {
            get
            {
                return _colorObject;
            }
            set
            {
                SetProperty(ref _colorObject, value);
            }
        }

        private bool _isOpenBackstage;

        public bool IsOpenBackstage
        {
            get
            {
                return _isOpenBackstage;
            }
            set
            {
                SetProperty(ref _isOpenBackstage, value);
            }
        }

        private ObservableCollection<string> _historyList;
        public ObservableCollection<string> HistoryList
        {
            get
            {
                return _historyList;
            }
            set
            {
                SetProperty(ref _historyList, value);
            }
        }

        public Color[] StandardColor { get; set; }

        public IDrawModeViewModel DrawModeViewModel
        {
            get
            {
                return _service.DrawModeViewModel;
            }
        }

        public IFontViewModel FontViewModel
        {
            get
            {
                return _service.FontViewModel;
            }
        }
        public IColorViewModel ColorViewModel
        {
            get
            {
                return _service.ColorViewModel;
            }
        }
        public IQuickThemeViewModel QuickThemeViewModel
        {
            get
            {
                return _service.QuickThemeViewModel;
            }
        }
        public ILockObjectViewModel LockObjectViewModel
        {
            get
            {
                return _service.LockObjectViewModel;
            }
        }

        public SelectableDesignerItemViewModelBase SelectedItem
        {
            get
            {
                return _service.SelectedItem;
            }
        }

        public Color ThemeColor
        {
            get => ((SolidColorBrush)Application.Current.Resources["Fluent.Ribbon.Brushes.AccentBaseColorBrush"])?.Color ?? Colors.Pink;

            set
            {
                var solidColorBrush = new SolidColorBrush(value);
                solidColorBrush.Freeze();
                Application.Current.Resources["Fluent.Ribbon.Brushes.AccentBaseColorBrush"] = solidColorBrush;
            }
        }

        public string CurrentBaseColor
        {
            get => this.CurrentTheme.BaseColorScheme;

            set
            {
                if (value is null)
                {
                    return;
                }

                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, value);
                RaisePropertyChanged(nameof(this.CurrentTheme));
            }
        }

        public Theme CurrentTheme
        {
            get => ThemeManager.Current.DetectTheme(Application.Current);

            set
            {
                if (value is null)
                {
                    return;
                }

                ThemeManager.Current.ChangeTheme(Application.Current, value);
                RaisePropertyChanged(nameof(this.CurrentBaseColor));
            }
        }
        #endregion

        public Func<DiagramsViewModel> NewItemFactory
        {
            get
            {
                return
                    () =>
                    {
                        return new DiagramsViewModel(this, NewNameHelper.GetNewName(DiagramsViewModels.Select(p => p.Title), "新建-"), "*", DiagramType.Normal);
                    };
            }
        }

        #region 命令
        private ICommand _newCommand;
        public ICommand NewCommand
        {
            get
            {
                return this._newCommand ?? (this._newCommand = new DelegateCommand<string>(para => this.New_Executed(para)));
            }
        }

        private ICommand _openCommand;
        public ICommand OpenCommand
        {
            get
            {
                return this._openCommand ?? (this._openCommand = new DelegateCommand<string>(para => this.OpenExecuted(para)));
            }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return this._saveCommand ?? (this._saveCommand = new CanExecuteDelegateCommand(() => this.SaveExecuted(), () => this.Save_Enable()));
            }
        }

        private ICommand _saveAsCommand;
        public ICommand SaveAsCommand
        {
            get
            {
                return this._saveAsCommand ?? (this._saveAsCommand = new CanExecuteDelegateCommand(() => this.SaveAsExecuted(), () => this.Save_Enable()));
            }
        }

        private ICommand _pasteCommand;
        public ICommand PasteCommand
        {
            get
            {
                return this._pasteCommand ?? (this._pasteCommand = new CanExecuteDelegateCommand(() => this.PasteExecuted(), () => this.Paste_Enabled()));
            }
        }

        private ICommand _cutCommand;
        public ICommand CutCommand
        {
            get
            {
                return this._cutCommand ?? (this._cutCommand = new CanExecuteDelegateCommand(() => this.CutExecuted(), () => this.Cut_Enabled()));
            }
        }

        private ICommand _copyCommand;
        public ICommand CopyCommand
        {
            get
            {
                return this._copyCommand ?? (this._copyCommand = new CanExecuteDelegateCommand(() => this.CopyExecuted(), () => Copy_Enabled()));
            }
        }

        private ICommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                return this._exitCommand ?? (this._exitCommand = new CanExecuteDelegateCommand(() => this.ExitExecuted()));
            }
        }

        private ICommand _keyCommand;
        public ICommand KeyCommand
        {
            get
            {
                return this._keyCommand ?? (this._keyCommand = new DelegateCommand<string>(para => this.KeyExecuted(para)));
            }
        }

        private ICommand _formatCommand;
        public ICommand FormatCommand
        {
            get
            {
                return this._formatCommand ?? (this._formatCommand = new CanExecuteDelegateCommand(() => this.FormatExecuted(), () => Format_Enabled()));
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return this._deleteCommand ?? (this._deleteCommand = new CanExecuteDelegateCommand(() => this.DeleteExecuted(), () => Delete_Enabled()));
            }
        }

        private ICommand _alignTopCommand;
        public ICommand AlignTopCommand
        {
            get
            {
                return this._alignTopCommand ?? (this._alignTopCommand = new DelegateCommand<object>(para => this.AlignTopExecuted(para)));
            }
        }

        private ICommand _alignVerticalCentersCommand;
        public ICommand AlignVerticalCentersCommand
        {
            get
            {
                return this._alignVerticalCentersCommand ?? (this._alignVerticalCentersCommand = new DelegateCommand<object>(para => this.AlignVerticalCentersExecuted(para)));
            }
        }

        private ICommand _alignBottomCommand;
        public ICommand AlignBottomCommand
        {
            get
            {
                return this._alignBottomCommand ?? (this._alignBottomCommand = new DelegateCommand<object>(para => this.AlignBottomExecuted(para)));
            }
        }

        private ICommand _alignLeftCommand;
        public ICommand AlignLeftCommand
        {
            get
            {
                return this._alignLeftCommand ?? (this._alignLeftCommand = new DelegateCommand<object>(para => this.AlignLeftExecuted(para)));
            }
        }

        private ICommand _alignHorizontalCentersCommand;
        public ICommand AlignHorizontalCentersCommand
        {
            get
            {
                return this._alignHorizontalCentersCommand ?? (this._alignHorizontalCentersCommand = new DelegateCommand<object>(para => this.AlignHorizontalCentersExecuted(para)));
            }
        }

        private ICommand _alignRightCommand;
        public ICommand AlignRightCommand
        {
            get
            {
                return this._alignRightCommand ?? (this._alignRightCommand = new DelegateCommand<object>(para => this.AlignRightExecuted(para)));
            }
        }

        private ICommand _groupCommand;
        public ICommand GroupCommand
        {
            get
            {
                return this._groupCommand ?? (this._groupCommand = new DelegateCommand<object>(para => this.GroupExecuted(para)));
            }
        }

        private ICommand _ungroupCommand;
        public ICommand UngroupCommand
        {
            get
            {
                return this._ungroupCommand ?? (this._ungroupCommand = new DelegateCommand<object>(para => this.UngroupExecuted(para)));
            }
        }

        private ICommand _bringForwardCommand;
        public ICommand BringForwardCommand
        {
            get
            {
                return this._bringForwardCommand ?? (this._bringForwardCommand = new DelegateCommand<object>(para => this.BringForwardExecuted(para)));
            }
        }

        private ICommand _bringToFrontCommand;
        public ICommand BringToFrontCommand
        {
            get
            {
                return this._bringToFrontCommand ?? (this._bringToFrontCommand = new DelegateCommand<object>(para => this.BringToFrontExecuted(para)));
            }
        }

        private ICommand _sendBackwardCommand;
        public ICommand SendBackwardCommand
        {
            get
            {
                return this._sendBackwardCommand ?? (this._sendBackwardCommand = new DelegateCommand<object>(para => this.SendBackwardExecuted(para)));
            }
        }

        private ICommand _sendToBackCommand;
        public ICommand SendToBackCommand
        {
            get
            {
                return this._sendToBackCommand ?? (this._sendToBackCommand = new DelegateCommand<object>(para => this.SendToBackExecuted(para)));
            }
        }

        private ICommand _distributeHorizontalCommand;
        public ICommand DistributeHorizontalCommand
        {
            get
            {
                return this._distributeHorizontalCommand ?? (this._distributeHorizontalCommand = new DelegateCommand<object>(para => this.DistributeHorizontalExecuted(para)));
            }
        }

        private ICommand _distributeVerticalCommand;
        public ICommand DistributeVerticalCommand
        {
            get
            {
                return this._distributeVerticalCommand ?? (this._distributeVerticalCommand = new DelegateCommand<object>(para => this.DistributeVerticalExecuted(para)));
            }
        }

        private ICommand _selectAllCommand;
        public ICommand SelectAllCommand
        {
            get
            {
                return this._selectAllCommand ?? (this._selectAllCommand = new DelegateCommand<object>(para => this.SelectAllExecuted(para)));
            }
        }

        private ICommand _centerCommand;
        public ICommand CenterCommand
        {
            get
            {
                return this._centerCommand ?? (this._centerCommand = new DelegateCommand<object>(para => this.CenterExecuted(para)));
            }
        }

        private ICommand _sameWidthCommand;
        public ICommand SameWidthCommand
        {
            get
            {
                return this._sameWidthCommand ?? (this._sameWidthCommand = new DelegateCommand<object>(para => this.SameWidthExecuted(para)));
            }
        }

        private ICommand _sameHeightCommand;
        public ICommand SameHeightCommand
        {
            get
            {
                return this._sameHeightCommand ?? (this._sameHeightCommand = new DelegateCommand<object>(para => this.SameHeightExecuted(para)));
            }
        }

        private ICommand _sameSizeCommand;
        public ICommand SameSizeCommand
        {
            get
            {
                return this._sameSizeCommand ?? (this._sameSizeCommand = new DelegateCommand<object>(para => this.SameSizeExecuted(para)));
            }
        }

        private ICommand _sameAngleCommand;
        public ICommand SameAngleCommand
        {
            get
            {
                return this._sameAngleCommand ?? (this._sameAngleCommand = new DelegateCommand<object>(para => this.SameAngleExecuted(para)));
            }
        }

        private ICommand _lockCommand;
        public ICommand LockCommand
        {
            get
            {
                return this._lockCommand ?? (this._lockCommand = new DelegateCommand<object>(para => this.LockExecuted(para)));
            }
        }

        private ICommand _unlockCommand;
        public ICommand UnlockCommand
        {
            get
            {
                return this._unlockCommand ?? (this._unlockCommand = new DelegateCommand<object>(para => this.UnlockExecuted(para)));
            }
        }

        private ICommand _selectedColorCommand;
        public ICommand SelectedColorCommand
        {
            get
            {
                return this._selectedColorCommand ?? (this._selectedColorCommand = new DelegateCommand<object>(para => this.SelectedColorExecuted(para)));
            }
        }

        private ICommand _addPageCommand;
        public ICommand AddPageCommand
        {
            get
            {
                return this._addPageCommand ?? (this._addPageCommand = new DelegateCommand<object>(para => this.AddPageExecuted(para)));
            }
        }


        private ICommand _addCopyPageCommand;
        public ICommand AddCopyPageCommand
        {
            get
            {
                return this._addCopyPageCommand ?? (this._addCopyPageCommand = new DelegateCommand<object>(para => this.AddCopyPageExecuted(para)));
            }
        }

        private ICommand _renamePageCommand;
        public ICommand RenamePageCommand
        {
            get
            {
                return this._renamePageCommand ?? (this._renamePageCommand = new DelegateCommand<object>(para => this.RenamePageExecuted(para)));
            }
        }

        private ICommand _endRenamePageCommand;
        public ICommand EndRenamePageCommand
        {
            get
            {
                return this._endRenamePageCommand ?? (this._endRenamePageCommand = new DelegateCommand<object>(para => this.EndRenamePageExecuted(para)));
            }
        }

        private ICommand _deletePageCommand;
        public ICommand DeletePageCommand
        {
            get
            {
                return this._deletePageCommand ?? (this._deletePageCommand = new DelegateCommand<object>(para => this.DeletePageExecuted(para)));
            }
        }


        private ICommand _addImageCommand;
        public ICommand AddImageCommand
        {
            get
            {
                return this._addImageCommand ?? (this._addImageCommand = new DelegateCommand<object>(para => this.AddImageExecuted(para)));
            }
        }

        private ICommand _editImageCommand;
        public ICommand EditImageCommand
        {
            get
            {
                return this._editImageCommand ?? (this._editImageCommand = new DelegateCommand<object>(para => this.EditImageExecuted(para)));
            }
        }

        private ICommand _resizeImageCommand;
        public ICommand ResizeImageCommand
        {
            get
            {
                return this._resizeImageCommand ?? (this._resizeImageCommand = new DelegateCommand<object>(para => this.ResizeImageExecuted(para)));
            }
        }

        private ICommand _resetImageCommand;
        public ICommand ResetImageCommand
        {
            get
            {
                return this._resetImageCommand ?? (this._resetImageCommand = new DelegateCommand<object>(para => this.ResetImageExecuted(para)));
            }
        }

        private ICommand _addVideoCommand;
        public ICommand AddVideoCommand
        {
            get
            {
                return this._addVideoCommand ?? (this._addVideoCommand = new DelegateCommand<object>(para => this.AddVideoExectued(para)));
            }
        }


        private ICommand _addOutLineTextCommand;
        public ICommand AddOutLineTextCommand
        {
            get
            {
                return this._addOutLineTextCommand ?? (this._addOutLineTextCommand = new DelegateCommand<object>(para => this.AddOutLineTextExecuted(para)));
            }
        }

        private ICommand _addBarcodeCommand;
        public ICommand AddBarcodeCommand
        {
            get
            {
                return this._addBarcodeCommand ?? (this._addBarcodeCommand = new DelegateCommand<object>(para => this.AddBarcodeExecuted(para)));
            }
        }

        private ICommand _aboutCommand;
        public ICommand AboutCommand
        {
            get
            {
                return this._aboutCommand ?? (this._aboutCommand = new DelegateCommand(() => this.AboutExecuted()));
            }
        }
        #endregion

        public ItemActionCallback ClosingTabItemHandler
        {
            get { return ClosingTabItemHandlerImpl; }
        }

        /// <summary>
        /// Callback to handle tab closing.
        /// </summary>        
        private void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            //here's how you can cancel stuff:
            //args.Cancel(); 
        }

        private void MainWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Provider_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DrawModeViewModel)
                || e.PropertyName == nameof(FontViewModel)
                || e.PropertyName == nameof(ColorViewModel)
                || e.PropertyName == nameof(QuickThemeViewModel)
                || e.PropertyName == nameof(LockObjectViewModel)
                || e.PropertyName == nameof(SelectedItem))
            {
                RaisePropertyChanged(e.PropertyName);
            }

            if (DiagramsViewModel == null) return;

            if (sender is IFontViewModel)
                DiagramsViewModel.SetFont(sender as IFontViewModel, e.PropertyName);

            if (sender is IColorViewModel)
                DiagramsViewModel.SetColor(sender as IColorViewModel, e.PropertyName);

            if (sender is IQuickThemeViewModel)
                DiagramsViewModel.SetQuickItem(sender as IQuickThemeViewModel, e.PropertyName);

            if (sender is LockObject)
                DiagramsViewModel.LockAction(sender as LockObject, e.PropertyName);

            if (sender is DesignerItemViewModelBase designer
                && (e.PropertyName == nameof(designer.Angle)
                   || e.PropertyName == nameof(designer.ItemWidth)
                   || e.PropertyName == nameof(designer.ItemHeight)
                   || e.PropertyName == nameof(designer.ScaleX)
                   || e.PropertyName == nameof(designer.ScaleY)))
            {
                DiagramsViewModel.SetPropertyValue(designer, e.PropertyName);
            }
        }


        private void KeyExecuted(string para)
        {
            switch (para)
            {
                case "Control+A": SelectedAllExecuted(); break;
                case "Control+C": CopyExecuted(); break;
                case "Control+V": PasteExecuted(); break;
                case "Control+X": CutExecuted(); break;
                case "Control+O": OpenExecuted(); break;
                case "Control+N": New_Executed(); break;
                case "Control+S": SaveExecuted(); break;
                case "Control+Z": UnDoExecuted(); break;
                case "Control+Y": ReDoExecuted(); break;
                case "None+Delete": DeleteExecuted(); break;
            }
        }

        private void UnDoExecuted()
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.UnDoExecuted();
        }

        private void ReDoExecuted()
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.ReDoExecuted();
        }

        private void SelectedAllExecuted()
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.SelectedAllExecuted();
        }

        private void OpenExecuted(string para = null)
        {
            string filename = string.Empty;

            if (string.IsNullOrEmpty(para))
            {
                Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
                openFile.Filter = "Designer Files (*.xml;*.json)|*.xml;*.json|All Files (*.*)|*.*";

                if (openFile.ShowDialog() == false)
                {
                    return;
                }

                filename = openFile.FileName;
            }
            else
            {
                filename = para;
            }

            var viewmodel = DiagramsViewModels.FirstOrDefault(p => p.FileName == filename);
            if (viewmodel != null)
            {
                DiagramsViewModel = viewmodel;
                MessageBox.Show("文档已经打开");
                return;
            }

            var flow = new DiagramsViewModel(this, filename);
            DiagramsViewModels.Add(flow);
            DiagramsViewModel = flow;

            if (string.IsNullOrEmpty(para))
            {
                SaveHistory(DiagramsViewModel);
            }
            else
            {
                IsOpenBackstage = false;
            }

        }

        private void SaveExecuted()
        {
            if (DiagramsViewModel == null) return;

            if (DiagramsViewModel.SaveFile())
            {
                SaveHistory(DiagramsViewModel);
            }
        }

        private void SaveAsExecuted()
        {
            if (DiagramsViewModel == null) return;

            if (DiagramsViewModel.SaveFile(true))
            {
                SaveHistory(DiagramsViewModel);
            }
        }

        private void SaveHistory(DiagramsViewModel diagramsViewModel)
        {
            HistoryList.Remove(DiagramsViewModel.FileName);
            HistoryList.Insert(0, DiagramsViewModel.FileName);
            File.WriteAllText(_history, JsonConvert.SerializeObject(HistoryList));
        }

        private bool Save_Enable()
        {
            return DiagramsViewModel != null;
        }

        private void PasteExecuted()
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.Paste();
        }

        private bool Paste_Enabled()
        {
            return Clipboard.ContainsData(DataFormats.Xaml);
        }

        private void CopyExecuted()
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.CopyCurrentSelection();
        }

        private bool Copy_Enabled()
        {
            return DiagramsViewModel != null && DiagramsViewModel.DiagramViewModel != null && DiagramsViewModel.DiagramViewModel.SelectedItems.Count() > 0;
        }

        private void DeleteExecuted()
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.DeleteCurrentSelection();
        }

        private bool Delete_Enabled()
        {
            return DiagramsViewModel != null && DiagramsViewModel.DiagramViewModel != null && DiagramsViewModel.DiagramViewModel.SelectedItems.Count() > 0;
        }

        private void CutExecuted()
        {
            if (DiagramsViewModel != null)

                DiagramsViewModel.CutCurrentSelection();
        }

        private bool Cut_Enabled()
        {
            return DiagramsViewModel != null && DiagramsViewModel.DiagramViewModel != null && DiagramsViewModel.DiagramViewModel.SelectedItems.Count() > 0;
        }

        private void FormatExecuted()
        {
            _service.DrawModeViewModel.CursorMode = CursorMode.Format;
        }

        private bool Format_Enabled()
        {
            return DiagramsViewModel != null && DiagramsViewModel.DiagramViewModel != null && DiagramsViewModel.DiagramViewModel.SelectedItems.Count() == 1;
        }

        private void New_Executed(string type = "Normal")
        {
            IsOpenBackstage = false;
            if (type == DiagramType.FlowChart.ToString())
            {
                DiagramsViewModel = new FlowchartViewModel(this, NewNameHelper.GetNewName(DiagramsViewModels.Select(p => p.Title), "新建-"), "*", (DiagramType)Enum.Parse(typeof(DiagramType), type));
            }
            else if (type == DiagramType.Logical.ToString())
            {
                DiagramsViewModel = new LogicalViewModel(this, NewNameHelper.GetNewName(DiagramsViewModels.Select(p => p.Title), "新建-"), "*", (DiagramType)Enum.Parse(typeof(DiagramType), type));
            }
            else
            {
                DiagramsViewModel = new DiagramsViewModel(this, NewNameHelper.GetNewName(DiagramsViewModels.Select(p => p.Title), "新建-"), "*", (DiagramType)Enum.Parse(typeof(DiagramType), type));
            }

            DiagramsViewModels.Add(DiagramsViewModel);
        }

        private void ExitExecuted()
        {
            throw new NotImplementedException();
        }

        private void GroupExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.GroupExecuted(para);
        }

        private void UngroupExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.UngroupExecuted(para);
        }

        #region 布局
        private void AlignTopExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AlignTopExecuted(para);
        }
        private void AlignVerticalCentersExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AlignVerticalCentersExecuted(para);
        }
        private void AlignBottomExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AlignBottomExecuted(para);
        }
        private void AlignLeftExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AlignLeftExecuted(para);
        }
        private void AlignHorizontalCentersExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AlignHorizontalCentersExecuted(para);
        }
        private void AlignRightExecuted(object para)
        {
            DiagramsViewModel.AlignRightExecuted(para);
        }
        private void BringForwardExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.BringForwardExecuted(para);
        }
        private void BringToFrontExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.BringToFrontExecuted(para);
        }
        private void SendBackwardExecuted(object para)
        {
            DiagramsViewModel.SendBackwardExecuted(para);
        }
        private void SendToBackExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.SendToBackExecuted(para);
        }
        private void DistributeHorizontalExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.DistributeHorizontalExecuted(para);
        }
        private void DistributeVerticalExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.DistributeVerticalExecuted(para);
        }
        private void SelectAllExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.SelectAllExecuted(para);
        }
        #endregion

        private void CenterExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.CenterExecuted(para);
        }

        private void SameWidthExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.SameWidthExecuted(para);
        }

        private void SameHeightExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.SameHeightExecuted(para);
        }

        private void SameSizeExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.SameSizeExecuted(para);
        }

        private void SameAngleExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.SameAngleExecuted(para);
        }

        private void LockExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            LockObjectViewModel.LockObject[0].IsChecked = true;
        }

        private void UnlockExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            LockObjectViewModel.LockObject.ForEach(p => p.IsChecked = false);
        }

        private void AddPageExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AddPageExecuted(para);
        }

        private void AddCopyPageExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AddCopyPageExecuted(para);
        }

        private void RenamePageExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.RenamePageExecuted(para);
        }

        private void EndRenamePageExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.EndRenamePageExecuted(para);
        }


        private void DeletePageExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.DeletePageExecuted(para);
        }


        private void AddImageExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AddImageExecuted(null);
        }

        private void EditImageExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.EditImageExecuted(DiagramsViewModel.DiagramViewModel.SelectedItems?.FirstOrDefault());
        }

        private void ResizeImageExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.ResizeImageExecuted(DiagramsViewModel.DiagramViewModel.SelectedItems?.FirstOrDefault());
        }

        private void ResetImageExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.ResetImageExecuted(DiagramsViewModel.DiagramViewModel.SelectedItems?.FirstOrDefault());
        }

        private void AddVideoExectued(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AddVideoExecuted(null);
        }

        private void AddOutLineTextExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AddOutLineTextExecuted(para);
        }

        private void AddBarcodeExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            DiagramsViewModel.AddBarcodeExecuted(para);
        }

        private void SelectedColorExecuted(object para)
        {
            if (DiagramsViewModel == null) return;

            switch (ColorType)
            {
                case ColorType.Text: DiagramsViewModel.SetFont(new FontViewModel() { FontColor = (Color)para }, "FontColor"); break;
                case ColorType.Fill: DiagramsViewModel.SetColor(new ColorViewModel() { FillColor = new ColorObject() { Color = (Color)para } }, "FillColor"); break;
                case ColorType.Line: DiagramsViewModel.SetColor(new ColorViewModel() { LineColor = new ColorObject() { Color = (Color)para } }, "LineColor"); break;
            }
        }

        private void AboutExecuted()
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        #region 方法
        private Color[] GenerateStandardGradients()
        {
            var count = ColorGallery.StandardThemeColors.Length;
            List<Color> result = new List<Color>();
            for (var i = 0; i < count; i++)
            {
                //var colors = ColorGallery.GetGradient(ColorGallery.StandardThemeColors[i], 10);
                //for (var j = 9; j >= 0; j--)
                //{
                //    result.Add(colors[j]);
                //}
            }
            {
                //var colors = ColorGallery.GetGradient(Colors.Black, 10);
                //for (var j = 9; j >= 0; j--)
                //{
                //    result.Add(colors[j]);
                //}
            }
            return result.ToArray();
        }
        #endregion
    }
}
