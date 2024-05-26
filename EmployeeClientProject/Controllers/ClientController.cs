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
    public class ClientController : BaseController
    {
        // GET: ClientController
        public ActionResult Index()
        {
            DataTable dataTable = ExecuteProcedure("GetClient");
            List<ClientModel> model = new List<ClientModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                ClientModel item = new ClientModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
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

        // GET: ClientController/Details/5
        public ActionResult Details(int id)
        {
            DataTable dataTable = ExecuteProcedure(CommandName: "GetParticularClient", id: id);
            List<ClientModel> model = new List<ClientModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                ClientModel item = new ClientModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString(),
                    Address = row["Address"].ToString()
                };
                model.Add(item);
            }
            return View(model[0]);
        }

        // GET: ClientController/Create
        public ActionResult Create()
        {
            DataTable dataTable = ExecuteProcedure("GetProject");
            var model = new ClientModel
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

        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientModel model)
        {
            try
                            {
                SqlParameter[] parameters = new SqlParameter[6];
                int i = 0;
                parameters[i] = new SqlParameter("@Id", SqlDbType.VarChar);
                parameters[i].Value = 0;
                parameters[++i] = new SqlParameter("@Name", SqlDbType.VarChar);
                parameters[i].Value = model.Name;
                parameters[++i] = new SqlParameter("@Email", SqlDbType.VarChar);
                parameters[i].Value = model.Email;
                parameters[++i] = new SqlParameter("@Phone", SqlDbType.VarChar);
                parameters[i].Value = model.Phone;
                parameters[++i] = new SqlParameter("@Address", SqlDbType.VarChar);
                parameters[i].Value = model.Address;
                parameters[++i] = new SqlParameter("@SelectedProject", SqlDbType.VarChar);
                parameters[i].Value = model.SelectedProject;
                ExecuteProcedure("UpsertClient", parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientController/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dataTable = ExecuteProcedure(CommandName: "GetParticularClient", id: id);
            List<ClientModel> model = new List<ClientModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                ClientModel item = new ClientModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString(),
                    Address = row["Address"].ToString()
                };
                model.Add(item);
            }
            return View(model[0]);
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ClientModel model)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                int i = 0;
                parameters[i] = new SqlParameter("@Id", SqlDbType.VarChar);
                parameters[i].Value = id;
                parameters[++i] = new SqlParameter("@Name", SqlDbType.VarChar);
                parameters[i].Value = model.Name;
                parameters[++i] = new SqlParameter("@Email", SqlDbType.VarChar);
                parameters[i].Value = model.Email;
                parameters[++i] = new SqlParameter("@Phone", SqlDbType.VarChar);
                parameters[i].Value = model.Phone;
                parameters[++i] = new SqlParameter("@Address", SqlDbType.VarChar);
                parameters[i].Value = model.Address;
                ExecuteProcedure("UpsertClient", parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientController/Delete/5
        public ActionResult Delete(int id)
        {
            DataTable dataTable = ExecuteProcedure(CommandName: "GetParticularClient", id: id);
            List<ClientModel> model = new List<ClientModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                ClientModel item = new ClientModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString(),
                    Address = row["Address"].ToString()
                };
                model.Add(item);
            }
            return View(model[0]);
        }

        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ClientModel model)
        {
            try
            {
                ExecuteProcedure(CommandName: "DeleteClient", id: id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
