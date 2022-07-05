using Annual_Compliance_Declaration.Models;
using Annual_Compliance_Declaration.Repository;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Annual_Compliance_Declaration.Models.MenuViewModels;

namespace Annual_Compliance_Declaration.Controllers
{
    public class ListViewController : Controller
    {
        // GET: ListView
        public ActionResult ListView(ListViewModels model, string submit)
        {
           
            string url = Request.Url.OriginalString;
            Session["url"] = url;

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (submit == "Search" || string.IsNullOrEmpty(submit) == true)
            {
                
                ViewBag.menu = Session["menu"];
                Session["Role"] = Common.GetRole(Session["EmployeeNumber"].ToString());
                Session["EntityLogin"] = Common.GetEmployeeDetail(Session["EmployeeNumber"].ToString(), "Entity");
                Session["controller"] = "ListViewController";

                model.NIK = string.IsNullOrEmpty(model.NIK) == true ? "" : model.NIK;
                model.Name = string.IsNullOrEmpty(model.Name) == true ? "" : model.Name;
                                                                                                                                                                                                                                                                                                                                                                                                                                                        model.Department = string.IsNullOrEmpty(model.Department) == true ? "" : model.Department;
                model.Role = string.IsNullOrEmpty(model.Role) == true ? "" : model.Role;
                model.Entity = string.IsNullOrEmpty(model.Entity) == true ? "" : model.Entity;
                model.EmployeeID = string.IsNullOrEmpty(model.EmployeeID) == true ? "" : model.EmployeeID;
                model.EmployeeName = string.IsNullOrEmpty(model.EmployeeName) == true ? "" : model.EmployeeName;
            }
            else if (submit == "Download")
            {
                ViewBag.menu = Session["menu"];
                Session["Role"] = Common.GetRole(Session["EmployeeNumber"].ToString());
                Session["EntityLogin"] = Common.GetEmployeeDetail(Session["EmployeeNumber"].ToString(), "Entity");
                Session["controller"] = "ListViewController";

                model.NIK = string.IsNullOrEmpty(model.NIK) == true ? "" : model.NIK;
                model.Name = string.IsNullOrEmpty(model.Name) == true ? "" : model.Name;
                model.Department = string.IsNullOrEmpty(model.Department) == true ? "" : model.Department;
                model.Role = string.IsNullOrEmpty(model.Role) == true ? "" : model.Role;
                model.Entity = string.IsNullOrEmpty(model.Entity) == true ? "" : model.Entity;
                model.EmployeeID = string.IsNullOrEmpty(model.EmployeeID) == true ? "" : model.EmployeeID;
                model.EmployeeName = string.IsNullOrEmpty(model.EmployeeName) == true ? "" : model.EmployeeName;

                string query = string.Empty;

                query = "dbo.[sp_Get_List_Data_Compliance] '" + model.EmployeeID + "','" + model.EmployeeName + "','" + model.Department + "','" + model.Entity + "' ";
                  
                DataTable dt = Common.ExecuteQuery(query);

                DataTable dtReport = new DataTable();

                dtReport.Columns.Add("No");
                dtReport.Columns.Add("NIK");
                dtReport.Columns.Add("Name");
                dtReport.Columns.Add("Department");
                dtReport.Columns.Add("Entity");
                dtReport.Columns.Add("Date of Declaration");
                dtReport.Columns.Add("AXA Group Standards and other Company Regulations");
                dtReport.Columns.Add("Reason for Not Comply with AXA Group Standards and other Company Regulations");
                dtReport.Columns.Add("Conflict of Interest Declaration");
                dtReport.Columns.Add("Reason for Conflict of Duty");

                foreach (DataRow item in dt.Rows)
                {
                    var row = dtReport.NewRow();

                    row["No"] = item["No"].ToString();
                    row["NIK"] = item["NIK"].ToString();
                    row["Name"] = item["Name"].ToString();
                    row["Department"] = item["Department"].ToString();
                    row["ENTITY"] = (item["Entity"].ToString());
                    row["Date of Declaration"] = item["Date of Declaration"].ToString();
                    row["AXA Group Standards and other Company Regulations"] = item["Compliance & Ethics Manual for Employees"].ToString();
                    row["Reason for Not Comply with AXA Group Standards and other Company Regulations"] = (item["Reason for Not Comply with Compliance & Ethics Manual"].ToString());
                    row["Conflict of Interest Declaration"] = item["Conflict of Interest Declaration"].ToString();
                    row["Reason for Conflict of Duty"] = (item["Reason for Conflict of Duty"].ToString());

                    dtReport.Rows.Add(row);
                }

                if (dt.Rows.Count > 0)
                {
                    var grid = new GridView();
                    grid.DataSource = dtReport;
                    grid.DataBind();

                    Response.ClearContent();
                    Response.Buffer = true;
                    string filename = "Report Annual Compliance";
                    Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
                    Response.ContentType = "application/ms-excel";

                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);

                    foreach (GridViewRow r in grid.Rows)
                    {
                        if ((r.RowType == DataControlRowType.DataRow))
                        {
                            for (int columnIndex = 0; (columnIndex
                                        <= (r.Cells.Count - 1)); columnIndex++)
                            {
                                r.Cells[columnIndex].Attributes.Add("class", "text");
                            }

                        }

                    }

                    grid.RenderControl(htw);
                    string style = "<style> .text { mso-number-format:\\@; } </style> ";
                    Response.Write(style);

                    Response.Write(sw.ToString());
                    Response.End();
                    ModelState.Clear();

                }
            }

