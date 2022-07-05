using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.IO;
using Annual_Compliance_Declaration.Models;

namespace Annual_Compliance_Declaration.Repository
{
    public class Common
    {
        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConSql"].ConnectionString);
            return con;
        }
        public static DataTable ExecuteQuery(string query)
        {
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader read;
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 36000000;
            con.Open();
            DataTable dt = new DataTable();
            read = cmd.ExecuteReader();
            dt.Load(read);
            con.Close();
            return dt;
        }
        public static void ExecuteNonQuery(string Query)
        {
            try
            {
                SqlConnection con = GetConnection();
                SqlCommand cmd = new SqlCommand(Query, con);
                con.OpenAsync();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string DataTableToJsonWithStringBuilder(DataTable table)
        {
            var jsonString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                jsonString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            jsonString.Append("\"" + table.Columns[j].ColumnName.ToString()
                                              + "\":" + "\""
                                              + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            jsonString.Append("\"" + table.Columns[j].ColumnName.ToString()
                                              + "\":" + "\""
                                              + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        jsonString.Append("}");
                    }
                    else
                    {
                        jsonString.Append("},");
                    }
                }
                jsonString.Append("]");
            }
            return jsonString.ToString();
        }
        public static string DataTableToJsonWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
        public static string DataTableToJsonWithJsonNet(DataTable table)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(table);
            return jsonString;
        }
        public static string DateFormatStringMMddyyyy(DateTime dt)
        {
            //return dt.ToString("yyyy/MM/dd");
            return dt.ToString("MM/dd/yyyy");
        }
        public static string DateFormatStringddMMyyyy(DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy");
        }

        public static DataTable Exc_Global(string sp, object[] parameters)
        {
            string sql = sp + " " + parameters;
            DataTable dt = Common.ExecuteQuery(sql);

            return dt;
        }
        public static string DateDiff(string format, string fromDate, string toDate)
        {
            string result = "";
            DateTime dt = DateTime.Parse(Convert.ToDateTime(fromDate).ToShortDateString());
            DateTime dt1 = DateTime.Parse(Convert.ToDateTime(toDate).ToShortDateString());
            double day = Convert.ToInt32(dt1.Subtract(dt).TotalDays.ToString());
            int a = 0;
            if (format == "m")
            {
                double month = day / (365.25 / 12);
                a = (int)month;
                result = a.ToString();
            }
            else if (format == "y")
            {
                double year = day / (365.25);
                a = (int)year;
                result = a.ToString();
            }
            else if (format == "d")
            {
                result = dt1.Subtract(dt).TotalDays.ToString();
            }
            else
            {
                result = "Format tidak ditemukan.";
            }


            return result;
        }
        public static string SqlDateTime(string value)
        {
            var SqlDateTime = string.IsNullOrEmpty(value) ? "" : Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");

            return SqlDateTime;
        }
        public static string SqlDate(string value)
        {
            var SqlDate = string.IsNullOrEmpty(value) ? "" : Convert.ToDateTime(value).ToString("dd/MM/yyyy");

            return SqlDate;
        }

        #region Encrypt
        public static string Encrypt(string str)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        #endregion

        #region Decrypt 

        public static string Decrypt(string str)
        {
            str = str.Replace(" ", "+");
            string DecryptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
        #endregion

        #region GetRole
        public static string GetRole(string EmployeeID)
        {
            string Result = string.Empty;

            try
            {
                DataTable dt = new DataTable();
                dt = ExecuteQuery("Select Distinct R.ROLE From MST_Privilege P LEFT JOIN MST_ROLE R ON P.RoleID=R.ID Where IDEmployee = '" + EmployeeID + "'");

                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0][0].ToString();
                }
                else
                {
                    Result = "USER";
                }

                return Result;
            }
            catch (Exception ex)
            {
                return Result;
            }
        }
        #endregion

        #region GetEmployeeDetail
        public static string GetEmployeeDetail(string EmployeeID, string param)
        {
            string Result = string.Empty;

            try
            {
                DataTable dt = new DataTable();
                dt = ExecuteQuery("Select * from mst_employee Where [EmployeeID] = '" + EmployeeID + "'");

                if (dt.Rows.Count > 0)
                {
                    if (param == "Entity")
                    {
                        Result = dt.Rows[0]["Entities"].ToString();
                    }
                    else if (param == "JoinDate")
                    {
                        Result = Convert.ToDateTime(dt.Rows[0]["Employee Hire Date"].ToString()).ToString("dd/MM/yyyy");
                    }

                }
                else
                {
                    Result = null;
                }

                return Result;
            }
            catch (Exception ex)
            {
                return Result;
            }
        }
        #endregion

        public static List<Compliance> GetEmployeeData(string ID)
        {
            SqlConnection conn = Common.GetConnection();
            List<Annual_Compliance_Declaration.Models.Compliance> model = new List<Compliance>();
            SqlCommand cmd = new SqlCommand("sp_Get_Employee_Detail", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@EMPLOYEEID", SqlDbType.VarChar).Value = ID;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

            foreach (DataRow dr in dt.Rows)
            {
                model.Add(new Compliance
                {
                    EmployeeName = dr["EmployeeName"].ToString(),
                    EmployeeID = dr["EmployeeID"].ToString(),
                    DepartmentID = dr["Department"].ToString(),
                    EntityID = dr["Entity"].ToString()
                });
            }

            return model;
        }
    }
}