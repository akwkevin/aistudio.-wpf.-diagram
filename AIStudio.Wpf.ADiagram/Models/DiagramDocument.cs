using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Models
{
    [XmlRootAttribute(Namespace = DiagramDocument.XMLNS, IsNullable = false)]
    public class DiagramDocument
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public DiagramType DiagramType { get; set; }

        [XmlArray]
        public List<DiagramItem> DiagramItems { get; set; }

        public const string XMLNS = "http://AIStudio.Wpf.ADiagram/DesignLayout";
        private readonly object saveLock = new Object();

        public void Save(FileInfo designFile)
        {
            lock (saveLock)
            {

                FileStream streamToUse;
                XmlSerializer serializer = new XmlSerializer(typeof(DiagramDocument));

                if (designFile.Exists)
                {
                    File.Delete(designFile.FullName);
                }
                streamToUse = designFile.Open(FileMode.OpenOrCreate, FileAccess.Write);

                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    XmlWriter writer = XmlWriter.Create(streamToUse, settings);
                    serializer.Serialize(writer, this);
                }
                finally
                {
                    streamToUse.Close();
                }
            }
        }

    }
}
