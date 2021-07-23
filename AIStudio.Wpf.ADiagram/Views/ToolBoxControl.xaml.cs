using System.Windows;
using System.Windows.Controls;

namespace AIStudio.Wpf.ADiagram.Views
{
    /// <summary>
    /// ToolBoxControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolBoxControl : UserControl
    {

        #region IsExpanded

        /// <summary>
        /// BindingWidthAndHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ToolBoxControl),
                new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Gets or sets the BindingWidthAndHeight property. This dependency property 
        /// indicates if the ResizableItemsControl is in Composing mode.
        /// </summary>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        #endregion

        public ToolBoxControl()
        {
            InitializeComponent();
        }
    }
}
