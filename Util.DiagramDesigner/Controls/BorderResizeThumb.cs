using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Util.DiagramDesigner.Controls
{
    public class BorderResizeThumb : Thumb
    {
        #region ResizeMode

        /// <summary>
        /// BindingWidthAndHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty ResizeModeProperty =
            DependencyProperty.Register("ResizeMode", typeof(ResizeMode), typeof(BorderResizeThumb),
                new FrameworkPropertyMetadata(ResizeMode.Size));

        /// <summary>
        /// Gets or sets the BindingWidthAndHeight property. This dependency property 
        /// indicates if the ResizableItemsControl is in Composing mode.
        /// </summary>
        public ResizeMode ResizeMode
        {
            get { return (ResizeMode)GetValue(ResizeModeProperty); }
            set { SetValue(ResizeModeProperty, value); }
        }

        #endregion

        #region ResizeElement

        /// <summary>
        /// BindingWidthAndHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty ResizeElementProperty =
            DependencyProperty.Register("ResizeElement", typeof(FrameworkElement), typeof(BorderResizeThumb),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the BindingWidthAndHeight property. This dependency property 
        /// indicates if the ResizableItemsControl is in Composing mode.
        /// </summary>
        public FrameworkElement ResizeElement
        {
            get { return (FrameworkElement)GetValue(ResizeElementProperty); }
            set { SetValue(ResizeElementProperty, value); }
        }
        #endregion

        public BorderResizeThumb()
        {
            base.DragDelta += new DragDeltaEventHandler(ResizeThumb_DragDelta);
        }

        void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ResizeElement == null) return;

            if (ResizeMode == ResizeMode.Size)
            {
                switch (base.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        if (ResizeElement.Height - e.VerticalChange > ResizeElement.MinHeight && ResizeElement.Height - e.VerticalChange < ResizeElement.MaxHeight)
                        {
                            ResizeElement.Height -= e.VerticalChange;
                        }
                        break;
                    case VerticalAlignment.Bottom:
                        if (ResizeElement.Height + e.VerticalChange > ResizeElement.MinHeight && ResizeElement.Height + e.VerticalChange < ResizeElement.MaxHeight)
                        {
                            ResizeElement.Height += e.VerticalChange;
                        }
                        break;
                    default:
                        break;
                }

                switch (base.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        if (ResizeElement.Width - e.HorizontalChange > ResizeElement.MinWidth && ResizeElement.Width - e.HorizontalChange < ResizeElement.MaxWidth)
                        {
                            ResizeElement.Width -= e.HorizontalChange;
                        }
                        break;
                    case HorizontalAlignment.Right:
                        if (ResizeElement.Width + e.HorizontalChange > ResizeElement.MinWidth && ResizeElement.Width + e.HorizontalChange < ResizeElement.MaxWidth)
                        {
                            ResizeElement.Width += e.HorizontalChange;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (ResizeMode == ResizeMode.Margin)
            {
                switch (base.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        if (ResizeElement.Margin.Top + e.VerticalChange > 0)
                        {
                            ResizeElement.Margin = new Thickness(ResizeElement.Margin.Left, ResizeElement.Margin.Top + e.VerticalChange, ResizeElement.Margin.Right, ResizeElement.Margin.Bottom);
                        }
                        break;
                    case VerticalAlignment.Bottom:
                        if (ResizeElement.Margin.Bottom - e.VerticalChange > 0)
                        {
                            ResizeElement.Margin = new Thickness(ResizeElement.Margin.Left, ResizeElement.Margin.Top, ResizeElement.Margin.Right, ResizeElement.Margin.Bottom - e.VerticalChange);
                        }
                        break;
                    default:
                        break;
                }

                switch (base.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        if (ResizeElement.Margin.Left + e.HorizontalChange > 0)
                        {
                            ResizeElement.Margin = new Thickness(ResizeElement.Margin.Left + e.HorizontalChange, ResizeElement.Margin.Top, ResizeElement.Margin.Right, ResizeElement.Margin.Bottom);
                        }
                        break;
                    case HorizontalAlignment.Right:
                        if (ResizeElement.Margin.Right - e.HorizontalChange > 0)
                        {
                            ResizeElement.Margin = new Thickness(ResizeElement.Margin.Left, ResizeElement.Margin.Top, ResizeElement.Margin.Right - e.HorizontalChange, ResizeElement.Margin.Bottom);
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (ResizeMode == ResizeMode.DragMargin)
            {
                ResizeElement.Margin = new Thickness(ResizeElement.Margin.Left + e.HorizontalChange, ResizeElement.Margin.Top + e.VerticalChange, ResizeElement.Margin.Right - e.HorizontalChange, ResizeElement.Margin.Bottom - e.VerticalChange);
            }
        }
    }

    public enum ResizeMode
    {
        Size,
        Margin,
        DragMargin,
    }
}