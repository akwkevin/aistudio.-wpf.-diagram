using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using AIStudio.Wpf.Flowchart;
using AIStudio.Wpf.BaseDiagram.Extensions.ViewModels;
using AIStudio.Wpf.BaseDiagram.Extensions.Models;
using Newtonsoft.Json;
using AIStudio.Wpf.Flowchart.Models;
using AIStudio.Wpf.Flowchart.ViewModels;
using AIStudio.Wpf.SFC.Models;
using AIStudio.Wpf.SFC.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace AIStudio.Wpf.ADiagram.Models
{
    [Serializable]
    public class DiagramItem
    {
        public DiagramItem()
        {
            this.ConnectionIds = new List<Guid>();
            this.Connections = new List<ConnectionItem>();
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public DiagramType DiagramType { get; set; }

        [XmlAttribute]
        public bool ShowGrid { get; set; }

        [XmlIgnore]
        public Size GridCellSize { get; set; }

        [JsonIgnore]
        [XmlAttribute("GridCellSize")]
        public string XmlGridCellSize
        {
            get
            {
                return SerializeHelper.SerializeSize(GridCellSize);
            }
            set
            {
                GridCellSize = SerializeHelper.DeserializeSize(value);
            }
        }

        [XmlAttribute]
        public CellHorizontalAlignment CellHorizontalAlignment { get; set; }

        [XmlAttribute]
        public CellVerticalAlignment CellVerticalAlignment { get; set; }

        [XmlAttribute]
        public PageSizeOrientation PageSizeOrientation { get; set; }

        [XmlIgnore]
        public Size PageSize { get; set; }

        [JsonIgnore]
        [XmlAttribute("PageSize")]
        public string XmlPageSize
        {
            get
            {
                return SerializeHelper.SerializeSize(PageSize);
            }
            set
            {
                PageSize = SerializeHelper.DeserializeSize(value);
            }
        }

        [XmlAttribute]
        public PageSizeType PageSizeType { get; set; }

        [XmlAttribute]
        public double GridMargin { get; set; }

        [XmlIgnore]
        public Color GridColor { get; set; }

        [JsonIgnore]
        [XmlAttribute("GridColor")]
        public string XmlGridColor
        {
            get
            {
                return SerializeHelper.SerializeColor(GridColor);
            }
            set
            {
                GridColor = SerializeHelper.DeserializeColor(value);
            }
        }

        [XmlArray]
        public List<DesignerItemBase> DesignerItems { get; set; } = new List<DesignerItemBase>();

        [XmlArray]
        public List<PersistDesignerItem> PersistDesignerItems { get; set; } = new List<PersistDesignerItem>();

        [XmlArray]
        public List<SettingsDesignerItem> SettingsDesignerItems { get; set; } = new List<SettingsDesignerItem>();

        [XmlArray]
        public List<PathDesignerItem> PathDesignerItems { get; set; } = new List<PathDesignerItem>();

        [XmlArray]
        public List<MediaDesignerItem> MediaDesignerItems { get; set; } = new List<MediaDesignerItem>();

        [XmlArray]
        public List<ImageDesignerItem> ImageDesignerItems { get; set; } = new List<ImageDesignerItem>();

        [XmlArray]
        public List<TextDesignerItem> TextDesignerItems { get; set; } = new List<TextDesignerItem>();

        [XmlArray]
        public List<LogicalGateDesignerItemBase> LogicalGateItems { get; set; } = new List<LogicalGateDesignerItemBase>();

        [XmlArray]
        public List<FlowNodeDesignerItem> FlowNodeDesignerItems { get; set; } = new List<FlowNodeDesignerItem>();

        [XmlArray]
        public List<SFCNodeDesignerItem> SFCNodeDesignerItems { get; set; } = new List<SFCNodeDesignerItem>();

        [JsonIgnore]
        [XmlIgnore]
        public List<DesignerItemBase> AllDesignerItems
        {
            get
            {
                return
DesignerItems.OfType<DesignerItemBase>()
.Union(TextDesignerItems.OfType<DesignerItemBase>())
.Union(LogicalGateItems.OfType<DesignerItemBase>())
.Union(MediaDesignerItems.OfType<DesignerItemBase>())
.Union(ImageDesignerItems.OfType<DesignerItemBase>())
.Union(PathDesignerItems.OfType<DesignerItemBase>())
.Union(PersistDesignerItems.OfType<DesignerItemBase>())
.Union(SettingsDesignerItems.OfType<DesignerItemBase>())
.Union(FlowNodeDesignerItems.OfType<DesignerItemBase>())
.Union(SFCNodeDesignerItems.OfType<DesignerItemBase>())
.ToList();
            }
        }

        [XmlArray]
        public List<Guid> ConnectionIds { get; set; }

        [XmlArray]
        public List<ConnectionItem> Connections { get; set; }

        public void AddItems(IEnumerable<SelectableDesignerItemViewModelBase> selectedDesignerItems)
        {
            foreach (var item in selectedDesignerItems.OfType<DesignerItemViewModelBase>())
            {
                if (item is PersistDesignerItemViewModel)
                {
                    PersistDesignerItems.Add(new PersistDesignerItem(item as PersistDesignerItemViewModel));
                }
                else if (item is SettingsDesignerItemViewModel)
                {
                    SettingsDesignerItems.Add(new SettingsDesignerItem(item as SettingsDesignerItemViewModel));
                }
                else if (item is PathItemViewModel)
                {
                    PathDesignerItems.Add(new PathDesignerItem(item));
                }
                else if (item is GifImageItemViewModel)
                {
                    MediaDesignerItems.Add(new MediaDesignerItem(item as GifImageItemViewModel));
                }
                else if (item is MediaItemViewModel)
                {
                    MediaDesignerItems.Add(new MediaDesignerItem(item as MediaItemViewModel));
                }
                else if (item is ImageItemViewModel)
                {
                    ImageDesignerItems.Add(new ImageDesignerItem(item as ImageItemViewModel));
                }
                else if (item is TextDesignerItemViewModel)
                {
                    TextDesignerItems.Add(new TextDesignerItem(item as TextDesignerItemViewModel));
                }
                else if (item is LogicalGateItemViewModelBase)
                {
                    LogicalGateItems.Add(new LogicalGateDesignerItemBase(item as LogicalGateItemViewModelBase));
                }
                else if (item is FlowNode)
                {
                    FlowNodeDesignerItems.Add(new FlowNodeDesignerItem(item as FlowNode));
                }
                else if (item is SFCNode)
                {
                    SFCNodeDesignerItems.Add(new SFCNodeDesignerItem(item as SFCNode));
                }
                else if (item is BarcodeDesignerItemViewModel)
                {
                    DesignerItems.Add(new DesignerItemBase(item, (item as BarcodeDesignerItemViewModel).Format.ToString()));
                }
                else
                {
                    DesignerItems.Add(new DesignerItemBase(item));
                }
            }
        }

        public static DesignerItemBase ToXmlObject(SelectableDesignerItemViewModelBase item)
        {
            if (item is PersistDesignerItemViewModel)
            {
                return new PersistDesignerItem(item as PersistDesignerItemViewModel);
            }
            else if (item is SettingsDesignerItemViewModel)
            {
                return new SettingsDesignerItem(item as SettingsDesignerItemViewModel);
            }
            else if (item is PathItemViewModel)
            {
                return new PathDesignerItem(item as PathItemViewModel);
            }
            else if (item is GifImageItemViewModel)
            {
                return new MediaDesignerItem(item as GifImageItemViewModel);
            }
            else if (item is MediaItemViewModel)
            {
                return new MediaDesignerItem(item as MediaItemViewModel);
            }
            else if (item is ImageItemViewModel)
            {
                return new ImageDesignerItem(item as ImageItemViewModel);
            }
            else if (item is TextDesignerItemViewModel)
            {
                return new TextDesignerItem(item as TextDesignerItemViewModel);
            }
            else if (item is LogicalGateItemViewModelBase)
            {
                return new LogicalGateDesignerItemBase(item as LogicalGateItemViewModelBase);
            }
            else if (item is FlowNode)
            {
                return new FlowNodeDesignerItem(item as FlowNode);
            }
            else if (item is SFCNode)
            {
                return new SFCNodeDesignerItem(item as SFCNode);
            }
            else
            {
                return new DesignerItemBase(item as DesignerItemViewModelBase);
            }
        }

        public static Type GetTypeOfDiagramItem(DesignerItemViewModelBase vmType)
        {
            if (vmType is PersistDesignerItemViewModel)
                return typeof(PersistDesignerItem);
            if (vmType is SettingsDesignerItemViewModel)
                return typeof(SettingsDesignerItem);
            if (vmType is PathItemViewModel)
                return typeof(PathToolBoxData);
            if (vmType is GifImageItemViewModel)
                return typeof(MediaDesignerItem);
            if (vmType is MediaItemViewModel)
                return typeof(MediaDesignerItem);
            if (vmType is ImageItemViewModel)
                return typeof(ImageDesignerItem);
            if (vmType is LogicalGateItemViewModelBase)
                return typeof(LogicalGateDesignerItemBase);
            if (vmType is FlowNode)
                return typeof(FlowNodeDesignerItem);
            if (vmType is SFCNode)
                return typeof(SFCNodeDesignerItem);
            throw new InvalidOperationException(string.Format("Unknown diagram type. Currently only {0} and {1} are supported",
                typeof(PersistDesignerItem).AssemblyQualifiedName,
                typeof(SettingsDesignerItemViewModel).AssemblyQualifiedName
                ));

        }
    }
}
