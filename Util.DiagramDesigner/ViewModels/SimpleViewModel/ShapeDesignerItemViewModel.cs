using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Util.DiagramDesigner
{
    public class ShapeDesignerItemViewModel : DesignerItemViewModelBase
    {
        public List<PointDesignerItemViewModel> PointDesignerItemViewModels { get; set; }

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
        public DrawMode DrawMode { get; set; }

        private bool _showConnectors = false;
        public new bool ShowConnectors
        {
            get
            {
                return _showConnectors;
            }
            set
            {
                if (SetProperty(ref _showConnectors, value))
                {
                    foreach (var connector in PointDesignerItemViewModels)
                    {
                        connector.ShowConnectors = value;
                    }
                }
            }
        }

        public SimpleCommand MenuItemCommand { get; private set; }

        public ShapeDesignerItemViewModel(DrawMode drawMode, List<Point> points) : base()
        {
            DrawMode = drawMode;
            ConnectionPoints = points;

            ItemWidth = ConnectionPoints.Max(p => p.X) - ConnectionPoints.Min(p => p.X);
            ItemHeight = ConnectionPoints.Max(p => p.Y) - ConnectionPoints.Min(p => p.Y);
            Left = ConnectionPoints.Min(p => p.X);
            Top = ConnectionPoints.Min(p => p.Y);

            PointDesignerItemViewModels = new List<PointDesignerItemViewModel>();
            ConnectionPoints.ForEach((Action<Point>)(p =>
            {
                var item = new PointDesignerItemViewModel(p);
                PointDesignerItemViewModels.Add((PointDesignerItemViewModel)item);
            }));

            PointDesignerItemViewModels.ForEach(p => p.PropertyChanged += PointDesignerItemViewModel_PropertyChanged);
        }

        private void PointDesignerItemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Left) || e.PropertyName == nameof(Top))
            {
                UpdatePoints();
            }
        }

        private void UpdatePoints()
        {
            ConnectionPoints = PointDesignerItemViewModels.Select(p => p.CurrentLocation).ToList();
            ItemWidth = ConnectionPoints.Max(p => p.X) - ConnectionPoints.Min(p => p.X);
            ItemHeight = ConnectionPoints.Max(p => p.Y) - ConnectionPoints.Min(p => p.Y);
            Left = ConnectionPoints.Min(p => p.X);
            Top = ConnectionPoints.Min(p => p.Y);
        }

        protected override void Init()
        {
            MenuItemCommand = new SimpleCommand(ExecuteMenuItemCommand);
            base.Init();

            this.ClearConnectors();

            BuildMenuOptions();
        }

        private void ExecuteMenuItemCommand(object obj)
        {
            ShowConnectors = (obj as CinchMenuItem).IsChecked;
        }

        private void BuildMenuOptions()
        {
            menuOptions = new ObservableCollection<CinchMenuItem>();
            CinchMenuItem menuItem = new CinchMenuItem();
            menuItem.Text = "显示点";
            menuItem.IsCheckable = true;
            menuItem.Command = MenuItemCommand;
            menuItem.CommandParameter = menuItem;
            menuOptions.Add(menuItem);
        }
    }
}
