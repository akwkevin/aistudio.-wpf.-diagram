using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Util.DiagramDesigner
{
    public class LockObjectViewModel : ILockObjectViewModel
    {
        public List<LockObject> LockObject { get; set; }

        public LockObjectViewModel()
        {
            LockObject = CopyHelper.DeepCopy<List<LockObject>>(LockObjectViewModelhelper.SourceLockObject);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValue(LockObject obj)
        {
            var item = LockObject.FirstOrDefault(p => p.LockFlag == obj.LockFlag);
            item.IsChecked = obj.IsChecked;
        }
    }

    public class LockObjectViewModelhelper
    {
        public static List<LockObject> SourceLockObject { get; private set; }
        static LockObjectViewModelhelper()
        {
            SourceLockObject = new List<LockObject>();
            var enums = Enum.GetValues(typeof(LockFlag));
            foreach (var _enum in enums.OfType<LockFlag>())
            {
                if (_enum == LockFlag.None) continue;

                var item = new LockObject() { Name = _enum.ToString(), LockFlag = _enum };
                SourceLockObject.Add(item);
            }
        }
    }

    [Serializable]
    public class LockObject : BindableBase
    {
        public string Name { get; set; }
        
        public LockFlag LockFlag { get; set; }

        private LockFlag _lockFlagValue = LockFlag.None;
        public LockFlag LockFlagValue
        {
            get
            {
                return _lockFlagValue;
            }
            set
            {
                SetProperty(ref _lockFlagValue, value);
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                if (SetProperty(ref _isChecked, value))
                {
                    if (_isChecked == true)
                    {
                        LockFlagValue = LockFlag;
                    }
                    else
                    {
                        LockFlagValue = LockFlag.None;
                    }
                }
            }
        }
    }
}
