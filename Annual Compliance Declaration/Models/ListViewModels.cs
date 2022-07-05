using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annual_Compliance_Declaration.Models
{
    public class ListViewModels
    {
        public string Username { get; set; }
        public string No { get; set; }
        public string NIK { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string EmployeeName { get; set; }
        public string Entity { get; set; }
        public string EntityID { get; set; }
        public IEnumerable<SelectListItem> EntitySearch { get; set; }
        public string EntityIDSearch { get; set; }
        public string Department { get; set; }
        public IEnumerable<SelectListItem> DepartmentID { get; set; }
        public string isBehave { get; set; }
        public string isConflict { get; set; }
        public string CommentBehave { get; set; }
        public string CommentConflict { get; set; }
        public string DateDeclare { get; set; }
        public string Role { get; set; }


        public List<string> listCol { set; get; }
        public List<List<string>> lists { set; get; }
        public int countRow { set; get; }
        public int countCol { get; set; }

    }
}