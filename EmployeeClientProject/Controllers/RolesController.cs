using EmployeeClientProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml.Linq;

namespace EmployeeClientProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : BaseController
    {
        // GET: ManageRoleController
        
        public ActionResult Index()
        {
            DataTable dataTable = ExecuteProcedure("GetAllRoles");
            List<RolesModel> model = new List<RolesModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                RolesModel item = new RolesModel
                {
                    RoleId = Convert.ToInt32(row["RoleId"]),
                    RoleName = row["RoleName"].ToString(),
                };
                model.Add(item);
            }
            return View(model);
        }
        
        public ActionResult Create() 
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(RolesModel model)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                int i = 0;
                parameters[i] = new SqlParameter("@RoleName", SqlDbType.VarChar);
                parameters[i].Value = model.RoleName;
                parameters[++i] = new SqlParameter("@RoleId", SqlDbType.VarChar);
                parameters[i].Value = 0;
                ExecuteProcedure("UpsertAllRoles", parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageRoleController/Edit/5
        
        public ActionResult Edit(int id,string Name)
        {
            RolesModel model = new RolesModel
            {
                RoleName = Name
            };
            return View(model);
        }

        // POST: ManageRoleController/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, RolesModel model)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                int i = 0;
                parameters[i] = new SqlParameter("@RoleName", SqlDbType.VarChar);
                parameters[i].Value = model.RoleName;
                parameters[++i] = new SqlParameter("@RoleId", SqlDbType.VarChar);
                parameters[i].Value = id;
                ExecuteProcedure("UpsertAllRoles", parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageRoleController/Delete/5
        
        public ActionResult Delete(int id,string Name)
        {
            RolesModel model = new RolesModel
            {
                RoleName = Name
            };
            return View(model);
        }

        // POST: ManageRoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                int i = 0;
                parameters[i] = new SqlParameter("@RoleId", SqlDbType.VarChar);
                parameters[i].Value = id;
                ExecuteProcedure(CommandName: "DeleteAllRoles",parameters:parameters);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
