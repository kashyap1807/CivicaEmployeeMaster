using CivicaEmployeeMaster.Models;

namespace CivicaEmployeeMaster.Data.Contract
{
    public interface IEmployeeDepartmentRepository
    {
        IEnumerable<EmployeeDepartment> GetAllEmployeeDepartment();
    }
}
