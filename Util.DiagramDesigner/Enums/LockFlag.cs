using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Util.DiagramDesigner
{
    public enum LockFlag
    {
        [Description("无锁定")]
        None,
        [Description("锁定所有")]
        All,
        [Description("宽度(敬请期待)")]
        Width,
        [Description("高度(敬请期待)")]
        Height,
        [Description("纵横比(敬请期待)")]
        Ratio,
        [Description("水平位置(敬请期待)")]
        Horizontal,
        [Description("垂直位置(敬请期待)")]
        Vertical,
        [Description("旋转(敬请期待)")]
        Rotate, 
        [Description("起点(敬请期待)")]
        Start, 
        [Description("终点(敬请期待)")]
        End,
        [Description("取消组合(敬请期待)")]
        UnGroup,
        [Description("编辑文本(敬请期待)")]
        Text,
        [Description("保护选中(敬请期待)")]
        IsSelected,
        [Description("保护删除(敬请期待)")]
        Delete,
        [Description("阻止连接(敬请期待)")]
        Link, 
        [Description("格式(敬请期待)")] 
        Format,
        [Description("保护主题(敬请期待)")]
        Theme,
    }
}
