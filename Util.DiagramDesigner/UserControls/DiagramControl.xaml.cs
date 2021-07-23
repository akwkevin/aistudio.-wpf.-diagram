using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Util.DiagramDesigner
{
    /// <summary>
    /// Interaction logic for DiagramControl.xaml
    /// </summary>
    public partial class DiagramControl : UserControl
    {
        public DiagramControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ZoomValueProperty = DependencyProperty.Register("ZoomValue", typeof(double), typeof(DiagramControl), new UIPropertyMetadata(1d));
        public double ZoomValue
        {
            get
            {
                return (double)GetValue(ZoomValueProperty);
            }
            set
            {
                SetValue(ZoomValueProperty, value);
            }
        }


        private void DesignerCanvas_Loaded(object sender, RoutedEventArgs e)
        {
     
        }

        private async void ScaleTransform_Changed(object sender, EventArgs e)
        {
            await System.Threading.Tasks.Task.Delay(100);
            ZoomValue = scale.ScaleX;
        }
    }
}
