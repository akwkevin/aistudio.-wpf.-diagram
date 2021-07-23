using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.DiagramDesigner
{
    public class ConnectorItem
    {
        public Guid ParentId { get; set; }
        public Guid Id { get; set; }
        public double XRatio { get; set; }
        public double YRatio { get; set; }
        public double ConnectorWidth { get; set; }
        public double ConnectorHeight { get; set; }
        public ConnectorOrientation Orientation { get; set; }
        public bool IsInnerPoint { get; set; }
        public ValueTypePoint ValueTypePoint { get; set; }
        public double ConnectorValue { get; set; }
    }
}
