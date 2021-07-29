using AIStudio.Wpf.BaseDiagram.Commands;
using AIStudio.Wpf.BaseDiagram.Models;
using System.Windows.Input;
using ZXing;

namespace AIStudio.Wpf.BaseDiagram.Extensions.ViewModels
{
    public class BarcodeDesignerItemData : TitleBindableBase
    {
        public BarcodeDesignerItemData()
        {

        }
        public BarcodeDesignerItemData(BarcodeDesignerItemViewModel item)
        {
            this.Title = "二维码";
            this.Text = item.Text;
            this.Icon = item.Icon;
            this.Margin = item.Margin;
            this.Format = item.Format;
        }

        public BarcodeFormat Format { get; set; }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                SetProperty(ref _text, value);
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

        private string _icon;
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

        
        private ICommand _iploadCommand;
        public ICommand UploadCommand
        {
            get
            {
                return this._iploadCommand ?? (this._iploadCommand = new DelegateCommand(() => this.Upload()));
            }
        }

        private void Upload()
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            openFile.Filter = "图片|*.bmp;*.jpg;*.jpeg;*.gif;*.png";

            if (openFile.ShowDialog() == true)
            {
                Icon = openFile.FileName;
            }
        }
    }
}
