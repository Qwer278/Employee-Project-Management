using EmployeeClientProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace EmployeeClientProject.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class EmployeeController : BaseController
    {
        // GET: EmployeeController
        public ActionResult Index()
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
            return View(model);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
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
            return View(model[0]);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            DataTable dataTable = ExecuteProcedure("GetUsers");
            var model = new EmployeeModel
            {
                ProjectOptions = dataTable.AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Value = row.Field<string>("Name"),
                    Text = row.Field<string>("Name")
                })
                .ToList()
                    };
        return View(model);
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeModel model)
        {
            try
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
                ExecuteProcedure("UpsertEmployee",parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dataTable=ExecuteProcedure(CommandName: "GetParticularEmployee",id: id);
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
            return View(model[0]);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmployeeModel model)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[6];
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
                ExecuteProcedure("UpsertEmployee", parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
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
            return View(model[0]);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                ExecuteProcedure(CommandName: "DeleteEmployee",id: id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
