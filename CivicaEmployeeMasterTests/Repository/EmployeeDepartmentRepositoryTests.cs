using CivicaEmployeeMaster.Data.Implementation;
using CivicaEmployeeMaster.Data;
using CivicaEmployeeMaster.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace CivicaEmployeeMasterTests.Repository
{
    public class EmployeeDepartmentRepositoryTests
    {
        [Fact]
        public void GetAll_ReturnsEmpty_WhenEmployeeNotExists()
        {
            //Arrange
            var employees = new List<EmployeeDepartment>().AsQueryable();
            var mockDbSet = new Mock<DbSet<EmployeeDepartment>>();
            mockDbSet.As<IQueryable<EmployeeDepartment>>().Setup(c => c.GetEnumerator()).Returns(employees.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.EmployeeDepartments).Returns(mockDbSet.Object);
            var target = new EmployeeDepartmentRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllEmployeeDepartment();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockDbSet.As<IQueryable<EmployeeDepartment>>().Verify(c => c.GetEnumerator(), Times.Once);

        }
        [Fact]
        public void GetAll_ReturnsEmployees_WhenEmployeesExist()
        {
            // Arrange
            var employees = new List<EmployeeDepartment>
    {
        new EmployeeDepartment { DepartmentName = "Name", DepartmentId = 101 },
        new EmployeeDepartment { DepartmentName = "Name 1", DepartmentId = 102 },
    }.AsQueryable();

            var mockDbSet = new Mock<DbSet<EmployeeDepartment>>();
            mockDbSet.As<IQueryable<EmployeeDepartment>>().Setup(m => m.Provider).Returns(employees.Provider);
            mockDbSet.As<IQueryable<EmployeeDepartment>>().Setup(m => m.Expression).Returns(employees.Expression);
            mockDbSet.As<IQueryable<EmployeeDepartment>>().Setup(m => m.ElementType).Returns(employees.ElementType);
            mockDbSet.As<IQueryable<EmployeeDepartment>>().Setup(m => m.GetEnumerator()).Returns(employees.GetEnumerator());

            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.EmployeeDepartments).Returns(mockDbSet.Object);

            var target = new EmployeeDepartmentRepository(mockAbContext.Object);

            // Act
            var actual = target.GetAllEmployeeDepartment();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count()); // Adjust the expected count based on the sample data provided
            mockDbSet.As<IQueryable<EmployeeDepartment>>().Verify(m => m.GetEnumerator(), Times.Once);
        }

    }
}
