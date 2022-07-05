using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annual_Compliance_Declaration.Models
{
    public class Compliance
    {
        public string EmployeeName { get; set; }
        public string EmployeeID { get; set; }
        public string DateofDeclaration { get; set; }
        public IEnumerable<SelectListItem> Department { get; set; }
        public IEnumerable<SelectListItem> WorkType { get; set; }
        public IEnumerable<SelectListItem> AssignName { get; set; }
        public IEnumerable<SelectListItem> Entity { get; set; }
        public string DepartmentID { get; set; }
        public string EntityID { get; set; }
        public string WorkTypeID { get; set; }
        public string AssignNameID { get; set; }
        public bool? CheckYesPer { get; set; }
        public bool? CheckNoPer { get; set; }
        public string Comment { get; set; }
        public bool? CheckYesConf { get; set; }
        public bool? CheckNoConf { get; set; }
        public string CommentConf { get; set; }
        public bool? Disabled { get; set; }
        public bool? DisabledDOWNLOAD { get; set; }
        public string DivCommentConf2 { get; set; }
        public string DivComment2 { get; set; }
        public string Years { get; set; }
        public string LINK { get; set; }
        public string CheckYesBehaveMessage { get; set; }
        public string CheckNoBehaveMessage { get; set; }
        public string CheckYesPernMessage { get; set; }
        public string CheckNoPernMessage { get; set; }

        //
        public bool? CheckCodeEthic { get; set; }
        public bool? CheckSocialMedia { get; set; }
        public bool? CheckTravel { get; set; }

    }
}