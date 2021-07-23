using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Util.DiagramDesigner
{
    [Serializable]
    [XmlInclude(typeof(DesignerItemBase))]
    public class DesignerItemBase : SelectableDesignerItemBase
    {
        public DesignerItemBase()
        {

        }

        public DesignerItemBase(DesignerItemViewModelBase viewmodel, string reserve = null) : base(viewmodel)
        {
            this.Left = viewmodel.Left;
            this.Top = viewmodel.Top;
            this.Angle = viewmodel.Angle;
            this.ScaleX = viewmodel.ScaleX;
            this.ScaleY = viewmodel.ScaleY;
            this.ItemWidth = viewmodel.ItemWidth;
            this.ItemHeight = viewmodel.ItemHeight;
            this.Icon = viewmodel.Icon;
            this.ItemTypeName = viewmodel.GetType().FullName;
            this.Margin = viewmodel.Margin;
            this.Reserve = reserve;
        }

        [XmlAttribute]
        public double Left { get; set; }

        [XmlAttribute]
        public double Top { get; set; }

        [XmlAttribute]
        public double Angle { get; set; }

        [XmlAttribute]
        public double ScaleX { get; set; }

        [XmlAttribute]
        public double ScaleY { get; set; }

        [XmlAttribute]
        public double Margin { get; set; }

        [XmlAttribute]
        public double ItemWidth { get; set; }

        [XmlAttribute]
        public double ItemHeight { get; set; }

        [XmlAttribute]
        public string Icon { get; set; }

        [XmlAttribute]
        public string Reserve { get; set; }

        [XmlAttribute]
        public string ItemTypeName { get; set; }

    }


}
