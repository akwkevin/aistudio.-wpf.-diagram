using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AIStudio.Wpf.ADiagram.Controls
{
    public class RectangleGridEventArgs
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public RectangleGridEventArgs(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

    public class RectangleGrid : Grid
    {
        public static readonly DependencyProperty RowNumProperty = DependencyProperty.Register("RowNum", typeof(int),
              typeof(RectangleGrid),
              new FrameworkPropertyMetadata(4));
        public int RowNum
        {
            get
            {
                return (int)GetValue(RowNumProperty);
            }
            set
            {
                SetValue(RowNumProperty, value);
            }
        }

        public static readonly DependencyProperty ColumnNumProperty = DependencyProperty.Register("ColumnNum", typeof(int),
            typeof(RectangleGrid),
            new FrameworkPropertyMetadata(4));
        public int ColumnNum
        {
            get
            {
                return (int)GetValue(ColumnNumProperty);
            }
            set
            {
                SetValue(ColumnNumProperty, value);
            }
        }

        public static readonly DependencyProperty SelectRowProperty = DependencyProperty.Register("SelectRow", typeof(int),
              typeof(RectangleGrid),
              new FrameworkPropertyMetadata(0));

        public int SelectRow
        {
            get
            {
                return (int)GetValue(SelectRowProperty);
            }
            set
            {
                SetValue(SelectRowProperty, value);
            }
        }

        public static readonly DependencyProperty SelectColumnProperty = DependencyProperty.Register("SelectColumn", typeof(int),
              typeof(RectangleGrid),
              new FrameworkPropertyMetadata(0));
        public int SelectColumn
        {
            get
            {
                return (int)GetValue(SelectColumnProperty);
            }
            set
            {
                SetValue(SelectColumnProperty, value);
            }
        }

        public static readonly DependencyProperty SelectProperty = DependencyProperty.Register("Select", typeof(RectangleGridEventArgs),
             typeof(RectangleGrid),
             new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSelectChanged)));
        public RectangleGridEventArgs Select
        {
            get
            {
                return (RectangleGridEventArgs)GetValue(SelectProperty);
            }
            set
            {
                SetValue(SelectProperty, value);
            }
        }

        #region MouseOverBrush 标题栏背景色

        public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.Register(
            "MouseOverBrush", typeof(Brush), typeof(RectangleGrid), new PropertyMetadata(null));

        public Brush MouseOverBrush
        {
            get { return (Brush)GetValue(MouseOverBrushProperty); }
            set { SetValue(MouseOverBrushProperty, value); }
        }

        #endregion

        private Rectangle[,] rectangles;
        public RectangleGrid()
        {
            this.Loaded += RectangleGrid_Loaded;
        }

        void RectangleGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= RectangleGrid_Loaded;
            for (int i = 0; i < RowNum; i++)
            {
                RowDefinition row = new RowDefinition() { Height = new GridLength(2, GridUnitType.Auto) };
                this.RowDefinitions.Add(row);
            }
            for (int j = 0; j < ColumnNum; j++)
            {
                ColumnDefinition col = new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Auto) };
                this.ColumnDefinitions.Add(col);
            }

            rectangles = new Rectangle[RowNum, ColumnNum];
            for (int i = 0; i < rectangles.GetLength(0); i++)
            {
                for (int j = 0; j < rectangles.GetLength(1); j++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.IsHitTestVisible = true;
                    rectangle.SnapsToDevicePixels = true;
                    rectangle.Stroke = new SolidColorBrush(Colors.Black);
                    rectangle.Width = 24;
                    rectangle.Height = 24;
                    rectangle.Margin = new Thickness(2);
                    rectangle.MouseEnter += rectangle_MouseEnter;
                    rectangle.MouseLeave += rectangle_MouseLeave;
                    rectangle.MouseLeftButtonDown += rectangle_MouseLeftButtonDown;
                    rectangle.Fill = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
                    Grid.SetRow(rectangle, i);
                    Grid.SetColumn(rectangle, j);
                    this.Children.Add(rectangle);

                    rectangles[i, j] = rectangle;


                }
            }
        }

        void rectangle_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Brush myBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0x30, 0x50, 0x68));
            Brush myBrush2 = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0xff, 0xff, 0xff));

            if (MouseOverBrush != null)
            {
                myBrush = MouseOverBrush;
            }

            #region
            Rectangle rect = sender as Rectangle;
            var rects = rectangles.Cast<Rectangle>().ToArray();
            int index = Array.IndexOf(rects, rect);

            int x = index / rectangles.GetLength(1);
            int y = index % rectangles.GetLength(1);
            FillRectangle(x, y);
            #endregion
        }

        private void FillRectangle(int row, int column)
        {
            Brush myBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x30, 0x50, 0x68));
            Brush myBrush2 = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));

            if (MouseOverBrush != null)
            {
                myBrush = MouseOverBrush;
            }

            for (int i = 0; i < rectangles.GetLength(0); i++)
            {
                for (int j = 0; j < rectangles.GetLength(1); j++)
                {
                    if (i <= row && j <= column)
                    {
                        rectangles[i, j].Fill = myBrush;
                    }
                    else
                    {
                        rectangles[i, j].Fill = myBrush2;
                    }
                }
            }
        }

        void rectangle_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        void rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            var rects = rectangles.Cast<Rectangle>().ToArray();
            int index = Array.IndexOf(rects, rect);

            int x = index / rectangles.GetLength(1);
            int y = index % rectangles.GetLength(1);

            this.SelectRow = x + 1;
            this.SelectColumn = y + 1;
            this.Select = new RectangleGridEventArgs(this.SelectRow, this.SelectColumn);
        }

        private static void OnSelectRowChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RectangleGrid grid = (RectangleGrid)sender;

            int oldRow = (int)e.OldValue;
            int newRow = (int)e.NewValue;
            if (oldRow == 0 || newRow == 0)
            {
                grid.FillRectangle((int)e.NewValue, grid.SelectColumn);
            }

            //避免两次触发
            var oldArgs = new RectangleGridEventArgs(oldRow, grid.SelectColumn);
            var newArgs = new RectangleGridEventArgs(newRow, grid.SelectColumn);
            grid.OnSelectColumnChanged(oldArgs, newArgs);
        }

        private static void OnSelectColumnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RectangleGrid grid = (RectangleGrid)sender;

            int oldColumn = (int)e.OldValue;
            int newColumn = (int)e.NewValue;

            grid.FillRectangle(grid.SelectRow, (int)e.NewValue);

            var oldArgs = new RectangleGridEventArgs(grid.SelectRow, oldColumn);
            var newArgs = new RectangleGridEventArgs(grid.SelectRow, newColumn);
            grid.OnSelectColumnChanged(oldArgs, newArgs);
        }

        private static void OnSelectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RectangleGrid grid = (RectangleGrid)sender;

            RectangleGridEventArgs oldArgs = e.OldValue as RectangleGridEventArgs;
            RectangleGridEventArgs newArgs = e.NewValue as RectangleGridEventArgs;

            //grid.FillRectangle(newArgs.Row, newArgs.Column);

            grid.OnSelectColumnChanged(oldArgs, newArgs);
        }

        public static readonly RoutedEvent SelectRowColumnChangedEvent =
          EventManager.RegisterRoutedEvent("SelectRowColumnChanged", RoutingStrategy.Bubble,
              typeof(RoutedPropertyChangedEventHandler<RectangleGridEventArgs>), typeof(RectangleGrid));

        public event RoutedPropertyChangedEventHandler<RectangleGridEventArgs> SelectRowColumnChanged
        {
            add { AddHandler(SelectRowColumnChangedEvent, value); }
            remove { RemoveHandler(SelectRowColumnChangedEvent, value); }
        }

        private void OnSelectColumnChanged(RectangleGridEventArgs oldargs, RectangleGridEventArgs newargs)
        {
            RoutedPropertyChangedEventArgs<RectangleGridEventArgs> args = new RoutedPropertyChangedEventArgs<RectangleGridEventArgs>(oldargs, newargs);
            args.RoutedEvent = RectangleGrid.SelectRowColumnChangedEvent;
            RaiseEvent(args);

            switch (Command)
            {
                case null:
                    return;
                case RoutedCommand command:
                    command.Execute(CommandParameter, CommandTarget);
                    break;
                default:
                    Command.Execute(CommandParameter);
                    break;
            }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
           "Command", typeof(ICommand), typeof(RectangleGrid), new PropertyMetadata(default(ICommand), OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (RectangleGrid)d;
            if (e.OldValue is ICommand oldCommand)
            {
                oldCommand.CanExecuteChanged -= ctl.CanExecuteChanged;
            }
            if (e.NewValue is ICommand newCommand)
            {
                newCommand.CanExecuteChanged += ctl.CanExecuteChanged;
            }
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(RectangleGrid), new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
            "CommandTarget", typeof(IInputElement), typeof(RectangleGrid), new PropertyMetadata(default(IInputElement)));

        public IInputElement CommandTarget
        {
            get => (IInputElement)GetValue(CommandTargetProperty);
            set => SetValue(CommandTargetProperty, value);
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            if (Command == null) return;

            IsEnabled = Command is RoutedCommand command
                ? command.CanExecute(CommandParameter, CommandTarget)
                : Command.CanExecute(CommandParameter);
        }
    }
}
