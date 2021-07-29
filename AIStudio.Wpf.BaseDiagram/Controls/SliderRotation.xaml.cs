using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIStudio.Wpf.BaseDiagram.Controls
{
    /// <summary>
    /// SliderRotation.xaml 的交互逻辑
    /// </summary>
    public partial class SliderRotation : UserControl
    {
        #region 私有属性
        private Point cen;   //中心点
        private Point first;  //初始状态位置
        private Point second;  //
        private bool flag = false;
        #endregion
        #region 公有属性
        /// <summary>
        /// 获取或设置Value数值
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
                "Value",
                typeof(double),
                typeof(SliderRotation),
                new FrameworkPropertyMetadata((double)0, new PropertyChangedCallback(ValuePropertyChangedCallback)));

        #endregion
        public SliderRotation()
        {
            InitializeComponent();
        }
        private static void ValuePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if (sender != null && sender is SliderRotation)
            {
                SliderRotation sliderRotation = sender as SliderRotation;
                RoutedPropertyChangedEventArgs<object> valueArg =
                    new RoutedPropertyChangedEventArgs<object>(arg.OldValue, arg.NewValue, ValueChangedEvent);
                sliderRotation.RaiseEvent(valueArg);
            }
        }


        #region 事件路由
        public event RoutedPropertyChangedEventHandler<object> ValueChanged
        {
            add
            {
                this.AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(ValueChangedEvent, value);
            }
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<object>),
            typeof(SliderRotation));
        #endregion

        private double GetAngle(Point point)     //获取点到中心的角度      构造平面直角坐标系 计算点在该坐标系与y轴（正方向）的夹角
        {
            const double M_PI = 3.1415926535897;
            if (point.X >= 0)
            {
                double hypotenuse = Math.Sqrt(point.X * point.X + point.Y * point.Y);
                return Math.Acos(point.Y / hypotenuse) * 180 / M_PI;
            }
            else
            {
                double hypotenuse = Math.Sqrt(point.X * point.X + point.Y * point.Y);
                return 360 - Math.Acos(point.Y / hypotenuse) * 180 / M_PI;
            }
        }
        private void ellipseBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            flag = true;
            cen = new Point(ellipseBack.Width / 2, ellipseBack.Height / 2);
            first = new Point(e.GetPosition(canvas).X - cen.X, cen.Y - e.GetPosition(canvas).Y);
        }

        private void ellipseBack_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            flag = false;
        }
        private void ellipseBack_MouseLeave(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            flag = false;
        }
        private void ellipseBack_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;        //停止路由事件向上传递
            if (flag)
            {
                second = new Point(e.GetPosition(canvas).X - cen.X, cen.Y - e.GetPosition(canvas).Y);    //确定鼠标移动的点坐标（相对中心点的位置）

                if (second == new Point(0, 0))
                {
                    return;
                }
                double anglePointToPoint = GetAngle(second) - GetAngle(first);        //得到鼠标移动之前与鼠标移动之后之间的夹角
                first = second;
                if (Math.Abs(anglePointToPoint) > 90)                               //夹角如果大于90度忽略(大于90度的夹角有可能是计算错误得出来的)
                {
                    anglePointToPoint = 0;
                }

                var angle = Value + anglePointToPoint;   
                if (angle < 0)
                {
                    angle += 360;
                }
                angle = angle % 360;
                Value = Math.Round(angle, 0);                                  //计算出旋转角度
            }
        }
    }
}