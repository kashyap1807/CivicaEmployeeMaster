using CivicaEmployeeMaster.Data.Contract;
using CivicaEmployeeMaster.Data.Implementation;
using CivicaEmployeeMaster.Dtos;
using CivicaEmployeeMaster.Models;
using CivicaEmployeeMaster.Services.Contract;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CivicaEmployeeMaster.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeeRepository;
        public EmployeeService(IEmployeeRepository employeeeRepository)
        {
            _employeeeRepository = employeeeRepository;
        }

        public ServiceResponse<IEnumerable<EmployeeDto>> GetEmployees()
        {
            var response = new ServiceResponse<IEnumerable<EmployeeDto>>();
            var employees = _employeeeRepository.GetAllEmployee();
            if (employees != null && employees.Any())
            {
                List<EmployeeDto> employeeDtos = new List<EmployeeDto>();
                foreach (var employee in employees)
                {
                    employeeDtos.Add(new EmployeeDto()
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        EmployeeEmail = employee.EmployeeEmail,
                        DepartmentId = employee.DepartmentId,
                        BasicSalary = employee.BasicSalary,
                        HRA = employee.HRA,
                        Gender = employee.Gender,
                        Allowance = employee.Allowance,
                        GrossSalary = employee.GrossSalary,
                        PfDeduction = employee.PfDeduction,
                        ProfTax = employee.ProfTax,
                        GrossDeductions = employee.GrossDeductions,
                        TotalSalary = employee.TotalSalary,
                        DateOfJoining = employee.DateOfJoining,
                        EmployeeDepartment = new EmployeeDepartment
                        {
                            DepartmentId = employee.EmployeeDepartment.DepartmentId,
                            DepartmentName = employee.EmployeeDepartment.DepartmentName,
                        },

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
        public ServiceResponse<IEnumerable<EmployeeDto>> GetPaginatedEmployees(int page, int pageSize, string? search, string sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<EmployeeDto>>();
            if (!string.IsNullOrEmpty(search) && search.Length < 3)
            {
                response.Success = false;
                response.Message = "Search query must be at least 3 characters long.";
                return response;
            }
            var employees = _employeeeRepository.GetPaginatedEmployees(page, pageSize, search, sortOrder);
            if (employees != null && employees.Any())
            {
                List<EmployeeDto> employeeDtos = new List<EmployeeDto>();
                foreach (var employee in employees.ToList())
                {
                    employeeDtos.Add(new EmployeeDto()
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        EmployeeEmail = employee.EmployeeEmail,
                        DepartmentId = employee.DepartmentId,
                        BasicSalary = employee.BasicSalary,
                        HRA = employee.HRA,
                        Gender = employee.Gender,
                        Allowance = employee.Allowance,
                        GrossSalary = employee.GrossSalary,
                        PfDeduction = employee.PfDeduction,
                        ProfTax = employee.ProfTax,
                        GrossDeductions = employee.GrossDeductions,
                        TotalSalary = employee.TotalSalary,
                        DateOfJoining = employee.DateOfJoining,
                        EmployeeDepartment = new EmployeeDepartment
                        {
                            DepartmentId = employee.EmployeeDepartment.DepartmentId,
                            DepartmentName = employee.EmployeeDepartment.DepartmentName,
                        },
                    });
                }
                response.Data = employeeDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
        public ServiceResponse<int> TotalEmployees(string? search)
        {

            var response = new ServiceResponse<int>();
            int totalPositions = _employeeeRepository.TotalEmployees(search);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Paginated";
            return response;
        }
        public ServiceResponse<EmployeeDto> GetEmployeeById(int id)
        {
            var response = new ServiceResponse<EmployeeDto>();

            var employee = _employeeeRepository.GetEmployeeById(id);
            if (employee != null)
            {
                var employeeDto = new EmployeeDto()
                {
                    Id = employee.Id,
                    EmployeeEmail = employee.EmployeeEmail,
                    DepartmentId = employee.DepartmentId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Gender = employee.Gender,
                    BasicSalary = employee.BasicSalary,
                    HRA = employee.HRA,
                    Allowance = employee.Allowance,
                    GrossSalary = employee.GrossSalary,
                    PfDeduction = employee.PfDeduction,
                    ProfTax = employee.ProfTax,
                    GrossDeductions = employee.GrossDeductions,
                    TotalSalary = employee.TotalSalary,
                    DateOfJoining = employee.DateOfJoining,
                    EmployeeDepartment = new EmployeeDepartment()
                    {
                        DepartmentId = employee.EmployeeDepartment.DepartmentId,
                        DepartmentName = employee.EmployeeDepartment.DepartmentName
                    }

                };

                response.Data = employeeDto;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;

        }
        public ServiceResponse<string> UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
        {
            var response = new ServiceResponse<string>();

            if (updateEmployeeDto == null)
            {
                response.Success = false;
                response.Message = "Something went wrong. Please try after sometime.";
                return response;
            }
            
            var employee = _employeeeRepository.GetEmployeeById(updateEmployeeDto.Id);
            if (employee == null)
            {
                response.Success = false;
                response.Message = "Something went wrong. Please try after sometime.";
                return response;
            }

            if (updateEmployeeDto.DepartmentId < 1 || updateEmployeeDto.DepartmentId > 4 )
            {
                response.Success = false;
                response.Message = "Please enter valid department.";
                return response ;
            }
            if (_employeeeRepository.EmployeesExists(updateEmployeeDto.Id, updateEmployeeDto.EmployeeEmail))
            {
                response.Success = false;
                response.Message = "Employee already exists.";
                return response;
            }
            if (updateEmployeeDto.DateOfJoining > DateTime.Now)
            {
                response.Success = false;
                response.Message = "Join date can't be a future date.";
                return response;
            }
            if (updateEmployeeDto.BasicSalary < 1000)
            {
                response.Success = false;
                response.Message = "Basic Salary must be greater than or equal to 1000.";
                return response;
            }
            if (updateEmployeeDto.BasicSalary < updateEmployeeDto.Allowance)
            {
                response.Success = false;
                response.Message = "BasicSalary should be always higher than allowance.";
                return response;

            }
            if (!ValidateEmail(updateEmployeeDto.EmployeeEmail))
            {
                response.Success = false;
                response.Message = "Email should be in xyz@abc.com format only!";
                return response;
            }

            

            if (employee != null)
            {

                if (employee.BasicSalary <= 10000)
                {
                    employee.ProfTax = 100;
                }
                else if (employee.BasicSalary <= 20000)
                {
                    employee.ProfTax = 200;
                }
                else if (employee.BasicSalary <= 30000)
                {
                    employee.ProfTax = 300;
                }
                else if (employee.BasicSalary <= 40000)
                {
                    employee.ProfTax = 400;
                }
                else if (employee.BasicSalary <= 50000)
                {
                    employee.ProfTax = 500;
                }
                else
                {
                    employee.ProfTax = 1000;
                }
                employee.EmployeeEmail = updateEmployeeDto.EmployeeEmail;
                employee.DepartmentId = updateEmployeeDto.DepartmentId;
                employee.FirstName = updateEmployeeDto.FirstName;
                employee.LastName = updateEmployeeDto.LastName;
                employee.Gender = updateEmployeeDto.Gender;
                employee.BasicSalary = updateEmployeeDto.BasicSalary;
                employee.Allowance = updateEmployeeDto.Allowance;
                employee.HRA = updateEmployeeDto.BasicSalary * 0.10m;
                employee.PfDeduction = updateEmployeeDto.BasicSalary * 0.075m;
                employee.GrossSalary = updateEmployeeDto.BasicSalary + employee.HRA + updateEmployeeDto.Allowance;
                employee.GrossDeductions = employee.PfDeduction + employee.ProfTax;
                employee.TotalSalary = employee.GrossSalary - employee.GrossDeductions;
                employee.DateOfJoining = updateEmployeeDto.DateOfJoining;
               


                var result = _employeeeRepository.UpdateEmployee(employee);
                response.Success = true;
                response.Message = result ? "Employee updated successfully." : "Something went wrong. Please try after sometime.";
            }
            //else
            //{
            //    response.Success = false;
            //    response.Message = "Something went wrong. Please try after sometime.";
            //    return response;
            //}
            return response;
        }
        public ServiceResponse<string> RemoveEmployee(int id)
        {
            var response = new ServiceResponse<string>();
            var result = _employeeeRepository.DeleteEmployee(id);
            if (result)
            {
                response.Success = true;
                response.Message = "Employee Deleted Successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong please try after sometime";
            }
            return response;
        }


        public ServiceResponse<string> AddContact(Employee employee)
        {
            var response = new ServiceResponse<string>();
            if (_employeeeRepository.EmployeeExist(employee.EmployeeEmail))
            {
                response.Success = false;
                response.Message = "Employee Already Exist";
                return response;
            }
            if (employee.DateOfJoining > DateTime.Now)
            {
                response.Success = false;
                response.Message = "Join date can't be a future date.";
                return response;
            }
            if (employee.BasicSalary < employee.Allowance)
            {
                response.Success = false;
                response.Message = "BasicSalary should be always higher than allowance.";
                return response;

            }

            if (!ValidateEmail(employee.EmployeeEmail))
            {
                response.Success = false;
                response.Message = "Email should be in xyz@abc.com format only!";
                return response;
            }
            if (employee.BasicSalary <= 10000)
            {
                employee.ProfTax = 100;
            }
            else if (employee.BasicSalary <= 20000)
            {
                employee.ProfTax = 200;
            }
            else if (employee.BasicSalary <= 30000)
            {
                employee.ProfTax = 300;
            }
            else if (employee.BasicSalary <= 40000)
            {
                employee.ProfTax = 400;
            }
            else if (employee.BasicSalary <= 50000)
            {
                employee.ProfTax = 500;
            }
            else
            {
                employee.ProfTax = 1000;
            }

            employee.HRA = employee.BasicSalary * 0.10m;
            employee.PfDeduction = employee.BasicSalary * 0.075m;
            employee.GrossSalary = employee.BasicSalary + employee.HRA + employee.Allowance;
            employee.GrossDeductions = employee.PfDeduction + employee.ProfTax;
            employee.TotalSalary = employee.GrossSalary - employee.GrossDeductions;




            var result = _employeeeRepository.InsertEmployee(employee);
            if (result)
            {
                response.Success = true;
                response.Message = "Employee Saved Successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong. Please try later";
            }
            return response;
        }

        //[ExcludeFromCodeCoverage]
        private bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);

        }
        
        public ServiceResponse<IEnumerable<SalaryHeadTotal>> GetTotalSalaryByDepartmentAndYear(int year)
        {
            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>();

            try
            {
                var totals = _employeeeRepository.GetTotalSalaryByDepartmentAndYear(year);

                if (totals != null && totals.Any())
                {
                    var salaryHeadTotals = totals.Select(t => new SalaryHeadTotal
                    {
                        Head = t.Head, // Replace with actual head identifier
                        Year = t.Year,
                        TotalSalary = t.TotalSalary,
                        GrossDeductions = t.GrossDeductions,
                        Allowance = t.Allowance,
                        PfDeduction=t.PfDeduction,
                        BasicSalary=t.BasicSalary,
                        GrossSalary=t.GrossSalary,
                        HRA=t.HRA,
                        ProfTax=t.ProfTax,
                    }).ToList();

                    response.Data = salaryHeadTotals;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No salary data found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error: {ex.Message}";
                // Log the exception
            }

            return response;
        }
       
        public ServiceResponse<IEnumerable<SalaryHeadTotal>> GetTotalSalaryByYear(int year)
        {
            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>();

            try
            {
                var totals = _employeeeRepository.GetTotalSalaryByYear(year);

                if (totals != null && totals.Any())
                {
                    var salaryHeadTotals = totals.Select(t =>
                    {
                        // Calculate the number of months worked in the year
                        int monthsWorked = 12 - (t.Month - 1); // Assuming DateOfJoining is a DateTime

                        // If the employee joined in a month after the current year starts, assume full year contribution
                        if (t.Year < year)
                        {
                            monthsWorked = 12;
                        }

                        return new SalaryHeadTotal
                        {
                            Month = t.Month,
                            Year = t.Year,
                            TotalSalary = t.TotalSalary * monthsWorked,
                            GrossDeductions = t.GrossDeductions * monthsWorked,
                            Allowance = t.Allowance * monthsWorked,
                            PfDeduction = t.PfDeduction * monthsWorked,
                            BasicSalary = t.BasicSalary * monthsWorked,
                            GrossSalary = t.GrossSalary * monthsWorked,
                            HRA = t.HRA * monthsWorked,
                            ProfTax = t.ProfTax * monthsWorked,
                        };
                    }).ToList();


                    response.Data = salaryHeadTotals;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No salary data found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error: {ex.Message}";
                // Log the exception
            }

            return response;
        }
        public ServiceResponse<IEnumerable<SalaryHeadTotal>> GetTotalSalaryByMonth(int month, int year)
        {
            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>();
            if (year > DateTime.Now.Year || (year == DateTime.Now.Year && month > DateTime.Now.Month))
            {
                response.Success = true;
                response.Message = "Enter a valid month and year.";
                return response;
            }
            try
            {
                var totals = _employeeeRepository.GetTotalSalaryByMonth(month,year);

                if (totals != null && totals.Any())
                {
                    var salaryHeadTotals = totals.Select(t =>
                    {
                         return new SalaryHeadTotal
                        {
                            Month = t.Month,
                            Year = t.Year,
                            TotalSalary = t.TotalSalary,
                            GrossDeductions = t.GrossDeductions ,
                            Allowance = t.Allowance ,
                            PfDeduction = t.PfDeduction ,
                            BasicSalary = t.BasicSalary,
                            GrossSalary = t.GrossSalary,
                            HRA = t.HRA,
                            ProfTax = t.ProfTax,
                        };
                    }).ToList();

                    response.Data = salaryHeadTotals;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No salary data found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error: {ex.Message}";
                // Log the exception
            }

            return response;
        }
        public ServiceResponse<IEnumerable<TotalProfTax>> GetTotalProfTaxByMonth(int month, int year)
        {
            var response = new ServiceResponse<IEnumerable<TotalProfTax>>();
            if (year > DateTime.Now.Year || (year == DateTime.Now.Year && month > DateTime.Now.Month))
            {
                response.Success = false;
                response.Message = "Enter a valid month and year.";
                return response;
            }
            try
            {
                var totals = _employeeeRepository.GetTotalProfTaxByMonth(month, year);

                if (totals != null && totals.Any())
                {
                    var salaryHeadTotals = totals.Select(t =>
                    {
                        return new TotalProfTax
                        {
                            Month = t.Month,
                            Year = t.Year,
                            ProfTax = t.ProfTax,
                        };
                    }).ToList();

                    response.Data = salaryHeadTotals;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No prof. tax data found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error: {ex.Message}";
                // Log the exception
            }

            return response;
        }

        //[ExcludeFromCodeCoverage]
        public ServiceResponse<IEnumerable<SalaryHeadTotal>> GetTotalSalaryByMonthYearAndId(int employeeId,int month, int year)
        {
            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>();

            // Validate month and year
            if (year > DateTime.Now.Year || (year == DateTime.Now.Year && month > DateTime.Now.Month))
            {
                response.Success = true;
                response.Message = "Enter a valid month and year.";
                return response;
            }

            try
            {
                var totals = _employeeeRepository.GetTotalSalaryByMonthYearAndId(employeeId,month, year);

                if (totals != null && totals.Any())
                {
                    // Map to SalaryHeadTotal objects
                    var salaryHeadTotals = totals.Select(t => new SalaryHeadTotal
                    {
                        Month = t.Month,
                        Year = t.Year,
                        TotalSalary = t.TotalSalary,
                        GrossDeductions = t.GrossDeductions,
                        Allowance = t.Allowance,
                        PfDeduction = t.PfDeduction,
                        BasicSalary = t.BasicSalary,
                        GrossSalary = t.GrossSalary,
                        HRA = t.HRA,
                        ProfTax = t.ProfTax
                    }).ToList();

                    response.Data = salaryHeadTotals;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No salary data found for the specified employee.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error: {ex.Message}";
                // Log the exception
            }

            return response;
        }

        //[ExcludeFromCodeCoverage]
        public ServiceResponse<IEnumerable<SalaryHeadTotal>> GetTotalSalaryByYearAndId(int employeeId, int year)
        {
            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>();

            try
            {
                var employee = _employeeeRepository.GetEmployeeById(employeeId);
                if (employee == null)
                {
                    response.Success = false;
                    response.Message = $"Employee with ID {employeeId} not found.";
                    return response;
                }

                var currentYear = DateTime.Now.Year;
                var currentMonth = DateTime.Now.Month;

                var totals = _employeeeRepository.GetTotalSalaryByYearAndId(employeeId, year);

                if (totals != null && totals.Any())
                {
                    var salaryHeadTotals = totals.Select(t =>
                    {
                        int monthsWorked;

                        if (year == employee.DateOfJoining.Year)
                        {
                            // If joining year is the same as the requested year
                            monthsWorked = currentYear == year ? currentMonth - (employee.DateOfJoining.Month - 1) : 12 - (employee.DateOfJoining.Month - 1);
                        }
                        else if (year == currentYear)
                        {

                            monthsWorked = currentMonth-1;
                        }
                        else if (year > employee.DateOfJoining.Year)
                        {
                            // If the year is between joining year and requested year
                            monthsWorked = 12;
                        }
                      
                        else
                        {
                            // If the year is before the joining year or after the requested year
                            monthsWorked = 0;
                        }

                        return new SalaryHeadTotal
                        {
                            Month = t.Month,
                            Year = t.Year,
                            TotalSalary = t.TotalSalary * monthsWorked,
                            GrossDeductions = t.GrossDeductions * monthsWorked,
                            Allowance = t.Allowance * monthsWorked,
                            PfDeduction = t.PfDeduction * monthsWorked,
                            BasicSalary = t.BasicSalary * monthsWorked,
                            GrossSalary = t.GrossSalary * monthsWorked,
                            HRA = t.HRA * monthsWorked,
                            ProfTax = t.ProfTax * monthsWorked,
                        };
                    }).ToList();

                    response.Data = salaryHeadTotals;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No salary data found for the specified employee or year.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error: {ex.Message}";
                // Log the exception
            }

            return response;
        }

    }
}
