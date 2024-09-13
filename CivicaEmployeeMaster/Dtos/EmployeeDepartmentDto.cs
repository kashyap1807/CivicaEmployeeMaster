using CivicaEmployeeMaster.Models;

namespace CivicaEmployeeMaster.Dtos
{
    public class EmployeeDepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        //public ICollection<Employee> Employees { get; set; }
    }
}
