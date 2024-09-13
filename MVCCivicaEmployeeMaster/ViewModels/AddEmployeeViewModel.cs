using System.ComponentModel.DataAnnotations;

namespace MVCCivicaEmployeeMaster.ViewModels
{
    public class AddEmployeeViewModel
    {
        [Required(ErrorMessage = "Employee email is required")]
        [StringLength(50)]
        public string EmployeeEmail { get; set; }

        [Required(ErrorMessage = "Department ID is required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must only contain alphabetic characters")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must only contain alphabetic characters")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Gender must be a single character")]
        [RegularExpression(@"^[MF]$", ErrorMessage = "Gender must be 'M' or 'F'")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Basic salary is required")]
        [Range(1000, double.MaxValue, ErrorMessage = "Basic salary must be 1000 or more")]
        public double BasicSalary { get; set; }

        [Required(ErrorMessage = "Allowance is required")]
        public double Allowance { get; set; }   

        [Required(ErrorMessage = "Date of joining is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public string DateOfJoining { get; set; }
    }
}
