using CivicaEmployeeMaster.Controllers;
using CivicaEmployeeMaster.Dtos;
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
    public class EmployeeDepartmentControllerTests
    {
        [Fact]
        public void Constructor_Should_Set_Service_Successfully()
        {
            // Arrange
            var mockService = new Mock<IEmployeeDepartmentService>();

            // Act
            var controller = new EmployeeDepartmentController(mockService.Object);

            // Assert
            Assert.NotNull(controller);
        }
        [Fact]
        public void GetAllEmployeeDepartments_Returns_Data_Successfully()
        {
            // Arrange
            var mockService = new Mock<IEmployeeDepartmentService>();
            var departments = new List<EmployeeDepartmentDto>
            {
                new EmployeeDepartmentDto { DepartmentId = 1, DepartmentName = "HR" },
                new EmployeeDepartmentDto { DepartmentId = 2, DepartmentName = "IT" }
            };
            mockService.Setup(service => service.GetEmployeeDepartment())
                       .Returns(new ServiceResponse<IEnumerable<EmployeeDepartmentDto>>
                       {
                           Success = true,
                           Data = departments,
                           Message = "Success"
                       });
            var controller = new EmployeeDepartmentController(mockService.Object);

            // Act
            var actionResult = controller.GetAllEmployeeDepartments() as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(200, actionResult.StatusCode);
            var model = actionResult.Value as ServiceResponse<IEnumerable<EmployeeDepartmentDto>>;
            Assert.NotNull(model);
            Assert.True(model.Success);
            Assert.Equal("Success", model.Message);
        }

        [Fact]
        public void GetAllEmployeeDepartments_Returns_NotFound_When_No_Record_Found()
        {
            // Arrange
            var mockService = new Mock<IEmployeeDepartmentService>();
            mockService.Setup(service => service.GetEmployeeDepartment())
                       .Returns(new ServiceResponse<IEnumerable<EmployeeDepartmentDto>>
                       {
                           Success = false,
                           Message = "No record found!"
                       });
            var controller = new EmployeeDepartmentController(mockService.Object);

            // Act
            var actionResult = controller.GetAllEmployeeDepartments() as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(404, actionResult.StatusCode);
            var model = actionResult.Value as ServiceResponse<IEnumerable<EmployeeDepartmentDto>>;
            Assert.NotNull(model);
            Assert.False(model.Success);
            Assert.Equal("No record found!", model.Message);
            Assert.Null(model.Data);
        }
    }
}
