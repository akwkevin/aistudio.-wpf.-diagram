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
using WpfAnimatedGif;
using ZXing;
using ZXing.Presentation;

namespace AIStudio.Wpf.ADiagram.Controls
{
    /// <summary>
    /// Barcode.xaml 的交互逻辑
    /// </summary>
    public partial class Barcode : UserControl
    {
        public Barcode()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Barcode), new FrameworkPropertyMetadata(
            string.Empty,
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender, OnFormattedTextInvalidated));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
            "Format", typeof(BarcodeFormat), typeof(Barcode), new FrameworkPropertyMetadata(
            BarcodeFormat.QR_CODE,
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender, OnFormattedTextInvalidated));

        public BarcodeFormat Format
        {
            get => (BarcodeFormat)GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(double), typeof(Barcode), new FrameworkPropertyMetadata(
            512d,
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender, OnFormattedTextInvalidated));

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
           "Icon", typeof(string), typeof(Barcode), new FrameworkPropertyMetadata(null, OnIconChanged));

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }


        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var barcode = (Barcode)d;
            barcode.OnIconChanged();
        }

        private void OnIconChanged()
        {
            if (!string.IsNullOrEmpty(this.Icon))
            {
                var suffix = System.IO.Path.GetExtension(this.Icon).ToLower();
                Image image = new Image() { Stretch = Stretch.UniformToFill };
                var icon = new BitmapImage(new Uri(Icon));
                if (suffix != ".gif")
                {
                    image.Source = icon;
                }
                else
                {
                    image.SetCurrentValue(ImageBehavior.AnimatedSourceProperty, icon);
                    image.SetCurrentValue(ImageBehavior.AutoStartProperty, true);
                }
                PART_Icon.Content = image;
            }
            else
            {
                PART_Icon.Content = null;
            }
        }

        private static void OnFormattedTextInvalidated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var barcode = (Barcode)d;
            barcode.OnFormattedTextInvalidated();
        }

        private void OnFormattedTextInvalidated()
        {
            var writer = new BarcodeWriterGeometry
            {
                Format = Format,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = (int)this.Size,
                    Width = (int)this.Size,
                    Margin = 0
                }
            };
            var image = writer.Write(Text ?? "AIStudio画板");
            imageBarcodeEncoderGeometry.Data = image;
        }
    }
}
