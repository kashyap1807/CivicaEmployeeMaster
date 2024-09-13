using AutoFixture;
using CivicaEmployeeMaster.Controllers;
using CivicaEmployeeMaster.Dtos;
using CivicaEmployeeMaster.Models;
using CivicaEmployeeMaster.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicaEmployeeMasterTests.Controllers
{
    public class EmployeeControllerTests
    {
        [Fact]
        public void GetAllEmployees_ReturnsOkWithEmployees_WhenEmployeesExists()
        {
            //Arrange

            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = true,
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetEmployees()).Returns(response);

            //Act
            var actual = target.GetAllEmployees() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetEmployees(), Times.Once);
        }

        [Fact]
        public void GetAllEmployees_ReturnsNotFound_WhenNoEmployeeExists()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = false,
                Data = new List<EmployeeDto>(),

            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetEmployees()).Returns(response);

            //Act
            var actual = target.GetAllEmployees() as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetEmployees(), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsOkWithEmployees_WhenLetterIsNull_SearchIsNull()
        {
            //Arrange
            var Employees = new List<Employee>
            {
               new Employee{Id=1,FirstName="Employee 1"},
                 new Employee{Id=2,FirstName="Employee 2"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = true,
                //Data = Employees.Select(c => new EmployeeDto { Id = c.Id, FirstName = c.FirstName, EmployeeNumber = c.EmployeeNumber }) // Convert to EmployeeDto
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetPaginatedEmployees(page, pageSize, null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedEmployees(null, page, pageSize) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetPaginatedEmployees(page, pageSize,null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsOkWithEmployees_WhenLetterIsNull_SearchIsNotNull()
        {
            //Arrange
            var Employees = new List<Employee>
            {
               new Employee{Id=1,FirstName="Employee 1"},
                 new Employee{Id=2,FirstName="Employee 2"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "tac";
            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = true,
                // Data = Employees.Select(c => new EmployeeDto { Id = c.Id, FirstName = c.FirstName, EmployeeNumber = c.EmployeeNumber }) // Convert to EmployeeDto
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetPaginatedEmployees(page, pageSize,search, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedEmployees(search, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetPaginatedEmployees(page, pageSize,search, sortOrder), Times.Once);
        }

        [Fact]
        public void GetPaginatedEmployees_ReturnsOkWithEmployees_WhenLetterIsNotNull_SearchIsNull()
        {
            //Arrange
            var Employees = new List<Employee>
            {
               new Employee{Id=1,FirstName="Employee 1"},
                 new Employee{Id=2,FirstName="Employee 2"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = true,
                //Data = Employees.Select(c => new EmployeeDto { Id = c.Id, FirstName = c.FirstName, EmployeeNumber = c.EmployeeNumber }) // Convert to EmployeeDto
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetPaginatedEmployees(page, pageSize, null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedEmployees( null, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetPaginatedEmployees(page, pageSize, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsOkWithEmployees_WhenLetterIsNotNull_SearchIsNotNull()
        {
            //Arrange
            var Employees = new List<Employee>
            {
               new Employee{Id=1,FirstName="Employee 1"},
                 new Employee{Id=2,FirstName="Employee 2"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "dev";
            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = true,
                //Data = Employees.Select(c => new EmployeeDto { Id = c.Id, FirstName = c.FirstName, EmployeeNumber = c.EmployeeNumber }) // Convert to EmployeeDto
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetPaginatedEmployees(page, pageSize,search, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedEmployees(search, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetPaginatedEmployees(page, pageSize,search, sortOrder), Times.Once);
        }

        [Fact]
        public void GetPaginatedEmployees_ReturnsNotFound_WhenLetterIsNull_SearchIsNull()
        {
            //Arrange
            var Employees = new List<Employee>
            {
               new Employee{Id=1,FirstName="Employee 1"},
                 new Employee{Id=2,FirstName="Employee 2"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = false,
                //Data = Employees.Select(c => new EmployeeDto { Id = c.Id, FirstName = c.FirstName, EmployeeNumber = c.EmployeeNumber }) // Convert to EmployeeDto
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetPaginatedEmployees(page, pageSize,null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedEmployees(null, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetPaginatedEmployees(page, pageSize,null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsNotFound_WhenLetterIsNull_SearchIsNotNull()
        {
            //Arrange
            var Employees = new List<Employee>
            {
               new Employee{Id=1,FirstName="Employee 1"},
                 new Employee{Id=2,FirstName="Employee 2"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "dev";
            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = false,
                //Data = Employees.Select(c => new EmployeeDto { Id = c.Id, FirstName = c.FirstName, EmployeeNumber = c.EmployeeNumber }) // Convert to EmployeeDto
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetPaginatedEmployees(page, pageSize, search, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedEmployees(search, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetPaginatedEmployees(page, pageSize,search, sortOrder), Times.Once);
        }

        [Fact]
        public void GetPaginatedEmployees_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNull()
        {
            //Arrange
            var Employees = new List<Employee>
            {
               new Employee{Id=1,FirstName="Employee 1"},
                 new Employee{Id=2,FirstName="Employee 2"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = false,
                //Data = Employees.Select(c => new EmployeeDto { Id = c.Id, FirstName = c.FirstName, EmployeeNumber = c.EmployeeNumber }) // Convert to EmployeeDto
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetPaginatedEmployees(page, pageSize,null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedEmployees(null, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetPaginatedEmployees(page, pageSize,null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedEmployees_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNotNull()
        {
            //Arrange
            var Employees = new List<Employee>
            {
               new Employee{Id=1,FirstName="Employee 1"},
                 new Employee{Id=2,FirstName="Employee 2"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "dev";

            var response = new ServiceResponse<IEnumerable<EmployeeDto>>
            {
                Success = false,
                // Data = Employees.Select(c => new EmployeeDto { Id = c.Id, FirstName = c.FirstName, EmployeeNumber = c.EmployeeNumber }) // Convert to EmployeeDto
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetPaginatedEmployees(page, pageSize,search, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedEmployees(search, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetPaginatedEmployees(page, pageSize,search, sortOrder), Times.Once);
        }
        [Fact]
        public void GetTotalCountOfEmployees_ReturnsOkWithEmployees_WhenLetterIsNotNull_SearchIsNotNull()
        {
            //Arrange
            var Employees = new List<Employee>
             {
            new Employee{Id=1,FirstName="Employee 1"},
            new Employee{Id=2,FirstName="Employee 2"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = Employees.Count
            };
            string search = "dev";
            var letter = 'd';
            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.TotalEmployees(search)).Returns(response);

            //Act
            var actual = target.GetEmployeeCount(search) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockEmployeeService.Verify(c => c.TotalEmployees(search), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfEmployees_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = 'd';
            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.TotalEmployees(null)).Returns(response);

            //Act
            var actual = target.GetEmployeeCount(null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockEmployeeService.Verify(c => c.TotalEmployees(null), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfEmployees_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNotNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = 'd';
            string search = "dev";
            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.TotalEmployees(search)).Returns(response);

            //Act
            var actual = target.GetEmployeeCount(search) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockEmployeeService.Verify(c => c.TotalEmployees(search), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfEmployees_ReturnsNotFound_WhenLetterIsNull_SearchIsNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.TotalEmployees(null)).Returns(response);

            //Act
            var actual = target.GetEmployeeCount(null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockEmployeeService.Verify(c => c.TotalEmployees(null), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfEmployees_ReturnsNotFound_WhenLetterIsNull_SearchIsNotNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };
            string search = "dev";
            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.TotalEmployees(search)).Returns(response);

            //Act
            var actual = target.GetEmployeeCount(search) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockEmployeeService.Verify(c => c.TotalEmployees(search), Times.Once);
        }

        [Fact]
        public void GetEmployeeById_ReturnsOk()
        {

            var Id = 1;
            var Employee = new Employee
            {
                Id = Id,
                FirstName = "Employee 1"
            };

            var response = new ServiceResponse<EmployeeDto>
            {
                Success = true,
                Data = new EmployeeDto
                {
                    Id = Id,
                    FirstName = Employee.FirstName
                }
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetEmployeeById(Id)).Returns(response);

            //Act
            var actual = target.GetEmployeeById(Id) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetEmployeeById(Id), Times.Once);
        }

        [Fact]
        public void GetEmployeeById_ReturnsNotFound()
        {

            var Id = 1;
            var Employee = new Employee
            {
                Id = Id,
                FirstName = "Employee 1"
            };

            var response = new ServiceResponse<EmployeeDto>
            {
                Success = false,
                Data = new EmployeeDto
                {
                    Id = Id,
                    FirstName = Employee.FirstName
                }
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetEmployeeById(Id)).Returns(response);

            //Act
            var actual = target.GetEmployeeById(Id) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetEmployeeById(Id), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsOk_WhenContactIsUpdatesSuccessfully()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateEmployeeDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockContactService.Object);
            mockContactService.Setup(c => c.UpdateEmployee(It.IsAny<UpdateEmployeeDto>())).Returns(response);

            //Act

            var actual = target.UpdateEmployee(updateContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.UpdateEmployee(It.IsAny<UpdateEmployeeDto>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsBadRequest_WhenContactIsNotUpdated()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateEmployeeDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockContactService.Object);
            mockContactService.Setup(c => c.UpdateEmployee(It.IsAny<UpdateEmployeeDto>())).Returns(response);

            //Act

            var actual = target.UpdateEmployee(updateContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.UpdateEmployee(It.IsAny<UpdateEmployeeDto>()), Times.Once);

        }
        [Fact]
        public void AddContact_ReturnsOk_WhenContactIsAddedSuccessfully()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddEmployeeDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockContactService.Object);
            mockContactService.Setup(c => c.AddContact(It.IsAny<Employee>())).Returns(response);

            //Act

            var actual = target.AddEmployee(addContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<Employee>()), Times.Once);

        }

        [Fact]
        public void AddContact_ReturnsBadRequest_WhenContactIsNotAdded()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddEmployeeDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockContactService.Object);
            mockContactService.Setup(c => c.AddContact(It.IsAny<Employee>())).Returns(response);

            //Act

            var actual = target.AddEmployee(addContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<Employee>()), Times.Once);

        }
        [Fact]
        public void DeleteConfirmed_ReturnsOkResponse_WhenContactDeletedSuccessfully()
        {

            var contactId = 1;
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockContactService.Object);
            mockContactService.Setup(c => c.RemoveEmployee(contactId)).Returns(response);

            //Act

            var actual = target.DeleteConfirmed(contactId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveEmployee(contactId), Times.Once);
        }

        [Fact]
        public void DeleteConfirmed_ReturnsBadRequest_WhenContactNotDeleted()
        {

            var contactId = 1;
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockContactService.Object);
            mockContactService.Setup(c => c.RemoveEmployee(contactId)).Returns(response);

            //Act

            var actual = target.DeleteConfirmed(contactId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveEmployee(contactId), Times.Once);
        }

        [Fact]
        public void DeleteConfirmed_ReturnsBadRequest_WhenContactIsLessThanZero()
        {

            var contactId = 0;

            var mockContactService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockContactService.Object);

            //Act

            var actual = target.DeleteConfirmed(contactId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal("Please enter proper data", actual.Value);
        }
        [Fact]
        public void GetTotalSalaryByYear_ReturnsOkWithEmployees_WhenEmployeesExists()
        {
            int year = 2000;
            //Arrange

            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetTotalSalaryByYear(year)).Returns(response);

            //Act
            var actual = target.GetTotalSalaryByYear(year) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetTotalSalaryByYear(year), Times.Once);
        }

        [Fact]
        public void GetTotalSalaryByYear_ReturnsNotFound_WhenNoEmployeeExists()
        {
            int year = 2000;
            //Arrange
            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = false,
                Data = new List<SalaryHeadTotal>(),

            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetTotalSalaryByYear(year)).Returns(response);

            //Act
            var actual = target.GetTotalSalaryByYear(year) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetTotalSalaryByYear(year), Times.Once);
        }

        [Fact]
        public void GGetTotalSalaryByDepartmentAndYear_ReturnsOkWithEmployees_WhenEmployeesExists()
        {
            int year = 2000;
            //Arrange

            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetTotalSalaryByDepartmentAndYear(year)).Returns(response);

            //Act
            var actual = target.GetTotalSalaryByDepartmentAndYear(year) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetTotalSalaryByDepartmentAndYear(year), Times.Once);
        }

        [Fact]
        public void GetTotalSalaryByDepartmentAndYear_ReturnsNotFound_WhenNoEmployeeExists()
        {
            int year = 2000;
            //Arrange
            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = false,
                Data = new List<SalaryHeadTotal>(),

            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            var target = new EmployeeController(mockEmployeeService.Object);
            mockEmployeeService.Setup(c => c.GetTotalSalaryByDepartmentAndYear(year)).Returns(response);

            //Act
            var actual = target.GetTotalSalaryByDepartmentAndYear(year) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockEmployeeService.Verify(c => c.GetTotalSalaryByDepartmentAndYear(year), Times.Once);
        }
        [Fact]
        public void GetTotalSalaryByMonth_Returns_NotFoundObject()
        {
            // Arrange
            int month = 6;
            int year = 2024;
            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = false,
                Data = new List<SalaryHeadTotal>(),

            };

            // Mocking the IEmployeeService
            var mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeService.Setup(x => x.GetTotalSalaryByMonth(month, year))
                               .Returns(response);

            // Creating the controller instance with mocked service
            var controller = new EmployeeController(mockEmployeeService.Object);

            // Act
            IActionResult actionResult = controller.GetTotalSalaryByMonth(month, year);

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult); // Asserting that the result is OkObjectResult

            //var okResult = actionResult as OkObjectResult;
            //var responseData = okResult.Value as ServiceResponse<decimal>;

            Assert.NotNull(response); // Asserting that responseData is not null
            Assert.False(response.Success); // Asserting that Success is true
            //Assert.Equal(response); // Asserting the expected salary value
        }
        [Fact]
        public void GetTotalSalaryByMonth_Returns_OkObject()
        {
            int month = 6;
            int year = 2024;
            var response = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
                Data = new List<SalaryHeadTotal>(),
                Message="Data not found"

            };

            // Mocking the IEmployeeService
            var mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeService.Setup(x => x.GetTotalSalaryByMonth(month, year))
                               .Returns(response);

            // Creating the controller instance with mocked service
            var controller = new EmployeeController(mockEmployeeService.Object);

            // Act
            IActionResult actionResult = controller.GetTotalSalaryByMonth(month, year);

            // Assert
            Assert.IsType<OkObjectResult>(actionResult); // Asserting that the result is NotFoundObjectResult

            var notFoundResult = actionResult as NotFoundObjectResult;

            Assert.NotNull(response); // Asserting that responseData is not null
            Assert.True(response.Success); // Asserting that Success is false
            Assert.Equal("Data not found", response.Message); // Asserting the expected error message
        }
        [Fact]
        public void GetTotalProfTaxByMonth_Success_Returns_OkObject()
        {
            // Arrange
            int month = 6;
            int year = 2024;
            var successresponse = new ServiceResponse<IEnumerable<TotalProfTax>>
            {
                Success = true,
                Data = new List<TotalProfTax>(),

            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeService.Setup(x => x.GetTotalProfTaxByMonth(month, year))
                               .Returns(successresponse);

            var controller = new EmployeeController(mockEmployeeService.Object);

            // Act
            IActionResult actionResult = controller.GetTotalProfTaxByMonth(month, year);

            // Assert
            Assert.IsType<OkObjectResult>(actionResult); // Asserting that the result is OkObjectResult

            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult); // Asserting that OkObjectResult is not null

            var responseData = okObjectResult.Value as ServiceResponse<TotalProfTax>;
        }
        [Fact]
        public void GetTotalProfTaxByMonth_NotFound_Returns_NotFoundObject()
        {
            // Arrange
            int month = 6;
            int year = 2024;

            var notFoundResponse = new ServiceResponse<IEnumerable<TotalProfTax>>
            {
                Success = false,
                Message = "Data not found"
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeService.Setup(x => x.GetTotalProfTaxByMonth(month, year))
                               .Returns(notFoundResponse);

            var controller = new EmployeeController(mockEmployeeService.Object);

            // Act
            IActionResult actionResult = controller.GetTotalProfTaxByMonth(month, year);

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult); // Asserting that the result is NotFoundObjectResult

            var notFoundObjectResult = actionResult as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult); // Asserting that NotFoundObjectResult is not null

            var responseData = notFoundObjectResult.Value as ServiceResponse<TotalProfTax>;
            
            
            
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_Success_Returns_OkObject()
        {
            // Arrange
            int month = 6;
            int year = 2024;
            var successresponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
                Data = new List<SalaryHeadTotal>(),

            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeService.Setup(x => x.GetTotalSalaryByYearAndId(month, year))
                               .Returns(successresponse);

            var controller = new EmployeeController(mockEmployeeService.Object);

            // Act
            IActionResult actionResult = controller.GetTotalSalaryByYearAndId(month, year);

            // Assert
            Assert.IsType<OkObjectResult>(actionResult); // Asserting that the result is OkObjectResult

            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult); // Asserting that OkObjectResult is not null

            var responseData = okObjectResult.Value as ServiceResponse<TotalProfTax>;
        }
        [Fact]
        public void GetTotalSalaryByYearAndId_NotFound_Returns_NotFoundObject()
        {
            // Arrange
            int month = 6;
            int year = 1;

            var notFoundResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = false,
                Message = "Data not found"
            };

            var mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeService.Setup(x => x.GetTotalSalaryByYearAndId(month, year))
                               .Returns(notFoundResponse);

            var controller = new EmployeeController(mockEmployeeService.Object);

            // Act
            IActionResult actionResult = controller.GetTotalSalaryByYearAndId(month, year);

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult); // Asserting that the result is NotFoundObjectResult

            var notFoundObjectResult = actionResult as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult); // Asserting that NotFoundObjectResult is not null

            var responseData = notFoundObjectResult.Value as ServiceResponse<TotalProfTax>;



        }


    }
}
