using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Resources;
using System.Runtime.InteropServices;

namespace Util.DiagramDesigner
{
    public class DesignerCanvas : Canvas
    {
        private IDiagramViewModel _viewModel { get { return DataContext as IDiagramViewModel; } }
        private IDiagramServiceProvider _service { get { return DiagramServicesProvider.Instance.Provider; } }

        private ConnectorViewModel partialConnection;
        private List<Connector> connectorsHit = new List<Connector>();

        private Point? rubberbandSelectionStartPoint = null;

        #region GridCellSize

        public static readonly DependencyProperty GridCellSizeProperty =
            DependencyProperty.Register("GridCellSize",
                                       typeof(Size),
                                       typeof(DesignerCanvas),
                                       new FrameworkPropertyMetadata(new Size(50, 50), FrameworkPropertyMetadataOptions.AffectsRender));

        public Size GridCellSize
        {
            get { return (Size)GetValue(GridCellSizeProperty); }
            set { SetValue(GridCellSizeProperty, value); }
        }

        #endregion

        #region ShowGrid

        public static readonly DependencyProperty ShowGridProperty =
            DependencyProperty.Register("ShowGrid",
                                       typeof(bool),
                                       typeof(DesignerCanvas),
                                       new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool ShowGrid
        {
            get { return (bool)GetValue(ShowGridProperty); }
            set { SetValue(ShowGridProperty, value); }
        }

        #endregion

        #region GridColor

        public static readonly DependencyProperty GridColorProperty =
            DependencyProperty.Register("GridColor",
                                       typeof(Color),
                                       typeof(DesignerCanvas),
                                       new FrameworkPropertyMetadata(Colors.LightGray, FrameworkPropertyMetadataOptions.AffectsRender));

        public Color GridColor
        {
            get { return (Color)GetValue(GridColorProperty); }
            set { SetValue(GridColorProperty, value); }
        }

        #endregion

        #region GridMargin 单位mm

        public static readonly DependencyProperty GridMarginProperty =
            DependencyProperty.Register("GridMargin",
                                       typeof(double),
                                       typeof(DesignerCanvas),
                                       new FrameworkPropertyMetadata(28d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double GridMargin
        {
            get { return (double)GetValue(GridMarginProperty); }
            set { SetValue(GridMarginProperty, value); }
        }

        #endregion

        public DesignerCanvas()
        {
            this.AllowDrop = true;
            Mediator.Instance.Register(this);

            _service.PropertyChanged += _service_PropertyChanged;
        }

        protected override void OnRender(DrawingContext dc)
        {
            var rect = new Rect(0, 0, RenderSize.Width, RenderSize.Height);
            dc.DrawRectangle(Background, null, rect);
            if (ShowGrid && GridCellSize.Width > 0 && GridCellSize.Height > 0)
                DrawGrid(dc, rect);
        }

        protected virtual void DrawGrid(DrawingContext dc, Rect rect)
        {
            //using .5 forces wpf to draw a single pixel line
            for (var i = GridMargin + 0.5; i < rect.Height - GridMargin; i += GridCellSize.Height)
                dc.DrawLine(new Pen(new SolidColorBrush(GridColor), 1), new Point(GridMargin, i), new Point(rect.Width - GridMargin, i));
            dc.DrawLine(new Pen(new SolidColorBrush(GridColor), 1), new Point(GridMargin, rect.Height - GridMargin), new Point(rect.Width - GridMargin, rect.Height - GridMargin));

            for (var i = GridMargin + 0.5; i < rect.Width - GridMargin; i += GridCellSize.Width)
                dc.DrawLine(new Pen(new SolidColorBrush(GridColor), 1), new Point(i, GridMargin), new Point(i, rect.Height - GridMargin));
            dc.DrawLine(new Pen(new SolidColorBrush(GridColor), 1), new Point(rect.Width - GridMargin, GridMargin), new Point(rect.Width - GridMargin, rect.Height - GridMargin));
        }

        private void _service_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is IDrawModeViewModel)
            {
                if (e.PropertyName == nameof(CursorMode))
                {
                    if (_service.DrawModeViewModel.CursorMode == CursorMode.Format)
                    {
                        EnterFormat();
                    }
                    else if (_service.DrawModeViewModel.CursorMode == CursorMode.Move)
                    {
                        EnterMove();
                    }
                }
            }
        }

        #region Format/Move
        private void EnterFormat()
        {
            StreamResourceInfo sri = Application.GetResourceStream(new Uri("pack://application:,,,/Util.DiagramDesigner;component/Images/FormatPainter.cur", UriKind.RelativeOrAbsolute));
            this.Cursor = new Cursor(sri.Stream);
            foreach (SelectableDesignerItemViewModelBase item in _viewModel.Items)
            {
                item.IsHitTestVisible = false;
            }
        }

        private void EnterMove()
        {
            this.Cursor = Cursors.SizeAll;
            foreach (SelectableDesignerItemViewModelBase item in _viewModel.Items)
            {
                item.IsHitTestVisible = false;
            }
        }

        private void ExitCursor()
        {
            this.Cursor = Cursors.Arrow;
            foreach (SelectableDesignerItemViewModelBase item in _viewModel.Items)
            {
                item.IsHitTestVisible = true;
            }
            _service.DrawModeViewModel.CursorMode = CursorMode.Normal;
        }
        #endregion

        private void Format(SelectableDesignerItemViewModelBase source, SelectableDesignerItemViewModelBase target)
        {
            CopyHelper.CopyPropertyValue(source.ColorViewModel, target.ColorViewModel);
            CopyHelper.CopyPropertyValue(source.FontViewModel, target.FontViewModel);
        }

        private Connector sourceConnector;
        public Connector SourceConnector
        {
            get { return sourceConnector; }
            set
            {
                if (sourceConnector != value)
                {
                    sourceConnector = value;
                    connectorsHit.Add(sourceConnector);
                    FullyCreatedConnectorInfo sourceDataItem = sourceConnector.DataContext as FullyCreatedConnectorInfo;


                    Rect rectangleBounds = sourceConnector.TransformToVisual(this).TransformBounds(new Rect(sourceConnector.RenderSize));
                    Point point = new Point(rectangleBounds.Left + (rectangleBounds.Width / 2),
                                            rectangleBounds.Bottom + (rectangleBounds.Height / 2));
                    partialConnection = new ConnectorViewModel(sourceDataItem, new PartCreatedConnectionInfo(point));
                    _viewModel.DirectAddItemCommand.Execute(partialConnection);
                }
            }
        }

        private FullyCreatedConnectorInfo sourceConnectorInfo;
        public FullyCreatedConnectorInfo SourceConnectorInfo
        {
            get { return sourceConnectorInfo; }
            set
            {
                if (sourceConnectorInfo != value)
                {
                    sourceConnectorInfo = value;
                    sourceConnector = new Connector() { Name = "占位" };//占位使用
                    connectorsHit.Add(sourceConnector);

                    Rect rectangleBounds = new Rect(sourceConnectorInfo.DataItem.Left, sourceConnectorInfo.DataItem.Top, 3, 3);
                    Point point = new Point(rectangleBounds.Left + (rectangleBounds.Width / 2),
                                            rectangleBounds.Bottom + (rectangleBounds.Height / 2));
                    partialConnection = new ConnectorViewModel(sourceConnectorInfo, new PartCreatedConnectionInfo(point));
                    _viewModel.DirectAddItemCommand.Execute(partialConnection);
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (_service.DrawModeViewModel.CursorMode == CursorMode.Format)
            {
                var element = (e.OriginalSource as FrameworkElement);
                if (element.DataContext is SelectableDesignerItemViewModelBase target)
                {
                    Format(_viewModel.SelectedItems.FirstOrDefault(), target);
                    return;
                }

                ExitCursor();
            }
            else if (_service.DrawModeViewModel.CursorMode == CursorMode.Move)
            {
                ExitCursor();
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //if we are source of event, we are rubberband selecting
                if (e.Source == this)
                {
                    // in case that this click is the start for a 
                    // drag operation we cache the start point
                    rubberbandSelectionStartPoint = e.GetPosition(this);

                    IDiagramViewModel vm = (this.DataContext as IDiagramViewModel);
                    if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    {
                        vm.ClearSelectedItemsCommand.Execute(null);

                    }
                    e.Handled = true;

                    if (_service.DrawModeViewModel.GetDrawMode() == DrawMode.ConnectingLine)
                    {
                        if (connectorsHit.Count == 0)
                        {
                            LinkPointDesignerItemViewModel pointItemView = new LinkPointDesignerItemViewModel(rubberbandSelectionStartPoint.Value);                        
                            _viewModel.DirectAddItemCommand.Execute(pointItemView);
                            SourceConnectorInfo = pointItemView.TopConnector;
                        }
                    }
                }
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Point currentPoint = e.GetPosition(this);
            _viewModel.CurrentPoint = currentPoint;
            var point = CursorPointManager.GetCursorPosition();
            _viewModel.CurrentColor = ColorPickerManager.GetColor(point.X, point.Y);

            if (_service.DrawModeViewModel.CursorMode == CursorMode.Move)
            {
                _viewModel.SelectedItems.OfType<DesignerItemViewModelBase>().ToList().ForEach(p =>
                {
                    p.Left = currentPoint.X;
                    p.Top = currentPoint.Y;
                });
                return;
            }

            if (SourceConnector != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    partialConnection.SinkConnectorInfo = new PartCreatedConnectionInfo(currentPoint);
                    HitTesting(currentPoint);
                }
            }
            else
            {
                // if mouse button is not pressed we have no drag operation, ...
                if (e.LeftButton != MouseButtonState.Pressed && _service.DrawModeViewModel.GetDrawMode() != DrawMode.DirectLine)
                    rubberbandSelectionStartPoint = null;

                // ... but if mouse button is pressed and start
                // point value is set we do have one
                if (this.rubberbandSelectionStartPoint.HasValue)
                {
                    // create rubberband adorner
                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                    if (adornerLayer != null)
                    {
                        RubberbandAdorner adorner = new RubberbandAdorner(this, rubberbandSelectionStartPoint);
                        if (adorner != null)
                        {
                            adornerLayer.Add(adorner);
                        }
                    }
                }
            }
            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (_service.DrawModeViewModel.GetDrawMode() == DrawMode.DirectLine)
            {
                return;
            }

            Mediator.Instance.NotifyColleagues<bool>("DoneDrawingMessage", true);

            if (sourceConnector != null)
            {
                FullyCreatedConnectorInfo sourceDataItem = sourceConnectorInfo ?? (sourceConnector.DataContext as FullyCreatedConnectorInfo);
                if (connectorsHit.Count() == 2)
                {
                    Connector sinkConnector = connectorsHit.Last();
                    FullyCreatedConnectorInfo sinkDataItem = sinkConnector.DataContext as FullyCreatedConnectorInfo;

                    int indexOfLastTempConnection = sinkDataItem.DataItem.Parent.Items.Count - 1;
                    sinkDataItem.DataItem.Parent.DirectRemoveItemCommand.Execute(
                        sinkDataItem.DataItem.Parent.Items[indexOfLastTempConnection]);
                    sinkDataItem.DataItem.Parent.AddItemCommand.Execute(new ConnectorViewModel(sourceDataItem, sinkDataItem));
                }
                else if (_service.DrawModeViewModel.GetDrawMode() == DrawMode.ConnectingLine && connectorsHit.Count() == 1)
                {
                    LinkPointDesignerItemViewModel pointItemView = new LinkPointDesignerItemViewModel(e.GetPosition(this));
                    FullyCreatedConnectorInfo sinkDataItem = pointItemView.TopConnector;

                    int indexOfLastTempConnection = _viewModel.Items.Count - 1;
                    _viewModel.DirectRemoveItemCommand.Execute(_viewModel.Items[indexOfLastTempConnection]);
                    _viewModel.DirectAddItemCommand.Execute(pointItemView);

                    var connector = new ConnectorViewModel(sourceDataItem, sinkDataItem);
                    _viewModel.AddItemCommand.Execute(connector);

                    sourceDataItem.DataItem.ZIndex++;
                    connector.ZIndex--;
                }
                else
                {
                    //Need to remove last item as we did not finish drawing the path
                    int indexOfLastTempConnection = sourceDataItem.DataItem.Parent.Items.Count - 1;
                    sourceDataItem.DataItem.Parent.DirectRemoveItemCommand.Execute(
                        sourceDataItem.DataItem.Parent.Items[indexOfLastTempConnection]);


                }
            }
     
            connectorsHit = new List<Connector>();
            sourceConnector = null;
            sourceConnectorInfo = null;

            _service.DrawModeViewModel.ResetDrawMode();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            foreach (UIElement element in this.InternalChildren)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                //measure desired size for each child
                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            // add margin 
            size.Width += 10;
            size.Height += 10;

            return size;
        }

        private void HitTesting(Point hitPoint)
        {
            DependencyObject hitObject = this.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null &&
                    hitObject.GetType() != typeof(DesignerCanvas))
            {
                if (hitObject is Connector)
                {
                    if (connectorsHit.Any(p => p.Name == "占位"))
                    {
                        connectorsHit.Remove(connectorsHit.FirstOrDefault(p => p.Name == "占位"));
                    }
                    if (!connectorsHit.Contains(hitObject as Connector))
                        connectorsHit.Add(hitObject as Connector);
                }
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }

        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            DragObject dragObject = e.Data.GetData(typeof(DragObject)) as DragObject;
            if (dragObject != null)
            {
                (DataContext as IDiagramViewModel).ClearSelectedItemsCommand.Execute(null);
                Point position = e.GetPosition(this);
                DesignerItemViewModelBase itemBase = null;
                if (dragObject.DesignerItem != null)
                {
                    itemBase = (DesignerItemViewModelBase)Activator.CreateInstance(dragObject.ContentType, null, dragObject.DesignerItem);
                }
                else
                {
                    itemBase = (DesignerItemViewModelBase)Activator.CreateInstance(dragObject.ContentType);
                    itemBase.Icon = dragObject.Icon;
                    itemBase.ColorViewModel = CopyHelper.Mapper(dragObject.ColorViewModel);
                }
                itemBase.Left = Math.Max(0, position.X - itemBase.ItemWidth / 2);
                itemBase.Top = Math.Max(0, position.Y - itemBase.ItemHeight / 2);
                (DataContext as IDiagramViewModel).AddItemCommand.Execute(itemBase);
            }
            var dragFile = e.Data.GetData(DataFormats.FileDrop);
            if (dragFile != null && dragFile is string[] files)
            {
                foreach (var file in files)
                {
                    (DataContext as IDiagramViewModel).ClearSelectedItemsCommand.Execute(null);
                    Point position = e.GetPosition(this);
                    ImageItemViewModel itemBase = new ImageItemViewModel();
                    itemBase.Icon = file;
                    itemBase.Suffix = System.IO.Path.GetExtension(itemBase.Icon).ToLower();
                    itemBase.InitWidthAndHeight();
                    itemBase.AutoSize();
                    
                    itemBase.Left = Math.Max(0, position.X - itemBase.ItemWidth / 2);
                    itemBase.Top = Math.Max(0, position.Y - itemBase.ItemHeight / 2);
                    (DataContext as IDiagramViewModel).AddItemCommand.Execute(itemBase);
                }
            }
            e.Handled = true;
        }
    }
}
