using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.DiagramDesigner
{
    public interface IGroupable
    {
        Guid Id { get; }
        Guid ParentId { get; set; }
        bool IsGroup { get; set; }
    }
}
