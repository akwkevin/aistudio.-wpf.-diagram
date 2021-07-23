using System;
using System.Collections.Generic;
using System.Text;

namespace Util.DiagramDesigner
{
    public interface IQuickThemeViewModel
    {
        QuickTheme[] QuickThemes { get; }
        QuickTheme QuickTheme { get; set; }
    }
}
