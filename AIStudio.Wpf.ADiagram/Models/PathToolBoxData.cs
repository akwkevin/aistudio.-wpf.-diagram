using Util.DiagramDesigner.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Util.DiagramDesigner;
using AIStudio.Wpf.ADiagram.Demos.Flowchart;
using AIStudio.Wpf.ADiagram.Helpers;

namespace AIStudio.Wpf.ADiagram.Models
{
    public class PathToolBoxData : ToolBoxData
    {
        public PathToolBoxData(string icon, Type type, double width = 32, double height = 32) : base(null, icon, type, width, height)
        {
            ColorViewModel.FillColor.Color = Colors.Orange;
        }

    }

    public class TextToolBoxData : ToolBoxData
    {
        public TextToolBoxData(string text, Type type, double width = 32, double height = 32) : base(text, null, type, width, height)
        {
            ColorViewModel.FillColor.Color = Colors.Orange;
        }

    }

    public class EllipseTextToolBoxData : ToolBoxData
    {
        public EllipseTextToolBoxData(string text, Type type, double width = 32, double height = 32) : base(text, null, type, width, height)
        {
            ColorViewModel.FillColor.Color = Colors.Orange;
        }

    }

    public class ImageToolBoxData : ToolBoxData
    {
        public ImageToolBoxData(string icon, Type type, double width = 32, double height = 32) : base(null, icon, type, width, height)
        {

        }
    }

    public class FlowchartToolBoxData : ToolBoxData
    {
        public NodeKinds Kind { get; set; }
        public FlowchartToolBoxData(NodeKinds kind, Type type, double width = 32, double height = 32) : base(kind.GetDescription(), null, type, width, height)
        {
            Kind = kind;
            ColorViewModel.LineColor.Color = Colors.Black;
        }

    }

    public class DesignerItemToolBoxData : ToolBoxData
    {
        public string FileName { get; set; }
        public DesignerItemViewModelBase DesignerItemViewModel { get; set; }
        public DesignerItemToolBoxData(DesignerItemBase designerItemBase, string filename, Type type,  double width = 32, double height = 32) : base(null, null, type, width, height)
        {
            Addition = designerItemBase;
            DesignerItemViewModel = (DesignerItemViewModelBase)Activator.CreateInstance(type, null, designerItemBase);
            FileName = filename;
            
        }
    }

    public class SvgToolBoxData : ToolBoxData
    {
        public SvgToolBoxData(string filename, Type type, double width = 32, double height = 32) : base(null, filename, type, width, height)
        {
            ColorViewModel.FillColor.Color = Colors.Blue;
        }
    }

    public class MediaToolBoxData : ToolBoxData
    {
        public MediaToolBoxData(string icon, Type type, double width = 32, double height = 32) : base(icon, null, type, width, height)
        {

        }
    }
}
