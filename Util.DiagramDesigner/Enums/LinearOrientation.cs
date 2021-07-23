using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Util.DiagramDesigner
{
    public enum LinearOrientation
    {
        [Description("左到右")]
        LeftToRight,
        [Description("左上到右下")]
        LeftTopToRightBottom,
        [Description("上到下")]
        TopToBottom,
        [Description("右上到左下")]
        RightTopToLeftBottom,
        [Description("右到左")]
        RightToLeft,
        [Description("右下到左上")]
        RightBottomToLeftTop,
        [Description("下到上")]
        BottomToTop,
        [Description("左下到右上")]
        LeftBottomToRightTop
    }
}
