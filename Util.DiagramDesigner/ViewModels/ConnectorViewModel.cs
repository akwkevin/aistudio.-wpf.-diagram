using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Util.DiagramDesigner.Helpers;

namespace Util.DiagramDesigner
{
    public class ConnectorViewModel : SelectableDesignerItemViewModelBase
    {
        public ConnectorViewModel(IDiagramViewModel parent, FullyCreatedConnectorInfo sourceConnectorInfo, FullyCreatedConnectorInfo sinkConnectorInfo,
            SelectableDesignerItemBase designer) : base(parent, designer)
        {
            Init(sourceConnectorInfo, sinkConnectorInfo);
        }

        public ConnectorViewModel(FullyCreatedConnectorInfo sourceConnectorInfo, ConnectorInfoBase sinkConnectorInfo)
        {
            Init(sourceConnectorInfo, sinkConnectorInfo);
        }


        public static IPathFinder PathFinder { get; set; }

        public bool IsFullConnection
        {
            get { return _sinkConnectorInfo is FullyCreatedConnectorInfo; }
        }

        private Point _sourceA;
        public Point SourceA
        {
            get
            {
                return _sourceA;
            }
            set
            {
                if (SetProperty(ref _sourceA, value))
                {
                    UpdateArea();
                }
            }
        }

        private Point _sourceB;
        public Point SourceB
        {
            get
            {
                return _sourceB;
            }
            set
            {
                if (SetProperty(ref _sourceB, value))
                {
                    UpdateArea();
                }
            }
        }

        private List<Point> _connectionPoints;
        public List<Point> ConnectionPoints
        {
            get
            {
                return _connectionPoints;
            }
            private set
            {
                SetProperty(ref _connectionPoints, value);
            }
        }

        private Point _startPoint;
        public Point StartPoint
        {
            get
            {
                return _startPoint;
            }
            private set
            {
                SetProperty(ref _startPoint, value);
            }
        }

        private Point _endPoint;
        public Point EndPoint
        {
            get
            {
                return _endPoint;
            }
            private set
            {
                SetProperty(ref _endPoint, value);
            }
        }

        private Rect _area;
        public Rect Area
        {
            get
            {
                return _area;
            }
            private set
            {
                if (SetProperty(ref _area, value))
                {                    
                    UpdateConnectionPoints();
                    OutTextItemLocation(_area, value);
                }
            }
        }

        public ConnectorInfo ConnectorInfo(ConnectorOrientation orientation, double left, double top, double width, double height, Point position)
        {

            return new ConnectorInfo()
            {
                Orientation = orientation,
                DesignerItemSize = new Size(width, height),
                DesignerItemLeft = left,
                DesignerItemTop = top,
                Position = position

            };
        }

        private FullyCreatedConnectorInfo _sourceConnectorInfo;
        public FullyCreatedConnectorInfo SourceConnectorInfo
        {
            get
            {
                return _sourceConnectorInfo;
            }
            set
            {
                if (SetProperty(ref _sourceConnectorInfo, value))
                {
                    SourceA = PointHelper.GetPointForConnector(_sourceConnectorInfo);
                    (_sourceConnectorInfo.DataItem as INotifyPropertyChanged).PropertyChanged += new WeakINPCEventHandler(ConnectorViewModel_PropertyChanged).Handler;
                }
            }
        }

        private ConnectorInfoBase _sinkConnectorInfo;
        public ConnectorInfoBase SinkConnectorInfo
        {
            get
            {
                return _sinkConnectorInfo;
            }
            set
            {
                if (SetProperty(ref _sinkConnectorInfo, value))
                {
                    if (_sinkConnectorInfo is FullyCreatedConnectorInfo)
                    {
                        SourceB = PointHelper.GetPointForConnector((FullyCreatedConnectorInfo)_sinkConnectorInfo);
                        (((FullyCreatedConnectorInfo)_sinkConnectorInfo).DataItem as INotifyPropertyChanged).PropertyChanged += new WeakINPCEventHandler(ConnectorViewModel_PropertyChanged).Handler;
                    }
                    else
                    {
                        SourceB = ((PartCreatedConnectionInfo)SinkConnectorInfo).CurrentLocation;
                    }
                }
            }
        }

        private void UpdateArea()
        {
            Area = new Rect(SourceA, SourceB);
        }

        private void UpdateConnectionPoints()
        {
            if (SinkConnectorInfo is FullyCreatedConnectorInfo && SourceConnectorInfo.DataItem is LinkPointDesignerItemViewModel && ((FullyCreatedConnectorInfo)SinkConnectorInfo).DataItem is LinkPointDesignerItemViewModel)
            {
                UpdateConnectionPointsByLine();
                return;
            }
            if (Parent.DiagramType == DiagramType.FlowChart)
            {
                UpdateConnectionPointsByFlowChart();
            }
            else
            {
                UpdateConnectionPointsByNormal();
            }

        }

