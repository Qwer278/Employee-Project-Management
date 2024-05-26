using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestfullApi.Controllers
{

    public class BaseController : ControllerBase
    {
        public static string ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionString"];
        public static SqlConnection conn = new SqlConnection(ConnectionString);
        public static SqlCommand cmd = conn.CreateCommand();
        [NonAction]
        public void CloseConnection()
        {
            if (conn.State.ToString() == "Open")
            {
                conn.Close();
            }
        }
        [NonAction]
        public void OpenConnection()
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
        }
        [NonAction]
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
                if (parameters != null)
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
