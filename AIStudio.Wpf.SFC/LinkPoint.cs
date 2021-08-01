using System;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC
{
    public class LinkPoint: BindableBase
    {
        public Guid Id { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }
        public string Despcription { get; set; }

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
