using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Util.DiagramDesigner
{
    public enum PageUnit
    {
        [Description("毫米(暂未实现)")]
        mm,
        [Description("厘米")]
        cm,
        [Description("米(暂未实现)")]
        m,
        [Description("千米(暂未实现)")]
        km,
        [Description("英寸")]
        inch,
        [Description("英尺和英寸(暂未实现)")]
        ftin,
        [Description("英尺(暂未实现)")]
        foot,
        [Description("码(暂未实现)")]
        yard,
        [Description("英里(暂未实现)")]
        mile,
        [Description("点(暂未实现)")]
        tiny,
        [Description("皮卡(暂未实现)")]
        pickup,
        [Description("迪多(暂未实现)")]
        ditto,
        [Description("西塞罗(暂未实现)")]
        cicero,
        [Description("像素(暂未实现)")]
        pixel,
    }
}
