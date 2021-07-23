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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Util.DiagramDesigner
{
    /// <summary>
    /// TextControl.xaml 的交互逻辑
    /// </summary>
    public partial class TextControl : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
           "DoubleEdit", typeof(bool), typeof(TextControl), new FrameworkPropertyMetadata(
           true));

        public bool DoubleEdit
        {
            get => (bool)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public TextControl()
        {
            InitializeComponent();

            this.Loaded += TextControl_Loaded;           
        }


        private void TextControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= TextControl_Loaded;

            PART_ShowText.Visibility = Visibility.Visible;
            PART_TextBlock.Visibility = Visibility.Collapsed;
            PART_ShowText.Focus();
            if (!string.IsNullOrEmpty(PART_ShowText.Text))
            {
                PART_ShowText.SelectionStart = PART_ShowText.Text.Length;
            }

            (this.DataContext as SelectableDesignerItemViewModelBase).PropertyChanged += TextControl_PropertyChanged;
            TextControl_PropertyChanged(this.DataContext, new System.ComponentModel.PropertyChangedEventArgs("IsSelected"));
        }

        private void TextControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                if (sender is SelectableDesignerItemViewModelBase itemViewModelBase)
                {
                    if (itemViewModelBase.IsSelected == false)
                    {
                        PART_ShowText.Visibility = Visibility.Collapsed;
                        PART_TextBlock.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (DoubleEdit == false)
            {
                PART_ShowText.Visibility = Visibility.Visible;
                PART_TextBlock.Visibility = Visibility.Collapsed;
                PART_ShowText.Focus();
                if (!string.IsNullOrEmpty(PART_ShowText.Text))
                {
                    PART_ShowText.SelectionStart = PART_ShowText.Text.Length;
                }
            }
        }

        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDoubleClick(e);

            if (DoubleEdit == true)
            {
                PART_ShowText.Visibility = Visibility.Visible;
                PART_TextBlock.Visibility = Visibility.Collapsed;
                PART_ShowText.Focus();
                if (!string.IsNullOrEmpty(PART_ShowText.Text))
                {
                    PART_ShowText.SelectionStart = PART_ShowText.Text.Length;
                }
            }
        }
    }

    public class ControlAttachProperty
    {
        #region WatermarkProperty 水印
        /// <summary>
        /// 水印
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
            "Watermark", typeof(string), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(""));

        public static string GetWatermark(DependencyObject d)
        {
            return (string)d.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }
        #endregion
    }
}