        private void UpdateConnectionPointsByLine()
        {
            ConnectionPoints = new List<Point>()
                                   {

                                       new Point(SourceA.X  <  SourceB.X ? 0d : Area.Width, SourceA.Y  <  SourceB.Y ? 0d : Area.Height ),
                                       new Point(SourceA.X  >  SourceB.X ? 0d : Area.Width, SourceA.Y  >  SourceB.Y ? 0d : Area.Height)
                                   };
        }

        private void UpdateConnectionPointsByNormal()
        {
            ConnectionPoints = new List<Point>()
                                   {

                                       new Point(SourceA.X  <  SourceB.X ? 0d : Area.Width, SourceA.Y  <  SourceB.Y ? 0d : Area.Height ),
                                       new Point(SourceA.X  >  SourceB.X ? 0d : Area.Width, SourceA.Y  >  SourceB.Y ? 0d : Area.Height)
                                   };

            ConnectorInfo sourceInfo = ConnectorInfo(SourceConnectorInfo.Orientation,
                                            ConnectionPoints[0].X,
                                            ConnectionPoints[0].Y,
                                            SourceConnectorInfo.DataItem.ItemWidth,
                                            SourceConnectorInfo.DataItem.ItemHeight,
                                            ConnectionPoints[0]);

            StartPoint = ConnectionPoints[0];
            if (IsFullConnection)
            {
                EndPoint = ConnectionPoints.Last();
                ConnectorInfo sinkInfo = ConnectorInfo(SinkConnectorInfo.Orientation,
                                  ConnectionPoints[1].X,
                                  ConnectionPoints[1].Y,
                                  ((FullyCreatedConnectorInfo)_sinkConnectorInfo).DataItem.ItemWidth,
                                  ((FullyCreatedConnectorInfo)_sinkConnectorInfo).DataItem.ItemHeight,
                                  ConnectionPoints[1]);

                ConnectionPoints = PathFinder.GetConnectionLine(sourceInfo, sinkInfo, false, SourceConnectorInfo.IsInnerPoint);
            }
            else
            {
                ConnectionPoints = PathFinder.GetConnectionLine(sourceInfo, ConnectionPoints[1], SourceConnectorInfo.Orientation, SourceConnectorInfo.IsInnerPoint);
                EndPoint = new Point();
            }
        }

        #region
        private void UpdateConnectionPointsByFlowChart()
        {
            var points = new List<Point>();
            var ends = GetEndPoinds();

            points.Add(ends[0]);
            points.AddRange(GetMiddlePoints(ends[0], ends[1]));
            points.Add(ends[1]);
            var res = points.ToArray();
            //UpdateEdges(res);
            DoShift(res);
            ConnectionPoints = res.ToList();
            StartPoint = ConnectionPoints[0];
            EndPoint = ConnectionPoints.Last();
        }

        private IEnumerable<Point> GetMiddlePoints(Point start, Point end)
        {
            var points = new List<Point>();
            if (IsFullConnection)
            {
                var p0 = GetFirstSegment(SourceConnectorInfo, start, Parent.GridCellSize, Parent.GridMargin);
                var p1 = GetFirstSegment(SinkConnectorInfo, end, Parent.GridCellSize, Parent.GridMargin);

                if (p0 == p1)
                    return points;


                var p2 = new Point(GetNearestCross(p0.X, p1.X), GetNearestCross(p0.Y, p1.Y));
                var p3 = new Point(GetNearestCross(p1.X, p0.X), GetNearestCross(p1.Y, p0.Y));
                if (p2 == p3)
                {
                    points.Add(p0);
                    points.Add(p2);
                    points.Add(p1);
                }
                else
                {
                    points.Add(p0);
                    points.Add(p2);
                    if (!(Math.Abs(p2.X - p3.X) < 0.0001) && !(Math.Abs(p2.Y - p3.Y) < 0.0001))
                        points.Add(new Point(p2.X, p3.Y));
                    points.Add(p3);
                    points.Add(p1);
                }
                DoScale(points, Parent.GridCellSize, Parent.GridMargin);
            }
            return points;
        }

        private Point[] GetEndPoinds()
        {
            var linePoints = new Point[2];
            linePoints[0] = SourceA;
            linePoints[1] = SourceB;

            return linePoints;
        }

