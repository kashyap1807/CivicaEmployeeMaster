using System.ComponentModel.DataAnnotations;

namespace CivicaEmployeeMaster.Models
{
    public class EmployeeDepartment
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
