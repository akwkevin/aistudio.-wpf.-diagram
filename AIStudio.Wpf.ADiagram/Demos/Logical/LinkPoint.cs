using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Demos.Logical
{
    public class LinkPoint: BindableBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                SetProperty(ref _value, value);
            }
        }
    }
}
