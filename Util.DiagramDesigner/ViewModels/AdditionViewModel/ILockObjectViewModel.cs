using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Util.DiagramDesigner
{
    public interface ILockObjectViewModel
    {
        List<LockObject> LockObject { get; set; }
        void SetValue(LockObject obj);
        event PropertyChangedEventHandler PropertyChanged;
    }
}
