using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Util.DiagramDesigner
{
    public interface IPathFinder
    {
        List<Point> GetConnectionLine(ConnectorInfo source, ConnectorInfo sink, bool showLastLine, bool sourceInnerPoint = false);
        List<Point> GetConnectionLine(ConnectorInfo source, Point sinkPoint, ConnectorOrientation preferredOrientation, bool isInnerPoint = false);
    }
}
