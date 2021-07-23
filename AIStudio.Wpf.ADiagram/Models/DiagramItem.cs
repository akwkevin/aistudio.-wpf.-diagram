using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using AIStudio.Wpf.ADiagram.Demos.Flowchart;
using AIStudio.Wpf.ADiagram.Demos.Others;
using Newtonsoft.Json;

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

        [JsonIgnore]
        [XmlIgnore]
        public List<DesignerItemBase> AllDesignerItems { get { return 
                            DesignerItems.OfType<DesignerItemBase>()
                     .Union(TextDesignerItems.OfType<DesignerItemBase>())
                     .Union(LogicalGateItems.OfType<DesignerItemBase>())
                     .Union(MediaDesignerItems.OfType<DesignerItemBase>())
                     .Union(ImageDesignerItems.OfType<DesignerItemBase>())
                     .Union(PathDesignerItems.OfType<DesignerItemBase>())
                     .Union(PersistDesignerItems.OfType<DesignerItemBase>())
                     .Union(SettingsDesignerItems.OfType<DesignerItemBase>())
                     .Union(FlowNodeDesignerItems.OfType<FlowNodeDesignerItem>())
                     .ToList(); } }

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
            throw new InvalidOperationException(string.Format("Unknown diagram type. Currently only {0} and {1} are supported",
                typeof(PersistDesignerItem).AssemblyQualifiedName,
                typeof(SettingsDesignerItemViewModel).AssemblyQualifiedName
                ));

        }
    }
}
