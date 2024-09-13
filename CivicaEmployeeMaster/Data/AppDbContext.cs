using CivicaEmployeeMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace CivicaEmployeeMaster.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public DbSet<PasswordHint> PasswordHints { get; set; }
        public EntityState GetEntryState<TEntity>(TEntity entity) where TEntity : class
        {
            return Entry(entity).State;
        }

        public void SetEntryState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            Entry(entity).State = entityState;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
             .HasOne(d => d.EmployeeDepartment)
             .WithMany(p => p.Employees)
             .HasForeignKey(d => d.DepartmentId)
             .OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("FK_Employee_EmployeeDepartment");
    }
    }
}
