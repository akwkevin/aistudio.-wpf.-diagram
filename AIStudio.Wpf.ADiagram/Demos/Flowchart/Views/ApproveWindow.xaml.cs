using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Demos.Flowchart.Views
{
    /// <summary>
    /// ApproveWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ApproveWindow : Window
    {
        public ApproveWindow()
        {
            InitializeComponent();
        }
    }

    public class ApproveWindowViewModel : BindableBase
    {
        private int _status;
        public int Status
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

        private string _remark;
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                SetProperty(ref _remark, value);
            }
        }
    }
}
