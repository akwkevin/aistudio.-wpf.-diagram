using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Util.DiagramDesigner.Controls
{
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            base.DragDelta += new DragDeltaEventHandler(ResizeThumb_DragDelta);
            base.DragStarted += ResizeThumb_DragStarted;
            base.DragCompleted += ResizeThumb_DragCompleted;
        }

        private List<SelectableDesignerItemViewModelBase> designerItems;
        IDiagramViewModel diagarmViewModel;

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            DesignerItemViewModelBase designerItem = this.DataContext as DesignerItemViewModelBase;
            diagarmViewModel = designerItem.Parent;
            if (designerItem != null && designerItem.IsSelected)
            {
                designerItems = designerItem.SelectedItems.ToList();
                foreach (DesignerItemViewModelBase item in designerItems.OfType<DesignerItemViewModelBase>())
                {
                    item.BeginDo = true;
                    item.SetOldValue(item.ItemWidth, nameof(item.ItemWidth));
                    item.SetOldValue(item.ItemHeight, nameof(item.ItemHeight));
                }

                e.Handled = true;
            }
            else
            {
                designerItems = null;
            }
        }

        private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (designerItems != null)
            {
                foreach (DesignerItemViewModelBase item in designerItems.OfType<DesignerItemViewModelBase>())
                {
                    item.BeginDo = false;
                    item.RaiseItemWidthHeight();
                }

                e.Handled = true;
            }
        }


        void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (designerItems != null)
            {
                double minLeft, minTop, minDeltaHorizontal, minDeltaVertical;
                double dragDeltaVertical, dragDeltaHorizontal, scale;

                CalculateDragLimits(designerItems.OfType<DesignerItemViewModelBase>(), out minLeft, out minTop,
                                    out minDeltaHorizontal, out minDeltaVertical);

                foreach (DesignerItemViewModelBase item in designerItems.OfType<DesignerItemViewModelBase>())
                {
                    if (item != null && item.ParentId == Guid.Empty)
                    {
                        switch (base.VerticalAlignment)
                        {
                            case VerticalAlignment.Bottom:
                                dragDeltaVertical = Math.Min(-e.VerticalChange, minDeltaVertical);
                                scale = (item.ItemHeight - dragDeltaVertical) / item.ItemHeight;
                                DragBottom(scale, item, diagarmViewModel.SelectionService);
                                break;
                            case VerticalAlignment.Top:
                                double top = item.Top;
                                dragDeltaVertical = Math.Min(Math.Max(-minTop, e.VerticalChange), minDeltaVertical);
                                scale = (item.ItemHeight - dragDeltaVertical) / item.ItemHeight;
                                DragTop(scale, item, diagarmViewModel.SelectionService);
                                break;
                            default:
                                break;
                        }

                        switch (base.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                double left = item.Left;
                                dragDeltaHorizontal = Math.Min(Math.Max(-minLeft, e.HorizontalChange), minDeltaHorizontal);
                                scale = (item.ItemWidth - dragDeltaHorizontal) / item.ItemWidth;
                                DragLeft(scale, item, diagarmViewModel.SelectionService);
                                break;
                            case HorizontalAlignment.Right:
                                dragDeltaHorizontal = Math.Min(-e.HorizontalChange, minDeltaHorizontal);
                                scale = (item.ItemWidth - dragDeltaHorizontal) / item.ItemWidth;
                                DragRight(scale, item, diagarmViewModel.SelectionService);
                                break;
                            default:
                                break;
                        }
                    }
                }

                e.Handled = true;
            }
        }

        #region Helper methods

        private void DragLeft(double scale, DesignerItemViewModelBase item, SelectionService selectionService)
        {
            IEnumerable<DesignerItemViewModelBase> groupItems = selectionService.GetGroupMembers(item).Cast<DesignerItemViewModelBase>();

            double groupLeft = item.Left + item.ItemWidth;
            foreach (DesignerItemViewModelBase groupItem in groupItems)
            {
                double groupItemLeft = groupItem.Left;
                double delta = (groupLeft - groupItemLeft) * (scale - 1);
                groupItem.Left = groupItemLeft - delta;
                groupItem.ItemWidth = groupItem.ItemWidth * scale;
            }
        }

        private void DragTop(double scale, DesignerItemViewModelBase item, SelectionService selectionService)
        {
            IEnumerable<DesignerItemViewModelBase> groupItems = selectionService.GetGroupMembers(item).Cast<DesignerItemViewModelBase>();
            double groupBottom = item.Top + item.ItemHeight;
            foreach (DesignerItemViewModelBase groupItem in groupItems)
            {
                double groupItemTop = groupItem.Top;
                double delta = (groupBottom - groupItemTop) * (scale - 1);
                groupItem.Top = groupItemTop - delta;
                groupItem.ItemHeight = groupItem.ItemHeight * scale;
            }
        }

        private void DragRight(double scale, DesignerItemViewModelBase item, SelectionService selectionService)
        {
            IEnumerable<DesignerItemViewModelBase> groupItems = selectionService.GetGroupMembers(item).Cast<DesignerItemViewModelBase>();

            double groupLeft = item.Left;
            foreach (DesignerItemViewModelBase groupItem in groupItems)
            {
                double groupItemLeft = groupItem.Left;
                double delta = (groupItemLeft - groupLeft) * (scale - 1);

                groupItem.Left = groupItemLeft + delta;
                groupItem.ItemWidth = groupItem.ItemWidth * scale;
            }
        }

        private void DragBottom(double scale, DesignerItemViewModelBase item, SelectionService selectionService)
        {
            IEnumerable<DesignerItemViewModelBase> groupItems = selectionService.GetGroupMembers(item).Cast<DesignerItemViewModelBase>();
            double groupTop = item.Top;
            foreach (DesignerItemViewModelBase groupItem in groupItems)
            {
                double groupItemTop = groupItem.Top;
                double delta = (groupItemTop - groupTop) * (scale - 1);

                groupItem.Top = groupItemTop + delta;
                groupItem.ItemHeight = groupItem.ItemHeight * scale;
            }
        }

        private void CalculateDragLimits(IEnumerable<DesignerItemViewModelBase> selectedItems, out double minLeft, out double minTop, out double minDeltaHorizontal, out double minDeltaVertical)
        {
            minLeft = double.MaxValue;
            minTop = double.MaxValue;
            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;

            // drag limits are set by these parameters: canvas top, canvas left, minHeight, minWidth
            // calculate min value for each parameter for each item
            foreach (DesignerItemViewModelBase item in selectedItems)
            {
                double left = item.Left;
                double top = item.Top;

                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                minDeltaVertical = Math.Min(minDeltaVertical, 10);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal, 10);
            }
        }

        #endregion
    }
}