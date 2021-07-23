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
    public class DragThumb : Thumb
    {
        public DragThumb()
        {
            base.DragDelta += new DragDeltaEventHandler(DragThumb_DragDelta);
            base.DragStarted += DragThumb_DragStarted;
            base.DragCompleted += DragThumb_DragCompleted;
        }

        private List<SelectableDesignerItemViewModelBase> designerItems;

        private void DragThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            SelectableDesignerItemViewModelBase designerItem = this.DataContext as SelectableDesignerItemViewModelBase;

            if (designerItem != null && designerItem.IsSelected)
            {
                // we only move DesignerItems
                designerItems = designerItem.SelectedItems.ToList();
                if (designerItem is ConnectorViewModel connector)
                {
                    designerItems.Add(connector.SourceConnectorInfo.DataItem);
                    designerItems.Add((connector.SinkConnectorInfo as FullyCreatedConnectorInfo).DataItem);

                    if (designerItem.OutTextItem != null)
                    {
                        designerItems.Remove(designerItem.OutTextItem);//这个自动计算位置
                    }
                }

                if (designerItem is PointDesignerItemViewModel)
                {
                    designerItems = new List<SelectableDesignerItemViewModelBase> { designerItem };
                }

                foreach (DesignerItemViewModelBase item in designerItems.OfType<DesignerItemViewModelBase>())
                {
                    item.BeginDo = true;
                    item.SetOldValue(item.Left, nameof(item.Left));
                    item.SetOldValue(item.Top, nameof(item.Top));
                }

                e.Handled = true;
            }
            else
            {
                designerItems = null;
            }
        }
        
        private void DragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (designerItems != null)
            {
                foreach (DesignerItemViewModelBase item in designerItems.OfType<DesignerItemViewModelBase>())
                {               
                    item.SetCellAlignment();
                    item.BeginDo = false;
                    item.RaiseTopLeft();
                }             

                e.Handled = true;
            }
        }

        void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (designerItems != null)
            {
                double minLeft = double.MaxValue;
                double minTop = double.MaxValue;        

                foreach (DesignerItemViewModelBase item in designerItems.OfType<DesignerItemViewModelBase>())
                {
                    double left = item.Left;
                    double top = item.Top;
                    minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                    minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                    double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                    double deltaVertical = Math.Max(-minTop, e.VerticalChange);
                    item.Left += deltaHorizontal;
                    item.Top += deltaVertical;
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
