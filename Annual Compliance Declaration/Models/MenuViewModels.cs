using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annual_Compliance_Declaration.Models
{
    public class MenuViewModels
    {
        public class GetMenu

        {
            public string MenuID { get; set; }
            public string MenuName { get; set; }
            public string ActionName { get; set; }
            public string ControllerName { get; set; }

        }

        public class SubMenu

        {
            public string MenuID { get; set; }
            public string MenuName { get; set; }
            public string ActionName { get; set; }
            public string ControllerName { get; set; }

        }
    }
}