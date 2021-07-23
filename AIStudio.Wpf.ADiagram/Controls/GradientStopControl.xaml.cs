using System;
using System.Collections.Generic;
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
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Controls
{
    /// <summary>
    /// GradientStopControl.xaml 的交互逻辑
    /// </summary>
    public partial class GradientStopControl : UserControl
    {
        public GradientStopControl()
        {
            InitializeComponent();
        }

        public ColorObject ColorObject
        {
            get { return this.DataContext as ColorObject; }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            var element = (e.OriginalSource as FrameworkElement);
            if (element.DataContext is Util.DiagramDesigner.GradientStop target)
            {
                ColorObject.SelectedGradientStop = target;

            }
        }
    }
}