            var modelEmployee = ListCompliance(model.EmployeeID, model.EmployeeName, model.Department,model.Entity);
            return View(modelEmployee);

        }

        public ActionResult ListView_DownloadData(ListViewModels model, string submit)
        {

            string url = Request.Url.OriginalString;
            Session["url"] = url;

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (submit == "Search" || string.IsNullOrEmpty(submit) == true)
            {

                ViewBag.menu = Session["menu"];
                Session["Role"] = Common.GetRole(Session["EmployeeNumber"].ToString());
                Session["EntityLogin"] = Common.GetEmployeeDetail(Session["EmployeeNumber"].ToString(), "Entity");
                Session["controller"] = "ListViewController";

                model.NIK = string.IsNullOrEmpty(model.NIK) == true ? "" : model.NIK;
                model.Name = string.IsNullOrEmpty(model.Name) == true ? "" : model.Name;
                model.Department = string.IsNullOrEmpty(model.Department) == true ? "" : model.Department;
                model.Role = string.IsNullOrEmpty(model.Role) == true ? "" : model.Role;
                model.Entity = string.IsNullOrEmpty(model.Entity) == true ? "" : model.Entity;
                model.EmployeeID = string.IsNullOrEmpty(model.EmployeeID) == true ? "" : model.EmployeeID;
                model.EmployeeName = string.IsNullOrEmpty(model.EmployeeName) == true ? "" : model.EmployeeName;
            }
            else if (submit == "Download")
            {
                ViewBag.menu = Session["menu"];
                Session["Role"] = Common.GetRole(Session["EmployeeNumber"].ToString());
                Session["EntityLogin"] = Common.GetEmployeeDetail(Session["EmployeeNumber"].ToString(), "Entity");
                Session["controller"] = "ListViewController";

                model.NIK = string.IsNullOrEmpty(model.NIK) == true ? "" : model.NIK;
                model.Name = string.IsNullOrEmpty(model.Name) == true ? "" : model.Name;
                model.Department = string.IsNullOrEmpty(model.Department) == true ? "" : model.Department;
                model.Role = string.IsNullOrEmpty(model.Role) == true ? "" : model.Role;
                model.Entity = string.IsNullOrEmpty(model.Entity) == true ? "" : model.Entity;
                model.EmployeeID = string.IsNullOrEmpty(model.EmployeeID) == true ? "" : model.EmployeeID;
                model.EmployeeName = string.IsNullOrEmpty(model.EmployeeName) == true ? "" : model.EmployeeName;

                string query = string.Empty;

                query = "dbo.[sp_Get_List_Data_Compliance_Download_For_Not_Input] '" + model.EmployeeID + "','" + model.EmployeeName + "','" + model.Department + "','" + model.Entity + "' ";

                DataTable dt = Common.ExecuteQuery(query);

                DataTable dtReport = new DataTable();

                dtReport.Columns.Add("No");
                dtReport.Columns.Add("NIK");
                dtReport.Columns.Add("Name");
                dtReport.Columns.Add("Department");
                dtReport.Columns.Add("Entity");
               
                foreach (DataRow item in dt.Rows)
                {
                    var row = dtReport.NewRow();

                    row["No"] = item["No"].ToString();
                    row["NIK"] = item["NIK"].ToString();
                    row["Name"] = item["Name"].ToString();
                    row["Department"] = item["Department"].ToString();
                    row["ENTITY"] = (item["Entity"].ToString());
                    
                    dtReport.Rows.Add(row);
                }

                if (dt.Rows.Count > 0)
                {
                    var grid = new GridView();
                    grid.DataSource = dtReport;
                    grid.DataBind();

                    Response.ClearContent();
                    Response.Buffer = true;
                    string filename = "Report Annual Compliance Not Input";
                    Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
                    Response.ContentType = "application/ms-excel";

                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);

                    foreach (GridViewRow r in grid.Rows)
                    {
                        if ((r.RowType == DataControlRowType.DataRow))
                        {
                            for (int columnIndex = 0; (columnIndex
                                        <= (r.Cells.Count - 1)); columnIndex++)
                            {
                                r.Cells[columnIndex].Attributes.Add("class", "text");
                            }

                        }

                    }

                    grid.RenderControl(htw);
                    string style = "<style> .text { mso-number-format:\\@; } </style> ";
                    Response.Write(style);

                    Response.Write(sw.ToString());
                    Response.End();
                    ModelState.Clear();

                }
            }

