using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Util.DiagramDesigner.Controls
{
    public class PointDragThumb : Thumb
    {
        public PointDragThumb()
        {
            base.DragDelta += new DragDeltaEventHandler(DragThumb_DragDelta);
            base.DragStarted += DragThumb_DragStarted;
            base.DragCompleted += DragThumb_DragCompleted;
        }


        private void DragThumb_DragStarted(object sender, DragStartedEventArgs e)
        {

        }

        private void DragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {

        }

        void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.DataContext is PointInfoBase point)
            {
                double minLeft = double.MaxValue;
                double minTop = double.MaxValue;

                double left = point.X;
                double top = point.Y;
                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                point.X += deltaHorizontal;
                point.Y += deltaVertical;
            }
        }


    }
}
