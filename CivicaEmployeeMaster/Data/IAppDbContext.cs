using CivicaEmployeeMaster.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace CivicaEmployeeMaster.Data
{
    public interface IAppDbContext : IDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public DbSet<PasswordHint> PasswordHints { get; set; }
    }
}
