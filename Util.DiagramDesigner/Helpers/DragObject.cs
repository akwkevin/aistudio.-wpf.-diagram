using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Util.DiagramDesigner
{
    // Wraps info of the dragged object into a class
    public class DragObject
    {
        public Size? DesiredSize { get; set; }
        public Type ContentType { get; set; }
        public string Icon { get; set; }
        public IColorViewModel ColorViewModel { get; set; }
        public DesignerItemBase DesignerItem { get; set; }
    }
}
