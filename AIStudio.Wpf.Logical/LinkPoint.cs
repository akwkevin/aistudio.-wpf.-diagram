using System;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.Logical
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
