using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.DiagramDesigner
{
    class DoCommandManager
    {
        #region Command定义
        public class Command
        {
            string name;
            Action action;
            Action unDoAction;
            Action clearAction;

            internal Command(string name, Action action, Action unDoAction, Action clearAction)
            {
                this.name = name;
                this.action = action;
                this.unDoAction = unDoAction;
                this.clearAction = clearAction;
            }

            internal void Do() { action(); }
            internal void UnDo() { unDoAction(); }
            internal void Clear() { if (clearAction != null) clearAction(); }

            public override string ToString()
            {
                return name.ToString();
            }
        }
        #endregion

        public Stack<Command> ReDoActionStack { get; private set; }
        public Stack<Command> UnDoActionStack { get; private set; }

        public int Capacity { get; set; } = 10;

        public DoCommandManager()
        {
            ReDoActionStack = new Stack<Command>();
            UnDoActionStack = new Stack<Command>();
        }

        public void DoNewCommand(string name, Action action, Action unDoAction, Action clearAction = null, bool doit = true)
        {
            if (UnDoActionStack.Count >= Capacity)
            {  
                //清理
                var clear = UnDoActionStack.LastOrDefault();
                clear.Clear();

                UnDoActionStack = new Stack<Command>(UnDoActionStack.Take(Capacity - 1).Reverse());
            }

            var cmd = new Command(name, action, unDoAction, clearAction);
            UnDoActionStack.Push(cmd);
    
            ReDoActionStack.Clear();
            if (doit)
            {
                cmd.Do();
            }
        }

        public void UnDo()
        {
            if (!CanUnDo)
                return;

            var cmd = UnDoActionStack.Pop();
            ReDoActionStack.Push(cmd);
            cmd.UnDo();
        }

        public void ReDo()
        {
            if (!CanReDo)
                return;

            var cmd = ReDoActionStack.Pop();
            UnDoActionStack.Push(cmd);
            cmd.Do();
        }

        public bool CanUnDo { get { return UnDoActionStack.Count != 0; } }
        public bool CanReDo { get { return ReDoActionStack.Count != 0; } }
        //public IEnumerable<Command> Actions { get { return ReDoActionStack.Reverse().Concat(UnDoActionStack); } }
    }
}
