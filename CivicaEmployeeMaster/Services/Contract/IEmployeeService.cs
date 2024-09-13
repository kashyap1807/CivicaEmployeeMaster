using CivicaEmployeeMaster.Dtos;
using CivicaEmployeeMaster.Models;

namespace CivicaEmployeeMaster.Services.Contract
{
    public interface IEmployeeService
    {
        ServiceResponse<IEnumerable<EmployeeDto>> GetEmployees();
        ServiceResponse<IEnumerable<EmployeeDto>> GetPaginatedEmployees(int page, int pageSize, string? search, string sortOrder);
        ServiceResponse<int> TotalEmployees(string? search);
        ServiceResponse<EmployeeDto> GetEmployeeById(int id);
        ServiceResponse<string> UpdateEmployee(UpdateEmployeeDto updateEmployeeDto);
        ServiceResponse<string> AddContact(Employee employee);
        ServiceResponse<string> RemoveEmployee(int id);
        ServiceResponse<IEnumerable<SalaryHeadTotal>> GetTotalSalaryByDepartmentAndYear(int year);
        ServiceResponse<IEnumerable<SalaryHeadTotal>> GetTotalSalaryByYear(int year);
        ServiceResponse<IEnumerable<SalaryHeadTotal>> GetTotalSalaryByMonth(int month, int year);
        ServiceResponse<IEnumerable<TotalProfTax>> GetTotalProfTaxByMonth(int month, int year);
        ServiceResponse<IEnumerable<SalaryHeadTotal>> GetTotalSalaryByMonthYearAndId(int employeeId,int month, int year);
        ServiceResponse<IEnumerable<SalaryHeadTotal>> GetTotalSalaryByYearAndId(int employeeId, int year);
    }
}
