using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeeClientProject.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain alphabetic characters.")]
        public string Name { get; set; }
        [RegularExpression(@"^\d{10,12}$", ErrorMessage = "Phone number must be between 10 and 12 digits.")]
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public string SelectedProject { get; set; }
        public List<SelectListItem> ProjectOptions { get; set; }
    }
}
