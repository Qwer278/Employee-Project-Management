using EmployeeClientProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestfullApi.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        // GET: api/<CustomerController>
        [HttpGet]
        public List<EmployeeModel> Get()
        {
            DataTable dataTable = ExecuteProcedure("GetEmployee");
            List<EmployeeModel> model = new List<EmployeeModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                EmployeeModel item = new EmployeeModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Department = row["Department"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString(),
                    Address = row["Address"].ToString(),
                    IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                    SelectedProject = row["SelectedProject"].ToString()
                };
                model.Add(item);
            }
            return model;
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public EmployeeModel Get(int id)
        {
            DataTable dataTable = ExecuteProcedure(CommandName: "GetParticularEmployee", id: id);
            List<EmployeeModel> model = new List<EmployeeModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                EmployeeModel item = new EmployeeModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Department = row["Department"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString(),
                    Address = row["Address"].ToString()
                };
                model.Add(item);
            }
            return model[0];
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] EmployeeModel model)
        {
            SqlParameter[] parameters = new SqlParameter[7];
            int i = 0;
            parameters[i] = new SqlParameter("@Id", SqlDbType.VarChar);
            parameters[i].Value = 0;
            parameters[++i] = new SqlParameter("@Name", SqlDbType.VarChar);
            parameters[i].Value = model.Name;
            parameters[++i] = new SqlParameter("@Department", SqlDbType.VarChar);
            parameters[i].Value = model.Department;
            parameters[++i] = new SqlParameter("@Email", SqlDbType.VarChar);
            parameters[i].Value = model.Email;
            parameters[++i] = new SqlParameter("@Phone", SqlDbType.VarChar);
            parameters[i].Value = model.Phone;
            parameters[++i] = new SqlParameter("@Address", SqlDbType.VarChar);
            parameters[i].Value = model.Address;
            parameters[++i] = new SqlParameter("@SelectedProject", SqlDbType.VarChar);
            parameters[i].Value = model.SelectedProject;
            ExecuteProcedure("UpsertEmployee", parameters);
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] EmployeeModel model)
        {
            SqlParameter[] parameters = new SqlParameter[7];
            int i = 0;
            parameters[i] = new SqlParameter("@Id", SqlDbType.VarChar);
            parameters[i].Value = id;
            parameters[++i] = new SqlParameter("@Name", SqlDbType.VarChar);
            parameters[i].Value = model.Name;
            parameters[++i] = new SqlParameter("@Department", SqlDbType.VarChar);
            parameters[i].Value = model.Department;
            parameters[++i] = new SqlParameter("@Email", SqlDbType.VarChar);
            parameters[i].Value = model.Email;
            parameters[++i] = new SqlParameter("@Phone", SqlDbType.VarChar);
            parameters[i].Value = model.Phone;
            parameters[++i] = new SqlParameter("@Address", SqlDbType.VarChar);
            parameters[i].Value = model.Address;
            parameters[++i] = new SqlParameter("@SelectedProject", SqlDbType.VarChar);
            parameters[i].Value = model.SelectedProject;
            ExecuteProcedure("UpsertEmployee", parameters);
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            ExecuteProcedure(CommandName: "DeleteEmployee", id: id);
        }
    }
}
