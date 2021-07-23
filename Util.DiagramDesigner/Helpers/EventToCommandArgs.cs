using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Util.DiagramDesigner
{
    public class EventToCommandArgs
    {
        public Object Sender { get; private set; }
        public ICommand CommandRan { get; private set; }
        public Object CommandParameter { get; private set; }
        public EventArgs EventArgs { get; private set; }

        public EventToCommandArgs(Object sender, ICommand commandRan, Object commandParameter, EventArgs eventArgs)
        {
            this.Sender = sender;
            this.CommandRan = commandRan;
            this.CommandParameter = commandParameter;
            this.EventArgs = eventArgs;
        }
    }
}
