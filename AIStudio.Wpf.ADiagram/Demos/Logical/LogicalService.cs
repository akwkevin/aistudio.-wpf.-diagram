using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Wpf.ADiagram.Demos.Logical
{
    public static class LogicalService
    {
        public static List<LinkPoint> LinkPoint { get; set; }

        static LogicalService()
        {
            LinkPoint = new List<LinkPoint>();
            for (int i = 0; i < 10; i++)
            {
                LinkPoint.Add(new Logical.LinkPoint { Id = Guid.NewGuid(), Name = $"测点{i}", Value = i });
            }

        }
    }
}
