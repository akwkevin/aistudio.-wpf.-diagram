using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Util.DiagramDesigner
{

    public abstract class SelectableDesignerItemBase
    {
        public SelectableDesignerItemBase()
        {
            ColorItem = new ColorItem() { LineColor = new ColorObjectItem(), FillColor = new ColorObjectItem() };
            FontItem = new FontItem();
        }

        //public SelectableDesignerItemBase(Guid id, int zIndex, bool isGroup, Guid parentId, IColorViewModel colorViewModel, IFontViewModel fontViewModel)
        //{
        //    this.Id = id;
        //    this.ZIndex = zIndex;
        //    this.IsGroup = isGroup;
        //    this.ParentId = parentId;


        //    ColorItem = CopyHelper.Mapper<ColorItem>(colorViewModel);
        //    FontItem = CopyHelper.Mapper<FontItem, IFontViewModel>(fontViewModel);
        //}

        public SelectableDesignerItemBase(SelectableDesignerItemViewModelBase viewmodel)
        {
            this.Id = viewmodel.Id;
            this.ZIndex = viewmodel.ZIndex;
            this.IsGroup = viewmodel.IsGroup;
            this.ParentId = viewmodel.ParentId;
            this.Text = viewmodel.Text;

            ColorItem = CopyHelper.Mapper<ColorItem>(viewmodel.ColorViewModel);
            FontItem = CopyHelper.Mapper<FontItem, IFontViewModel>(viewmodel.FontViewModel);
        }

        [XmlAttribute]
        public Guid Id { get; set; }

        [XmlAttribute]
        public int ZIndex { get; set; }

        [XmlAttribute]
        public bool IsGroup { get; set; }

        [XmlAttribute]
        public Guid ParentId { get; set; }

        [XmlAttribute]
        public string Text { get; set; }

        [XmlElement]
        public ColorItem ColorItem { get; set; }

        [XmlElement]
        public FontItem FontItem { get; set; }

    }

    public class ColorItem : IColorViewModel
    {        
        [XmlIgnore]
        public IColorObject LineColor { get; set; }

        [JsonIgnore]
        [XmlElement("LineColor")]
        public ColorObjectItem XmlLineColor
        {
            get
            {
                return LineColor as ColorObjectItem;
            }
            set
            {
                LineColor = value;
            }
        }

        [XmlIgnore]
        public IColorObject FillColor { get; set; }

        [JsonIgnore]
        [XmlElement("FillColor")]
        public ColorObjectItem XmlFillColor
        {
            get
            {
                return FillColor as ColorObjectItem;
            }
            set
            {
                FillColor = value;
            }
        }


        [XmlIgnore]
        public Color ShadowColor { get; set; }

        [JsonIgnore]
        [XmlElement("ShadowColor")]
        public string XmlShadowColor
        {
            get
            {
                return SerializeHelper.SerializeColor(ShadowColor);
            }
            set
            {
                ShadowColor = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlAttribute]
        public double LineWidth { get; set; }

        [XmlAttribute]
        public ArrowPathStyle LeftArrowPathStyle { get; set; }

        [XmlAttribute]
        public ArrowPathStyle RightArrowPathStyle { get; set; }

        [XmlAttribute]
        public ArrowSizeStyle LeftArrowSizeStyle { get; set; }

        [XmlAttribute]
        public ArrowSizeStyle RightArrowSizeStyle { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [XmlInclude(typeof(FontItem))]
    public class FontItem : IFontViewModel
    {
        [XmlIgnore]
        public FontWeight FontWeight { get; set; }
        [XmlIgnore]
        public FontStyle FontStyle { get; set; }
        [XmlIgnore]
        public FontStretch FontStretch { get; set; }
        [XmlAttribute]
        public bool Underline { get; set; }
        [XmlAttribute]
        public bool Strikethrough { get; set; }
        [XmlAttribute]
        public bool OverLine { get; set; }

        [XmlIgnore]
        public Color FontColor { get; set; }

        [JsonIgnore]
        [XmlElement("FontColor")]
        public string XmlFontColor
        {
            get
            {
                return SerializeHelper.SerializeColor(FontColor);
            }
            set
            {
                FontColor = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlIgnore]
        public string FontFamily { get; set; }

        [XmlIgnore]
        public double FontSize { get; set; }


        [XmlIgnore]
        public System.Drawing.Font FontObject
        {
            get
            {
                var xmlFontStyle = System.Drawing.FontStyle.Regular;
                if (FontStyle == FontStyles.Italic)
                {
                    xmlFontStyle |= System.Drawing.FontStyle.Italic;
                }
                if (FontWeight == FontWeights.Bold)
                {
                    xmlFontStyle |= System.Drawing.FontStyle.Bold;
                }
                return new System.Drawing.Font(FontFamily, (float)FontSize, xmlFontStyle);
            }

            set
            {
                FontFamily = value.FontFamily.Name;
                FontSize = value.Size;
                var xmlFontStyle = value.Style;
                if ((xmlFontStyle & System.Drawing.FontStyle.Italic) == System.Drawing.FontStyle.Italic)
                {
                    FontStyle = FontStyles.Italic;
                }
                else
                {
                    FontStyle = FontStyles.Normal;
                }
                if ((xmlFontStyle & System.Drawing.FontStyle.Bold) == System.Drawing.FontStyle.Bold)
                {
                    FontWeight = FontWeights.Bold;
                }
                else
                {
                    FontWeight = FontWeights.Regular;
                }
            }
        }

        [JsonIgnore]
        [XmlElement("FontObject")]
        public XmlFont XmlFontObject
        {
            get
            {
                return SerializeHelper.SerializeFont(FontObject);
            }

            set
            {
                FontObject = SerializeHelper.DeserializeFont(value);
            }
        }

        [XmlIgnore]
        public Color TextEffectColor { get; set; }

        [JsonIgnore]
        [XmlElement("TextEffectColor")]
        public string XmlTextEffectColor
        {
            get
            {
                return SerializeHelper.SerializeColor(TextEffectColor);
            }
            set
            {
                TextEffectColor = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlIgnore]
        public Color HighlightColor { get; set; }

        [JsonIgnore]
        [XmlElement("HighlightColor")]
        public string XmlHighlightColor
        {
            get
            {
                return SerializeHelper.SerializeColor(HighlightColor);
            }
            set
            {
                HighlightColor = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlAttribute]
        public FontCase FontCase { get; set; }
        [XmlAttribute]
        public HorizontalAlignment HorizontalAlignment { get; set; }
        [XmlAttribute]
        public VerticalAlignment VerticalAlignment { get; set; }
        [XmlAttribute]
        public double LineHeight { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class SerializeHelper
    {
        public static string SerializeColor(Color color)
        {
            return string.Format("{0}:{1}:{2}:{3}", color.A, color.R, color.G, color.B);
        }

        public static Color DeserializeColor(string color)
        {
            byte a, r, g, b;
            string[] pieces = color.Split(new char[] { ':' });
            a = byte.Parse(pieces[0]);
            r = byte.Parse(pieces[1]);
            g = byte.Parse(pieces[2]);
            b = byte.Parse(pieces[3]);
            return Color.FromArgb(a, r, g, b);
        }

        public static GradientStop DeserializeGradientStop(string str)
        {
            var strList = str.Split('-');
            return new GradientStop(DeserializeColor(strList[0]), double.Parse(strList[1]));
        }

        public static string SerializeColorList(IEnumerable<Color> colors)
        {
            return string.Join("-", colors.Select(color => string.Format("{0}:{1}:{2}:{3}", color.A, color.R, color.G, color.B)));
        }

        public static List<Color> DeserializeColorList(string colorstring)
        {
            List<Color> colorlist = new List<Color>();
            var colors = colorstring.Split('-');
            foreach (var color in colors)
            {
                byte a, r, g, b;
                string[] pieces = color.Split(new char[] { ':' });
                a = byte.Parse(pieces[0]);
                r = byte.Parse(pieces[1]);
                g = byte.Parse(pieces[2]);
                b = byte.Parse(pieces[3]);
                colorlist.Add(Color.FromArgb(a, r, g, b));
            }
            return colorlist;
        }

        public static XmlFont SerializeFont(System.Drawing.Font font)
        {
            return new XmlFont(font);
        }

        public static System.Drawing.Font DeserializeFont(XmlFont font)
        {
            return font.ToFont();
        }

        public static string SerializePoint(Point point)
        {
            return string.Format("{0},{1}", point.X, point.Y);
        }

        public static Point DeserializePoint(string point)
        {
            string[] pieces = point.Split(new char[] { ',' });
            return new Point(double.Parse(pieces[0]), double.Parse(pieces[1]));
        }

        public static string SerializeSize(Size size)
        {
            return string.Format("{0},{1}", size.Width, size.Height);
        }

        public static Size DeserializeSize(string size)
        {
            string[] pieces = size.Split(new char[] { ',' });
            return new Size(double.Parse(pieces[0]), double.Parse(pieces[1]));
        }
    }


    public struct XmlFont
    {
        public string FontFamily;
        public System.Drawing.GraphicsUnit GraphicsUnit;
        public float Size;
        public System.Drawing.FontStyle Style;

        public XmlFont(System.Drawing.Font f)
        {
            FontFamily = f.FontFamily.Name;
            GraphicsUnit = f.Unit;
            Size = f.Size;
            Style = f.Style;
        }

        public System.Drawing.Font ToFont()
        {
            return new System.Drawing.Font(FontFamily, Size, Style, GraphicsUnit);
        }

    }

    public class ColorObjectItem : IColorObject
    {

        [XmlAttribute]
        public BrushType BrushType { get; set; }

        [XmlIgnore]
        public Color Color { get; set; }

        [JsonIgnore]
        [XmlElement("FillColor")]
        public string XmlFillColor
        {
            get
            {
                return SerializeHelper.SerializeColor(Color);
            }
            set
            {
                Color = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlIgnore]
        public ObservableCollection<GradientStop> GradientStop { get; set; }

        [JsonIgnore]
        [XmlArray("GradientStop")]
        public List<string> XmlGradientStop
        {
            get
            {
                return GradientStop?.Select(p => SerializeHelper.SerializeColor(p.Color) + "-" + p.Offset).ToList();
            }
            set
            {
                GradientStop = new ObservableCollection<GradientStop>(value?.Select(p => SerializeHelper.DeserializeGradientStop(p)));
            }
        }



        [XmlIgnore]
        public IEnumerable<double> Offset { get; set; }

        [JsonIgnore]
        [XmlArray("Offset")]
        public List<double> XmlOffset
        {
            get
            {
                return Offset?.ToList();
            }
            set
            {
                Offset = value;
            }
        }

        [XmlAttribute]
        public string Image { get; set; }

        [XmlAttribute]
        public int SubType { get; set; }

        [XmlIgnore]
        public Point StartPoint { get; set; }

        [JsonIgnore]
        [XmlAttribute("StartPoint")]
        public string XmlStartPoint
        {
            get
            {
                return SerializeHelper.SerializePoint(StartPoint);
            }
            set
            {
                StartPoint = SerializeHelper.DeserializePoint(value);
            }
        }

        [XmlIgnore]
        public Point EndPoint { get; set; }

        [JsonIgnore]
        [XmlAttribute("EndPoint")]
        public string XmlEndPoint
        {
            get
            {
                return SerializeHelper.SerializePoint(EndPoint);
            }
            set
            {
                EndPoint = SerializeHelper.DeserializePoint(value);
            }
        }

        [XmlAttribute]
        public double Opacity { get; set; }
        [XmlAttribute]
        public LinearOrientation LinearOrientation { get; set; }
        [XmlAttribute]
        public RadialOrientation RadialOrientation { get; set; }
        [XmlAttribute]
        public int Angle { get; set; }
    }

}