            var modelEmployee = ListCompliance_NotInput(model.EmployeeID, model.EmployeeName, model.Department, model.Entity);
            return View(modelEmployee);

        }

        public ActionResult ListEmployee(ListViewModels model, string submit)
        {

            string url = Request.Url.OriginalString;
            Session["url"] = url;

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (submit == "Search" || string.IsNullOrEmpty(submit) == true)
            {

                ViewBag.menu = Session["menu"];
                Session["Role"] = Common.GetRole(Session["EmployeeNumber"].ToString());
                Session["EntityLogin"] = Common.GetEmployeeDetail(Session["EmployeeNumber"].ToString(), "Entity");
                Session["controller"] = "ListViewController";

                model.NIK = string.IsNullOrEmpty(model.NIK) == true ? "" : model.NIK;
                model.Name = string.IsNullOrEmpty(model.Name) == true ? "" : model.Name;
                model.Department = string.IsNullOrEmpty(model.Department) == true ? "" : model.Department;
                model.Role = string.IsNullOrEmpty(model.Role) == true ? "" : model.Role;
                model.Entity = string.IsNullOrEmpty(model.Entity) == true ? "" : model.Entity;
                model.EmployeeID = string.IsNullOrEmpty(model.EmployeeID) == true ? "" : model.EmployeeID;
                model.EmployeeName = string.IsNullOrEmpty(model.EmployeeName) == true ? "" : model.EmployeeName;
            }
            else if (submit == "Download")
            {
                ViewBag.menu = Session["menu"];
                Session["Role"] = Common.GetRole(Session["EmployeeNumber"].ToString());
                Session["EntityLogin"] = Common.GetEmployeeDetail(Session["EmployeeNumber"].ToString(), "Entity");
                Session["controller"] = "ListViewController";

                model.NIK = string.IsNullOrEmpty(model.NIK) == true ? "" : model.NIK;
                model.Name = string.IsNullOrEmpty(model.Name) == true ? "" : model.Name;
                model.Department = string.IsNullOrEmpty(model.Department) == true ? "" : model.Department;
                model.Role = string.IsNullOrEmpty(model.Role) == true ? "" : model.Role;
                model.Entity = string.IsNullOrEmpty(model.Entity) == true ? "" : model.Entity;
                model.EmployeeID = string.IsNullOrEmpty(model.EmployeeID) == true ? "" : model.EmployeeID;
                model.EmployeeName = string.IsNullOrEmpty(model.EmployeeName) == true ? "" : model.EmployeeName;

                string query = string.Empty;

                query = "dbo.[sp_Get_List_Data_Compliance_Download_For_Not_Input] '" + model.EmployeeID + "','" + model.EmployeeName + "','" + model.Department + "','" + model.Entity + "' ";

                DataTable dt = Common.ExecuteQuery(query);

                DataTable dtReport = new DataTable();

                dtReport.Columns.Add("No");
                dtReport.Columns.Add("NIK");
                dtReport.Columns.Add("Name");
                dtReport.Columns.Add("Department");
                dtReport.Columns.Add("Entity");

                foreach (DataRow item in dt.Rows)
                {
                    var row = dtReport.NewRow();

                    row["No"] = item["No"].ToString();
                    row["NIK"] = item["NIK"].ToString();
                    row["Name"] = item["Name"].ToString();
                    row["Department"] = item["Department"].ToString();
                    row["ENTITY"] = (item["Entity"].ToString());

                    dtReport.Rows.Add(row);
                }

                if (dt.Rows.Count > 0)
                {
                    var grid = new GridView();
                    grid.DataSource = dtReport;
                    grid.DataBind();

                    Response.ClearContent();
                    Response.Buffer = true;
                    string filename = "Report Annual Compliance Not Input";
                    Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
                    Response.ContentType = "application/ms-excel";

                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);

                    foreach (GridViewRow r in grid.Rows)
                    {
                        if ((r.RowType == DataControlRowType.DataRow))
                        {
                            for (int columnIndex = 0; (columnIndex
                                        <= (r.Cells.Count - 1)); columnIndex++)
                            {
                                r.Cells[columnIndex].Attributes.Add("class", "text");
                            }

                        }

                    }

                    grid.RenderControl(htw);
                    string style = "<style> .text { mso-number-format:\\@; } </style> ";
                    Response.Write(style);

                    Response.Write(sw.ToString());
                    Response.End();
                    ModelState.Clear();

                }
            }

            var modelEmployee = ListAllEmployee(model.EmployeeID, model.EmployeeName, model.Department, model.Entity);
            return View(modelEmployee);

        }

        public static List<ListViewModels> ListAllEmployee(string ID, string Name, string Dept, string Entity)
        {
            SqlConnection conn = Common.GetConnection();
            List<Annual_Compliance_Declaration.Models.ListViewModels> model = new List<ListViewModels>();
            SqlCommand cmd = new SqlCommand("sp_Get_List_All_Employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
            cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = Dept;
            cmd.Parameters.Add("@Entity", SqlDbType.VarChar).Value = Entity;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

            foreach (DataRow dr in dt.Rows)
            {
                model.Add(new ListViewModels
                {
                    No = dr["No"].ToString(),
                    NIK = dr["NIK"].ToString(),
                    Name = dr["Name"].ToString(),
                    Department = dr["Department"].ToString(),
                    Entity = dr["ENTITY"].ToString()

                });
            }

            return model;
        }

        public static List<ListViewModels> ListCompliance_NotInput(string ID, string Name, string Dept, string Entity)
        {
            SqlConnection conn = Common.GetConnection();
            List<Annual_Compliance_Declaration.Models.ListViewModels> model = new List<ListViewModels>();
            SqlCommand cmd = new SqlCommand("sp_Get_List_Data_Compliance_Download_For_Not_Input", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
            cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = Dept;
            cmd.Parameters.Add("@Entity", SqlDbType.VarChar).Value = Entity;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

            foreach (DataRow dr in dt.Rows)
            {
                model.Add(new ListViewModels
                {
                    No = dr["No"].ToString(),
                    NIK = dr["NIK"].ToString(),
                    Name = dr["Name"].ToString(),
                    Department = dr["Department"].ToString(),
                    Entity = dr["ENTITY"].ToString()

                });
            }

            return model;
        }

        public static List<ListViewModels> ListCompliance(string ID, string Name, string Dept,string Entity)
        {
            SqlConnection conn = Common.GetConnection();
            List<Annual_Compliance_Declaration.Models.ListViewModels> model = new List<ListViewModels>();
            SqlCommand cmd = new SqlCommand("sp_Get_List_Data_Compliance", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
            cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = Dept;
            cmd.Parameters.Add("@Entity", SqlDbType.VarChar).Value = Entity;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

            foreach (DataRow dr in dt.Rows)
            {
                model.Add(new ListViewModels
                {
                    No = dr["No"].ToString(),
                    NIK = dr["NIK"].ToString(),
                    Name = dr["Name"].ToString(),
                    Department = dr["Department"].ToString(),
                    Entity = dr["ENTITY"].ToString(),
                    isBehave = dr["Compliance & Ethics Manual for Employees"].ToString(),
                    CommentBehave = dr["Reason for Not Comply with Compliance & Ethics Manual"].ToString(),
                    isConflict = dr["Conflict of Interest Declaration"].ToString(),
                    CommentConflict = dr["Reason for Conflict of Duty"].ToString(),
                    DateDeclare = dr["Date of Declaration"].ToString()
                    
            });
            }

            return model;
        }
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
        public ActionResult Download(string NIK, string Name, string Department)
        {
            try
            {
                CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
                DataTable dtDCR;
                dtDCR = Common.ExecuteQuery("dbo.[sp_GET_REPORT_ANNUAL_EMPLOYEE] '" + NIK + "','" + Name + "','" + Department + "'");
                ReportClass rptH = new ReportClass();
                rptH.FileName = Server.MapPath("~/Report/Crystal/DownloadDeclaration.rpt");
                rptH.Load();

                rptH.SetDataSource(dtDCR);
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                File(stream, "application/pdf");
                rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, HttpContext.ApplicationInstance.Response, true, "Social Media Declaration_" + NIK + "_" + Convert.ToString(DateTime.Now.Year));
                rptH.Close();
                rptH.Dispose();
                return RedirectToAction("ListView");
            }
            catch (Exception)
            {
                TempData["message"] = "<script>alert('Download failed');</script>";
                return RedirectToAction("ListView");
            }
        }
        public ActionResult DeleteUser(string id)
        {
            string url = Request.Url.OriginalString;
            Session["url"] = url;


            try
            {
                Common.ExecuteNonQuery("Delete From TRN_ANNUAL_COMPLIANCE where [EmployeeID]='" + id + "'");
                TempData["message"] = "<script>alert('Delete succesfully');</script>";
                return RedirectToAction("ListView");
            }
            catch (Exception)
            {
                TempData["message"] = "<script>alert('Delete unsuccesfully');</script>";
                return RedirectToAction("ListView");
            }
        }
       // [HttpGet]
        public ActionResult DDL()
        {
            ListViewModels model = new ListViewModels();
            DataTable dt;
            dt = Common.ExecuteQuery("dbo.[sp_SEL_DepartmentID]");
            if (dt.Rows.Count > 0)
            {
                model.DepartmentID = DDLDept();
            
            }
            return Json(new SelectList(model.DepartmentID, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
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
    }

}