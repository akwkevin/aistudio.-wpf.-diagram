using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Util.DiagramDesigner.Controls
{
    public class RotateThumb : Thumb
    {
        private Point centerPoint;
        private Vector startVector;
        private double initialAngle;
        private Canvas designerCanvas;
        private DesignerItemViewModelBase designerItem;
        private RotateTransform rotateTransform;

        public RotateThumb()
        {
            DragDelta += RotateThumb_DragDelta;
            DragStarted += RotateThumb_DragStarted;
            DragCompleted += RotateThumb_DragCompleted;
        }

        private List<SelectableDesignerItemViewModelBase> designerItems;

        private void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = this.DataContext as DesignerItemViewModelBase;

            if (this.designerItem != null)
            {
                designerItems = designerItem.SelectedItems.ToList();

                foreach (DesignerItemViewModelBase item in designerItems.OfType<DesignerItemViewModelBase>())
                {
                    item.BeginDo = true;
                    item.SetOldValue(this.designerItem.Angle, nameof(this.designerItem.Angle));
                }

                this.designerCanvas = GetDesignerCanvas(this);
                if (this.designerCanvas != null)
                {
                    this.centerPoint =
                        new Point(this.designerItem.Left + this.designerItem.ItemWidth * 0.5,
                                  this.designerItem.Top + this.designerItem.ItemHeight * 0.5);

                    Point startPoint = Mouse.GetPosition(this.designerCanvas);
                    this.startVector = Point.Subtract(startPoint, this.centerPoint);


                    this.initialAngle = this.designerItem.Angle;
                }
                e.Handled = true;
            }
            else
            {
                designerItems = null;
            }    
        }

        private void RotateThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.designerItem = this.DataContext as DesignerItemViewModelBase;

            if (this.designerItems != null)
            {
                foreach (DesignerItemViewModelBase item in designerItems.OfType<DesignerItemViewModelBase>())
                {
                    item.BeginDo = false;
                    item.RaiseAngle();
                }

                e.Handled = true;
            }
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItems != null && this.designerCanvas != null)
            {
                Point currentPoint = Mouse.GetPosition(this.designerCanvas);
                Vector deltaVector = Point.Subtract(currentPoint, this.centerPoint);

                double angle = Vector.AngleBetween(this.startVector, deltaVector);

                foreach (DesignerItemViewModelBase item in designerItems.OfType<DesignerItemViewModelBase>())
                {
                    item.Angle = this.initialAngle + Math.Round(angle, 0);
                }
                e.Handled = true;
            }
        }

        private DesignerCanvas GetDesignerCanvas(DependencyObject element)
        {
            while (element != null && !(element is DesignerCanvas))
                element = VisualTreeHelper.GetParent(element);

            return element as DesignerCanvas;
        }
    }
}
