using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.DiagramDesigner.Helpers
{
    public class ToolBoxData
    {
        public string Text { get; protected set; }
        public string Icon { get; protected set; }
        public Type Type { get; protected set; }
        public IColorViewModel ColorViewModel { get; set; }
        public double Width { get; set; }
        public double Height { get; set; } 

        public object Addition { get; set; }

        public ToolBoxData(string text, string icon, Type type, double width, double height)
        {
            this.Text = text;
            this.Icon = icon;
            this.Type = type;
            this.Width = width;
            this.Height = height; 
            this.ColorViewModel = new ColorViewModel();
        }
    }
}
