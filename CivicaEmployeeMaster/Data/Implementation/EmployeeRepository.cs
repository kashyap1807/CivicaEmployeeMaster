using CivicaEmployeeMaster.Data.Contract;
using CivicaEmployeeMaster.Dtos;
using CivicaEmployeeMaster.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CivicaEmployeeMaster.Data.Implementation
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly IAppDbContext _appDbContext;
        public EmployeeRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Employee> GetAllEmployee()
        {
            List<Employee> employees = _appDbContext.Employees.Include(c => c.EmployeeDepartment).ToList();
            return employees;
        }
        public IEnumerable<Employee> GetPaginatedEmployees(int page, int pageSize, string? search, string sortOrder)
        {
            int skip = (page - 1) * pageSize;
            IQueryable<Employee> query = _appDbContext.Employees
                .Include(c => c.EmployeeDepartment);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search));
            }

            switch (sortOrder.ToLower())
            {
                case "asc":
                    query = query.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);
                    break;
                case "desc":
                    query = query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName);
                    break;
            }
            return query
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public int TotalEmployees(string? search)
        {
            IQueryable<Employee> query = _appDbContext.Employees;

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search));
            }

            return query.Count();
        }
        public Employee? GetEmployeeById(int id)
        {
            var employee = _appDbContext.Employees.Include(d => d.EmployeeDepartment).FirstOrDefault(e => e.Id == id);
            return employee;
        }

        public bool UpdateEmployee(Employee employee)
        {
            var result = false;
            if (employee != null)
            {
                _appDbContext.Employees.Update(employee);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool EmployeesExists(int employeeId,string email)
        {
            var e = _appDbContext.Employees.FirstOrDefault(e=>e.Id != employeeId && e.EmployeeEmail == email);
            if (e != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteEmployee(int id)
        {
            var result = false;
            var employee = _appDbContext.Employees.FirstOrDefault(c => c.Id == id);
            if (employee != null)
            {
                _appDbContext.Remove(employee);
                _appDbContext.SaveChanges();
                result = true;

            }
            return result;
        }

        public bool InsertEmployee(Employee employee)
        {
            var result = false;
            if (employee != null)
            {
                _appDbContext.Employees.Add(employee);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }
        public bool EmployeeExist(string email)
        {
            var employee = _appDbContext.Employees.FirstOrDefault(c => c.EmployeeEmail == email);
            if (employee != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<SalaryHeadTotal> GetTotalSalaryByDepartmentAndYear(int year)
        {
            var totals = _appDbContext.Employees
                .Where(e=>e.DateOfJoining.Year == year)
                .GroupBy(e => new { e.EmployeeDepartment.DepartmentName, Year = e.DateOfJoining.Year })
                .Select(g => new SalaryHeadTotal
                {
                    Head = g.Key.DepartmentName, 
                    Year = g.Key.Year,
                    TotalSalary = g.Sum(e => e.TotalSalary),
                    BasicSalary=g.Sum(e => e.BasicSalary),
                    GrossSalary=g.Sum(e=>e.GrossSalary),
                    PfDeduction=g.Sum(e=>e.PfDeduction),
                    Allowance=g.Sum(e=>e.Allowance),
                    GrossDeductions=g.Sum(e=>e.GrossDeductions),
                    HRA=g.Sum(e=>e.HRA),
                    ProfTax=g.Sum(e=>e.ProfTax),
                })
                .ToList();

            return totals;
        }
        public IEnumerable<SalaryHeadTotal> GetTotalSalaryByYear(int year)
        {
            var totals = _appDbContext.Employees
                .Where(e=>e.DateOfJoining.Year <= year)
                .GroupBy(e => new { Year = e.DateOfJoining.Year,Month=e.DateOfJoining.Month })
                .Select(g => new SalaryHeadTotal
                {
                    Year = g.Key.Year,
                    Month=g.Key.Month,
                    TotalSalary = g.Sum(e => e.TotalSalary),
                    BasicSalary = g.Sum(e => e.BasicSalary),
                    GrossSalary = g.Sum(e => e.GrossSalary),
                    PfDeduction = g.Sum(e => e.PfDeduction),
                    Allowance = g.Sum(e => e.Allowance),
                    GrossDeductions = g.Sum(e => e.GrossDeductions),
                    HRA = g.Sum(e => e.HRA),
                    ProfTax = g.Sum(e => e.ProfTax),
                })
                .ToList();
            return totals;
        }
        //public IEnumerable<SalaryHeadTotal> GetTotalSalaryByMonth(int month, int year)
        //{
        //    var totals = _appDbContext.Employees
        //        .Where(e => e.DateOfJoining.Month == month && e.DateOfJoining.Year == year)
        //        .GroupBy(e => new { Year = e.DateOfJoining.Year, Month = e.DateOfJoining.Month })
        //        .Select(g => new SalaryHeadTotal
        //        {
        //            Year = g.Key.Year,
        //            Month = g.Key.Month,
        //            TotalSalary = g.Sum(e => e.TotalSalary),
        //            BasicSalary = g.Sum(e => e.BasicSalary),
        //            GrossSalary = g.Sum(e => e.GrossSalary),
        //            PfDeduction = g.Sum(e => e.PfDeduction),
        //            Allowance = g.Sum(e => e.Allowance),
        //            GrossDeductions = g.Sum(e => e.GrossDeductions),
        //            HRA = g.Sum(e => e.HRA),
        //            ProfTax = g.Sum(e => e.ProfTax),
        //        })
        //        .ToList();
        //    return totals;
        //}
        public IEnumerable<SalaryHeadTotal> GetTotalSalaryByMonth(int month, int year)
        {
            var employees = _appDbContext.Employees.ToList(); // Fetch all employees into memory

            var totals = employees
                .Where(e => e.DateOfJoining.Year <= year && (e.DateOfJoining.Month <= month || e.DateOfJoining.Year < year))
                .GroupBy(e => new { Year = year, Month = month })
                .Select(g => new SalaryHeadTotal
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSalary = g.Sum(e => e.TotalSalary),
                    BasicSalary = g.Sum(e => e.BasicSalary),
                    GrossSalary = g.Sum(e => e.GrossSalary),
                    PfDeduction = g.Sum(e => e.PfDeduction),
                    Allowance = g.Sum(e => e.Allowance),
                    GrossDeductions = g.Sum(e => e.GrossDeductions),
                    HRA = g.Sum(e => e.HRA),
                    ProfTax = g.Sum(e => e.ProfTax),
                })
                .ToList();

            return totals;
        }
        public IEnumerable<TotalProfTax> GetTotalProfTaxByMonth(int month, int year)
        {
            var employees = _appDbContext.Employees.ToList(); // Fetch all employees into memory

            var totals = employees
                .Where(e => e.DateOfJoining.Year <= year && (e.DateOfJoining.Month <= month || e.DateOfJoining.Year < year))
                .GroupBy(e => new { Year = year, Month = month })
                .Select(g => new TotalProfTax
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    ProfTax = g.Sum(e => e.ProfTax),
                })
                .ToList();

            return totals;
        }
        [ExcludeFromCodeCoverage]
        public IEnumerable<SalaryHeadTotal> GetTotalSalaryByMonthYearAndId(int employeeId,int month, int year)
        {
            var employee = _appDbContext.Employees.FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
            {
                // Handle case where employee with given ID is not found
                return Enumerable.Empty<SalaryHeadTotal>();
            }

            var totals = _appDbContext.Employees
                .Where(e => e.Id == employeeId &&
                            e.DateOfJoining.Year <= year &&
                            (e.DateOfJoining.Month <= month || e.DateOfJoining.Year < year))
                .GroupBy(e => new { e.DateOfJoining.Year, e.DateOfJoining.Month }) // Group by year and month
                .Select(g => new SalaryHeadTotal
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSalary = g.Sum(e => e.TotalSalary),
                    BasicSalary = g.Sum(e => e.BasicSalary),
                    GrossSalary = g.Sum(e => e.GrossSalary),
                    PfDeduction = g.Sum(e => e.PfDeduction),
                    Allowance = g.Sum(e => e.Allowance),
                    GrossDeductions = g.Sum(e => e.GrossDeductions),
                    HRA = g.Sum(e => e.HRA),
                    ProfTax = g.Sum(e => e.ProfTax),
                })
                .ToList();

            return totals;
        }

        public IEnumerable<SalaryHeadTotal> GetTotalSalaryByYearAndId(int employeeId, int year)
        {
            var employee = _appDbContext.Employees.FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
            {
                // Handle case where employee with given ID is not found
                return Enumerable.Empty<SalaryHeadTotal>();
            }

            var totals = _appDbContext.Employees
                .Where(e => e.Id == employeeId && e.DateOfJoining.Year <= year)
                .GroupBy(e => new { e.DateOfJoining.Year, e.DateOfJoining.Month })
                .Select(g => new SalaryHeadTotal
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSalary = g.Sum(e => e.TotalSalary),
                    BasicSalary = g.Sum(e => e.BasicSalary),
                    GrossSalary = g.Sum(e => e.GrossSalary),
                    PfDeduction = g.Sum(e => e.PfDeduction),
                    Allowance = g.Sum(e => e.Allowance),
                    GrossDeductions = g.Sum(e => e.GrossDeductions),
                    HRA = g.Sum(e => e.HRA),
                    ProfTax = g.Sum(e => e.ProfTax),
                })
                .ToList();

            return totals;
        }

    }
}
