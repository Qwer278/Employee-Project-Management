using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeClientProject.Models
{
    public class ManageRolesModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<SelectListItem> ProjectOptionsRoles { get; set; }
        public List<SelectListItem> ProjectOptionsUser { get; set; }
    }
}
