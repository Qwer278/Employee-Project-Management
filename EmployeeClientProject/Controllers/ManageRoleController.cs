using EmployeeClientProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace EmployeeClientProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageRoleController : BaseController
    {
        // GET: RoleController
        
        public ActionResult Index()
        {
            DataTable dataTable=ExecuteProcedure("GetRoles");
            Dictionary<string, int> TempData = new Dictionary<string, int>();
            List<ManageRolesModel> model = new List<ManageRolesModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                ManageRolesModel item = new ManageRolesModel
                {

                    RoleId = Convert.ToInt32(row["RoleId"]),
                    RoleName = row["RoleName"].ToString(),
                    UserName = row["Username"].ToString()
                };
                model.Add(item);
            }
            return View(model);
        }
        // GET: RoleController/Create
        
        public ActionResult Create()
        {
            DataTable dataTable = ExecuteProcedure("GetUsers");
            DataTable dataTable2 = ExecuteProcedure("GetAllRoles");
            var model = new ManageRolesModel
            {
                ProjectOptionsUser = dataTable.AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Value = row.Field<string>("UserName"),
                    Text = row.Field<string>("UserName")
                })
                .ToList(),
                ProjectOptionsRoles = dataTable2.AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Value = row.Field<string>("RoleName"),
                    Text = row.Field<string>("RoleName")
                })
                .ToList()
            };

            return View(model);
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(ManageRolesModel model)
        {
            try
            {
                SqlParameter[] parameters1 = new SqlParameter[1];
                parameters1[0] = new SqlParameter("@UserName", SqlDbType.VarChar);
                parameters1[0].Value = model.UserName;
                DataTable dataTable1 = ExecuteProcedure(CommandName:"GetParticularUserId",parameters:parameters1);
                SqlParameter[] parameters = new SqlParameter[3];
                int i = 0;
                parameters[i] = new SqlParameter("@RoleId", SqlDbType.Int);
                parameters[i].Value = 0;
                parameters[++i] = new SqlParameter("@RoleName", SqlDbType.VarChar);
                parameters[i].Value = model.RoleName;
                parameters[++i] = new SqlParameter("@UserId", SqlDbType.Int);
                parameters[i].Value = Convert.ToInt32(dataTable1.Rows[0]["UserId"]);
                DataTable dataTable = ExecuteProcedure("UpsertRoles", parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Edit/5
        
        public ActionResult Edit(int id, string Name)
        {
            DataTable dataTable = ExecuteProcedure("GetAllRoles");
            var model = new ManageRolesModel
            {
                ProjectOptionsRoles = dataTable.AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Value = row.Field<string>("RoleName"),
                    Text = row.Field<string>("RoleName")
                })
                .ToList(),
                UserName = Name
            };
            return View(model);
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ManageRolesModel model)
        {
            try
            {
                SqlParameter[] parameters1 = new SqlParameter[1];
                parameters1[0] = new SqlParameter("@UserName", SqlDbType.VarChar);
                parameters1[0].Value = model.UserName;
                DataTable dataTable = ExecuteProcedure(CommandName: "GetParticularUserId", parameters: parameters1);
                SqlParameter[] parameters = new SqlParameter[3];
                int i = 0;
                parameters[i] = new SqlParameter("@RoleName", SqlDbType.VarChar);
                parameters[i].Value = model.RoleName;
                parameters[++i] = new SqlParameter("@RoleId", SqlDbType.VarChar);
                parameters[i].Value = id;
                parameters[++i] = new SqlParameter("@UserId", SqlDbType.VarChar);
                parameters[i].Value = Convert.ToInt32(dataTable.Rows[0]["UserId"]);
                ExecuteProcedure("UpsertRoles", parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
