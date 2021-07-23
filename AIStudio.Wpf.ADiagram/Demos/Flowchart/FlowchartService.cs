using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Wpf.ADiagram.Demos.Flowchart
{
    public static class FlowchartService
    {
        static FlowchartService()
        {
            Users = new List<SelectOption>()
            {
                new SelectOption(){ value = "操作员1",text = "操作员1" },
                new SelectOption(){ value = "操作员2",text = "操作员2" },
                new SelectOption(){ value = "Admin",text = "Admin" },
            };

            Roles = new List<SelectOption>()
            {
                new SelectOption(){ value = "操作员",text = "操作员" },
                new SelectOption(){ value = "管理员",text = "管理员" },
            };
        }

        private static List<SelectOption> _users;
        public static List<SelectOption> Users
        {
            get { return _users; }
            set
            {
                _users = value;
            }
        }

        private static List<SelectOption> _roles;
        public static List<SelectOption> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
            }
        }
    }
}
