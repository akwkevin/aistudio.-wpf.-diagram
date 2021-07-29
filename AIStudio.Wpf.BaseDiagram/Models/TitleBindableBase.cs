using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.BaseDiagram.Models
{
    public class TitleBindableBase : BindableBase
    {
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
            }
        }
    }
}
