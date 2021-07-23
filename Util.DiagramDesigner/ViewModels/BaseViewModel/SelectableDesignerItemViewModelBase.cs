using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace Util.DiagramDesigner
{

    public interface ISelectItems
    {
        SimpleCommand SelectItemCommand { get; }
    }


    public abstract class SelectableDesignerItemViewModelBase : BindableBase, ISelectItems, ISelectable, IGroupable
    {
        private IDiagramServiceProvider _service { get { return DiagramServicesProvider.Instance.Provider; } }

        public SelectableDesignerItemViewModelBase()
        {
            Init();
            (FontViewModel as FontViewModel).PropertyChanged += FontViewModel_PropertyChanged;
        }

        public SelectableDesignerItemViewModelBase(IDiagramViewModel parent, SelectableDesignerItemBase designer)
        {
            Init();
            LoadDesignerItemViewModel(parent, designer);
            (FontViewModel as FontViewModel).PropertyChanged += FontViewModel_PropertyChanged;
        }

        protected virtual void Init()
        {
            ColorViewModel = CopyHelper.Mapper(_service.ColorViewModel);
            FontViewModel = CopyHelper.Mapper<FontViewModel, IFontViewModel>(_service.FontViewModel);

            LockObjectViewModel = new LockObjectViewModel();
            SelectItemCommand = new SimpleCommand(ExecuteSelectItemCommand);
            EditCommand = new SimpleCommand(ExecuteEditCommand);
        }

        protected virtual void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            this.Parent = parent;

            this.Id = designerbase.Id;
            this.ParentId = designerbase.ParentId;
            this.IsGroup = designerbase.IsGroup;
            this.ZIndex = designerbase.ZIndex;
            this.Text = designerbase.Text;

            ColorViewModel = CopyHelper.Mapper(designerbase.ColorItem);
            FontViewModel = CopyHelper.Mapper<FontViewModel, FontItem>(designerbase.FontItem);
        }

        public virtual bool InitData()
        {
            return true;
        }
        public virtual bool EditData()
        {
            return true;
        }

        public List<SelectableDesignerItemViewModelBase> SelectedItems
        {
            //todo
            get { return Parent.SelectedItems; }
        }

        public IDiagramViewModel Parent { get; set; }
        public SimpleCommand SelectItemCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public Guid Id { get; set; }

        private Guid _parentId;
        public Guid ParentId
        {
            get
            {
                return _parentId;
            }
            set
            {
                SetProperty(ref _parentId, value);
            }
        }
        public SelectableDesignerItemViewModelBase ParentItem { get; set; }

        public bool IsGroup { get; set; }

        private bool _isSelected;
        [Browsable(false)]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    //如果没有文字，失去焦点自动清除
                    if (_isSelected == false && string.IsNullOrEmpty(Text))
                    {
                        ShowText = false;
                        if (this is TextDesignerItemViewModel)
                        {
                            if (ParentItem != null)
                            {
                                ParentItem.OutTextItem = null;
                            }
                            Parent.DirectRemoveItemCommand.Execute(this);
                        }
                    }

                }

            }
        }

        private int _zIndex;
        [Browsable(true)]
        [CanDo]
        public int ZIndex
        {
            get
            {
                return _zIndex;
            }
            set
            {
                SetProperty(ref _zIndex, value);
            }
        }

        private bool _isReadOnly;
        [Browsable(false)]
        public bool IsReadOnly
        {
            get
            {
                if (LockObjectViewModel != null && LockObjectViewModel.LockObject.FirstOrDefault(p => p.LockFlag == LockFlag.All).IsChecked == true)
                {
                    return true;
                }
                return _isReadOnly;
            }
            set
            {
                SetProperty(ref _isReadOnly, value);
            }
        }

        private bool _isHitTestVisible = true;
        [Browsable(false)]
        public bool IsHitTestVisible
        {
            get
            {
                return _isHitTestVisible;
            }
            set
            {
                if (SetProperty(ref _isHitTestVisible, value))
                {
                    RaisePropertyChanged("IsReadOnly");
                }
            }
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
                SetProperty(ref _colorViewModel, value);
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
                SetProperty(ref _fontViewModel, value);
            }
        }

        public ILockObjectViewModel LockObjectViewModel { get; set; }


        private string _text;
        [Browsable(true)]
        [CanDo]
        public string Text
        {
            get
            {
                if (FontViewModel.FontCase == FontCase.Upper)
                {
                    return _text?.ToUpper();
                }
                else if (FontViewModel.FontCase == FontCase.Lower)
                {
                    return _text?.ToLower();
                }
                else
                {
                    return _text;
                }
            }
            set
            {
                if (SetProperty(ref _text, value))
                {
                    if (!string.IsNullOrEmpty(_text))
                    {
                        ShowText = true;
                    }
                }
            }
        }

        private bool _showText;
        public virtual bool ShowText
        {
            get
            {
                return _showText;
            }
            set
            {
                SetProperty(ref _showText, value);
            }
        }

        public SelectableDesignerItemViewModelBase OutTextItem { get; set; }

        private void ExecuteSelectItemCommand(object param)
        {
            SelectItem((bool)param, !IsSelected);
        }

        private void SelectItem(bool newselect, bool select)
        {
            if (newselect)
            {
                foreach (var designerItemViewModelBase in Parent.SelectedItems.ToList())
                {
                    designerItemViewModelBase._isSelected = false;
                }
            }

            IsSelected = select;
        }

        private void FontViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FontCase")
            {
                RaisePropertyChanged("Text");
            }
        }

        protected virtual void ExecuteEditCommand(object param)
        {
            if (IsReadOnly == true) return;

            ShowText = true;
        }

        public virtual void Dispose()
        {
        }
    }
}
