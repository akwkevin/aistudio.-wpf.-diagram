using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Util.DiagramDesigner
{
    public enum ArrowSizeStyle
    {
        [Description("非常小")]
        VerySmall = 6,
        [Description("小")]
        Small = 8,
        [Description("中")]
        Middle = 10,
        [Description("大")]
        Large = 12,
        [Description("特别大")]
        ExtraLarge = 16,
        [Description("巨大")]
        Huge = 24,

    }
}
