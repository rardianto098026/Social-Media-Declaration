using Annual_Compliance_Declaration.Models;
using Annual_Compliance_Declaration.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annual_Compliance_Declaration.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            IndexModel model = new IndexModel();
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(string returnUrl, IndexModel model)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                String adPath = ConfigurationManager.AppSettings["LDAPPath"].ToString();
                String domain = ConfigurationManager.AppSettings["LDAPDomain"].ToString();
                LdapAuthentication adAuth = new LdapAuthentication(adPath);
                String LocalHostaddress = HttpContext.Request.UserHostAddress;
                String Ip_Local = LocalHostaddress.Replace(".", "").Replace("::", "").Trim();

                if (true == adAuth.IsAuthenticated(domain, model.Username, model.Password))
                {
                    Session["EmployeeNumber"] = adAuth.GetPropertyUser(domain, model.Username, model.Password);

                    DataTable dtCheck = Common.ExecuteQuery("sp_SEL_NAME_LOGIN'" + Session["EmployeeNumber"].ToString() + "'");
                    if (dtCheck.Rows.Count > 0)
                    {
                        Session["UserID"] = dtCheck.Rows[0]["NAME"].ToString();

                        DataTable dtlogin = Common.ExecuteQuery("sp_INSERT_LOGIN'" + model.Username + "', '" + Ip_Local + "','" + Session["EmployeeNumber"] + "'");
                        if (dtlogin.Rows.Count > 0)
                        {
                            if (dtlogin.Rows[0][0].ToString().ToUpper() == "SUCCESS")
                            { 
                                string con = ConfigurationManager.ConnectionStrings["ConSql"].ConnectionString;
                                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(con);
                                Session["Database"] = builder.InitialCatalog.ToUpper().Substring(9, 3);
                                Session["Entity"] = dtlogin.Rows[0]["ENTITY"].ToString();
                                Session["Role"] = dtlogin.Rows[0]["ROLE"].ToString();
                                //if (dtlogin.Rows[0]["ROLE"].ToString() == "ADMIN" || dtlogin.Rows[0]["ROLE"].ToString() == "SUPER ADMIN")
                                //{
                                //    return RedirectToAction("IndexAdmin", "Home");
                                //}
                                //else
                                //{

                                //if(dtlogin.Rows[0]["SHORTENTITY"].ToString() == "MAGI")
                                //{ 
                                return RedirectToAction("IndexUser", "Home");
                                //}
                                //else
                                //{
                                //    ViewBag.Message = "You Don't Have Access to the Website!";
                                //}

                                //}

                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Username Not Listed in Database!";
                        //ShowMessage("", model.Message);
                    }
                }
                else
                {

                    ViewBag.Message = "Wrong Username or Password!";
                    //ShowMessage("", model.Message);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                //ShowMessage("", model.Message);
                return View(model);
            }
        }
    }
}