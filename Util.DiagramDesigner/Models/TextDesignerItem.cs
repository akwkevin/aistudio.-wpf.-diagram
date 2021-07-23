using Util.DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Util.DiagramDesigner
{
    public class TextDesignerItem : DesignerItemBase
    {
        public TextDesignerItem()
        {

        }
        public TextDesignerItem(TextDesignerItemViewModel item) : base(item)
        {
            this.Text = item.Text;
        }

    }
}
