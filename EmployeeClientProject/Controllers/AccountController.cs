using EmployeeClientProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EmployeeClientProject.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(AccountModel model)
        {

            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                int i = 0;
                parameters[i] = new SqlParameter("@UserId", SqlDbType.VarChar);
                parameters[i].Value = 0;
                parameters[++i] = new SqlParameter("@UserName", SqlDbType.VarChar);
                parameters[i].Value = model.UserName;
                parameters[++i] = new SqlParameter("@Password", SqlDbType.VarChar);
                parameters[i].Value = model.Password;
                parameters[++i] = new SqlParameter("@Email", SqlDbType.VarChar);
                parameters[i].Value = model.Email;
                parameters[++i] = new SqlParameter("@RoleName", SqlDbType.VarChar);
                parameters[i].Value = "Manager";
                ExecuteProcedure("UpsertAccount", parameters);
                HttpContext.Session.SetString("user", "Authenticated");
                HttpContext.Session.SetString("RoleName", "Manager");
                return RedirectToAction("Index", "Project");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Login()
        {
            if (HttpContext.Session.GetString("user") == "Authenticated")
            {
                return RedirectToAction("Index", "Project");
            }
            else
            {
                return View();
            }
        }

        //SESSION

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountModel model)
        {
            ViewData["Message"] = "Username or Password is Incorrect";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Username",SqlDbType.VarChar);
            parameters[0].Value = model.UserName;
            DataTable dataTable = ExecuteProcedure("GetAccountPassword", parameters);
            if (model.Password == dataTable.Rows[0]["Password"].ToString())
            {
                HttpContext.Session.SetString("user", "Authenticated");
                HttpContext.Session.SetString("RoleName", dataTable.Rows[0]["RoleName"].ToString());
                

                // Create a list of claims for the user
                ClaimsIdentity Identity = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.Name, "model.UserName"),  // Example claim for user's name
                    new Claim(ClaimTypes.Role, dataTable.Rows[0]["RoleName"].ToString()),  // Example claim for user's role
                },CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(Identity);
                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);


                return RedirectToAction("Index", "Project");
            }
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            //SignOutAsync is Extension method for SignOut
            HttpContext.Session.SetString("user", "UnAuthenticated");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            return RedirectToAction("Login","Account");
        }
    }
}
