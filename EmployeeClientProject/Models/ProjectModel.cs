using System.ComponentModel.DataAnnotations;

namespace EmployeeClientProject.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain alphabetic characters.")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime CompletedDate { get; set; }
        public string Description { get; set; }

        public bool IsCompleted { get; set; } 
    }
}
