using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeeClientProject.Models
{
    public class EmployeeModel
    {
        public int? Id { get; set; } = 0;
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain alphabetic characters.")]
        public string Name { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public string? SelectedProject { get; set; }
        public List<SelectListItem>? ProjectOptions { get; set; }
    }
}
