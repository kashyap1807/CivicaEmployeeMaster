using CivicaEmployeeMaster.Data.Contract;
using CivicaEmployeeMaster.Dtos;
using CivicaEmployeeMaster.Models;
using CivicaEmployeeMaster.Services.Implementation;
using Fare;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace CivicaEmployeeMasterTests.Services
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void GetEmployees_ReturnList_WhenNoEmployeeExist()
        {
            //Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var target = new EmployeeService(mockRepository.Object);
            //Act
            var actual = target.GetEmployees();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found!", actual.Message);
            Assert.False(actual.Success);
        }
        [Fact]
        public void GetEmployees_ReturnsEmployeesList_WhenEmployeesExist()
        {
            //Arrange
            var Employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmployeeDepartment=new EmployeeDepartment
                {

                }
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
             EmployeeDepartment=new EmployeeDepartment
                {

                }
            }
        };

            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = true,
                //Data = Employees.Select(c => new EmployeeDto { EmployeeId = c.EmployeeId, FirstName = c.FirstName }).ToList(),
            };
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(c => c.GetAllEmployee()).Returns(Employees);
            var target = new EmployeeService(mockRepository.Object);

            //Act
            var actual = target.GetEmployees();

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            mockRepository.Verify(c => c.GetAllEmployee(), Times.Exactly(1));
            Assert.Equal(Employees.Count, actual.Data.Count()); // Ensure the counts are equal

            for (int i = 0; i < Employees.Count; i++)
            {
                Assert.Equal(Employees[i].Id, actual.Data.ElementAt(i).Id);
                Assert.Equal(Employees[i].FirstName, actual.Data.ElementAt(i).FirstName);
                Assert.Equal(Employees[i].LastName, actual.Data.ElementAt(i).LastName);
            }
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsEmployees_WhenEmployeesExistAndLetterIsNull_SearchIsNull()
        {

            // Arrange
            string sortOrder = "asc";
            var Employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmployeeDepartment=new EmployeeDepartment
                {

                }
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
             EmployeeDepartment=new EmployeeDepartment
                {

                }
            }
        };
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetPaginatedEmployees(page, pageSize, null, sortOrder)).Returns(Employees);

            var EmployeeService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = EmployeeService.GetPaginatedEmployees(page, pageSize, null, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(Employees.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedEmployees(page, pageSize, null, sortOrder), Times.Once);
        }

        [Fact]
        public void GetPaginatedEmployees_ReturnsEmployees_WhenEmployeesExistAndLetterIsNull_SearchIsNotNull()
        {

            // Arrange
            string sortOrder = "asc";
            string search = "abc";
            var Employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmployeeDepartment=new EmployeeDepartment
                {

                }
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
             EmployeeDepartment=new EmployeeDepartment
                {

                }
            }
        };
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetPaginatedEmployees(page, pageSize, search, sortOrder)).Returns(Employees);

            var EmployeeService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = EmployeeService.GetPaginatedEmployees(page, pageSize, search, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(Employees.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedEmployees(page, pageSize, search, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsNoRecord_WhenEmployeesExistAndLetterIsNull_SearchIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetPaginatedEmployees(page, pageSize, null, sortOrder)).Returns<IEnumerable<Employee>>(null);

            var EmployeeService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = EmployeeService.GetPaginatedEmployees(page, pageSize, null, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedEmployees(page, pageSize, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsNoRecord_WhenEmployeesExistAndLetterIsNull_SearchIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "abc";
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetPaginatedEmployees(page, pageSize, search, sortOrder)).Returns<IEnumerable<Employee>>(null);

            var EmployeeService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = EmployeeService.GetPaginatedEmployees(page, pageSize, search, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedEmployees(page, pageSize, search, sortOrder), Times.Once);
        }

        [Fact]
        public void GetPaginatedEmployees_ReturnsEmployees_WhenEmployeesExistAndLetterIsNotNull_SearchIsNull()
        {

            // Arrange
            string sortOrder = "asc";
            var Employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmployeeDepartment=new EmployeeDepartment
                {

                }
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
             EmployeeDepartment=new EmployeeDepartment
                {

                }
            }
        };
            var letter = 'x';
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetPaginatedEmployees(page, pageSize, null, sortOrder)).Returns(Employees);

            var EmployeeService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = EmployeeService.GetPaginatedEmployees(page, pageSize, null, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(Employees.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedEmployees(page, pageSize, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsEmployees_WhenEmployeesExistAndLetterIsNotNull_SearchIsNotNull()
        {

            // Arrange
            string sortOrder = "asc";
            string search = "abc";
            var Employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmployeeDepartment=new EmployeeDepartment
                {

                }
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
             EmployeeDepartment=new EmployeeDepartment
                {

                }
            }
        };
            var letter = 'x';
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetPaginatedEmployees(page, pageSize, search, sortOrder)).Returns(Employees);

            var EmployeeService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = EmployeeService.GetPaginatedEmployees(page, pageSize, search, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(Employees.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedEmployees(page, pageSize, search, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsNoRecord_WhenEmployeesExistAndLetterIsNotNull_SearchIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            var letter = 'x';
            string sortOrder = "asc";
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetPaginatedEmployees(page, pageSize, null, sortOrder)).Returns<IEnumerable<Employee>>(null);

            var EmployeeService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = EmployeeService.GetPaginatedEmployees(page, pageSize, null, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedEmployees(page, pageSize, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsNoRecord_WhenEmployeesExistAndLetterIsNotNull_SearchIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            var letter = 'x';
            string sortOrder = "asc";
            string search = "abc";
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetPaginatedEmployees(page, pageSize, search, sortOrder)).Returns<IEnumerable<Employee>>(null);

            var EmployeeService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = EmployeeService.GetPaginatedEmployees(page, pageSize, search, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedEmployees(page, pageSize, search, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_Returns_Error_When_Search_Query_Too_Short()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object); // Assuming EmployeeService implements IEmployeeService

            var page = 1;
            var pageSize = 10;
            var searchQuery = "ab"; // Less than 3 characters

            // Act
            var result = service.GetPaginatedEmployees(page, pageSize, searchQuery, "asc");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Search query must be at least 3 characters long.", result.Message);
            Assert.Null(result.Data); // Ensure data is null when search query is invalid
        }


        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNull_SearchIsNull()
        {
            var contacts = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FirstName = "John"

            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.TotalEmployees(null)).Returns(contacts.Count);

            var contactService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = contactService.TotalEmployees(null);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalEmployees(null), Times.Once);
        }
        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNull_SearchIsNotNull()
        {
            string search = "abc";
            var contacts = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FirstName = "John"

            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.TotalEmployees(search)).Returns(contacts.Count);

            var contactService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = contactService.TotalEmployees(search);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalEmployees(search), Times.Once);
        }
        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNotNull_SearchIsNull()
        {
            var letter = 'c';
            var contacts = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FirstName = "John"

            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.TotalEmployees(null)).Returns(contacts.Count);

            var contactService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = contactService.TotalEmployees(null);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalEmployees(null), Times.Once);
        }
        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNotNull_SearchIsNotNull()
        {
            var letter = 'c';
            string search = "abc";
            var contacts = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FirstName = "John"

            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.TotalEmployees(search)).Returns(contacts.Count);

            var contactService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = contactService.TotalEmployees(search);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalEmployees(search), Times.Once);
        }

        [Fact]
        public void GetContact_ReturnsContact_WhenContactExist()
        {
            // Arrange
            var contactId = 1;
            var contact = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmployeeDepartment = new EmployeeDepartment
                {

                }

            };

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetEmployeeById(contactId)).Returns(contact);

            var contactService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = contactService.GetEmployeeById(contactId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            mockRepository.Verify(r => r.GetEmployeeById(contactId), Times.Once);
        }

        [Fact]
        public void GetContact_ReturnsNoRecord_WhenNoContactsExist()
        {
            // Arrange
            var contactId = 1;


            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetEmployeeById(contactId)).Returns<IEnumerable<Employee>>(null);

            var contactService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = contactService.GetEmployeeById(contactId);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetEmployeeById(contactId), Times.Once);
        }
        [Fact]
        public void RemoveContact_ReturnsDeletedSuccessfully_WhenDeletedSuccessfully()
        {
            var contactId = 1;


            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.DeleteEmployee(contactId)).Returns(true);

            var contactService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveEmployee(contactId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Employee Deleted Successfully", actual.Message);
            mockRepository.Verify(r => r.DeleteEmployee(contactId), Times.Once);
        }

        [Fact]
        public void RemoveContact_SomethingWentWrong_WhenDeletionFailed()
        {
            var contactId = 1;


            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.DeleteEmployee(contactId)).Returns(false);

            var contactService = new EmployeeService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveEmployee(contactId);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong please try after sometime", actual.Message);
            mockRepository.Verify(r => r.DeleteEmployee(contactId), Times.Once);
        }

        [Fact]
        public void AddContact_Successfully_Adds_New_Employee()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var newEmployee = new Employee
            {
                EmployeeEmail = "newemployee@example.com",
                BasicSalary = 15000,
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for a valid new employee
            };

            mockRepository.Setup(x => x.EmployeeExist(newEmployee.EmployeeEmail))
                          .Returns(false); // Simulate employee does not exist

            mockRepository.Setup(x => x.InsertEmployee(It.IsAny<Employee>()))
                          .Returns(true); // Simulate successful insertion

            // Act
            var result = service.AddContact(newEmployee);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Employee Saved Successfully", result.Message);
        }

        [Fact]
        public void AddContact_Fails_If_Employee_Already_Exists()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var existingEmployee = new Employee
            {
                EmployeeEmail = "existing@example.com",
                BasicSalary = 25000,
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for an existing employee
            };

            mockRepository.Setup(x => x.EmployeeExist(existingEmployee.EmployeeEmail))
                          .Returns(true); // Simulate employee already exists

            // Act
            var result = service.AddContact(existingEmployee);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Employee Already Exist", result.Message);
        }

        [Fact]
        public void AddContact_Fails_If_Invalid_Joining_Date()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var employeeWithFutureJoiningDate = new Employee
            {
                EmployeeEmail = "newemployee@example.com",
                BasicSalary = 15000,
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(1), // Future joining date
                // Add other properties as needed for an employee with invalid joining date
            };

            // Act
            var result = service.AddContact(employeeWithFutureJoiningDate);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Join date can't be a future date.", result.Message);
        }

        [Fact]
        public void AddContact_Fails_If_BasicSalary_Lower_Than_Allowance()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var employeeWithLowSalary = new Employee
            {
                EmployeeEmail = "newemployee@example.com",
                BasicSalary = 5000, // BasicSalary lower than Allowance
                Allowance = 10000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for an employee with low salary
            };

            // Act
            var result = service.AddContact(employeeWithLowSalary);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("BasicSalary should be always higher than allowance.", result.Message);
        }

        [Fact]
        public void AddContact_Fails_If_Invalid_Email_Format()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var employeeWithInvalidEmail = new Employee
            {
                EmployeeEmail = "invalidemail", // Invalid email format
                BasicSalary = 15000,
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for an employee with invalid email
            };

            // Act
            var result = service.AddContact(employeeWithInvalidEmail);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email should be in xyz@abc.com format only!", result.Message);
        }
        [Fact]
        public void AddContact_Sets_ProfTax_Correctly_For_BasicSalary_Less_Or_Equal_10000()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var newEmployee = new Employee
            {
                EmployeeEmail = "newemployee@example.com",
                BasicSalary = 10000, // BasicSalary <= 10000
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for an employee with BasicSalary <= 10000
            };

            mockRepository.Setup(x => x.EmployeeExist(newEmployee.EmployeeEmail))
                          .Returns(false); // Simulate employee does not exist

            mockRepository.Setup(x => x.InsertEmployee(It.IsAny<Employee>()))
                          .Returns(true); // Simulate successful insertion

            // Act
            var result = service.AddContact(newEmployee);

            // Assert
            Assert.Equal(100, newEmployee.ProfTax);
        }

        [Fact]
        public void AddContact_Sets_ProfTax_Correctly_For_BasicSalary_Between_10001_and_20000()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var newEmployee = new Employee
            {
                EmployeeEmail = "newemployee@example.com",
                BasicSalary = 15000, // BasicSalary between 10001 and 20000
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for an employee with BasicSalary between 10001 and 20000
            };

            mockRepository.Setup(x => x.EmployeeExist(newEmployee.EmployeeEmail))
                          .Returns(false); // Simulate employee does not exist

            mockRepository.Setup(x => x.InsertEmployee(It.IsAny<Employee>()))
                          .Returns(true); // Simulate successful insertion

            // Act
            var result = service.AddContact(newEmployee);

            // Assert
            Assert.Equal(200, newEmployee.ProfTax);
        }

        [Fact]
        public void AddContact_Sets_ProfTax_Correctly_For_BasicSalary_Between_20001_and_30000()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var newEmployee = new Employee
            {
                EmployeeEmail = "newemployee@example.com",
                BasicSalary = 25000, // BasicSalary between 20001 and 30000
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for an employee with BasicSalary between 20001 and 30000
            };

            mockRepository.Setup(x => x.EmployeeExist(newEmployee.EmployeeEmail))
                          .Returns(false); // Simulate employee does not exist

            mockRepository.Setup(x => x.InsertEmployee(It.IsAny<Employee>()))
                          .Returns(true); // Simulate successful insertion

            // Act
            var result = service.AddContact(newEmployee);

            // Assert
            Assert.Equal(300, newEmployee.ProfTax);
        }

        [Fact]
        public void AddContact_Sets_ProfTax_Correctly_For_BasicSalary_Between_30001_and_40000()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var newEmployee = new Employee
            {
                EmployeeEmail = "newemployee@example.com",
                BasicSalary = 35000, // BasicSalary between 30001 and 40000
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for an employee with BasicSalary between 30001 and 40000
            };

            mockRepository.Setup(x => x.EmployeeExist(newEmployee.EmployeeEmail))
                          .Returns(false); // Simulate employee does not exist

            mockRepository.Setup(x => x.InsertEmployee(It.IsAny<Employee>()))
                          .Returns(true); // Simulate successful insertion

            // Act
            var result = service.AddContact(newEmployee);

            // Assert
            Assert.Equal(400, newEmployee.ProfTax);
        }

        [Fact]
        public void AddContact_Sets_ProfTax_Correctly_For_BasicSalary_Between_40001_and_50000()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var newEmployee = new Employee
            {
                EmployeeEmail = "newemployee@example.com",
                BasicSalary = 45000, // BasicSalary between 40001 and 50000
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for an employee with BasicSalary between 40001 and 50000
            };

            mockRepository.Setup(x => x.EmployeeExist(newEmployee.EmployeeEmail))
                          .Returns(false); // Simulate employee does not exist

            mockRepository.Setup(x => x.InsertEmployee(It.IsAny<Employee>()))
                          .Returns(true); // Simulate successful insertion

            // Act
            var result = service.AddContact(newEmployee);

            // Assert
            Assert.Equal(500, newEmployee.ProfTax);
        }

        [Fact]
        public void AddContact_Sets_ProfTax_Correctly_For_BasicSalary_Greater_Than_50000()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var newEmployee = new Employee
            {
                EmployeeEmail = "newemployee@example.com",
                BasicSalary = 60000, // BasicSalary greater than 50000
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for an employee with BasicSalary greater than 50000
            };

            mockRepository.Setup(x => x.EmployeeExist(newEmployee.EmployeeEmail))
                          .Returns(false); // Simulate employee does not exist

            mockRepository.Setup(x => x.InsertEmployee(It.IsAny<Employee>()))
                          .Returns(true); // Simulate successful insertion

            // Act
            var result = service.AddContact(newEmployee);

            // Assert
            Assert.Equal(1000, newEmployee.ProfTax);
        }
        [Fact]
        public void AddContact_Returns_Failure_Response_When_Employee_Already_Exists()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var existingEmployee = new Employee
            {
                EmployeeEmail = "existingemployee@example.com",
                BasicSalary = 15000,
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for an existing employee
            };

            mockRepository.Setup(x => x.EmployeeExist(existingEmployee.EmployeeEmail))
                          .Returns(true); // Simulate employee already exists

            // Act
            var result = service.AddContact(existingEmployee);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Employee Already Exist", result.Message);
        }

        [Fact]
        public void AddContact_Returns_Failure_Response_When_Insertion_Fails()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var newEmployee = new Employee
            {
                EmployeeEmail = "newemployee@example.com",
                BasicSalary = 15000,
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed for a new employee
            };

            mockRepository.Setup(x => x.EmployeeExist(newEmployee.EmployeeEmail))
                          .Returns(false); // Simulate employee does not exist

            mockRepository.Setup(x => x.InsertEmployee(It.IsAny<Employee>()))
                          .Returns(false); // Simulate insertion failure

            // Act
            var result = service.AddContact(newEmployee);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong. Please try later", result.Message);
        }

        [Fact]
        public void AddContact_Returns_Failure_Response_For_Invalid_Email_Format()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            var newEmployee = new Employee
            {
                EmployeeEmail = "invalidemail", // Invalid email format
                BasicSalary = 15000,
                Allowance = 5000,
                DateOfJoining = DateTime.Now.AddMonths(-1),
                // Add other properties as needed
            };

            // Act
            var result = service.AddContact(newEmployee);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email should be in xyz@abc.com format only!", result.Message);
        }
        [Fact]
        public void GetTotalSalaryByDepartmentAndYear_ValidData_ReturnsSuccessResponseWithData()
        {
            int year = 2000;
            // Arrange
            var expectedTotals = new List<SalaryHeadTotal>
        {
            new SalaryHeadTotal { Head = "Head1", Year = 2023, TotalSalary = 50000.00m /* add other properties */ },
            new SalaryHeadTotal { Head = "Head2", Year = 2023, TotalSalary = 60000.00m /* add other properties */ }
        };

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetTotalSalaryByDepartmentAndYear(year)).Returns(expectedTotals);

            var salaryService = new EmployeeService(employeeRepositoryMock.Object);

            // Act
            var response = salaryService.GetTotalSalaryByDepartmentAndYear(year);

            // Assert
            Assert.True(response.Success);
            Assert.Equal("Success", response.Message);
            Assert.NotNull(response.Data);

            var salaryHeadTotals = response.Data.ToList();
            Assert.Equal(expectedTotals.Count, salaryHeadTotals.Count);

            for (int i = 0; i < expectedTotals.Count; i++)
            {
                Assert.Equal(expectedTotals[i].Head, salaryHeadTotals[i].Head);
                Assert.Equal(expectedTotals[i].Year, salaryHeadTotals[i].Year);
                Assert.Equal(expectedTotals[i].TotalSalary, salaryHeadTotals[i].TotalSalary);
                // Assert other properties as needed
            }
        }
        [Fact]
        public void GetTotalSalaryByDepartmentAndYear_NoData_ReturnsFailureResponse()
        {
            int year = 2000;
            // Arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetTotalSalaryByDepartmentAndYear(year)).Returns(new List<SalaryHeadTotal>());

            var salaryService = new EmployeeService(employeeRepositoryMock.Object);

            // Act
            var response = salaryService.GetTotalSalaryByDepartmentAndYear(year);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("No salary data found.", response.Message);
            Assert.Null(response.Data);
        }

        [Fact]
        public void GetTotalSalaryByDepartmentAndYear_ExceptionThrown_ReturnsFailureResponse()
        {
            int year = 2000;
            // Arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetTotalSalaryByDepartmentAndYear(year)).Throws(new Exception("Simulated exception"));

            var salaryService = new EmployeeService(employeeRepositoryMock.Object);

            // Act
            var response = salaryService.GetTotalSalaryByDepartmentAndYear(year);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Error: Simulated exception", response.Message);
            Assert.Null(response.Data);
        }
        [Fact]
        public void GetTotalSalaryByYear_ValidData_ReturnsSuccessResponseWithData()
        {
            int year = 2000;
            // Arrange
            var expectedTotals = new List<SalaryHeadTotal>
        {
            new SalaryHeadTotal { Month = 1, Year = 2023, TotalSalary = 50000.00m, /* add other properties */ },
            new SalaryHeadTotal { Month = 2, Year = 2023, TotalSalary = 60000.00m, /* add other properties */ }
        };

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetTotalSalaryByYear(year)).Returns(expectedTotals);

            var salaryService = new EmployeeService(employeeRepositoryMock.Object);

            // Act
            var response = salaryService.GetTotalSalaryByYear(year);

            // Assert
            Assert.True(response.Success);
            Assert.Equal("Success", response.Message);
            Assert.NotNull(response.Data);

            var salaryHeadTotals = response.Data.ToList();
            Assert.Equal(expectedTotals.Count, salaryHeadTotals.Count);

            for (int i = 0; i < expectedTotals.Count; i++)
            {
                var expectedMonthsWorked = 12 - (expectedTotals[i].Month - 1);
                Assert.Equal(expectedTotals[i].Month, salaryHeadTotals[i].Month);
                Assert.Equal(expectedTotals[i].Year, salaryHeadTotals[i].Year);
                Assert.Equal(expectedTotals[i].TotalSalary * expectedMonthsWorked, salaryHeadTotals[i].TotalSalary);
                // Assert other properties as needed
            }
        }
       [Fact]
        public void GetTotalSalaryByYear_ValidDataWithmonthsWorked12_ReturnsSuccessResponseWithData()
        {
            int year = 2024;
            // Arrange
            var expectedTotals = new List<SalaryHeadTotal>
        {
            new SalaryHeadTotal { Month = 1, Year = 2023, TotalSalary = 50000.00m, /* add other properties */ },
            new SalaryHeadTotal { Month = 2, Year = 2023, TotalSalary = 60000.00m, /* add other properties */ }
        };

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetTotalSalaryByYear(year)).Returns(expectedTotals);

            var salaryService = new EmployeeService(employeeRepositoryMock.Object);

            // Act
            var response = salaryService.GetTotalSalaryByYear(year);

            // Assert
            Assert.True(response.Success);
            Assert.Equal("Success", response.Message);
            Assert.NotNull(response.Data);

            var salaryHeadTotals = response.Data.ToList();
            Assert.Equal(expectedTotals.Count, salaryHeadTotals.Count);

            for (int i = 0; i < expectedTotals.Count; i++)
            {
                var expectedMonthsWorked = 12 ;
                Assert.Equal(expectedTotals[i].Month, salaryHeadTotals[i].Month);
                Assert.Equal(expectedTotals[i].Year, salaryHeadTotals[i].Year);
                Assert.Equal(expectedTotals[i].TotalSalary * expectedMonthsWorked, salaryHeadTotals[i].TotalSalary);
                // Assert other properties as needed
            }
        }
        [Fact]
        public void GetTotalSalaryByYear_NoData_ReturnsFailureResponse()
        {
            int year = 2000;
            // Arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetTotalSalaryByYear(year)).Returns(new List<SalaryHeadTotal>());

            var salaryService = new EmployeeService(employeeRepositoryMock.Object);

            // Act
            var response = salaryService.GetTotalSalaryByYear(year);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("No salary data found.", response.Message);
            Assert.Null(response.Data);
        }

        [Fact]
        public void GetTotalSalaryByYear_ExceptionThrown_ReturnsFailureResponse()
        {
            int year = 2000;
            // Arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetTotalSalaryByYear(year)).Throws(new Exception("Simulated exception"));

            var salaryService = new EmployeeService(employeeRepositoryMock.Object);

            // Act
            var response = salaryService.GetTotalSalaryByYear(year);

            // Assert
            Assert.False(response.Success);
            Assert.StartsWith("Error:", response.Message); // StartsWith used because exception message may vary
            Assert.Null(response.Data);
        }
        [Fact]
        public void GetTotalSalaryByMonth_ValidInput_ReturnsSuccessResponseWithData()
        {
            // Arrange
            int targetMonth = 6;
            int targetYear = 2024;

            // Mocking the repository
            var mockRepository = new Mock<IEmployeeRepository>();
            var expectedTotals = new List<SalaryHeadTotal>
            {
                new SalaryHeadTotal { Year = 2024, Month = 6, TotalSalary = 5000, BasicSalary = 4000, GrossSalary = 4500, PfDeduction = 200, Allowance = 500, GrossDeductions = 300, HRA = 300, ProfTax = 100 },
                new SalaryHeadTotal { Year = 2024, Month = 6, TotalSalary = 5000, BasicSalary = 4000, GrossSalary = 4500, PfDeduction = 200, Allowance = 500, GrossDeductions = 300, HRA = 300, ProfTax = 100 }
                // Add more sample data as needed
            };
            mockRepository.Setup(repo => repo.GetTotalSalaryByMonth(targetMonth, targetYear)).Returns(expectedTotals);

            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalSalaryByMonth(targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.Equal(expectedTotals.Count, result.Data.Count());

            // Check specific values for each SalaryHeadTotal
            foreach (var expectedTotal in expectedTotals)
            {
                var actualTotal = result.Data.FirstOrDefault(t => t.Year == expectedTotal.Year && t.Month == expectedTotal.Month);
                Assert.NotNull(actualTotal);
                Assert.Equal(expectedTotal.TotalSalary, actualTotal.TotalSalary);
                Assert.Equal(expectedTotal.BasicSalary, actualTotal.BasicSalary);
                Assert.Equal(expectedTotal.GrossSalary, actualTotal.GrossSalary);
                Assert.Equal(expectedTotal.PfDeduction, actualTotal.PfDeduction);
                Assert.Equal(expectedTotal.Allowance, actualTotal.Allowance);
                Assert.Equal(expectedTotal.GrossDeductions, actualTotal.GrossDeductions);
                Assert.Equal(expectedTotal.HRA, actualTotal.HRA);
                Assert.Equal(expectedTotal.ProfTax, actualTotal.ProfTax);
            }
        }
        [Fact]
        public void GetTotalSalaryByMonth_InvalidYearMonth_ReturnsSuccessResponseWithMessageAsEnterValidMonth()
        {
            // Arrange
            int targetMonth = 13; // Invalid month
            int targetYear = 2024;

            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalSalaryByMonth(targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Enter a valid month and year.", result.Message);
            Assert.Null(result.Data); // No data should be returned
        }

        [Fact]
        public void GetTotalSalaryByMonth_ExceptionThrown_ReturnsErrorResponse()
        {
            // Arrange
            int targetMonth = 6;
            int targetYear = 2024;

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(repo => repo.GetTotalSalaryByMonth(targetMonth, targetYear)).Throws(new Exception("Repository exception"));

            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalSalaryByMonth(targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Error: Repository exception", result.Message);
            Assert.Null(result.Data); // No data should be returned
        }
        [Fact]
        public void GetTotalSalaryByMonth_NoSalaryData_ReturnsErrorResponse()
        {
            // Arrange
            int targetMonth = 6;
            int targetYear = 2024;

            var mockRepository = new Mock<IEmployeeRepository>();
            // Setup repository to return null or empty list for totals
            mockRepository.Setup(repo => repo.GetTotalSalaryByMonth(targetMonth, targetYear)).Returns((IEnumerable<SalaryHeadTotal>)null);

            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalSalaryByMonth(targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("No salary data found.", result.Message);
            Assert.Null(result.Data); // No data should be returned
        }
        [Fact]
        public void GetTotalSalaryByYear_ValidInput_ReturnsSuccessResponseWithData()
        {
            // Arrange
            int targetMonth = 6;
            int targetYear = 2024;

            // Mocking the repository
            var mockRepository = new Mock<IEmployeeRepository>();
            var expectedTotals = new List<TotalProfTax>
            {
                new TotalProfTax { Year = 2024, Month = 6, ProfTax = 100 },
                new TotalProfTax { Year = 2024, Month = 6,  ProfTax = 100 }
                // Add more sample data as needed
            };
            mockRepository.Setup(repo => repo.GetTotalProfTaxByMonth(targetMonth, targetYear)).Returns(expectedTotals);

            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalProfTaxByMonth(targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.Equal(expectedTotals.Count, result.Data.Count());

            // Check specific values for each SalaryHeadTotal
            foreach (var expectedTotal in expectedTotals)
            {
                var actualTotal = result.Data.FirstOrDefault(t => t.Year == expectedTotal.Year && t.Month == expectedTotal.Month);
                Assert.NotNull(actualTotal);
                Assert.Equal(expectedTotal.ProfTax, actualTotal.ProfTax);
            }
        }
        [Fact]
        public void GetTotalProfTaxByMonth_InvalidYearMonth_ReturnsErrorResponse()
        {
            // Arrange
            int targetMonth = 13; // Invalid month
            int targetYear = 2024;

            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalProfTaxByMonth(targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Enter a valid month and year.", result.Message);
            Assert.Null(result.Data); // No data should be returned
        }
        [Fact]
        public void GetTotalProfTaxByMonth_ExceptionThrown_ReturnsErrorResponse()
        {
            // Arrange
            int targetMonth = 6;
            int targetYear = 2024;

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(repo => repo.GetTotalProfTaxByMonth(targetMonth, targetYear)).Throws(new Exception("Repository exception"));

            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalProfTaxByMonth(targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Error: Repository exception", result.Message);
            Assert.Null(result.Data); // No data should be returned
        }
        [Fact]
        public void GetTotalProfTaxByMonth_NoSalaryData_ReturnsErrorResponse()
        {
            // Arrange
            int targetMonth = 6;
            int targetYear = 2024;

            var mockRepository = new Mock<IEmployeeRepository>();
            // Setup repository to return null or empty list for totals
            mockRepository.Setup(repo => repo.GetTotalProfTaxByMonth(targetMonth, targetYear)).Returns((IEnumerable<TotalProfTax>)null);

            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalProfTaxByMonth(targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("No prof. tax data found.", result.Message);
            Assert.Null(result.Data); // No data should be returned
        }
        // Test for successful employee update
        [Fact]
        public void UpdateEmployee_SuccessfulUpdate()
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 25000,
                Allowance = 3000,
                DateOfJoining = DateTime.Now.AddMonths(-6),
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetEmployeeById(updateEmployeeDto.Id))
                          .Returns(new Employee { Id = updateEmployeeDto.Id }); // Simulate existing employee

            mockRepository.Setup(r => r.UpdateEmployee(It.IsAny<Employee>()))
                          .Returns(true); // Simulate successful update

            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Employee updated successfully.", result.Message);
        }

        // Test for invalid department ID
        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public void UpdateEmployee_InvalidDepartmentId(int invalidDepartmentId)
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 0,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 25000,
                Allowance = 3000,
                DateOfJoining = DateTime.Now.AddMonths(-6),
            };
            var employee = new Employee
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 2000, // Basic salary less than allowance
                Allowance = 3000,
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            var employeeService = new EmployeeService(mockRepository.Object);
            mockRepository.Setup(r => r.GetEmployeeById(updateEmployeeDto.Id))
             .Returns(employee); // Simulate existing employee

            // Act
            var result = employeeService.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Please enter valid department.", result.Message);
        }

        // Test for employee already existing
        [Fact]
        public void UpdateEmployee_EmployeeAlreadyExists()
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 25000,
                Allowance = 3000,
                DateOfJoining = DateTime.Now.AddMonths(-6),
            };
            var employee = new Employee
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 2000, // Basic salary less than allowance
                Allowance = 3000,
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.EmployeesExists(updateEmployeeDto.Id, updateEmployeeDto.EmployeeEmail))
                          .Returns(true); // Simulate employee already exists
            mockRepository.Setup(r => r.GetEmployeeById(updateEmployeeDto.Id))
             .Returns(employee); // Simulate existing employee

            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Employee already exists.", result.Message);
        }
        [Fact]
        public void UpdateEmployee_NullEmployeeFromRepository()
        {
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                DateOfJoining = DateTime.Now,
                BasicSalary = 10000,    // Provide valid values as needed for testing
                Allowance = 2000,
                FirstName = "John",
                LastName = "Doe",
              };

            Employee employee = null;   // Simulate null employee

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetEmployeeById(updateEmployeeDto.Id)).Returns(employee);

            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Something went wrong. Please try after sometime.", result.Message);
        }

        // Test for future date of joining
        [Fact]
        public void UpdateEmployee_FutureDateOfJoining()
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 25000,
                Allowance = 3000,
                DateOfJoining = DateTime.Now.AddMonths(1), // Future date
            };
            var employee = new Employee
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 2000, // Basic salary less than allowance
                Allowance = 3000,
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            var employeeService = new EmployeeService(mockRepository.Object);
            mockRepository.Setup(r => r.GetEmployeeById(updateEmployeeDto.Id))
             .Returns(employee); // Simulate existing employee

            // Act
            var result = employeeService.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Join date can't be a future date.", result.Message);
        }
        // Test for future date of joining
        [Fact]
        public void UpdateEmployeee_BasicSalaryIsLessThan1000()
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 500,
                Allowance = 3000,
                DateOfJoining = DateTime.Now, // Future date
            };
            var employee = new Employee
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 500, // Basic salary less than allowance
                Allowance = 3000,
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            var employeeService = new EmployeeService(mockRepository.Object);
            mockRepository.Setup(r => r.GetEmployeeById(updateEmployeeDto.Id))
             .Returns(employee); // Simulate existing employee

            // Act
            var result = employeeService.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Basic Salary must be greater than or equal to 1000.", result.Message);
        }
        [Theory]
        [InlineData(8000, 100)]     // Test for BasicSalary <= 10000
        [InlineData(15000, 200)]    // Test for BasicSalary <= 20000
        [InlineData(25000, 300)]    // Test for BasicSalary <= 30000
        [InlineData(35000, 400)]    // Test for BasicSalary <= 40000
        [InlineData(45000, 500)]    // Test for BasicSalary <= 50000
        [InlineData(60000, 1000)]   // Test for BasicSalary > 50000 (falls into the else case)
        public void UpdateEmployee_ProfTax_CalculatedCorrectly(decimal basicSalary, decimal expectedProfTax)
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                DateOfJoining = DateTime.Now,
                BasicSalary = basicSalary,
                Allowance = 1000,   // Assuming a fixed allowance for testing
                FirstName = "John",
                LastName = "Doe",
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            var employeeService = new EmployeeService(mockRepository.Object);

            // Mock existing employee
            var existingEmployee = new Employee
            {
                Id = updateEmployeeDto.Id,
                BasicSalary = basicSalary - 1000,  // Assuming existing salary is lower by 1000
                ProfTax = 0,                      // Initial ProfTax set to 0 for testing
            };

            mockRepository.Setup(r => r.GetEmployeeById(updateEmployeeDto.Id)).Returns(existingEmployee);
            mockRepository.Setup(r => r.UpdateEmployee(It.IsAny<Employee>())).Returns(true);

            // Act
            var result = employeeService.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(expectedProfTax, existingEmployee.ProfTax);
        }

        // Test for basic salary less than allowance
        [Fact]
        public void UpdateEmployee_BasicSalaryLessThanAllowance()
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 2000, // Basic salary less than allowance
                Allowance = 3000,
                DateOfJoining = DateTime.Now.AddMonths(-6),
            };
            var employee = new Employee
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 2000, // Basic salary less than allowance
                Allowance = 3000,
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            var employeeService = new EmployeeService(mockRepository.Object);
            mockRepository.Setup(r => r.GetEmployeeById(updateEmployeeDto.Id))
                         .Returns(employee); // Simulate existing employee


            // Act
            var result = employeeService.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("BasicSalary should be always higher than allowance.", result.Message);
        }

        // Test for invalid email format
        [Fact]
        public void UpdateEmployee_InvalidEmailFormat()
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Id = 1,
                EmployeeEmail = "invalid-email-format", // Invalid email format
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 25000,
                Allowance = 3000,
                DateOfJoining = DateTime.Now.AddMonths(-6),
            };
            var employee = new Employee
            {
                Id = 1,
                EmployeeEmail = "test@example.com",
                DepartmentId = 2,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                BasicSalary = 2000, // Basic salary less than allowance
                Allowance = 3000,
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            var employeeService = new EmployeeService(mockRepository.Object);
            mockRepository.Setup(r => r.GetEmployeeById(updateEmployeeDto.Id))
                         .Returns(employee); // Simulate existing employee

            // Act
            var result = employeeService.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Email should be in xyz@abc.com format only!", result.Message);
        }

        // Test for null UpdateEmployeeDto
        [Fact]
        public void UpdateEmployee_NullUpdateEmployeeDto()
        {
            // Arrange
            UpdateEmployeeDto updateEmployeeDto = null;

            var mockRepository = new Mock<IEmployeeRepository>();
            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Something went wrong. Please try after sometime.", result.Message);
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_Success()
        {
            // Arrange
            int employeeId = 1;
            int year = DateTime.Now.Year; // Use current year for testing

            // Mocking the repository response
            var mockRepository = new Mock<IEmployeeRepository>();
            var mockEmployee = new Employee
            {
                Id = employeeId,
                DateOfJoining = new DateTime(2020, 1, 15), // Mock employee joining date
            };
            var mockTotals = new List<SalaryHeadTotal>
            {
                new SalaryHeadTotal
                {
                    Month = 6,
                    Year = year,
                    TotalSalary = 5000,
                    GrossDeductions = 1000,
                    Allowance = 200,
                    PfDeduction = 300,
                    BasicSalary = 4000,
                    GrossSalary = 6000,
                    HRA = 500,
                    ProfTax = 100
                },
                new SalaryHeadTotal
                {
                    Month = 7,
                    Year = year,
                    TotalSalary = 6000,
                    GrossDeductions = 1200,
                    Allowance = 240,
                    PfDeduction = 360,
                    BasicSalary = 4800,
                    GrossSalary = 7200,
                    HRA = 600,
                    ProfTax = 120
                }
            };
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Returns(mockEmployee);
            mockRepository.Setup(r => r.GetTotalSalaryByYearAndId(employeeId, year)).Returns(mockTotals);

            // Creating the service with mocked repository
            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);

            var salaryHeadTotals = result.Data.ToList();
            Assert.Equal(2, salaryHeadTotals.Count);

            // Assert values for the first salary head total
            var firstTotal = salaryHeadTotals[0];
            Assert.Equal(6, firstTotal.Month);
            Assert.Equal(year, firstTotal.Year);
            Assert.Equal(30000, firstTotal.TotalSalary); // 5000 * 6
            Assert.Equal(6000, firstTotal.GrossDeductions); // 1000 * 6
            Assert.Equal(1200, firstTotal.Allowance); // 200 * 6
            Assert.Equal(1800, firstTotal.PfDeduction); // 300 * 6
            Assert.Equal(24000, firstTotal.BasicSalary); // 4000 * 6
            Assert.Equal(36000, firstTotal.GrossSalary); // 6000 * 6
            Assert.Equal(3000, firstTotal.HRA); // 500 * 6
            Assert.Equal(600, firstTotal.ProfTax); // 100 * 6

            //Assert values for the second salary head total

           var secondTotal = salaryHeadTotals[1];
            Assert.Equal(7, secondTotal.Month);
            Assert.Equal(year, secondTotal.Year);
            Assert.Equal(36000, secondTotal.TotalSalary); // 6000 * 6
            Assert.Equal(7200, secondTotal.GrossDeductions); // 1200 * 6
            Assert.Equal(1440, secondTotal.Allowance); // 240 * 6
            Assert.Equal(2160, secondTotal.PfDeduction); // 360 * 6
            Assert.Equal(28800, secondTotal.BasicSalary); // 4800 * 6
            Assert.Equal(43200, secondTotal.GrossSalary); // 7200 * 6
            Assert.Equal(3600, secondTotal.HRA); // 600 * 6
            Assert.Equal(720, secondTotal.ProfTax); // 120 * 6
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_EmployeeNotFound()
        {
            // Arrange
            int employeeId = 1;
            int year = DateTime.Now.Year;

            // Mocking the repository response
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Returns((Employee)null);

            // Creating the service with mocked repository
            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.False(result.Success);
            Assert.Equal($"Employee with ID {employeeId} not found.", result.Message);
            Assert.Null(result.Data);
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_NoSalaryData()
        {
            // Arrange
            int employeeId = 1;
            int year = DateTime.Now.Year;

            // Mocking the repository response
            var mockRepository = new Mock<IEmployeeRepository>();
            var mockEmployee = new Employee
            {
                Id = employeeId,
                DateOfJoining = new DateTime(2020, 1, 15),
            };
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Returns(mockEmployee);
            mockRepository.Setup(r => r.GetTotalSalaryByYearAndId(employeeId, year)).Returns(new List<SalaryHeadTotal>());

            // Creating the service with mocked repository
            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No salary data found for the specified employee or year.", result.Message);
            Assert.Null(result.Data);
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_Exception()
        {
            // Arrange
            int employeeId = 1;
            int year = DateTime.Now.Year;

            // Mocking the repository response to throw an exception
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Throws(new Exception("Repository exception"));

            // Creating the service with mocked repository
            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.False(result.Success);
            Assert.StartsWith("Error:", result.Message); // Message should start with "Error:"
            Assert.Null(result.Data);
            // Additional assertion can include logging verification if logging is implemented
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_CalculateMonthsWorked_CurrentYearAndMonthAfterJoining()
        {
            // Arrange
            int employeeId = 1;
            int year = 2024; // Arbitrary year
            int currentYear = 2024; // Current year for testing
            int currentMonth = 6; // Current month for testing

            var mockRepository = new Mock<IEmployeeRepository>();
            var mockEmployee = new Employee
            {
                Id = employeeId,
                DateOfJoining = new DateTime(2020, 1, 15), // Joining date of employee
            };
            var mockTotals = new List<SalaryHeadTotal>
            {
                new SalaryHeadTotal { Month = 5, Year = 2024 }, // May 2024
                new SalaryHeadTotal { Month = 6, Year = 2024 }, // June 2024
                new SalaryHeadTotal { Month = 7, Year = 2024 }, // July 2024
            };
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Returns(mockEmployee);
            mockRepository.Setup(r => r.GetTotalSalaryByYearAndId(employeeId, year)).Returns(mockTotals);

            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);

            var salaryHeadTotals = result.Data.ToList();
            Assert.Equal(3, salaryHeadTotals.Count);
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_CalculateMonthsWorked_YearBetweenJoiningAndRequested()
        {
            // Arrange
            int employeeId = 1;
            int year = 2024; // Arbitrary year
            int currentYear = 2024; // Current year for testing

            var mockRepository = new Mock<IEmployeeRepository>();
            var mockEmployee = new Employee
            {
                Id = employeeId,
                DateOfJoining = new DateTime(2020, 1, 15), // Joining date of employee
            };
            var mockTotals = new List<SalaryHeadTotal>
            {
                new SalaryHeadTotal { Month = 1, Year = 2021 }, // January 2021
                new SalaryHeadTotal { Month = 2, Year = 2022 }, // February 2022
                new SalaryHeadTotal { Month = 3, Year = 2023 }, // March 2023
            };
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Returns(mockEmployee);
            mockRepository.Setup(r => r.GetTotalSalaryByYearAndId(employeeId, year)).Returns(mockTotals);

            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);

            var salaryHeadTotals = result.Data.ToList();
            Assert.Equal(3, salaryHeadTotals.Count);
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_CalculateMonthsWorked_YearGreaterThanJoiningYear()
        {
            // Arrange
            int employeeId = 1;
            int year = 2023; // Arbitrary year
            int currentYear = 2024; // Current year for testing

            var mockRepository = new Mock<IEmployeeRepository>();
            var mockEmployee = new Employee
            {
                Id = employeeId,
                DateOfJoining = new DateTime(2020, 1, 15), // Joining date of employee
            };
            var mockTotals = new List<SalaryHeadTotal>
            {
                new SalaryHeadTotal { Month = 1, Year = 2021 }, // January 2021
                new SalaryHeadTotal { Month = 2, Year = 2022 }, // February 2022
                new SalaryHeadTotal { Month = 3, Year = 2023 }, // March 2023
            };
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Returns(mockEmployee);
            mockRepository.Setup(r => r.GetTotalSalaryByYearAndId(employeeId, year)).Returns(mockTotals);

            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);

            var salaryHeadTotals = result.Data.ToList();
            Assert.Equal(3, salaryHeadTotals.Count);
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_CalculateMonthsWorked_YearLessThanJoiningYear()
        {
            // Arrange
            int employeeId = 1;
            int year = 2019; // Arbitrary year
            int currentYear = 2024; // Current year for testing

            var mockRepository = new Mock<IEmployeeRepository>();
            var mockEmployee = new Employee
            {
                Id = employeeId,
                DateOfJoining = new DateTime(2020, 1, 15), // Joining date of employee
            };
            var mockTotals = new List<SalaryHeadTotal>
            {
                new SalaryHeadTotal { Month = 1, Year = 2021 }, // January 2021
                new SalaryHeadTotal { Month = 2, Year = 2022 }, // February 2022
                new SalaryHeadTotal { Month = 3, Year = 2023 }, // March 2023
            };
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Returns(mockEmployee);
            mockRepository.Setup(r => r.GetTotalSalaryByYearAndId(employeeId, year)).Returns(mockTotals);

            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);

            var salaryHeadTotals = result.Data.ToList();
            Assert.Equal(3, salaryHeadTotals.Count);
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_CalculateMonthsWorked_RequestedYear()
        {
            // Arrange
            int employeeId = 1;
            int year = 2024; // Arbitrary year
            int currentYear = 2024; // Current year for testing

            var mockRepository = new Mock<IEmployeeRepository>();
            var mockEmployee = new Employee
            {
                Id = employeeId,
                DateOfJoining = new DateTime(2020, 1, 15), // Joining date of employee
            };
            var mockTotals = new List<SalaryHeadTotal>
            {
                new SalaryHeadTotal { Month = 1, Year = 2024 }, // January 2024
                new SalaryHeadTotal { Month = 2, Year = 2024 }, // February 2024
                new SalaryHeadTotal { Month = 3, Year = 2024 }, // March 2024
            };
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Returns(mockEmployee);
            mockRepository.Setup(r => r.GetTotalSalaryByYearAndId(employeeId, year)).Returns(mockTotals);

            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);

            var salaryHeadTotals = result.Data.ToList();
            Assert.Equal(3, salaryHeadTotals.Count);
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_CalculateMonthsWorked_BeforeJoiningOrAfterRequested()
        {
            // Arrange
            int employeeId = 1;
            int year = 2024; // Arbitrary year
            int currentYear = 2024; // Current year for testing

            var mockRepository = new Mock<IEmployeeRepository>();
            var mockEmployee = new Employee
            {
                Id = employeeId,
                DateOfJoining = new DateTime(2022, 1, 1), // Joining date of employee
            };
            var mockTotals = new List<SalaryHeadTotal>
            {
                new SalaryHeadTotal { Month = 1, Year = 2021 }, // January 2021
                new SalaryHeadTotal { Month = 2, Year = 2025 }, // February 2025
            };
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Returns(mockEmployee);
            mockRepository.Setup(r => r.GetTotalSalaryByYearAndId(employeeId, year)).Returns(mockTotals);

            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);

            var salaryHeadTotals = result.Data.ToList();
            Assert.Equal(2, salaryHeadTotals.Count);

        }
        [Fact]
        public void GetTotalSalaryByYearAndId_CalculateMonthsWorked_SameJoiningYearAsRequestedYear()
        {
            // Arrange
            int employeeId = 1;
            int year = 2024; // Arbitrary year
            int currentYear = 2024; // Current year for testing
            int currentMonth = 6; // Current month for testing

            var mockRepository = new Mock<IEmployeeRepository>();
            var mockEmployee = new Employee
            {
                Id = employeeId,
                DateOfJoining = new DateTime(2024, 1, 15), // Joining date of employee (same year as requested year)
            };
            var mockTotals = new List<SalaryHeadTotal>
            {
                new SalaryHeadTotal { Month = 1, Year = 2024 }, // January 2024
                new SalaryHeadTotal { Month = 2, Year = 2024 }, // February 2024
                new SalaryHeadTotal { Month = 3, Year = 2024 }, // March 2024
            };
            mockRepository.Setup(r => r.GetEmployeeById(employeeId)).Returns(mockEmployee);
            mockRepository.Setup(r => r.GetTotalSalaryByYearAndId(employeeId, year)).Returns(mockTotals);

            var employeeService = new EmployeeService(mockRepository.Object);

            // Act
            var result = employeeService.GetTotalSalaryByYearAndId(employeeId, year);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);

            var salaryHeadTotals = result.Data.ToList();
            Assert.Equal(3, salaryHeadTotals.Count);
        }


        [Fact]
        public void GetTotalSalaryByMonthYearAndId_InvalidYearMonth_ReturnsErrorResponse()
        {
            // Arrange
            int employeeId = 1;
            int targetMonth = 13; // Invalid month
            int targetYear = 2024;

            var mockRepository = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalSalaryByMonthYearAndId(employeeId, targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Enter a valid month and year.", result.Message);
            Assert.Null(result.Data); // No data should be returned
        }


        [Fact]
        public void GetTotalSalaryByMonthYearAndId_ExceptionThrown_ReturnsErrorResponse()
        {
            // Arrange
            int employeeId = 1;
            int targetMonth = 6;
            int targetYear = 2024;

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(repo => repo.GetTotalSalaryByMonthYearAndId(employeeId,targetMonth, targetYear)).Throws(new Exception("Repository exception"));

            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalSalaryByMonthYearAndId(employeeId,targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Error: Repository exception", result.Message);
            Assert.Null(result.Data); // No data should be returned
        }
        [Fact]
        public void GetTotalSalaryByMonthYearAndId_NoSalaryData_ReturnsErrorResponse()
        {
            // Arrange
            int employeeId = 1;
            int targetMonth = 6;
            int targetYear = 2024;

            var mockRepository = new Mock<IEmployeeRepository>();
            // Setup repository to return null or empty list for totals
            mockRepository.Setup(repo => repo.GetTotalSalaryByMonthYearAndId(employeeId,targetMonth, targetYear)).Returns((IEnumerable<SalaryHeadTotal>)null);

            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalSalaryByMonthYearAndId(employeeId,targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("No salary data found for the specified employee.", result.Message);
            Assert.Null(result.Data); // No data should be returned
        }

        [Fact]
        public void GetTotalSalaryByMonthYearAndId_ReturnsSalaryData()
        {
            // Arrange
            int employeeId = 1;
            int targetMonth = 5;
            int targetYear = 2024;
            var salaryHead = new List<SalaryHeadTotal>
            {
                new SalaryHeadTotal
                {
                    Month =5,
                    Year = 2024,
                    TotalSalary = 125000,
                    GrossDeductions = 1000,
                    Allowance = 500,
                    PfDeduction = 1000,
                    BasicSalary = 100000,
                    GrossSalary = 100000,
                    HRA = 1000,
                    ProfTax = 2000,
                }
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            // Setup repository to return null or empty list for totals
            mockRepository.Setup(repo => repo.GetTotalSalaryByMonthYearAndId(employeeId, targetMonth, targetYear)).Returns(salaryHead);

            var service = new EmployeeService(mockRepository.Object);

            // Act
            var result = service.GetTotalSalaryByMonthYearAndId(employeeId, targetMonth, targetYear);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);
        }

    }

}
