using CivicaEmployeeMaster.Data.Contract;
using CivicaEmployeeMaster.Models;
using CivicaEmployeeMaster.Services.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicaEmployeeMasterTests.Services
{
    public class EmployeeDepartmentServiceTests
    {
        [Fact]
        public void GetEmployeeDepartment_Returns_Data_Successfully()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeDepartmentRepository>();
            var service = new EmployeeDepartmentService(mockRepository.Object);

            var departments = new List<EmployeeDepartment>
            {
                new EmployeeDepartment { DepartmentId = 1, DepartmentName = "HR" },
                new EmployeeDepartment { DepartmentId = 2, DepartmentName = "IT" },
            };

            mockRepository.Setup(x => x.GetAllEmployeeDepartment())
                          .Returns(departments);

            // Act
            var result = service.GetEmployeeDepartment();

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(departments.Count, result.Data.Count());
            Assert.Collection(result.Data,
                dto => Assert.Equal(departments[0].DepartmentId, dto.DepartmentId),
                dto => Assert.Equal(departments[1].DepartmentId, dto.DepartmentId)
            );
        }
        

        [Fact]
        public void GetEmployeeDepartment_Returns_No_Record_Found_Message()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeDepartmentRepository>();
            var service = new EmployeeDepartmentService(mockRepository.Object);

            mockRepository.Setup(x => x.GetAllEmployeeDepartment())
                          .Returns(() => new List<EmployeeDepartment>()); // Return an empty list

            // Act
            var result = service.GetEmployeeDepartment();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No record found!", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public void GetEmployeeDepartment_Returns_Null_From_Repository()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeDepartmentRepository>();
            var service = new EmployeeDepartmentService(mockRepository.Object);

            mockRepository.Setup(x => x.GetAllEmployeeDepartment())
                          .Returns(() => null); // Return null

            // Act
            var result = service.GetEmployeeDepartment();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No record found!", result.Message);
            Assert.Null(result.Data);
        }
    }
}
