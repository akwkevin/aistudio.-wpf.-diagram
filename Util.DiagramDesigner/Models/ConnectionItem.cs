using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Util.DiagramDesigner
{
    [Serializable]
    [XmlInclude(typeof(ConnectionItem))]
    public class ConnectionItem : SelectableDesignerItemBase
    {
        public ConnectionItem()
        {

        }
        //public ConnectionItem(Guid id, Guid sourceId, ConnectorOrientation sourceOrientation, Type sourceType, double sourceXRatio, double sourceYRatio, bool sourceInnerPoint,
        //    Guid sinkId, ConnectorOrientation sinkOrientation, Type sinkType, double sinkXRatio, double sinkYRatio, bool sinkInnerPoint,
        //    int zIndex, bool isGroup, Guid parentId, DrawMode vectorLineDrawMode, ColorViewModel colorViewModel, FontViewModel fontViewModel) : base(id, zIndex, isGroup, parentId, colorViewModel, fontViewModel)
        //{
        //    this.SourceId = sourceId;
        //    this.SourceOrientation = sourceOrientation;
        //    this.SourceType = sourceType;
        //    this.SourceTypeName = sourceType.FullName;
        //    this.SourceXRatio = sourceXRatio;
        //    this.SourceYRatio = sourceYRatio;
        //    this.SourceInnerPoint = sourceInnerPoint;

        //    this.SinkId = sinkId;
        //    this.SinkOrientation = sinkOrientation;
        //    this.SinkType = sinkType;           
        //    this.SinkTypeName = sinkType.FullName;
        //    this.SinkXRatio = sinkXRatio;
        //    this.SinkYRatio = sinkYRatio;
        //    this.SinkInnerPoint = sinkInnerPoint;
        //    this.VectorLineDrawMode = vectorLineDrawMode;
        //}

        public ConnectionItem(Guid sourceId, ConnectorOrientation sourceOrientation, Type sourceType, double sourceXRatio, double sourceYRatio, bool sourceInnerPoint,
            Guid sinkId, ConnectorOrientation sinkOrientation, Type sinkType, double sinkXRatio, double sinkYRatio, bool sinkInnerPoint, ConnectorViewModel viewmodel) : base(viewmodel)
        {
            this.SourceId = sourceId;
            this.SourceOrientation = sourceOrientation;
            this.SourceType = sourceType;
            this.SourceTypeName = sourceType.FullName;
            this.SourceXRatio = sourceXRatio;
            this.SourceYRatio = sourceYRatio;
            this.SourceInnerPoint = sourceInnerPoint;

            this.SinkId = sinkId;
            this.SinkOrientation = sinkOrientation;
            this.SinkType = sinkType;
            this.SinkTypeName = sinkType.FullName;
            this.SinkXRatio = sinkXRatio;
            this.SinkYRatio = sinkYRatio;
            this.SinkInnerPoint = sinkInnerPoint;
            this.VectorLineDrawMode = viewmodel.VectorLineDrawMode;
        }

        [XmlAttribute]
        public Guid SourceId { get; set; }

        [XmlAttribute]
        public ConnectorOrientation SourceOrientation { get; set; }

        [XmlIgnore]
        public Type SourceType { get; set; }

        [XmlAttribute]
        public string SourceTypeName { get; set; }

        [XmlAttribute]
        public double SourceXRatio { get; set; }

        [XmlAttribute]
        public double SourceYRatio { get; set; }

        [XmlAttribute]
        public bool SourceInnerPoint { get; set; }

        [XmlAttribute]
        public Guid SinkId { get; set; }

        [XmlAttribute]
        public ConnectorOrientation SinkOrientation { get; set; }

        [XmlIgnore]
        public Type SinkType { get; set; }

        [XmlAttribute]
        public string SinkTypeName { get; set; }

        [XmlAttribute]
        public double SinkXRatio { get; set; }

        [XmlAttribute]
        public double SinkYRatio { get; set; }

        [XmlAttribute]
        public bool SinkInnerPoint { get; set; }

        [XmlAttribute]
        public DrawMode VectorLineDrawMode { get; set; }
    }
}
