using CivicaEmployeeMaster.Dtos;

namespace CivicaEmployeeMaster.Services.Contract
{
    public interface IEmployeeDepartmentService
    {
        ServiceResponse<IEnumerable<EmployeeDepartmentDto>> GetEmployeeDepartment();
    }
}
