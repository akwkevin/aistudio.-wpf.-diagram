using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Util.DiagramDesigner
{
    public enum RadialOrientation
    {
        [Description("中心")]
        Center,
        [Description("左上")]
        LeftTop,
        [Description("右上")]
        RightTop,
        [Description("右下")]
        RightBottom,
        [Description("左下")]
        LeftBottom,     
    }
}
