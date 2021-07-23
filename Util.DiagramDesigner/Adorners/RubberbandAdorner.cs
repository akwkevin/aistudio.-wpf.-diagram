using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.DiagramDesigner
{
    public class RubberbandAdorner : Adorner
    {
        private Point? startPoint;
        private Point? endPoint;
        private List<Point> pointList = new List<Point>();
        private List<PointDesignerItemViewModel> pointDesignerItemViewModelList = new List<PointDesignerItemViewModel>();
        private Pen rubberbandPen;

        private DesignerCanvas _designerCanvas;

        private IDiagramViewModel _viewModel { get { return _designerCanvas.DataContext as IDiagramViewModel; } }
        private IDiagramServiceProvider _service { get { return DiagramServicesProvider.Instance.Provider; } }

        public RubberbandAdorner(DesignerCanvas designerCanvas, Point? dragStartPoint)
            : base(designerCanvas)
        {
            this._designerCanvas = designerCanvas;
            this.startPoint = dragStartPoint;
            rubberbandPen = new Pen(Brushes.LightSlateGray, 1);
            rubberbandPen.DashStyle = new DashStyle(new double[] { 2 }, 1);
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.IsMouseCaptured)
                    this.CaptureMouse();

                endPoint = e.GetPosition(this);
                if (this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Polyline)
                {
                    if (pointList.Count == 0 && this.startPoint.HasValue)
                    {
                        pointList.Add(this.startPoint.Value);
                    }
                    pointList.Add(endPoint.Value);
                }

                UpdateSelection();
                this.InvalidateVisual();
            }
            else if (this._service.DrawModeViewModel.GetDrawMode() == DrawMode.DirectLine)
            {
                if (pointList.Count == 0 && this.startPoint.HasValue)
                {
                    pointList.Add(this.startPoint.Value);
                    var item = new PointDesignerItemViewModel(startPoint.Value);
                    item.ShowConnectors = true;
                    _viewModel.DirectAddItemCommand.Execute(item);
                    pointDesignerItemViewModelList.Add(item);
                }
            }
            else
            {
                if (this.IsMouseCaptured) this.ReleaseMouseCapture();
            }

            e.Handled = true;
        }

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._service.DrawModeViewModel.GetDrawMode() == DrawMode.DirectLine && e.ChangedButton == MouseButton.Left )
            {
                endPoint = e.GetPosition(this);              

                bool isend = false;
                var connector = ControlExtession.TryFindFromPoint<PointConnector>(this._designerCanvas, endPoint.Value);
                if (connector != null)
                {
                    if (object.Equals(connector.DataContext, pointDesignerItemViewModelList[0].TopConnector))
                    {
                        isend = true;
                    }
                }                

                if (isend == false)
                {
                    pointList.Add(endPoint.Value);
                    var item = new PointDesignerItemViewModel(endPoint.Value);
                    item.ShowConnectors = true;
                    _viewModel.DirectAddItemCommand.Execute(item);
                    pointDesignerItemViewModelList.Add(item);

                    UpdateSelection();
                    this.InvalidateVisual();

                    return;
                }
                else
                {
                    this._service.DrawModeViewModel.SetDrawMode(DrawMode.Polygon);
                }
                  
            }

            // release mouse capture
            if (this.IsMouseCaptured) this.ReleaseMouseCapture();

            // remove this adorner from adorner layer
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
            if (adornerLayer != null)
                adornerLayer.Remove(this);

            if (this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Line ||
                this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Rectangle ||
                this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Ellipse ||
                this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Polyline ||
                this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Polygon ||
                this._service.DrawModeViewModel.GetDrawMode() == DrawMode.DirectLine ||
                this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Text)
            {
                if (this.startPoint.HasValue && this.endPoint.HasValue)
                {
                    if (this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Polyline 
                        || this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Polygon
                        || this._service.DrawModeViewModel.GetDrawMode() == DrawMode.DirectLine)
                    {
                        ShapeDesignerItemViewModel itemBase = new ShapeDesignerItemViewModel(this._service.DrawModeViewModel.GetDrawMode(), pointList);
                        _viewModel.AddItemCommand.Execute(itemBase);
                        itemBase.PointDesignerItemViewModels.ForEach(p =>
                        {
                            p.ParentId = itemBase.Id;
                            _viewModel.DirectAddItemCommand.Execute(p);
                        });                        
                    }
                    else if (this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Text)
                    {
                        TextDesignerItemViewModel itemBase = new TextDesignerItemViewModel();
                        Point position = e.GetPosition(this);
                        itemBase.Left = Math.Min(this.startPoint.Value.X, this.endPoint.Value.X);
                        itemBase.Top = Math.Min(this.startPoint.Value.Y, this.endPoint.Value.Y);
                        itemBase.ItemWidth = Math.Abs(this.endPoint.Value.X - this.startPoint.Value.X);
                        itemBase.ItemHeight = Math.Abs(this.endPoint.Value.Y - this.startPoint.Value.Y);

                        _viewModel.AddItemCommand.Execute(itemBase);
                    }
                    else
                    {
                        ShapeDesignerItemViewModel itemBase = new ShapeDesignerItemViewModel(this._service.DrawModeViewModel.GetDrawMode(), new List<Point> { this.startPoint.Value, this.endPoint.Value });
                        _viewModel.AddItemCommand.Execute(itemBase);
                        itemBase.PointDesignerItemViewModels.ForEach(p =>
                        {
                            p.ParentId = itemBase.Id;
                            _viewModel.DirectAddItemCommand.Execute(p);
                        });
                    }
                }               
                this._service.DrawModeViewModel.ResetDrawMode();
            }

            pointDesignerItemViewModelList.ForEach(p => _viewModel.DirectRemoveItemCommand.Execute(p));

            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // without a background the OnMouseMove event would not be fired !
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (this.startPoint.HasValue && this.endPoint.HasValue)
            {
                if (this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Line)
                {
                    dc.DrawLine(rubberbandPen, this.startPoint.Value, this.endPoint.Value);
                }
                else if (this._service.DrawModeViewModel.GetDrawMode() == DrawMode.Ellipse)
                {
                    var centerPoint = new Point((this.startPoint.Value.X + this.endPoint.Value.X) / 2, (this.startPoint.Value.Y + this.endPoint.Value.Y) / 2);
                    dc.DrawEllipse(Brushes.Transparent, rubberbandPen, centerPoint, Math.Abs(this.startPoint.Value.X - this.endPoint.Value.X) / 2, Math.Abs(this.startPoint.Value.Y - this.endPoint.Value.Y) / 2);
                }
                else if (_service.DrawModeViewModel.GetDrawMode() == DrawMode.Polyline)
                {
                    var disList = pointList.ToList();
                    for (int i = 1; i < pointList.Count; i++)
                    {
                        dc.DrawLine(rubberbandPen, disList[i - 1], disList[i]);
                    }
                }
                else if (_service.DrawModeViewModel.GetDrawMode() == DrawMode.DirectLine)
                {
                    var disList = pointList.ToList();
                    for (int i = 1; i < pointList.Count; i++)
                    {
                        dc.DrawLine(rubberbandPen, disList[i - 1], disList[i]);
                    }
                }
                else
                {
                    dc.DrawRectangle(Brushes.Transparent, rubberbandPen, new Rect(this.startPoint.Value, this.endPoint.Value));
                }
            }
        }


        private T GetParent<T>(Type parentType, DependencyObject dependencyObject) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent.GetType() == parentType)
                return (T)parent;

            return GetParent<T>(parentType, parent);
        }



        private void UpdateSelection()
        {
            IDiagramViewModel vm = (_designerCanvas.DataContext as IDiagramViewModel);
            Rect rubberBand = new Rect(startPoint.Value, endPoint.Value);
            ItemsControl itemsControl = GetParent<ItemsControl>(typeof(ItemsControl), _designerCanvas);

            foreach (SelectableDesignerItemViewModelBase item in vm.Items)
            {
                if (item is SelectableDesignerItemViewModelBase)
                {
                    DependencyObject container = itemsControl.ItemContainerGenerator.ContainerFromItem(item);

                    Rect itemRect = VisualTreeHelper.GetDescendantBounds((Visual)container);
                    Rect itemBounds = ((Visual)container).TransformToAncestor(_designerCanvas).TransformBounds(itemRect);

                    if (rubberBand.Contains(itemBounds))
                    {
                        //item.IsSelected = true;
                        vm.SelectionService.AddToSelection(item);
                    }
                    else
                    {
                        if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                        {
                            //item.IsSelected = false;
                            vm.SelectionService.RemoveFromSelection(item);
                        }
                    }
                }
            }
        }
    }
}
