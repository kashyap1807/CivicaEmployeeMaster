using CivicaEmployeeMaster.Data.Contract;
using CivicaEmployeeMaster.Dtos;
using CivicaEmployeeMaster.Models;
using CivicaEmployeeMaster.Services.Contract;

namespace CivicaEmployeeMaster.Services.Implementation
{
    public class EmployeeDepartmentService: IEmployeeDepartmentService
    {
        private readonly IEmployeeDepartmentRepository _employeeeDepartmentRepository;
        public EmployeeDepartmentService(IEmployeeDepartmentRepository employeeeDepartmentRepository)
        {
            _employeeeDepartmentRepository = employeeeDepartmentRepository;
        }

        public ServiceResponse<IEnumerable<EmployeeDepartmentDto>> GetEmployeeDepartment()
        {
            var response = new ServiceResponse<IEnumerable<EmployeeDepartmentDto>>();
            var employees = _employeeeDepartmentRepository.GetAllEmployeeDepartment();
            if (employees != null && employees.Any())
            {
                List<EmployeeDepartmentDto> employeeDtos = new List<EmployeeDepartmentDto>();
                foreach (var employee in employees)
                {
                    employeeDtos.Add(new EmployeeDepartmentDto()
                    {
                        DepartmentId = employee.DepartmentId,
                        DepartmentName = employee.DepartmentName,
                       
                    });
                }
                response.Data = employeeDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;
        }
    }
}
