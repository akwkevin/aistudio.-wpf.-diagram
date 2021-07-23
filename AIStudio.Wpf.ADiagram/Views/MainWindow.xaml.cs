using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AIStudio.Wpf.ADiagram.ViewModels;
using ControlzEx.Theming;
using Fluent;
using Fluent.Localization;
using Button = Fluent.Button;

namespace AIStudio.Wpf.ADiagram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private MainWindowViewModel MainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();

            this.HookEvents();

            MainWindowViewModel = new MainWindowViewModel();
            this.DataContext = MainWindowViewModel;
            this.Closing += MainWindow_Closing;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            MainWindowViewModel.KeyCommand.Execute(e.KeyboardDevice.Modifiers.ToString() + "+" + e.Key.ToString());
        }     

        private void HookEvents()
        {

            this.PreviewMouseWheel += this.OnPreviewMouseWheel;
        }

        private void ZoomSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var textFormattingMode = e.NewValue > 1.0 || Math.Abs(e.NewValue - 1.0) < double.Epsilon ? TextFormattingMode.Ideal : TextFormattingMode.Display;
            TextOptions.SetTextFormattingMode(this, textFormattingMode);
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) == false
                && Keyboard.IsKeyDown(Key.RightCtrl) == false)
            {
                return;
            }

            var newZoomValue = this.zoomSlider.Value + (e.Delta > 0 ? 0.1 : -0.1);

            this.zoomSlider.Value = Math.Max(Math.Min(newZoomValue, this.zoomSlider.Maximum), this.zoomSlider.Minimum);

            e.Handled = true;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if (true == printDialog.ShowDialog())
            {
                printDialog.PrintVisual(this.table, "WPF Diagram");
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定要退出系统?", "退出", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

     
    }
}
