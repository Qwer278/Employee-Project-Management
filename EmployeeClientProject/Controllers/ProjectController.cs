using EmployeeClientProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeClientProject.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ProjectController : BaseController
    {
        // GET: ProjectController
        public ActionResult Index()
        {
            DataTable dataTable = ExecuteProcedure("GetProject");
            List<ProjectModel> model = new List<ProjectModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                ProjectModel item = new ProjectModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    CompletedDate = Convert.ToDateTime(row["CompletedDate"]),
                    StartDate = Convert.ToDateTime(row["StartDate"]),
                    Description = row["Description"].ToString(),
                    IsCompleted = Convert.ToBoolean(row["IsCompleted"])
                };
                model.Add(item);
            }
            return View(model);
        }

        // GET: ProjectController/Details/5
        public ActionResult Details(int id)
        {
            DataTable dataTable = ExecuteProcedure(CommandName: "GetParticularProject", id: id);
            List<ProjectModel> model = new List<ProjectModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                ProjectModel item = new ProjectModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    CompletedDate = Convert.ToDateTime(row["CompletedDate"]),
                    StartDate = Convert.ToDateTime(row["StartDate"]),
                    Description = row["Description"].ToString()
                };
                model.Add(item);
            }
            return View(model[0]);
        }

        // GET: ProjectController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectModel model)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                int i = 0;
                parameters[i] = new SqlParameter("@Id", SqlDbType.VarChar);
                parameters[i].Value = 0;
                parameters[++i] = new SqlParameter("@Name", SqlDbType.VarChar);
                parameters[i].Value = model.Name;
                parameters[++i] = new SqlParameter("@StartDate", SqlDbType.VarChar);
                parameters[i].Value = model.StartDate;
                parameters[++i] = new SqlParameter("@CompletedDate", SqlDbType.VarChar);
                parameters[i].Value = model.CompletedDate;
                parameters[++i] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[i].Value = model.Description;
                ExecuteProcedure("UpsertProject", parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectController/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dataTable = ExecuteProcedure(CommandName: "GetParticularProject", id: id);
            List<ProjectModel> model = new List<ProjectModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                ProjectModel item = new ProjectModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    CompletedDate = Convert.ToDateTime(row["CompletedDate"]),
                    StartDate = Convert.ToDateTime(row["StartDate"]),
                    Description = row["Description"].ToString()
                };
                model.Add(item);
            }
            return View(model[0]);
        }

        // POST: ProjectController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProjectModel model)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                int i = 0;
                parameters[i] = new SqlParameter("@Id", SqlDbType.VarChar);
                parameters[i].Value = id;
                parameters[++i] = new SqlParameter("@Name", SqlDbType.VarChar);
                parameters[i].Value = model.Name;
                parameters[++i] = new SqlParameter("@StartDate", SqlDbType.VarChar);
                parameters[i].Value = model.StartDate;
                parameters[++i] = new SqlParameter("@CompletedDate", SqlDbType.VarChar);
                parameters[i].Value = model.CompletedDate;
                parameters[++i] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[i].Value = model.Description;
                ExecuteProcedure("UpsertProject", parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectController/Delete/5
        public ActionResult Complete(int id)
        {
            DataTable dataTable = ExecuteProcedure(CommandName: "GetParticularProject", id: id);
            List<ProjectModel> model = new List<ProjectModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                ProjectModel item = new ProjectModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    StartDate = Convert.ToDateTime(row["StartDate"]),
                    Description = row["Description"].ToString()
                };
                model.Add(item);
            }
            return View(model[0]);
        }

        // POST: ProjectController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complete(int id, ProjectModel model)
        {
            try
            {
                DateTime currentDate = DateTime.Now.Date;
                SqlParameter[] parameters = new SqlParameter[2];
                int i = 0;
                parameters[i] = new SqlParameter("@Id", SqlDbType.VarChar);
                parameters[i].Value = id;
                parameters[++i] = new SqlParameter("@CompletedDate", SqlDbType.VarChar);
                parameters[i].Value = currentDate;
                ExecuteProcedure(CommandName: "CompleteProject", parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
