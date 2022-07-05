using Annual_Compliance_Declaration.Models;
using Annual_Compliance_Declaration.Repository;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Annual_Compliance_Declaration.Models.MenuViewModels;

namespace Annual_Compliance_Declaration.Controllers
{
    public class ComplianceController : Controller
    {
        // GET: Compliance
        public ActionResult Compliance()
        {
            return View();
        }
        //[HttpGet]
        public ActionResult AddCompliance(Compliance model, string ID)
        {
            string EmplID = Request.QueryString["NIK"];
            string Names = Request.QueryString["Name"];
            string Departments = Request.QueryString["Department"];
            string WorkerType = Request.QueryString["Department"];
            string Assign = Request.QueryString["Department"];

            if (model.CheckYesBehaveMessage == null)
            {
                model.CheckYesBehaveMessage = "";
            }

            if (model.CheckYesPernMessage == null)
            {
                model.CheckYesPernMessage = "";
            }
            
            model.Disabled = false;
            model.DisabledDOWNLOAD = true;
            model.DivCommentConf2 = "display:none";
            model.DivComment2 = "display:none";
            string url = Request.Url.OriginalString;
            Session["url"] = url;
            model.LINK = url;

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.menu = Session["menu"];
            Session["controller"] = "ComplianceController";
            if (Session["Role"].ToString().ToUpper() == "ADMIN" || Session["Role"].ToString().ToUpper() == "SUPER ADMIN")
            {
                ID = string.IsNullOrEmpty(EmplID) == true ? Session["EmployeeNumber"].ToString() : EmplID;
            }
            else
            {
                ID = Session["EmployeeNumber"].ToString();
            }
                
            DataTable dt = Common.ExecuteQuery("dbo.[sp_Get_Employee_Detail] '" + ID + "'");
            if (dt.Rows.Count > 0)
            {
                model.EmployeeID = dt.Rows[0]["EmployeeID"].ToString();
                model.EmployeeName = dt.Rows[0]["EmployeeName"].ToString();
                model.DepartmentID = dt.Rows[0]["Department"].ToString();
                model.EntityID = dt.Rows[0]["ShortEntity"].ToString();
                model.WorkTypeID = dt.Rows[0]["WorkerType"].ToString();
                model.AssignNameID = dt.Rows[0]["AssignName"].ToString();
            }
           
            string CountData;
            DataTable dtx;
            dtx = Common.ExecuteQuery("dbo.[sp_GET_COUNT_DATA_TRN_SOCIAL_MEDIA_EMPL] '" + model.EmployeeID + "','" + model.EmployeeName + "' ");
            if (dtx.Rows.Count > 0)
            {
                CountData = dtx.Rows[0]["STATUS_DATA"].ToString();
                if (CountData.ToString().ToUpper() == "* DATA HAS BEEN SUBMITTED")
                {
                    if (Session["Role"].ToString().ToUpper() == "ADMIN" || Session["Role"].ToString().ToUpper() == "SUPER ADMIN")
                    {
                        model.Disabled = true;
                        model.DisabledDOWNLOAD = false;
                    }
                    else
                    {
                        model.Disabled = true;
                        model.DisabledDOWNLOAD = false;
                    }
                    
                    TempData["CountData"] = CountData;
                    model.EmployeeID = dtx.Rows[0]["EmployeeID"].ToString();
                    model.EmployeeName = dtx.Rows[0]["EmployeeName"].ToString();
                    //model.DepartmentID = dtx.Rows[0]["Department"].ToString();
                    //model.EntityID = dtx.Rows[0]["Entity"].ToString();
                    model.Comment = dtx.Rows[0]["CommentBehave"].ToString();
                    model.CommentConf = dtx.Rows[0]["CommentConflict"].ToString();
                    model.CheckNoPer = Convert.ToBoolean(dtx.Rows[0]["isBehaveNo"].ToString());
                    model.CheckYesPer = Convert.ToBoolean(dtx.Rows[0]["isBehaveYes"].ToString());
                    model.CheckNoConf = Convert.ToBoolean(dtx.Rows[0]["isConflictNo"].ToString());
                    model.CheckYesConf = Convert.ToBoolean(dtx.Rows[0]["isConflictYes"].ToString());
                    model.Years = dtx.Rows[0]["Year"].ToString();
                    //isBehave = Convert.ToBoolean(dtx.Rows[0]["isBehave"]);
                    //isConflict = Convert.ToBoolean(dtx.Rows[0]["isConflict"]);
                    if (model.CheckYesPer.ToString().ToUpper() == "TRUE")
                    {
                        //model.CheckNoPer = false;
                        //model.CheckYesPer = true;
                        model.DivComment2 = "display:none";
                    }
                    else
                    {
                        //model.CheckNoPer = true;
                       // model.CheckYesPer = false;
                        model.DivComment2 = "display:inline";
                    }

                    if (model.CheckYesConf.ToString().ToUpper() == "TRUE")
                    {
                        //model.CheckNoConf = false;
                       // model.CheckYesConf = true;
                        model.DivCommentConf2 = "display:inline";
                    }
                    else
                    {
                        //model.CheckNoConf = true;
                        //model.CheckYesConf = false;
                        model.DivCommentConf2 = "display:none";
                    }

                }
            }
            model.Department = DDLDept();
            model.Entity = DDLEntity();
            model.WorkType = DDLWorkType();
            model.AssignName = DDLAssignName();
            return View(model);
        }
        public ActionResult Edit(Compliance model, string ID)
        {
            string EmplID = Request.QueryString["NIK"];
            string Names = Request.QueryString["Name"];
            string Departments = Request.QueryString["Department"];

            model.Disabled = false;
            model.DisabledDOWNLOAD = true;
            model.DivCommentConf2 = "display:none";
            model.DivComment2 = "display:none";
            string url = Request.Url.OriginalString;
            Session["urlEdit"] = url;
            model.LINK = url;

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.menu = Session["menu"];
            Session["controller"] = "ComplianceController";
            if (Session["Role"].ToString().ToUpper() == "ADMIN" || Session["Role"].ToString().ToUpper() == "SUPER ADMIN")
            {
                ID = string.IsNullOrEmpty(EmplID) == true ? Session["EmployeeNumber"].ToString() : EmplID;
            }
            else
            {
                ID = Session["EmployeeNumber"].ToString();
            }

            DataTable dt = Common.ExecuteQuery("dbo.[sp_Get_Employee_Detail] '" + ID + "'");
            if (dt.Rows.Count > 0)
            {
                model.EmployeeID = dt.Rows[0]["EmployeeID"].ToString();
                model.EmployeeName = dt.Rows[0]["EmployeeName"].ToString();
                model.DepartmentID = dt.Rows[0]["Department"].ToString();
                model.EntityID = dt.Rows[0]["ShortEntity"].ToString();
                model.WorkTypeID = dt.Rows[0]["WorkerType"].ToString();
                model.AssignNameID = dt.Rows[0]["AssignName"].ToString();
            }

            string CountData;
            DataTable dtx;
            dtx = Common.ExecuteQuery("dbo.[sp_GET_COUNT_DATA_TRN_SOCIAL_MEDIA_EMPL] '" + model.EmployeeID + "','" + model.EmployeeName + "' ");
            if (dtx.Rows.Count > 0)
            {
                CountData = dtx.Rows[0]["STATUS_DATA"].ToString();
                if (CountData.ToString().ToUpper() == "* DATA HAS BEEN SUBMITTED")
                {
                    if (Session["Role"].ToString().ToUpper() == "ADMIN" || Session["Role"].ToString().ToUpper() == "SUPER ADMIN")
                    {
                        model.Disabled = false;
                        model.DisabledDOWNLOAD = false;
                    }
                    else
                    {
                        model.Disabled = true;
                        model.DisabledDOWNLOAD = false;
                    }

                    TempData["CountData"] = CountData;
                    model.EmployeeID = dtx.Rows[0]["EmployeeID"].ToString();
                    model.EmployeeName = dtx.Rows[0]["EmployeeName"].ToString();
                    model.DepartmentID = dtx.Rows[0]["Department"].ToString();
                    model.EntityID = dtx.Rows[0]["Entity"].ToString();
                    model.Comment = dtx.Rows[0]["CommentBehave"].ToString();
                    model.CommentConf = dtx.Rows[0]["CommentConflict"].ToString();
                    model.CheckNoPer = Convert.ToBoolean(dtx.Rows[0]["isBehaveNo"].ToString());
                    model.CheckYesPer = Convert.ToBoolean(dtx.Rows[0]["isBehaveYes"].ToString());
                    model.CheckNoConf = Convert.ToBoolean(dtx.Rows[0]["isConflictNo"].ToString());
                    model.CheckYesConf = Convert.ToBoolean(dtx.Rows[0]["isConflictYes"].ToString());
                    model.Years = dtx.Rows[0]["Year"].ToString();
                    //isBehave = Convert.ToBoolean(dtx.Rows[0]["isBehave"]);
                    //isConflict = Convert.ToBoolean(dtx.Rows[0]["isConflict"]);
                    if (model.CheckYesPer.ToString().ToUpper() == "TRUE")
                    {
                        //model.CheckNoPer = false;
                        //model.CheckYesPer = true;
                        model.DivComment2 = "display:none";
                    }
                    else
                    {
                        //model.CheckNoPer = true;
                        // model.CheckYesPer = false;
                        model.DivComment2 = "display:inline";
                    }

                    if (model.CheckYesConf.ToString().ToUpper() == "TRUE")
                    {
                        //model.CheckNoConf = false;
                        // model.CheckYesConf = true;
                        model.DivCommentConf2 = "display:inline";
                    }
                    else
                    {
                        //model.CheckNoConf = true;
                        //model.CheckYesConf = false;
                        model.DivCommentConf2 = "display:none";
                    }

                }
            }
            model.Department = DDLDept();
            model.Entity = DDLEntity();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddCompliance(Compliance model, string Submit,string IDX)
        {
            string url = Request.Url.OriginalString;
            DataTable dt;
            Session["url"] = model.LINK;

            ViewBag.menu = Session["menu"];
            Session["controller"] = "ComplianceController";
            
            string ID;

            string EmplName = model.EmployeeName;
            string Dept = model.DepartmentID;
            string Entity = model.EntityID;
            string Comment;
            string CommentConflict;
            bool isBehave;
            bool isConflict;
            DataTable dtx;
            string CountData;
            dtx = Common.ExecuteQuery("dbo.[sp_GET_COUNT_DATA_TRN_SOCIAL_MEDIA_EMPL] '" + model.EmployeeID + "','" + model.EmployeeName + "' ");
            if (dtx.Rows.Count > 0)
            {
                CountData = dtx.Rows[0]["STATUS_DATA"].ToString();
                if (CountData.ToString().ToUpper() == "* DATA HAS BEEN SUBMITTED")
                {
                    model.Years = dtx.Rows[0]["Year"].ToString();
                }
            }

            if (model.CheckNoPer.ToString().ToUpper() == "FALSE" && model.CheckYesPer.ToString().ToUpper() == "TRUE")
            {
                isBehave = model.CheckYesPer.Value;
                Comment = "";
            }
            else
            {
                model.CheckNoPer = true;
                model.CheckYesPer = false;
                isBehave = true;
                Comment = model.Comment;
            }

            if (model.CheckNoConf.ToString().ToUpper() == "FALSE" && model.CheckYesConf.ToString().ToUpper() == "TRUE")
            {
                isConflict = model.CheckYesConf.Value;
                CommentConflict = model.CommentConf;
            }
            else
            {
                isConflict = true;
                CommentConflict = "";
            }

            Session["controller"] = "ComplianceController";
            if (Session["Role"].ToString().ToUpper() == "ADMIN" || Session["Role"].ToString().ToUpper() == "SUPER ADMIN")
            {
                ID = string.IsNullOrEmpty(model.EmployeeID) == true ? Session["EmployeeNumber"].ToString() : model.EmployeeID;
            }
            else
            {
                ID = Session["EmployeeNumber"].ToString();
            }
            
            model.Department = DDLDept();
            model.Entity = DDLEntity();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Submit == "Back")
            {
                return RedirectToAction("IndexUser", "Home");
            }
            else if (Submit == "Submit")
            {
                try
                {
                    if (model.CheckNoConf.ToString().ToUpper() == "FALSE" && model.CheckYesConf.ToString().ToUpper() == "FALSE")
                    {
                        //TempData["messageRequest"] = "<script>alert('Please Choose a Conflict of Duty');</script>";
                        model.CheckYesBehaveMessage = "Please Choose a Conflict of Duty";
                        model.CheckYesPernMessage = "";

                        //TempData["messageRequestConf"] = "Please Choose a Conflict of Duty";

                        return View(model);

                    }
                    else
                    {
                        model.CheckYesConf = true;
                        model.CheckNoConf = true;
                    }
                    if (model.CheckNoPer.ToString().ToUpper() == "FALSE" && model.CheckYesPer.ToString().ToUpper() == "FALSE")
                    {
                        //TempData["messageRequest"] = "<script>alert('Please Choose accordance with the policies and guidelines described in that Manual');</script>";
                        model.CheckYesPernMessage = "Please Choose accordance with the policies and guidelines described in that Manual";
                        model.CheckYesBehaveMessage = "";

                        //TempData["messageRequestPer"] = "Please Choose accordance with the policies and guidelines described in that Manual";

                        return View(model);
                    }

                    if (model.CheckNoPer.ToString().ToUpper() == "TRUE")
                    {
                        isBehave = false;
                        //if (string.IsNullOrEmpty(Comment) == true)
                        //{
                        //    TempData["messageRequest"] = "<script>alert('Please Fill Comment');</script>";
                        //    //return View(model);
                        //}
                    }
                    else if(model.CheckYesPer.ToString().ToUpper() == "TRUE")
                    {
                        isBehave = true;
                        
                    }

                    if (model.CheckNoConf.ToString().ToUpper() == "TRUE")
                    {
                        isConflict = false;
                    }
                    else if (model.CheckYesConf.ToString().ToUpper() == "TRUE")
                    {
                        isConflict = true;
                        //if (string.IsNullOrEmpty(CommentConflict) == true)
                        //{
                        //    TempData["messageRequest"] = "<script>alert('Please Fill Comment');</script>";
                        //    //return View(model);
                        //}
                    }

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConSql"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_INSERT_TRN_SOCIAL_MEDIA", conn))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EMPLOYEEID", Session["EmployeeNumber"].ToString() == "" ? model.EmployeeID : Session["EmployeeNumber"].ToString());
                            cmd.Parameters.AddWithValue("@EmployeeName", EmplName);
                            cmd.Parameters.AddWithValue("@Department", Dept);
                            cmd.Parameters.AddWithValue("@Entity", Entity);
                            cmd.Parameters.AddWithValue("@isBehaveYes", model.CheckYesPer);
                            cmd.Parameters.AddWithValue("@isBehaveNo", model.CheckNoPer);
                            cmd.Parameters.AddWithValue("@CommentBehave", string.IsNullOrEmpty(Comment) == true ? "" : Comment);
                            cmd.Parameters.AddWithValue("@isConflictYes", model.CheckYesConf);
                            cmd.Parameters.AddWithValue("@isConflictNo", model.CheckNoConf);
                            cmd.Parameters.AddWithValue("@CommentConflict", string.IsNullOrEmpty(CommentConflict) == true ? "" : CommentConflict);
                            cmd.Parameters.AddWithValue("@CreatedBy", Session["UserID"].ToString());
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        try
                        {

                            CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
                            DataTable dtDCR;
                            dtDCR = Common.ExecuteQuery("dbo.[sp_GET_REPORT_ANNUAL_EMPLOYEE] '" + model.EmployeeID + "','" + model.EmployeeName + "','" + model.DepartmentID + "'");
                            ReportClass rptH = new ReportClass();
                            rptH.FileName = Server.MapPath("~/Report/Crystal/DownloadDeclaration.rpt");
                            rptH.Load();

                            rptH.SetDataSource(dtDCR);
                            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            File(stream, "application/pdf");
                            //rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, HttpContext.ApplicationInstance.Response, true, "Social Media_" + model.EmployeeID + "_" + model.Years);
                            var path = Server.MapPath("~/Document/" + Entity + "/Social Media_" + model.EmployeeID + "_" + DateTime.Now.Year + ".pdf");
                            rptH.ExportToDisk(ExportFormatType.PortableDocFormat, path);
                            //rptH.ExportToDisk(ExportFormatType.PortableDocFormat, "wrbmdtapp01/Application/Social_Media/Document/'" + model.Entity + "'/Social Media_'" + model.EmployeeID + "'_'" + model.Years + "'.pdf");

                            using (SqlCommand cmd = new SqlCommand("SP_SEND_EMAIL", conn))
                            {

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@EMPID", model.EmployeeID);
                                cmd.Parameters.AddWithValue("@PATH", path);
                                //conn.Open();
                                cmd.ExecuteNonQuery();
                            }
                            rptH.Close();
                            rptH.Dispose();
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorRequest"] = ex.ToString();
                            return View(model);
                        }
                        
                    }
                    //TempData["SuccessRequest"] = "Submit Request successfully.";
                    TempData["messageRequest"] = "<script>alert('I hereby declare that all my information that I have provided on Social Media Declaration according to the actual circumstances.');</script>";
                    //return Content("jAlert('Wow !My alert message with custom titile','My custom title bar here...');");
                   //return Content("<script language='javascript' type='text/javascript'>jAlert('Wow !My alert message with custom titile','My custom title bar here...');</script>");
                    return RedirectToAction("AddCompliance", "Compliance");
                }
                catch (Exception ex)
                {
                    TempData["ErrorRequest"] = ex.ToString();
                    return View(model);
                }
            }
            else if(Submit == "Download")
            {
                try
                {

                    CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
                    DataTable dtDCR;
                    dtDCR = Common.ExecuteQuery("dbo.[sp_GET_REPORT_ANNUAL_EMPLOYEE] '" + model.EmployeeID + "','" + model.EmployeeName + "','" + model.DepartmentID + "'");
                    ReportClass rptH = new ReportClass();
                    rptH.FileName = Server.MapPath("~/Report/Crystal/DownloadDeclaration.rpt");
                    rptH.Load();
                    
                    rptH.SetDataSource(dtDCR);
                    Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    File(stream, "application/pdf");
                    rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, HttpContext.ApplicationInstance.Response, true, "Social Media_" + model.EmployeeID + "_" +model.Years );

                    // Using Stream
                    // byte[] _contentBytes;
                    //ReportDocument reportDocument = new ReportDocument();
                    //reportDocument.Load(Server.MapPath("~/Report/Crystal/DownloadDeclaration.rpt"));
                    //reportDocument.SetDataSource(dtDCR);
                    //_contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.PortableDocFormat));
                    //var response = HttpContext.ApplicationInstance.Response;
                    //response.Clear();
                    //response.Buffer = false;
                    //response.ClearContent();
                    //response.ClearHeaders();
                    //response.Cache.SetCacheability(HttpCacheability.Public);
                    //response.ContentType = "application/pdf";

                    //using (var stream = new MemoryStream(_contentBytes))
                    //{
                    //    stream.WriteTo(response.OutputStream);
                    //    stream.Flush();
                    //}
                    // end using  // Using Stream

                    rptH.Close();
                    rptH.Dispose();

                }
                catch (Exception ex)
                {
                    TempData["ErrorRequest"] = ex.ToString();
                    return View(model);
                }

            }
            string worktype = model.WorkTypeID;
            string depart = model.DepartmentID;
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Compliance model, string Submit, string IDX)
        {
            string url = Request.Url.OriginalString;
            DataTable dt;

            //string url = Request.Url.OriginalString;
            //Session["url"] = url;

            ViewBag.menu = Session["menu"];
            Session["controller"] = "ComplianceController";

            string ID;

            string EmplName = model.EmployeeName;
            string Dept = model.DepartmentID;
            string Entity = model.EntityID;
            string Comment;
            string CommentConflict;
            bool isBehave;
            bool isConflict;
            DataTable dtx;
            string CountData;
            dtx = Common.ExecuteQuery("dbo.[sp_GET_COUNT_DATA_TRN_SOCIAL_MEDIA_EMPL] '" + model.EmployeeID + "','" + model.EmployeeName + "' ");
            if (dtx.Rows.Count > 0)
            {
                CountData = dtx.Rows[0]["STATUS_DATA"].ToString();
                if (CountData.ToString().ToUpper() == "* DATA HAS BEEN SUBMITTED")
                {
                    model.Years = dtx.Rows[0]["Year"].ToString();
                }
            }

            if (model.CheckNoPer.ToString().ToUpper() == "FALSE" && model.CheckYesPer.ToString().ToUpper() == "TRUE")
            {
                isBehave = model.CheckYesPer.Value;
                Comment = "";
            }
            else
            {
                isBehave = model.CheckNoPer.Value;
                Comment = model.Comment;
            }

            if (model.CheckNoConf.ToString().ToUpper() == "FALSE" && model.CheckYesConf.ToString().ToUpper() == "TRUE")
            {
                isConflict = model.CheckYesConf.Value;
                CommentConflict = model.CommentConf;
            }
            else
            {
                isConflict = model.CheckNoConf.Value;
                CommentConflict = "";
            }

            Session["controller"] = "ComplianceController";
            if (Session["Role"].ToString().ToUpper() == "ADMIN" || Session["Role"].ToString().ToUpper() == "SUPER ADMIN")
            {
                ID = string.IsNullOrEmpty(model.EmployeeID) == true ? Session["EmployeeNumber"].ToString() : model.EmployeeID;
            }
            else
            {
                ID = Session["EmployeeNumber"].ToString();
            }

            model.Department = DDLDept();
            model.Entity = DDLEntity();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Submit == "Back")
            {
                return RedirectToAction("IndexUser", "Home");
            }
            else if (Submit == "Submit")
            {
                try
                {
                    String LinkURL = Session["urlEdit"].ToString();
                    if (model.CheckNoConf.ToString().ToUpper() == "FALSE" && model.CheckYesConf.ToString().ToUpper() == "FALSE")
                    {
                        TempData["messageRequest"] = "<script>alert('Please Choose a Conflict of Duty');</script>";

                        model.CheckYesBehaveMessage = "Please Choose a Conflict of Duty";

                        return Redirect(LinkURL);

                    }
                    if (model.CheckNoPer.ToString().ToUpper() == "FALSE" && model.CheckYesPer.ToString().ToUpper() == "FALSE")
                    {
                        TempData["messageRequest"] = "<script>alert('Please Choose accordance with the policies and guidelines described in that Manual');</script>";

                        model.CheckYesPernMessage = "Please Choose accordance with the policies and guidelines described in that Manual";
                        return Redirect(LinkURL);
                    }

                    if (model.CheckNoPer.ToString().ToUpper() == "TRUE")
                    {
                        isBehave = false;
                    }
                    else if (model.CheckYesPer.ToString().ToUpper() == "TRUE")
                    {
                        isBehave = true;
                        Comment = "";
                        model.Comment = "";
                    }

                    if (model.CheckNoConf.ToString().ToUpper() == "TRUE")
                    {
                        isConflict = false;
                        CommentConflict = "";
                        model.CommentConf = "";
                    }
                    else if (model.CheckYesConf.ToString().ToUpper() == "TRUE")
                    {
                        isConflict = true;
                    }

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConSql"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_INSERT_TRN_SOCIAL_MEDIA", conn))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EMPLOYEEID", model.EmployeeID);
                            cmd.Parameters.AddWithValue("@EmployeeName", model.EmployeeName);
                            cmd.Parameters.AddWithValue("@Department", Dept);
                            cmd.Parameters.AddWithValue("@Entity", Entity);
                            cmd.Parameters.AddWithValue("@isBehaveYes", model.CheckYesPer);
                            cmd.Parameters.AddWithValue("@isBehaveNo", model.CheckNoPer);
                            cmd.Parameters.AddWithValue("@CommentBehave", string.IsNullOrEmpty(Comment) == true ? "" : Comment);
                            cmd.Parameters.AddWithValue("@isConflictYes", model.CheckYesConf);
                            cmd.Parameters.AddWithValue("@isConflictNo", model.CheckNoConf);
                            cmd.Parameters.AddWithValue("@CommentConflict", string.IsNullOrEmpty(CommentConflict) == true ? "" : CommentConflict);
                            cmd.Parameters.AddWithValue("@CreatedBy", Session["UserID"].ToString());
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        try
                        {

                            CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
                            DataTable dtDCR;
                            dtDCR = Common.ExecuteQuery("dbo.[sp_GET_REPORT_ANNUAL_EMPLOYEE] '" + model.EmployeeID + "','" + model.EmployeeName + "','" + model.DepartmentID + "'");
                            ReportClass rptH = new ReportClass();
                            rptH.FileName = Server.MapPath("~/Report/Crystal/DownloadDeclaration.rpt");
                            rptH.Load();

                            rptH.SetDataSource(dtDCR);
                            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            File(stream, "application/pdf");
                            //rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, HttpContext.ApplicationInstance.Response, true, "Social Media_" + model.EmployeeID + "_" + model.Years);
                            rptH.ExportToDisk(ExportFormatType.PortableDocFormat, "~/Document/'" + Entity +"'/Social Media_'"+ model.EmployeeID +"'_'"+ DateTime.Now.Year +"'.pdf");

                            rptH.Close();
                            rptH.Dispose();
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorRequest"] = ex.ToString();
                            return View(model);
                        }
                        using (SqlCommand cmd = new SqlCommand("SP_SEND_EMAIL", conn))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EMPID", model.EmployeeID);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }


                    //TempData["SuccessRequest"] = "Submit Request successfully.";
                    //TempData["messageRequest"] = "<script>alert('All my information that I have provided on annual compliance certification according to the actual circumstances.');</script>";
                    return Content("<script language='javascript' type='text/javascript'>alert('I hereby declare that all my information that I have provided on Social Media Declaration according to the actual circumstances.');window.location.href = '" + Session["urlEdit"].ToString() + "' ;</script>");
                    //return RedirectToAction("ListView", "ListView");
                    //return View(model);
                }
                catch (Exception ex)
                {
                    TempData["ErrorRequest"] = ex.ToString();
                    return View(model);
                }
            }
            else if (Submit == "Download")
            {
                try
                {

                    CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
                    DataTable dtDCR;
                    dtDCR = Common.ExecuteQuery("dbo.[sp_GET_REPORT_ANNUAL_EMPLOYEE] '" + model.EmployeeID + "','" + model.EmployeeName + "','" + model.DepartmentID + "'");
                    ReportClass rptH = new ReportClass();
                    rptH.FileName = Server.MapPath("~/Report/Crystal/DownloadDeclaration.rpt");
                    rptH.Load();

                    rptH.SetDataSource(dtDCR);
                    Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    File(stream, "application/pdf");
                    rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, HttpContext.ApplicationInstance.Response, true, "Social Media_" + model.EmployeeID + "_" + model.Years);

                    rptH.Close();
                    rptH.Dispose();
                }
                catch (Exception ex)
                {
                    TempData["ErrorRequest"] = ex.ToString();
                    return View(model);
                }

            }
            return View(model);
        }
        private static byte[] StreamToBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
       
        #region DDL
        private static List<SelectListItem> DDLDept()
        {
            SqlConnection con = Common.GetConnection();
            List<SelectListItem> item = new List<SelectListItem>();
            string query = "exec sp_SEL_DepartmentID";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        item.Add(new SelectListItem
                        {
                            Text = dr["Department"].ToString(),
                            Value = dr["Department"].ToString()
                        });
                    }
                }
                con.Close();
            }
            return item;
        }
        private static List<SelectListItem> DDLEntity()
        {
            SqlConnection con = Common.GetConnection();
            List<SelectListItem> item = new List<SelectListItem>();
            string query = "exec sp_GET_MST_ENTITY";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        item.Add(new SelectListItem
                        {
                            Text = dr["ShortEntity"].ToString(),
                            Value = dr["ShortEntity"].ToString()
                        });
                    }
                }
                con.Close();
            }
            return item;
        }

        private static List<SelectListItem> DDLWorkType()
        {
            SqlConnection con = Common.GetConnection();
            List<SelectListItem> item = new List<SelectListItem>();
            string query = "exec SP_SEL_WORKERTYPE";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        item.Add(new SelectListItem
                        {
                            Text = dr["WORKER_TYPE"].ToString(),
                            Value = dr["WORKER_TYPE"].ToString()
                        });
                    }
                }
                con.Close();
            }
            return item;
        }

        private static List<SelectListItem> DDLAssignName()
        {
            SqlConnection con = Common.GetConnection();
            List<SelectListItem> item = new List<SelectListItem>();
            string query = "exec SP_SEL_ASSIGNNAME";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        item.Add(new SelectListItem
                        {
                            Text = dr["AssignName"].ToString(),
                            Value = dr["AssignName"].ToString()
                        });
                    }
                }
                con.Close();
            }
            return item;
        }
        #endregion
        public DataSet Get_SubMenu(string ParentID)

        {

            SqlCommand com = new SqlCommand("exec [sp_Get_SubMenu] '" + Session["EmployeeNumber"].ToString() + "',@ParentID", Common.GetConnection());

            com.Parameters.AddWithValue("@ParentID", ParentID);

            SqlDataAdapter da = new SqlDataAdapter(com);

            DataSet ds = new DataSet();

            da.Fill(ds);

            return ds;

        }

        public void get_Submenu(string catid)

        {

            DataSet ds = Get_SubMenu(catid);

            List<SubMenu> submenulist = new List<SubMenu>();

            foreach (DataRow dr in ds.Tables[0].Rows)

            {

                submenulist.Add(new SubMenu
                {

                    MenuID = dr["MenuID"].ToString(),

                    MenuName = dr["MenuName"].ToString(),

                    ActionName = dr["ActionName"].ToString(),

                    ControllerName = dr["ControllerName"].ToString()

                });

            }

            Session["submenu"] = submenulist;

        }
    }
}