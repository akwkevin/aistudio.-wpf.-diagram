using System;
using System.Collections.Generic;
using System.Text;

namespace Util.DiagramDesigner
{
    public enum DrawMode
    {
        Normal = 0,
        Line = 1,
        Rectangle = 2,
        Ellipse = 3,
        Polyline = 4,
        Polygon = 5,
        DirectLine = 6,
        ConnectingLine = 10,
        CornerConnectingLine = 11,
        RadiusConnectingLine = 12,
        Text = 20,
    }
}
