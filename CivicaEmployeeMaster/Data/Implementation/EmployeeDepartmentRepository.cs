using CivicaEmployeeMaster.Data.Contract;
using CivicaEmployeeMaster.Models;
using System.Diagnostics.CodeAnalysis;

namespace CivicaEmployeeMaster.Data.Implementation
{
    public class EmployeeDepartmentRepository: IEmployeeDepartmentRepository
    {
        private readonly IAppDbContext _appDbContext;
        public EmployeeDepartmentRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<EmployeeDepartment> GetAllEmployeeDepartment()
        {
            List<EmployeeDepartment> employeeDepartments = _appDbContext.EmployeeDepartments.ToList();
            return employeeDepartments;
        }
    }
}
