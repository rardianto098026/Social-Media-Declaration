using Annual_Compliance_Declaration.Models;
using Annual_Compliance_Declaration.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static Annual_Compliance_Declaration.Models.MenuViewModels;

namespace Annual_Compliance_Declaration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult IndexUser()
        {
            string url = Request.Url.OriginalString;
            Session["url"] = url;

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            DataSet ds = Get_Menu();
            Session["menu"] = ds.Tables[0];
            Session["Role"] = Common.GetRole(Session["EmployeeNumber"].ToString());
            Session["EntityLogin"] = Common.GetEmployeeDetail(Session["EmployeeNumber"].ToString(), "Entity");
            Session["controller"] = "HomeController";

            ViewBag.menu = Session["menu"];
            return View();
           
        }
        public static List<IndexUserModel> ListCompliance(string ID, string Name, string Dept)
        {
            SqlConnection conn = Common.GetConnection();
            List<Annual_Compliance_Declaration.Models.IndexUserModel> model = new List<IndexUserModel>();
            SqlCommand cmd = new SqlCommand("sp_Get_List_Data_Compliance", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
            cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = Dept;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

            foreach (DataRow dr in dt.Rows)
            {
                model.Add(new IndexUserModel
                {
                    No = dr["No"].ToString(),
                    NIK = dr["NIK"].ToString(),
                    Name = dr["Name"].ToString(),
                    Department = dr["Department"].ToString()
                });
            }

            return model;
        }
        public DataSet Get_Menu()

        {

            SqlCommand com = new SqlCommand("exec [sp_Get_Menu_Parent] '" + Session["EmployeeNumber"] + "'", Common.GetConnection());

            SqlDataAdapter da = new SqlDataAdapter(com);

            DataSet ds = new DataSet();

            da.Fill(ds);


            return ds;

        }

        public DataSet Get_SubMenu(string ParentID)

        {

            SqlCommand com = new SqlCommand("exec [sp_Get_SubMenu] '" + Session["EmployeeNumber"] + "',@ParentID", Common.GetConnection());

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
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public ActionResult Edit(string id, IndexUserModel model)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            model.NIK = string.IsNullOrEmpty(model.NIK) == true ? "" : model.NIK;
            model.Name = string.IsNullOrEmpty(model.Name) == true ? "" : model.Name;
            model.Department = string.IsNullOrEmpty(model.Department) == true ? "" : model.Department;

            Session["NIK"] = model.NIK;
            Session["Name"] = model.Name;
            Session["Department"] = model.Department;

           
            DataSet ds = Get_Menu();
            Session["menu"] = ds.Tables[0];
            ViewBag.menu = Session["menu"];

            return View(model);

        }

    }
}