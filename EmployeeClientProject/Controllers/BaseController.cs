using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeClientProject.Controllers
{
    public class BaseController : Controller
    {
        public static string ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionString"];
        public static SqlConnection conn = new SqlConnection(ConnectionString);
        public static SqlCommand cmd = conn.CreateCommand();

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    if (HttpContext.Session.GetString("user") == "Authenticated")
        //    {
        //        context.Result = this.RedirectToAction("Index", "Project");
        //        return;
        //    }

        //    if (HttpContext.Session.GetString("AUTHORIZATIONSTATUS") == "0")
        //    {   
        //        context.Result = this.RedirectToAction("AccessDenied", "Account");
        //        HttpContext.Session.SetString("AUTHORIZATIONSTATUS", "1");
        //        return;
        //    }

        //    if ((context.HttpContext.Request.Path) != "/" && !context.HttpContext.Request.Path.ToString().Contains("Account"))
        //    {
        //        if (HttpContext.Session.GetString("user") == null)
        //        {
        //            context.Result = this.RedirectToAction("Login", "Account");
        //            return; // Important to return here to stop further execution
        //        }
        //    }
        //    base.OnActionExecuting(context);
        //}

        public void CloseConnection()
        {
            if (conn.State.ToString()=="Open")
            {
                conn.Close();
            }
        }

        public void OpenConnection()
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
        }
        public DataTable ExecuteProcedure(string CommandName, SqlParameter[]? parameters = null, int id = -1)
        {
            DataTable dataTable = new DataTable();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = CommandName;
                cmd.CommandType = CommandType.StoredProcedure;
                if (id != -1)
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                }
                OpenConnection();
                if (parameters!=null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                if (CommandName.Contains("Get"))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }
                
                CloseConnection();
            }
            return dataTable;
        }
    }
}