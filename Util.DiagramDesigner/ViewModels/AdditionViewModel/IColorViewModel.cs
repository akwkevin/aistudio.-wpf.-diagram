using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public interface IColorViewModel
    {
        IColorObject LineColor { get; set; }
        IColorObject FillColor { get; set; }
        Color ShadowColor { get; set; }
        double LineWidth { get; set; }
        ArrowPathStyle LeftArrowPathStyle { get; set; }
        ArrowPathStyle RightArrowPathStyle { get; set; }
        ArrowSizeStyle LeftArrowSizeStyle { get; set; }
        ArrowSizeStyle RightArrowSizeStyle { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}
