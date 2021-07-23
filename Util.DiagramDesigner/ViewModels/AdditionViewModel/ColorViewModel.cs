using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    [Serializable]
    public class ColorViewModel : BindableBase, IColorViewModel
    {
        #region 界面使用
        public static Color[] FillColors { get; } = new Color[] { Colors.Red, Colors.Green, Colors.Blue, Colors.White, Colors.Black, Colors.Purple };
        public static Color[] LineColors { get; } = new Color[] { Colors.Red, Colors.Green, Colors.Blue, Colors.White, Colors.Black, Colors.Purple };
        #endregion

        public ColorViewModel()
        {
            LineColor = new ColorObject() { Color = Colors.Gray };
            FillColor = new ColorObject() { Color = Colors.Transparent };
        }
        private IColorObject _lineColor;
        public IColorObject LineColor
        {
            get
            {
                return _lineColor;
            }
            set
            {
                if (_lineColor != value)
                {
                    if (_lineColor != null && _lineColor is ColorObject _lineColor1)
                    {
                        _lineColor1.PropertyChanged -= ColorViewModel_PropertyChanged;
                    }
                    SetProperty(ref _lineColor, value);
                    if (_lineColor != null && _lineColor is ColorObject _lineColor2)
                    {
                        _lineColor2.PropertyChanged += ColorViewModel_PropertyChanged;
                    }
                }
                else
                {
                    RaisePropertyChanged(nameof(LineColor));
                }
            }
        }

        private IColorObject _fillcolor;
        public IColorObject FillColor
        {
            get
            {
                return _fillcolor;
            }
            set
            {
                if (_fillcolor != value)
                {
                    if (_fillcolor != null && _fillcolor is ColorObject colorObject1)
                    {
                        colorObject1.PropertyChanged -= ColorViewModel_PropertyChanged;
                    }
                    SetProperty(ref _fillcolor, value);
                    if (_fillcolor != null && _fillcolor is ColorObject colorObject2)
                    {
                        colorObject2.PropertyChanged += ColorViewModel_PropertyChanged;
                    }
                }
                else
                {
                    RaisePropertyChanged(nameof(FillColor));
                }
            }
        }

        private void ColorViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender == LineColor)
            {
                RaisePropertyChanged(nameof(LineColor));
            }
            else if (sender == FillColor)
            {
                RaisePropertyChanged(nameof(FillColor));
            }
        }

        private Color _shadowColor = Colors.Transparent;
        public Color ShadowColor
        {
            get
            {
                return _shadowColor;
            }
            set
            {
                if (!SetProperty(ref _shadowColor, value))
                {
                    RaisePropertyChanged(nameof(ShadowColor));
                }
            }
        }

        private double _lineWidth = 1;
        public double LineWidth
        {
            get
            {
                return _lineWidth;
            }
            set
            {
                if (!SetProperty(ref _lineWidth, value))
                {
                    RaisePropertyChanged(nameof(LineWidth));
                }
            }
        }

        private ArrowPathStyle _leftArrowPathStyle = ArrowPathStyle.None;
        public ArrowPathStyle LeftArrowPathStyle
        {
            get
            {
                return _leftArrowPathStyle;
            }
            set
            {
                if (!SetProperty(ref _leftArrowPathStyle, value))
                {
                    RaisePropertyChanged(nameof(LeftArrowPathStyle));
                }
            }
        }

        private ArrowPathStyle _rightArrowPathStyle = ArrowPathStyle.Arrow1;
        public ArrowPathStyle RightArrowPathStyle
        {
            get
            {
                return _rightArrowPathStyle;
            }
            set
            {
                if (!SetProperty(ref _rightArrowPathStyle, value))
                {
                    RaisePropertyChanged(nameof(RightArrowPathStyle));
                }
            }
        }

        private ArrowSizeStyle _leftArrowSizeStyle = ArrowSizeStyle.Middle;
        public ArrowSizeStyle LeftArrowSizeStyle
        {
            get
            {
                return _leftArrowSizeStyle;
            }
            set
            {
                if (!SetProperty(ref _leftArrowSizeStyle, value))
                {
                    RaisePropertyChanged(nameof(LeftArrowSizeStyle));
                }
            }
        }

        private ArrowSizeStyle _rightArrowSizeStyle = ArrowSizeStyle.Middle;
        public ArrowSizeStyle RightArrowSizeStyle
        {
            get
            {
                return _rightArrowSizeStyle;
            }
            set
            {
                if (!SetProperty(ref _rightArrowSizeStyle, value))
                {
                    RaisePropertyChanged(nameof(RightArrowSizeStyle));
                }
            }
        }

        private LineDashStyle _lineDashStyle = LineDashStyle.None;
        public LineDashStyle LineDashStyle
        {
            get
            {
                return _lineDashStyle;
            }
            set
            {
                if (!SetProperty(ref _lineDashStyle, value))
                {
                    RaisePropertyChanged(nameof(LineDashStyle));
                }
            }
        }
    }


    [Serializable]
    public class ColorObject : BindableBase, IColorObject
    {
        public ColorObject()
        {

        }

        private void GradientStop_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var old in e.OldItems.OfType<GradientStop>())
                {
                    old.PropertyChanged -= GradientStop_PropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var old in e.NewItems.OfType<GradientStop>())
                {
                    old.PropertyChanged += GradientStop_PropertyChanged;
                }
            }
        }

        private void GradientStop_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(GradientStop));
        }


        public void BrushTypeChanged()
        {
            if (BrushType == BrushType.LinearGradientBrush || BrushType == BrushType.RadialGradientBrush)
            {
                if (GradientStop == null)
                {
                    GradientStop = new ObservableCollection<GradientStop>();
                    GradientStop.Add(new GradientStop(Color, 0));
                    GradientStop.Add(new GradientStop(Colors.Gray, 1));
                    SelectedGradientStop = GradientStop.FirstOrDefault();
                    RaisePropertyChanged(nameof(GradientStop));
                }
            }
        }

        private BrushType _brushType = BrushType.SolidColorBrush;
        public BrushType BrushType
        {
            get
            {
                return _brushType;
            }
            set
            {
                if (SetProperty(ref _brushType, value))
                {
                    BrushTypeChanged();
                }
            }
        }

        private Color _color = new Color();
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                SetProperty(ref _color, value);

            }
        }

        private ObservableCollection<GradientStop> _gradientStop;
        public ObservableCollection<GradientStop> GradientStop
        {
            get
            {
                return _gradientStop;
            }
            set
            {
                if (_gradientStop != value)
                {
                    if (_gradientStop != null)
                    {
                        _gradientStop.CollectionChanged -= GradientStop_CollectionChanged;
                    }
                    SetProperty(ref _gradientStop, value);
                    if (_gradientStop != null)
                    {
                        _gradientStop.CollectionChanged += GradientStop_CollectionChanged;
                    }
                }
            }
        }

        private GradientStop _selectedGradientStop;
        public GradientStop SelectedGradientStop
        {
            get
            {
                return _selectedGradientStop;
            }
            set
            {
                SetProperty(ref _selectedGradientStop, value);
            }
        }
        private Point _startPoint;
        public Point StartPoint
        {
            get
            {
                return _startPoint;
            }
            set
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
            set
            {
                SetProperty(ref _endPoint, value);
            }
        }

        private double _opacity = 1;
        public double Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                SetProperty(ref _opacity, value);
            }
        }

        private double _light;
        public double Light
        {
            get
            {
                return _light;
            }
            set
            {
                SetProperty(ref _light, value);
            }
        }

        private string _image;
        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                SetProperty(ref _image, value);
            }
        }

        private LinearOrientation _linearOrientation;
        public LinearOrientation LinearOrientation
        {
            get
            {
                return _linearOrientation;
            }
            set
            {
                SetProperty(ref _linearOrientation, value);
            }
        }

        private RadialOrientation _radialOrientation;
        public RadialOrientation RadialOrientation
        {
            get
            {
                return _radialOrientation;
            }
            set
            {
                SetProperty(ref _radialOrientation, value);
            }
        }

        private int _angle;
        public int Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                SetProperty(ref _angle, value);
            }
        }

        private int _subType;

        public int SubType
        {
            get
            {
                return _subType;
            }
            set
            {
                SetProperty(ref _subType, value);
            }
        }

        public ICommand AddGradientStopCommand
        {
            get
            {
                return new SimpleCommand(para =>
                {
                    var offset = GradientStop.Skip(GradientStop.Count - 2).Select(p => p.Offset).Average();
                    GradientStop.Add(new GradientStop(Colors.Gray, offset));
                });
            }
        }
        public ICommand DeleteGradientStopCommand
        {
            get
            {
                return new SimpleCommand(para =>
                {
                    if (SelectedGradientStop != null && GradientStop != null && GradientStop.Count > 2)
                    {
                        GradientStop.Remove(SelectedGradientStop);
                    }
                });
            }
        }
    }

    public interface IColorObject
    {
        BrushType BrushType { get; set; }
        Color Color { get; set; }
        ObservableCollection<GradientStop> GradientStop { get; set; }
        Point StartPoint { get; set; }
        Point EndPoint { get; set; }
        double Opacity { get; set; }
        LinearOrientation LinearOrientation { get; set; }
        RadialOrientation RadialOrientation { get; set; }
        int Angle { get; set; }
        string Image { get; set; }
        int SubType { get; set; }
    }

    public class GradientStop : BindableBase
    {
        public GradientStop()
        {

        }
        public GradientStop(Color color, double offset)
        {
            Color = color;
            Offset = offset;
        }
        private Color _color = new Color();
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                SetProperty(ref _color, value);
            }
        }

        private double _offset;
        public double Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                SetProperty(ref _offset, value);
            }
        }



    }
}
