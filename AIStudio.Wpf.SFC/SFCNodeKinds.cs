using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AIStudio.Wpf.SFC
{
    public enum SFCNodeKinds
    {
        [Description("开始")]
        Start = 1,
        [Description("节点")]
        Node = 2,
        [Description("转移条件")]
        Condition = 3,
        [Description("动作")]
        Action = 4,
        [Description("并行开始")]
        COBegin = 5,
        [Description("并行结束")]
        COEnd = 6,
    }
}
