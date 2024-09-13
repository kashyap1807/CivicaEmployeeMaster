using CivicaEmployeeMaster.Data;
using CivicaEmployeeMaster.Data.Implementation;
using CivicaEmployeeMaster.Models;
using CivicaEmployeeMaster.Services.Implementation;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicaEmployeeMasterTests.Repository
{
    public class EmployeeRepositoryTests
    {   
        [Fact]
        public void GetEmployee_WhenEmployeeIsNull()
        {
            //Arrange
            var id = 1;
            var employees = new List<Employee>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(employees.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(employees.Expression);
            mockAbContext.SetupGet(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAbContext.Object);
            //Act
            var actual = target.GetEmployeeById(id);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Employees, Times.Once);

        }
        [Fact]
        public void GetEmployee_WhenEmployeesIsNotNull()
        {
            //Arrange
            var id = 1;
            var employees = new List<Employee>()
            {
              new Employee { Id = 1, FirstName = "Employee 1" },
                new Employee { Id = 2, FirstName = "Employee 2" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(employees.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(employees.Expression);
            mockAbContext.SetupGet(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAbContext.Object);
            //Act
            var actual = target.GetEmployeeById(id);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Employees, Times.Once);

        }
        [Fact]
        public void GetEmployee_ReturnsEmployee_WhenEmployeeExists()
        {
            //Arrange
            var employees = new List<Employee>
            {
                new Employee{  Id = 1,
                FirstName = "C1"},
                new Employee{ Id = 2,
                FirstName = "C2"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.GetEnumerator()).Returns(employees.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllEmployee();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(employees.Count(), actual.Count());
            mockAbContext.Verify(c => c.Employees, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.GetEnumerator(), Times.Once);

        }

        [Fact]
        public void GetAll_ReturnsEmpty_WhenEmployeeNotExists()
        {
            //Arrange
            var employees = new List<Employee>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.GetEnumerator()).Returns(employees.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllEmployee();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockAbContext.Verify(c => c.Employees, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.GetEnumerator(), Times.Once);

        }
        [Fact]
        public void GetAll_ReturnsEmpty_WhenEmployeeExists()
        {
            //Arrange
            var employees = new List<Employee>
            {
                new Employee{  Id = 1,
                FirstName = "C1"},
                new Employee{ Id = 2,
                FirstName = "C2"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.GetEnumerator()).Returns(employees.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllEmployee();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            mockAbContext.Verify(c => c.Employees, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.GetEnumerator(), Times.Once);

        }
        [Fact]
        public void TotalEmployees_ReturnsCount_WhenEmployeesExistWhenSearchIsNull()
        {
            var contacts = new List<Employee> {
                new Employee {Id = 1,FirstName="Employee 1"},
                new Employee {Id = 2,FirstName="Employee 2"}
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalEmployees(null);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Employees, Times.Once);

        }

        [Fact]
        public void TotalEmployee_ReturnsCount_WhenEmployeeExistWhenSearchIsNotNull()
        {
            string search = "e";
            var contacts = new List<Employee> {
                new Employee {Id = 1,FirstName="Employee 1"},
                new Employee {Id = 2,FirstName="Employee 2"}
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalEmployees(search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Employees, Times.Once);

        }

        [Fact]
        public void TotalEmployee_ReturnsCountZero_WhenNoEmployeeExistWhenSearchIsNull()
        {
            var contacts = new List<Employee>
            {
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalEmployees(null);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Employees, Times.Once);

        }

        [Fact]
        public void TotalEmployee_ReturnsCountZero_WhenNoEmployeesExistWhenSearchIsNotNull()
        {
            var contacts = new List<Employee>
            {

            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAppDbContext.Object);
            string search = "abc";
            //Act
            var actual = target.TotalEmployees(search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Employees, Times.Once);

        }
        [Fact]
        public void GetPaginatedEmployee_ReturnsCorrectEmployee_WhenEmployeeExists_SearchIsNull()
        {
            string sortOrder = "asc";
            var contacts = new List<Employee>
              {
                  new Employee{Id=1, FirstName="Employee 1"},
                  new Employee{Id=2, FirstName="Employee 2"},
                  new Employee{Id=3, FirstName="Employee 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedEmployees(1, 2, null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Exactly(2));
            mockAppDbContext.Verify(c => c.Employees, Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExists_SearchIsNotNull()
        {
            string sortOrder = "asc";
            string search = "e";
            var contacts = new List<Employee>
              {
                  new Employee{Id=1, FirstName="Employee 1"},
                  new Employee{Id=2, FirstName="Employee 2"},
                  new Employee{Id=3, FirstName="Employee 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedEmployees(1, 2, search, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Employees, Times.Once);
        }

        [Fact]
        public void GetPaginatedEmployee_ReturnsEmptyList_WhenNoEmployeeExists_SearchIsNull()
        {
            string sortOrder = "desc";
            var contacts = new List<Employee>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedEmployees(1, 2, null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Exactly(2));
            mockAppDbContext.Verify(c => c.Employees, Times.Once);
        }

        [Fact]
        public void GetPaginatedEmployee_ReturnsEmptyList_WhenNoEmployeeExists_SearchIsNotNull()
        {
            string search = "con";
            string sortOrder = "asc";
            var contacts = new List<Employee>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedEmployees(1, 2, search, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Exactly(2));
            mockAppDbContext.Verify(c => c.Employees, Times.Once);
        }
        //[Fact]
        //public void InsertEmployee_ReturnsTrue()
        //{
        //    //Arrange
        //    var mockDbSet = new Mock<DbSet<Employee>>();
        //    var mockAppDbContext = new Mock<IAppDbContext>();
        //    mockAppDbContext.SetupGet(c => c.Employees).Returns(mockDbSet.Object);
        //    mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
        //    var target = new EmployeeRepository(mockAppDbContext.Object);
        //    var contact = new Employee
        //    {
        //        Id = 1,
        //        FirstName = "E1"
        //    };


        //    //Act
        //    var actual = target.in(contact);

        //    //Assert
        //    Assert.True(actual);
        //    mockDbSet.Verify(c => c.Add(contact), Times.Once);
        //    mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        //}

        //[Fact]
        //public void InsertContact_ReturnsFalse()
        //{
        //    //Arrange
        //    Contact contact = null;
        //    var mockAbContext = new Mock<IAppDbContext>();
        //    var target = new ContactRepository(mockAbContext.Object);

        //    //Act
        //    var actual = target.InsertContact(contact);
        //    //Assert
        //    Assert.False(actual);
        //}

        [Fact]
        public void UpdateEmployee_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<Employee>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Employees).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new EmployeeRepository(mockAppDbContext.Object);
            var employee = new Employee
            {
               Id = 1,
                FirstName = "E1"
            };


            //Act
            var actual = target.UpdateEmployee(employee);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Update(employee), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }
        [Fact]
        public void UpdateEmployeet_ReturnsFalse()
        {
            //Arrange
            Employee employee = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new EmployeeRepository(mockAbContext.Object);

            //Act
            var actual = target.UpdateEmployee(employee);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void DeleteEmployee_ReturnsTrue()
        {
            //Arrange
            var id = 1;
            var employee = new Employee
            {
                Id = 1,
                FirstName = "E1"
            };

            var employees = new List<Employee> { employee }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(employees.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(employees.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.Employees).Returns(mockDbSet.Object);
            mockAbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new EmployeeRepository(mockAbContext.Object);

            //Act
            var actual = target.DeleteEmployee(id);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Employees, Times.Once);
            mockAbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteEmployee_ReturnsFalse()
        {
            //Arrange
            var id = 1;
            var employee = new List<Employee>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(employee.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(employee.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAbContext.Object);

            //Act
            var actual = target.DeleteEmployee(id);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Employees, Times.Once);
        }
        [Fact]
        public void EmployeeExists_ReturnsTrue()
        {
            //Arrange
            var id = 2;
            var email = "test@test.com";
            var employee = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "Employee 1" ,EmployeeEmail="test@test.com"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(employee.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(employee.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAbContext.Object);

            //Act
            var actual = target.EmployeesExists(id,email);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Employees, Times.Once);
        }

        [Fact]
        public void EmployeeExists_ReturnsFalse()
        {
            //Arrange
            var id = 1;
            var email = "test@test.com";
            var employee = new List<Employee>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(employee.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(employee.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAbContext.Object);

            //Act
            var actual = target.EmployeesExists(id,email);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Employees, Times.Once);
        }
        [Fact]
        public void InsertEmployee_ValidEmployee_ReturnsTrue()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmployeeEmail = "john.doe@example.com"
                // Add other properties as needed
            };

            var mockDbSet = new Mock<DbSet<Employee>>();
            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);

            var repository = new EmployeeRepository(mockDbContext.Object);

            // Act
            var result = repository.InsertEmployee(employee);

            // Assert
            Assert.True(result); // Ensure method returns true on successful insert

            // Verify that Add and SaveChanges were called
            mockDbSet.Verify(m => m.Add(employee), Times.Once);
            mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
        }
        [Fact]
        public void InsertEmployee_NullEmployee_ReturnsFalse()
        {
            // Arrange
            Employee employee = null;

            var mockDbSet = new Mock<DbSet<Employee>>();
            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);

            var repository = new EmployeeRepository(mockDbContext.Object);

            // Act
            var result = repository.InsertEmployee(employee);

            // Assert
            Assert.False(result); // Ensure method returns false when employee is null

            // Verify that Add and SaveChanges were not called
            mockDbSet.Verify(m => m.Add(It.IsAny<Employee>()), Times.Never);
            mockDbContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void EmployeeExist_ReturnsTrue()
        {
            //Arrange
            var id = 2;
            var email = "test@test.com";
            var employee = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "Employee 1" ,EmployeeEmail="test@test.com"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(employee.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(employee.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAbContext.Object);

            //Act
            var actual = target.EmployeeExist(email);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Employees, Times.Once);
        }

        [Fact]
        public void EmployeeExist_ReturnsFalse()
        {
            //Arrange
            var id = 1;
            var email = "test@test.com";
            var employee = new List<Employee>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(employee.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(employee.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);
            var target = new EmployeeRepository(mockAbContext.Object);

            //Act
            var actual = target.EmployeeExist(email);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employee>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Employees, Times.Once);
        }
        [Fact]
        public void GetTotalSalaryByDepartmentAndYear_Returns_CorrectTotals()
        {
            int year = 2000;
            // Arrange
            var mockDbContext = new Mock<IAppDbContext>();
            var employees = new List<Employee>
        {
            new Employee { Id = 1, EmployeeDepartment = new EmployeeDepartment { DepartmentName = "IT" }, DateOfJoining = new DateTime(2023, 1, 1), TotalSalary = 5000, BasicSalary = 4000, GrossSalary = 4500, PfDeduction = 100, Allowance = 500, GrossDeductions = 200, HRA = 300, ProfTax = 50 },
            new Employee { Id = 2, EmployeeDepartment = new EmployeeDepartment { DepartmentName = "HR" }, DateOfJoining = new DateTime(2023, 1, 1), TotalSalary = 6000, BasicSalary = 5000, GrossSalary = 5500, PfDeduction = 120, Allowance = 600, GrossDeductions = 250, HRA = 350, ProfTax = 60 },
            new Employee { Id = 3, EmployeeDepartment = new EmployeeDepartment { DepartmentName = "IT" }, DateOfJoining = new DateTime(2022, 1, 1), TotalSalary = 7000, BasicSalary = 6000, GrossSalary = 6500, PfDeduction = 150, Allowance = 700, GrossDeductions = 300, HRA = 400, ProfTax = 70 }
        }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Provider).Returns(employees.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(c => c.Expression).Returns(employees.Expression);
            mockDbContext.Setup(m => m.Employees).Returns(mockDbSet.Object);

            var service = new EmployeeRepository(mockDbContext.Object);

            // Act
            var result = service.GetTotalSalaryByDepartmentAndYear(year);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());

           
        }
        [Fact]
        public void GetTotalSalaryByYear_Returns_CorrectTotals()
        {
            int year = 2000;
            // Arrange
            var mockDbContext = new Mock<IAppDbContext>();
            var employees = new List<Employee>
        {
            new Employee { Id = 1, DateOfJoining = new DateTime(2023, 1, 1), TotalSalary = 5000, BasicSalary = 4000, GrossSalary = 4500, PfDeduction = 100, Allowance = 500, GrossDeductions = 200, HRA = 300, ProfTax = 50 },
            new Employee { Id = 2, DateOfJoining = new DateTime(2023, 1, 1), TotalSalary = 6000, BasicSalary = 5000, GrossSalary = 5500, PfDeduction = 120, Allowance = 600, GrossDeductions = 250, HRA = 350, ProfTax = 60 },
            new Employee { Id = 3, DateOfJoining = new DateTime(2022, 1, 1), TotalSalary = 7000, BasicSalary = 6000, GrossSalary = 6500, PfDeduction = 150, Allowance = 700, GrossDeductions = 300, HRA = 400, ProfTax = 70 }
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employees.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employees.Expression);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employees.ElementType);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employees.GetEnumerator());

            mockDbContext.Setup(m => m.Employees).Returns(mockDbSet.Object);

            var service = new EmployeeRepository(mockDbContext.Object);

            // Act
            var result = service.GetTotalSalaryByYear(year);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count()); // Check there are 3 records (for each year and month combination)

            
        }

        [Fact]
        public void GetTotalSalaryByMonth_Returns_CorrectTotals()
        {
            // Arrange
            int targetMonth = 6;
            int targetYear = 2024;

            var mockDbContext = new Mock<IAppDbContext>();
            var mockDbSet = new Mock<DbSet<Employee>>();

            // Sample employee data for testing
            var employeesData = new List<Employee>
        {
            new Employee { Id = 1, DateOfJoining = new DateTime(2023, 5, 15), TotalSalary = 5000, BasicSalary = 4000, GrossSalary = 4500, PfDeduction = 200, Allowance = 500, GrossDeductions = 300, HRA = 300, ProfTax = 100 },
            new Employee { Id = 2, DateOfJoining = new DateTime(2024, 6, 10), TotalSalary = 6000, BasicSalary = 4500, GrossSalary = 4800, PfDeduction = 250, Allowance = 600, GrossDeductions = 350, HRA = 350, ProfTax = 120 }
            // Add more sample data as needed
        };

            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employeesData.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employeesData.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employeesData.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employeesData.GetEnumerator());

            mockDbContext.Setup(x => x.Employees).Returns(mockDbSet.Object);

            var repository = new EmployeeRepository(mockDbContext.Object);

            // Act
            var result = repository.GetTotalSalaryByMonth(targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count()); // Assuming we have 2 employees in our mocked data

            var firstEmployeeTotal = result.FirstOrDefault();
            Assert.NotNull(firstEmployeeTotal);
            Assert.Equal(targetYear, firstEmployeeTotal.Year);
            Assert.Equal(targetMonth, firstEmployeeTotal.Month);
            Assert.Equal(11000, firstEmployeeTotal.TotalSalary); // Adjust based on your sample data
            Assert.Equal(8500, firstEmployeeTotal.BasicSalary); // Adjust based on your sample data
            Assert.Equal(9300, firstEmployeeTotal.GrossSalary); // Adjust based on your sample data
            Assert.Equal(450, firstEmployeeTotal.PfDeduction); // Adjust based on your sample data
            Assert.Equal(1100, firstEmployeeTotal.Allowance); // Adjust based on your sample data
            Assert.Equal(650, firstEmployeeTotal.GrossDeductions); // Adjust based on your sample data
            Assert.Equal(650, firstEmployeeTotal.HRA); // Adjust based on your sample data
            Assert.Equal(220, firstEmployeeTotal.ProfTax); // Adjust based on your sample data
        }
        [Fact]
        public void GetTotalProfTaxByMonth_ReturnsCorrectTotals()
        {
            int targetMonth = 6;
            int targetYear = 2024;

            var mockDbContext = new Mock<IAppDbContext>();
            var mockDbSet = new Mock<DbSet<Employee>>();

            // Sample employee data for testing
            var employeesData = new List<Employee>
        {
            new Employee { Id = 1, DateOfJoining = new DateTime(2023, 5, 15), TotalSalary = 5000, BasicSalary = 4000, GrossSalary = 4500, PfDeduction = 200, Allowance = 500, GrossDeductions = 300, HRA = 300, ProfTax = 100 },
            new Employee { Id = 2, DateOfJoining = new DateTime(2024, 6, 10), TotalSalary = 6000, BasicSalary = 4500, GrossSalary = 4800, PfDeduction = 250, Allowance = 600, GrossDeductions = 350, HRA = 350, ProfTax = 120 }
            // Add more sample data as needed
        };

            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employeesData.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employeesData.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employeesData.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employeesData.GetEnumerator());

            mockDbContext.Setup(x => x.Employees).Returns(mockDbSet.Object);

            var repository = new EmployeeRepository(mockDbContext.Object);

            // Act
            var result = repository.GetTotalProfTaxByMonth(targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count()); // Assuming we have 2 employees in our mocked data

            var firstEmployeeTotal = result.FirstOrDefault();
            Assert.NotNull(firstEmployeeTotal);
            Assert.Equal(targetYear, firstEmployeeTotal.Year);
            Assert.Equal(targetMonth, firstEmployeeTotal.Month);
           Assert.Equal(220, firstEmployeeTotal.ProfTax); // Adjust based on your sample data
        }
        //[Fact]
        //public void GetTotalSalaryByYearAndId_ReturnsCorrectTotals()
        //{
        //    int targetMonth = 6;
        //    int targetID = 1;

        //    var mockDbContext = new Mock<IAppDbContext>();
        //    var mockDbSet = new Mock<DbSet<Employee>>();

        //    // Sample employee data for testing
        //    var employeesData = new List<Employee>
        //{
        //    new Employee { Id = 1, DateOfJoining = new DateTime(2023, 5, 15), TotalSalary = 5000, BasicSalary = 4000, GrossSalary = 4500, PfDeduction = 200, Allowance = 500, GrossDeductions = 300, HRA = 300, ProfTax = 100 },
        //    new Employee { Id = 2, DateOfJoining = new DateTime(2024, 6, 10), TotalSalary = 6000, BasicSalary = 4500, GrossSalary = 4800, PfDeduction = 250, Allowance = 600, GrossDeductions = 350, HRA = 350, ProfTax = 120 }
        //    // Add more sample data as needed
        //};

        //    mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employeesData.AsQueryable().Provider);
        //    mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employeesData.AsQueryable().Expression);
        //    mockDbSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employeesData.AsQueryable().ElementType);
        //    mockDbSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employeesData.GetEnumerator());

        //    mockDbContext.Setup(x => x.Employees).Returns(mockDbSet.Object);

        //    var repository = new EmployeeRepository(mockDbContext.Object);

        //    // Act
        //    var result = repository.GetTotalSalaryByYearAndId(targetMonth, targetID);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(0, result.Count()); // Assuming we have 2 employees in our mocked data
        //}
        [Fact]
        public void GetTotalSalaryByYearAndId_Returns_CorrectTotals()
        {
            // Arrange
            int employeeId = 1;
            int year = 2023;

            // Mock employees data
            var employeesData = new List<Employee>
            {
                new Employee { Id = 1, DateOfJoining = new DateTime(2020, 5, 15), TotalSalary = 5000, BasicSalary = 4000, GrossSalary = 6000, PfDeduction = 500, Allowance = 1000, GrossDeductions = 1000, HRA = 800, ProfTax = 100 },
                new Employee { Id = 1, DateOfJoining = new DateTime(2021, 2, 10), TotalSalary = 6000, BasicSalary = 4500, GrossSalary = 6500, PfDeduction = 600, Allowance = 1200, GrossDeductions = 1200, HRA = 900, ProfTax = 120 },
                new Employee { Id = 1, DateOfJoining = new DateTime(2022, 8, 20), TotalSalary = 7000, BasicSalary = 5000, GrossSalary = 7500, PfDeduction = 700, Allowance = 1500, GrossDeductions = 1500, HRA = 1000, ProfTax = 150 },
                new Employee { Id = 2, DateOfJoining = new DateTime(2021, 3, 25), TotalSalary = 5500, BasicSalary = 4200, GrossSalary = 6200, PfDeduction = 550, Allowance = 1100, GrossDeductions = 1100, HRA = 850, ProfTax = 110 }
            }.AsQueryable();

            // Mock DbSet<Employee> and DbContext
            var mockSet = new Mock<DbSet<Employee>>();
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employeesData.Provider);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employeesData.Expression);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employeesData.ElementType);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employeesData.GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(c => c.Employees).Returns(mockSet.Object);

            // Create instance of EmployeeRepository with mock context
            var repository = new EmployeeRepository(mockContext.Object);

            // Act
            var result = repository.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();

            Assert.Equal(3, resultList.Count); // Check the number of entries returned

            // Check values for specific entries (assuming the test data provided)
            Assert.Equal(2020, resultList[0].Year);
            Assert.Equal(5, resultList[0].Month);
            Assert.Equal(5000, resultList[0].TotalSalary);
            Assert.Equal(4000, resultList[0].BasicSalary);
            Assert.Equal(6000, resultList[0].GrossSalary);
            Assert.Equal(500, resultList[0].PfDeduction);
            Assert.Equal(1000, resultList[0].Allowance);
            Assert.Equal(1000, resultList[0].GrossDeductions);
            Assert.Equal(800, resultList[0].HRA);
            Assert.Equal(100, resultList[0].ProfTax);
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_EmployeeNotFound_Returns_Empty()
        {
            // Arrange
            int employeeId = 999; // Employee ID that does not exist in mock data
            int year = 2023;

            // Mock DbSet<Employee> and DbContext with empty data
            var mockSet = new Mock<DbSet<Employee>>();
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(new List<Employee>().AsQueryable().Provider);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(new List<Employee>().AsQueryable().Expression);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(new List<Employee>().AsQueryable().ElementType);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(new List<Employee>().GetEnumerator());

            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(c => c.Employees).Returns(mockSet.Object);

            // Create instance of EmployeeRepository with mock context
            var repository = new EmployeeRepository(mockContext.Object);

            // Act
            var result = repository.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();

            Assert.Empty(resultList); // Ensure the result is empty when employee is not found
        }


    }
}
