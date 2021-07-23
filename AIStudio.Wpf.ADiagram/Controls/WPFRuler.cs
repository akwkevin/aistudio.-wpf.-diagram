using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace AIStudio.Wpf.ADiagram.Controls
{
    /// <summary>
    /// A ruler control which supports both centimeters and inches. In order to use it vertically, change the <see cref="Marks">Marks</see> property to <c>Up</c> and rotate it ninety degrees.
    /// </summary>
    /// <remarks>
    /// Contributions from <see
    /// cref="http://www.orbifold.net/default/?p=2295&amp;cpage=1#comment-61500">Raf
    /// Lenfers</see>
    /// </remarks>
    public class Ruler : FrameworkElement
    {
        #region Fields
        private double SegmentHeight;
        private readonly Pen p = new Pen(Brushes.Black, 1.0);
        private readonly Pen ThinPen = new Pen(Brushes.Black, 0.5);
        private readonly Pen BorderPen = new Pen(Brushes.Gray, 1.0);
        private readonly Pen RedPen = new Pen(Brushes.Red, 2.0);
        #endregion

        #region Properties
        #region Background
        public Brush Background
        {
            get
            {
                return (Brush)GetValue(BackgroundProperty);
            }
            set
            {
                SetValue(BackgroundProperty, value);
            }
        }

        /// <summary>
        /// Identifies the Length dependency property.
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
             DependencyProperty.Register(
                  "Background",
                  typeof(Brush),
                  typeof(Ruler),
                  new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), FrameworkPropertyMetadataOptions.AffectsRender));
        #endregion

        #region Length
        /// <summary>
        /// Gets or sets the length of the ruler. If the <see cref="AutoSize"/> property is set to false (default) this
        /// is a fixed length. Otherwise the length is calculated based on the actual width of the ruler.
        /// </summary>
        public double Length
        {
            get
            {
                if (this.AutoSize)
                {
                    return (double)(Unit == Unit.Cm ? DipHelper.DipToCm(this.ActualWidth) : DipHelper.DipToInch(this.ActualWidth)) / this.Zoom;
                }
                else
                {
                    return (double)GetValue(LengthProperty);
                }
            }
            set
            {
                SetValue(LengthProperty, value);
            }
        }

        /// <summary>
        /// Identifies the Length dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty =
             DependencyProperty.Register(
                  "Length",
                  typeof(double),
                  typeof(Ruler),
                  new FrameworkPropertyMetadata(20D, FrameworkPropertyMetadataOptions.AffectsRender));
        #endregion

        #region AutoSize
        /// <summary>
        /// Gets or sets the AutoSize behavior of the ruler.
        /// false (default): the lenght of the ruler results from the <see cref="Length"/> property. If the window size is changed, e.g. wider
        ///						than the rulers length, free space is shown at the end of the ruler. No rescaling is done.
        /// true				 : the length of the ruler is always adjusted to its actual width. This ensures that the ruler is shown
        ///						for the actual width of the window.
        /// </summary>
        public bool AutoSize
        {
            get
            {
                return (bool)GetValue(AutoSizeProperty);
            }
            set
            {
                SetValue(AutoSizeProperty, value);
                this.InvalidateVisual();
            }
        }

        /// <summary>
        /// Identifies the AutoSize dependency property.
        /// </summary>
        public static readonly DependencyProperty AutoSizeProperty =
             DependencyProperty.Register(
                  "AutoSize",
                  typeof(bool),
                  typeof(Ruler),
                  new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        #endregion

        #region Zoom
        /// <summary>
        /// Gets or sets the zoom factor for the ruler. The default value is 1.0. 
        /// </summary>
        public double Zoom
        {
            get
            {
                return (double)GetValue(ZoomProperty);
            }
            set
            {
                SetValue(ZoomProperty, value);
                this.InvalidateVisual();
            }
        }

        /// <summary>
        /// Identifies the Zoom dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(double), typeof(Ruler),
            new FrameworkPropertyMetadata((double)1.0,
                FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #region Chip

        /// <summary>
        /// Chip Dependency Property
        /// </summary>
        public static readonly DependencyProperty ChipProperty =
             DependencyProperty.Register("Chip", typeof(double), typeof(Ruler),
                  new FrameworkPropertyMetadata((double)-1000,
                        FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Sets the location of the chip in the units of the ruler.
        /// So, to set the chip to 10 in cm units the chip needs to be set to 10.
        /// Use the <see cref="DipHelper"/> class for conversions.
        /// </summary>
        public double Chip
        {
            get { return (double)GetValue(ChipProperty); }
            set { SetValue(ChipProperty, value); }
        }
        #endregion

        #region CountShift

        /// <summary>
        /// CountShift Dependency Property
        /// </summary>
        public static readonly DependencyProperty CountShiftProperty =
             DependencyProperty.Register("CountShift", typeof(double), typeof(Ruler),
                  new FrameworkPropertyMetadata(0d,
                        FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// By default the counting of inches or cm starts at zero, this property allows you to shift
        /// the counting.
        /// </summary>
        public double CountShift
        {
            get { return (double)GetValue(CountShiftProperty); }
            set { SetValue(CountShiftProperty, value); }
        }

        #endregion

        #region Marks

        /// <summary>
        /// Marks Dependency Property
        /// </summary>
        public static readonly DependencyProperty MarksProperty =
             DependencyProperty.Register("Marks", typeof(MarksLocation), typeof(Ruler),
                  new FrameworkPropertyMetadata(MarksLocation.Up,
                         FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Gets or sets where the marks are shown in the ruler.
        /// </summary>
        public MarksLocation Marks
        {
            get { return (MarksLocation)GetValue(MarksProperty); }
            set { SetValue(MarksProperty, value); }
        }

        #endregion

        #region Unit
        /// <summary>
        /// Gets or sets the unit of the ruler.
        /// Default value is Unit.Cm.
        /// </summary>
        public Unit Unit
        {
            get { return (Unit)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        /// <summary>
        /// Identifies the Unit dependency property.
        /// </summary>
        public static readonly DependencyProperty UnitProperty =
             DependencyProperty.Register(
                  "Unit",
                  typeof(Unit),
                  typeof(Ruler),
                  new FrameworkPropertyMetadata(Unit.Cm, FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #endregion

        #region Constructor
        static Ruler()
        {
            HeightProperty.OverrideMetadata(typeof(Ruler), new FrameworkPropertyMetadata(20.0));
        }
        public Ruler()
        {
            SegmentHeight = this.Height - 10;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Participates in rendering operations.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var rect = new Rect(0, 0, RenderSize.Width, RenderSize.Height);
            drawingContext.DrawRectangle(Background, null, rect);

            double xDest = (Unit == Unit.Cm ? DipHelper.CmToDip(Length) : DipHelper.InchToDip(Length)) * this.Zoom;
            drawingContext.DrawRectangle(null, BorderPen, new Rect(new Point(0.0, 0.0), new Point(xDest, Height)));
            double chip = Unit == Unit.Cm ? DipHelper.CmToDip(Chip) : DipHelper.InchToDip(Chip);
            drawingContext.DrawLine(RedPen, new Point(chip, 0), new Point(chip, Height));

            //画偏移位置之前的
            if (CountShift < 0)
            {
                for (double dUnit = -CountShift; dUnit > -1; dUnit--)
                {
                    double d;
                    if (Unit == Unit.Cm)
                    {
                        d = DipHelper.CmToDip(dUnit) * this.Zoom;
                        if (dUnit < Length)
                        {
                            for (int i = 1; i <= 9; i++)
                            {
                                if (i != 5)
                                {
                                    double dMm = DipHelper.CmToDip(dUnit + 0.1 * i) * this.Zoom;
                                    if (Marks == MarksLocation.Up)
                                        drawingContext.DrawLine(ThinPen, new Point(dMm, 0), new Point(dMm, SegmentHeight / 3.0));
                                    else
                                        drawingContext.DrawLine(ThinPen, new Point(dMm, Height), new Point(dMm, Height - SegmentHeight / 3.0));
                                }
                            }
                            double dMiddle = DipHelper.CmToDip(dUnit + 0.5) * this.Zoom;
                            if (Marks == MarksLocation.Up)
                                drawingContext.DrawLine(p, new Point(dMiddle, 0), new Point(dMiddle, SegmentHeight * 2.0 / 3.0));
                            else
                                drawingContext.DrawLine(p, new Point(dMiddle, Height), new Point(dMiddle, Height - SegmentHeight * 2.0 / 3.0));
                        }
                    }
                    else
                    {
                        d = DipHelper.InchToDip(dUnit) * this.Zoom;
                        if (dUnit < Length)
                        {
                            if (Marks == MarksLocation.Up)
                            {
                                double dQuarter = DipHelper.InchToDip(dUnit + 0.25) * this.Zoom;
                                drawingContext.DrawLine(ThinPen, new Point(dQuarter, 0),
                                                                new Point(dQuarter, SegmentHeight / 3.0));
                                double dMiddle = DipHelper.InchToDip(dUnit + 0.5) * this.Zoom;
                                drawingContext.DrawLine(p, new Point(dMiddle, 0),
                                                                new Point(dMiddle, SegmentHeight * 2D / 3D));
                                double d3Quarter = DipHelper.InchToDip(dUnit + 0.75) * this.Zoom;
                                drawingContext.DrawLine(ThinPen, new Point(d3Quarter, 0),
                                                                new Point(d3Quarter, SegmentHeight / 3.0));
                            }
                            else
                            {
                                double dQuarter = DipHelper.InchToDip(dUnit + 0.25) * this.Zoom;
                                drawingContext.DrawLine(ThinPen, new Point(dQuarter, Height),
                                                                new Point(dQuarter, Height - SegmentHeight / 3.0));
                                double dMiddle = DipHelper.InchToDip(dUnit + 0.5) * this.Zoom;
                                drawingContext.DrawLine(p, new Point(dMiddle, Height),
                                                                new Point(dMiddle, Height - SegmentHeight * 2D / 3D));
                                double d3Quarter = DipHelper.InchToDip(dUnit + 0.75) * this.Zoom;
                                drawingContext.DrawLine(ThinPen, new Point(d3Quarter, Height),
                                                                new Point(d3Quarter, Height - SegmentHeight / 3.0));
                            }
                        }
                    }
                    if (Marks == MarksLocation.Up)
                        drawingContext.DrawLine(p, new Point(d, 0), new Point(d, SegmentHeight));
                    else
                        drawingContext.DrawLine(p, new Point(d, Height), new Point(d, Height - SegmentHeight));


                    if ((dUnit != 0.0) && (dUnit < Length))
                    {
                        FormattedText ft = new FormattedText(
                            (dUnit + CountShift).ToString(CultureInfo.CurrentCulture),
                             CultureInfo.CurrentCulture,
                             FlowDirection.LeftToRight,
                             new Typeface("Arial"),
                             DipHelper.PtToDip(6),
                             Brushes.DimGray);
                        ft.SetFontWeight(FontWeights.Regular);
                        ft.TextAlignment = TextAlignment.Center;

                        if (Marks == MarksLocation.Up)
                            drawingContext.DrawText(ft, new Point(d, Height - ft.Height));
                        else
                            drawingContext.DrawText(ft, new Point(d, Height - SegmentHeight - ft.Height));
                    }
                }
            }


            //画偏移位置之后的
            for (double dUnit = -CountShift; dUnit <= Length; dUnit++)
            {
                double d;
                if (Unit == Unit.Cm)
                {
                    d = DipHelper.CmToDip(dUnit) * this.Zoom;
                    if (dUnit < Length)
                    {
                        for (int i = 1; i <= 9; i++)
                        {
                            if (i != 5)
                            {
                                double dMm = DipHelper.CmToDip(dUnit + 0.1 * i) * this.Zoom;
                                if (Marks == MarksLocation.Up)
                                    drawingContext.DrawLine(ThinPen, new Point(dMm, 0), new Point(dMm, SegmentHeight / 3.0));
                                else
                                    drawingContext.DrawLine(ThinPen, new Point(dMm, Height), new Point(dMm, Height - SegmentHeight / 3.0));
                            }
                        }
                        double dMiddle = DipHelper.CmToDip(dUnit + 0.5) * this.Zoom;
                        if (Marks == MarksLocation.Up)
                            drawingContext.DrawLine(p, new Point(dMiddle, 0), new Point(dMiddle, SegmentHeight * 2.0 / 3.0));
                        else
                            drawingContext.DrawLine(p, new Point(dMiddle, Height), new Point(dMiddle, Height - SegmentHeight * 2.0 / 3.0));
                    }
                }
                else
                {
                    d = DipHelper.InchToDip(dUnit) * this.Zoom;
                    if (dUnit < Length)
                    {
                        if (Marks == MarksLocation.Up)
                        {
                            double dQuarter = DipHelper.InchToDip(dUnit + 0.25) * this.Zoom;
                            drawingContext.DrawLine(ThinPen, new Point(dQuarter, 0),
                                                            new Point(dQuarter, SegmentHeight / 3.0));
                            double dMiddle = DipHelper.InchToDip(dUnit + 0.5) * this.Zoom;
                            drawingContext.DrawLine(p, new Point(dMiddle, 0),
                                                            new Point(dMiddle, SegmentHeight * 2D / 3D));
                            double d3Quarter = DipHelper.InchToDip(dUnit + 0.75) * this.Zoom;
                            drawingContext.DrawLine(ThinPen, new Point(d3Quarter, 0),
                                                            new Point(d3Quarter, SegmentHeight / 3.0));
                        }
                        else
                        {
                            double dQuarter = DipHelper.InchToDip(dUnit + 0.25) * this.Zoom;
                            drawingContext.DrawLine(ThinPen, new Point(dQuarter, Height),
                                                            new Point(dQuarter, Height - SegmentHeight / 3.0));
                            double dMiddle = DipHelper.InchToDip(dUnit + 0.5) * this.Zoom;
                            drawingContext.DrawLine(p, new Point(dMiddle, Height),
                                                            new Point(dMiddle, Height - SegmentHeight * 2D / 3D));
                            double d3Quarter = DipHelper.InchToDip(dUnit + 0.75) * this.Zoom;
                            drawingContext.DrawLine(ThinPen, new Point(d3Quarter, Height),
                                                            new Point(d3Quarter, Height - SegmentHeight / 3.0));
                        }
                    }
                }
                if (Marks == MarksLocation.Up)
                    drawingContext.DrawLine(p, new Point(d, 0), new Point(d, SegmentHeight));
                else
                    drawingContext.DrawLine(p, new Point(d, Height), new Point(d, Height - SegmentHeight));


                if ((dUnit != 0.0) && (dUnit < Length))
                {
                    FormattedText ft = new FormattedText(
                        (dUnit + CountShift).ToString(CultureInfo.CurrentCulture),
                         CultureInfo.CurrentCulture,
                         FlowDirection.LeftToRight,
                         new Typeface("Arial"),
                         DipHelper.PtToDip(6),
                         Brushes.DimGray);
                    ft.SetFontWeight(FontWeights.Regular);
                    ft.TextAlignment = TextAlignment.Center;

                    if (Marks == MarksLocation.Up)
                        drawingContext.DrawText(ft, new Point(d, Height - ft.Height));
                    else
                        drawingContext.DrawText(ft, new Point(d, Height - SegmentHeight - ft.Height));
                }
            }
        }

        /// <summary>
        /// Measures an instance during the first layout pass prior to arranging it.
        /// </summary>
        /// <param name="availableSize">A maximum Size to not exceed.</param>
        /// <returns>The maximum Size for the instance.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
            //Size desiredSize;
            //if (Unit == Unit.Cm)
            //{
            //    desiredSize = new Size(DipHelper.CmToDip(Length), Height);
            //}
            //else
            //{
            //    desiredSize = new Size(DipHelper.InchToDip(Length), Height);
            //}
            //return desiredSize;
        }
        #endregion
    }

    /// <summary>
    /// The unit type of the ruler.
    /// </summary>
    public enum Unit
    {
        /// <summary>
        /// the unit is Centimeter.
        /// </summary>
        Cm,

        /// <summary>
        /// The unit is Inch.
        /// </summary>
        Inch
    };

    public enum MarksLocation
    {
        Up, Down
    }

    /// <summary>
    /// A helper class for DIP (Device Independent Pixels) conversion and scaling operations.
    /// </summary>
    public static class DipHelper
    {
        /// <summary>
        /// Converts millimeters to DIP (Device Independant Pixels).
        /// </summary>
        /// <param name="mm">A millimeter value.</param>
        /// <returns>A DIP value.</returns>
        public static double MmToDip(double mm)
        {
            return CmToDip(mm / 10.0);
        }

        /// <summary>
        /// Converts centimeters to DIP (Device Independant Pixels).
        /// </summary>
        /// <param name="cm">A centimeter value.</param>
        /// <returns>A DIP value.</returns>
        public static double CmToDip(double cm)
        {
            return (cm * 96.0 / 2.54);
        }

        /// <summary>
        /// Converts inches to DIP (Device Independant Pixels).
        /// </summary>
        /// <param name="inch">An inch value.</param>
        /// <returns>A DIP value.</returns>
        public static double InchToDip(double inch)
        {
            return (inch * 96.0);
        }
        public static double DipToInch(double dip)
        {
            return dip / 96D;
        }

        /// <summary>
        /// Converts font points to DIP (Device Independant Pixels).
        /// </summary>
        /// <param name="pt">A font point value.</param>
        /// <returns>A DIP value.</returns>
        public static double PtToDip(double pt)
        {
            return (pt * 96.0 / 72.0);
        }

        /// <summary>
        /// Converts DIP (Device Independant Pixels) to centimeters.
        /// </summary>
        /// <param name="dip">A DIP value.</param>
        /// <returns>A centimeter value.</returns>
        public static double DipToCm(double dip)
        {
            return (dip * 2.54 / 96.0);
        }

        /// <summary>
        /// Converts DIP (Device Independant Pixels) to millimeters.
        /// </summary>
        /// <param name="dip">A DIP value.</param>
        /// <returns>A millimeter value.</returns>
        public static double DipToMm(double dip)
        {
            return DipToCm(dip) * 10.0;
        }

        /// <summary>
        /// Gets the system DPI scale factor (compared to 96 dpi).
        /// From http://blogs.msdn.com/jaimer/archive/2007/03/07/getting-system-dpi-in-wpf-app.aspx
        /// Should not be called before the Loaded event (else XamlException mat throw)
        /// </summary>
        /// <returns>A Point object containing the X- and Y- scale factor.</returns>
        private static Point GetSystemDpiFactor()
        {
            PresentationSource source = PresentationSource.FromVisual(Application.Current.MainWindow);
            Matrix m = source.CompositionTarget.TransformToDevice;
            return new Point(m.M11, m.M22);
        }

        private const double DpiBase = 96.0;

        /// <summary>
        /// Gets the system configured DPI.
        /// </summary>
        /// <returns>A Point object containing the X- and Y- DPI.</returns>
        public static Point GetSystemDpi()
        {
            Point sysDpiFactor = GetSystemDpiFactor();
            return new Point(
                 sysDpiFactor.X * DpiBase,
                 sysDpiFactor.Y * DpiBase);
        }

        /// <summary>
        /// Gets the physical pixel density (DPI) of the screen.
        /// </summary>
        /// <param name="diagonalScreenSize">Size - in inch - of the diagonal of the screen.</param>
        /// <returns>A Point object containing the X- and Y- DPI.</returns>
        public static Point GetPhysicalDpi(double diagonalScreenSize)
        {
            Point sysDpiFactor = GetSystemDpiFactor();
            double pixelScreenWidth = SystemParameters.PrimaryScreenWidth * sysDpiFactor.X;
            double pixelScreenHeight = SystemParameters.PrimaryScreenHeight * sysDpiFactor.Y;
            double formatRate = pixelScreenWidth / pixelScreenHeight;

            double inchHeight = diagonalScreenSize / Math.Sqrt(formatRate * formatRate + 1.0);
            double inchWidth = formatRate * inchHeight;

            double xDpi = Math.Round(pixelScreenWidth / inchWidth);
            double yDpi = Math.Round(pixelScreenHeight / inchHeight);

            return new Point(xDpi, yDpi);
        }

        /// <summary>
        /// Converts a DPI into a scale factor (compared to system DPI).
        /// </summary>
        /// <param name="dpi">A Point object containing the X- and Y- DPI to convert.</param>
        /// <returns>A Point object containing the X- and Y- scale factor.</returns>
        public static Point DpiToScaleFactor(Point dpi)
        {
            Point sysDpi = GetSystemDpi();
            return new Point(
                 dpi.X / sysDpi.X,
                 dpi.Y / sysDpi.Y);
        }

        /// <summary>
        /// Gets the scale factor to apply to a WPF application
        /// so that 96 DIP always equals 1 inch on the screen (whatever the system DPI).
        /// </summary>
        /// <param name="diagonalScreenSize">Size - in inch - of the diagonal of the screen</param>
        /// <returns>A Point object containing the X- and Y- scale factor.</returns>
        public static Point GetScreenIndependentScaleFactor(double diagonalScreenSize)
        {
            return DpiToScaleFactor(GetPhysicalDpi(diagonalScreenSize));
        }
    }
}
