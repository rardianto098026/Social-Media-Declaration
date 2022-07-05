using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annual_Compliance_Declaration.Models
{
    public class UserMatrix

    {
        public IEnumerable<SelectListItem> MenuList { get; set; }
        public IEnumerable<SelectListItem> ChildMenuList { get; set; }
        public string No { get; set; }
        public string NIK { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Entity { get; set; }
        public string Role { get; set; }
        public string IDMenu { get; set; }
        public string MenuName { get; set; }
        public string ChildMenu { get; set; }
        public bool Active { get; set; }

    }

}