        private Point GetFirstSegment(ConnectorInfoBase port, Point point, Size cellSize, double margin)
        {
            double x = (int)((point.X - margin) / cellSize.Width) + 0.5;
            double y = (int)((point.Y - margin) / cellSize.Height) + 0.5;
            if (port.Orientation == ConnectorOrientation.Top)
                return new Point(x, y - 0.5);
            else if (port.Orientation == ConnectorOrientation.Bottom)
                return new Point(x, y + 0.5);
            else if (port.Orientation == ConnectorOrientation.Left)
                return new Point(x - 0.5, y);
            else
                return new Point(x + 0.5, y);
        }

        private double GetNearestCross(double a, double b)
        {
            if (Math.Abs(a - b) < 0.0001 && (int)a == a)
                return a;
            else if (a < b)
                return Math.Ceiling(a);
            else
                return Math.Floor(a);
        }

        private void DoScale(List<Point> points, Size cellSize, double margin)
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = new Point(points[i].X * cellSize.Width + margin,
                    points[i].Y * cellSize.Height + margin);
            }
        }

        private void DoShift(Point[] points)
        {
            double left = new Point[] { points.FirstOrDefault(), points.LastOrDefault() }.Min(p => p.X);
            double top = new Point[] { points.FirstOrDefault(), points.LastOrDefault() }.Min(p => p.Y);

            for (int i = 0; i < points.Length; i++)
            {
                points[i].X = points[i].X - left;
                points[i].Y = points[i].Y - top;
            }


        }

        private Point SegmentMiddlePoint(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }
        #endregion

        private void ConnectorViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ItemHeight":
                case "ItemWidth":
                case "Left":
                case "Top":
                    SourceA = PointHelper.GetPointForConnector(this.SourceConnectorInfo);
                    if (this.SinkConnectorInfo is FullyCreatedConnectorInfo)
                    {
                        SourceB = PointHelper.GetPointForConnector((FullyCreatedConnectorInfo)this.SinkConnectorInfo);
                    }
                    break;

            }
        }

        private void Init(FullyCreatedConnectorInfo sourceConnectorInfo, ConnectorInfoBase sinkConnectorInfo)
        {
            this.Parent = sourceConnectorInfo.DataItem.Parent;
            this.SourceConnectorInfo = sourceConnectorInfo;
            this.SinkConnectorInfo = sinkConnectorInfo;
            PathFinder = new OrthogonalPathFinder();
            DeleteConnectionCommand = new SimpleCommand(DeleteConnection);
        }

        public SimpleCommand DeleteConnectionCommand { get; set; }
        private void DeleteConnection(object args)
        {
            if (this.Parent is IDiagramViewModel)
            {
                var diagramVM = this.Parent as IDiagramViewModel;
                diagramVM.RemoveItemCommand.Execute(this);
            }
        }

        protected override void ExecuteEditCommand(object param)
        {
            if (this.OutTextItem != null) return;
            AddText("");
        }

        public void AddText(string text)
        {
            if (this.Parent is IDiagramViewModel)
            {
                var diagramVM = this.Parent as IDiagramViewModel;

                TextDesignerItemViewModel textitem = new TextDesignerItemViewModel();
                textitem.ItemWidth = Double.NaN;
                textitem.ItemHeight = double.NaN;
                if (diagramVM.DiagramType == DiagramType.FlowChart)
                {
                    var mid = (int)(ConnectionPoints.Count / 2);
                    var p = SegmentMiddlePoint(ConnectionPoints[mid - 1], ConnectionPoints[mid]);
                    textitem.Left = this.Area.Left + p.X + 2;
                    textitem.Top = this.Area.Top + p.Y - 15;
                }
                else
                {
                    textitem.Left = this.Area.Left + this.Area.Width / 2 - 16;
                    textitem.Top = this.Area.Top + this.Area.Height / 2 - 5;
                }
                textitem.Watermark = null;
                textitem.ZIndex = diagramVM.Items.Count;
                textitem.ParentId = this.Id;
                textitem.ParentItem = this;
                textitem.ColorViewModel.FillColor = new ColorObject() { Color = Colors.White };
                textitem.Text = text;

                diagramVM.DirectAddItemCommand.Execute(textitem);

                this.OutTextItem = textitem;
            }
        }

        public void OutTextItemLocation(Rect oldArea, Rect newArea)
        {
            if (this.OutTextItem is TextDesignerItemViewModel text)
            {
                var oldpoint = new Point(oldArea.Left + oldArea.Width / 2, oldArea.Top + oldArea.Height / 2);
                var newpoint = new Point(newArea.Left + newArea.Width / 2, newArea.Top + newArea.Height / 2);

                text.Left = text.Left + newpoint.X - oldpoint.X;
                text.Top = text.Top + newpoint.Y - oldpoint.Y;
            }
        }

    }
}
