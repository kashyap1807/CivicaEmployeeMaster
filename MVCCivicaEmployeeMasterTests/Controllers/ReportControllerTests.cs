using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using MVCCivicaEmployeeMaster.Controllers;
using MVCCivicaEmployeeMaster.Infrastructure;
using MVCCivicaEmployeeMaster.ViewModel;
using MVCCivicaEmployeeMaster.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MVCCivicaEmployeeMasterTests.Controllers
{
    public class ReportControllerTests
    {
        [Fact]
        public void MonthlySalaryReport_ReturnsViewWithDefaultModelAndViewBagValues()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthlySalaryReport() as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(DateTime.Now.Year, model.Year); // Verify default year value
        }
        [Fact]
        public void MonthlySalaryReport_ReturnsViewWithYearSpecificYearMonthList()
        {
            // Arrange
            int specificYear = 2023;
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthlySalaryReport() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void MonthlySalaryReport_InvalidModelState_ReturnsViewWithError()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };
            controller.ModelState.AddModelError("Year", "Year is required."); // Simulate invalid ModelState

            // Act
            var result = controller.MonthlySalaryReport() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid); // Ensure ModelState is invalid
        }

        [Fact]
        public void MonthlySalaryReport_Returns_ViewResult_With_Model()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response data
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
           
            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 2024;
            int month = 7;

            // Act
            var result = controller.MonthlySalaryReport(month, year) as ViewResult;

            // Assert
            mockHttpClientService.Verify(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()),Times.Once);
            Assert.NotNull(result);
            Assert.Equal("MonthlySalaryReport", result.ViewName); // Ensure correct view name
        }
        [Fact]
        public void MonthlySalaryReport_Returns_Error_View_When_Api_Fails()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response with unsuccessful status code
            var mockErrorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockErrorResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 2024;
            int month = 7;

            // Act
            var result = controller.MonthlySalaryReport(month, year) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("MonthlySalaryReport", result.ActionName); // Ensure redirect to the same action
            Assert.Equal("No data available for this year.", controller.TempData["ErrorMessage"]);
            if (controller.TempData != null) // Ensure TempData is not null
            {
                Assert.Equal("No data available for this year.", controller.TempData["ErrorMessage"]);
            }
            else
            {
                // Handle case where TempData is unexpectedly null
                Assert.False(true, "TempData is null");
            }
        }
        [Fact]
        public void MonthlySalaryReport_ValidInput_ReturnsViewWithModel()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");
            var mockDataProvider = new Mock<ITempDataProvider>();

            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            // Mock response with unsuccessful status code
            var mockErrorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockErrorResponse);

            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            int month = 0;
            int year = 0;
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };
            // Act
            var result = controller.MonthlySalaryReport(month, year) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void MonthlySalaryReport_ValidServiceResponse_ReturnsViewWithCalculatedTotals()
        {
            // Arrange
            int month = 6;
            int year = 2023;
            var mockServiceResponse = new List<SalaryHeadTotal>
    {
        new SalaryHeadTotal { BasicSalary = 5000, HRA = 1000, Allowance = 500 },
        new SalaryHeadTotal { BasicSalary = 6000, HRA = 1200, Allowance = 600 },
        new SalaryHeadTotal { BasicSalary = 7000, HRA = 1400, Allowance = 700 }
    };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();

            var mockResponseContent = new JObject(new JProperty("data", JArray.FromObject(mockServiceResponse)));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthlySalaryReport(month, year) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void MonthlySalaryReport_YearIsCurrent_MonthIsFuture()
        {
            // Arrange
            int month = 12;
            int year = 2024;
            var mockServiceResponse = new List<SalaryHeadTotal>
    {
        new SalaryHeadTotal { BasicSalary = 5000, HRA = 1000, Allowance = 500 },
        new SalaryHeadTotal { BasicSalary = 6000, HRA = 1200, Allowance = 600 },
        new SalaryHeadTotal { BasicSalary = 7000, HRA = 1400, Allowance = 700 }
    };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
                Data = mockServiceResponse,
            };
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };

            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthlySalaryReport(month, year) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void MonthlySalaryReport_InValidServiceResponse_ReturnsEmptyView()
        {
            // Arrange
            int month = 6;
            int year = 2023;
            var mockServiceResponse = new List<SalaryHeadTotal>
            { };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = false,
                Data = mockServiceResponse,
            };
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthlySalaryReport(month, year) as ViewResult;


            // Assert
            Assert.NotNull(result);
            Assert.Equal("MonthlySalaryReport", result.ViewName); // Ensure it returns the correct view
        }

        [Fact]
        public void MonthlyProfTaxReport_ReturnsViewWithDefaultModelAndYearMonthList()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthalyProfTaxReport() as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
        }
        [Fact]
        public void MonthlyProfTaxReport_ReturnsViewWithCorrectYearMonthListRange()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthalyProfTaxReport() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void MonthlyProfTaxReport_InvalidModelState_ReturnsViewWithError()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };
            controller.ModelState.AddModelError("Year", "Year is required."); // Simulate invalid ModelState

            // Act
            var result = controller.MonthalyProfTaxReport() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void MonthalyProfTaxReport_Returns_ViewResult_With_Model()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response data
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 2024;
            int month = 7;

            // Act
            var result = controller.MonthalyProfTaxReport(year, month) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("MonthalyProfTaxReport", result.ViewName); // Ensure correct view name
        }
        [Fact]
        public void MonthalyProfTaxReport_Returns_Error_View_When_Api_Fails()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response with unsuccessful status code
            var mockErrorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockErrorResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 2024;
            int month = 7;

            // Act
            var result = controller.MonthalyProfTaxReport(year, month) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("MonthalyProfTaxReport", result.ActionName); // Ensure redirect to the same action
            Assert.Equal("No data available for this year.", controller.TempData["ErrorMessage"]);
            if (controller.TempData != null) // Ensure TempData is not null
            {
                Assert.Equal("No data available for this year.", controller.TempData["ErrorMessage"]);
            }
            else
            {
                // Handle case where TempData is unexpectedly null
                Assert.False(true, "TempData is null");
            }
        }
        [Fact]
        public void MonthalyProfTaxReport_InValidMonthYear()
        {
            // Arrange
            int year = 1999;
            int month = 11;
            var mockServiceResponse = new List<SalaryHeadTotal>
    {
        new SalaryHeadTotal { BasicSalary = 5000, HRA = 1000, Allowance = 500 },
        new SalaryHeadTotal { BasicSalary = 6000, HRA = 1200, Allowance = 600 },
        new SalaryHeadTotal { BasicSalary = 7000, HRA = 1400, Allowance = 700 }
    };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
                Data = mockServiceResponse,
            }; var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthalyProfTaxReport(year, month) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);

        }

        [Fact]
        public void MonthalyProfTaxReport_ValidServiceResponse_ReturnsViewWithCalculatedTotals()
        {
            // Arrange
            int year = 2023;
            int month = 11;
            var mockServiceResponse = new List<SalaryHeadTotal>
    {
        new SalaryHeadTotal { BasicSalary = 5000, HRA = 1000, Allowance = 500 },
        new SalaryHeadTotal { BasicSalary = 6000, HRA = 1200, Allowance = 600 },
        new SalaryHeadTotal { BasicSalary = 7000, HRA = 1400, Allowance = 700 }
    };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockResponseContent = new JObject(new JProperty("data", JArray.FromObject(mockServiceResponse)));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
                Data = mockServiceResponse,
            };

            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthalyProfTaxReport(year, month) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("MonthalyProfTaxReport", result.ViewName); // Ensure it returns the correct view


        }
        [Fact]
        public void MonthalyProfTaxReport_YearIsCurrent_MonthIsFuture()
        {
            // Arrange
            int month = 12;
            int year = 2024;
            var mockServiceResponse = new List<SalaryHeadTotal>
    {
        new SalaryHeadTotal { BasicSalary = 5000, HRA = 1000, Allowance = 500 },
        new SalaryHeadTotal { BasicSalary = 6000, HRA = 1200, Allowance = 600 },
        new SalaryHeadTotal { BasicSalary = 7000, HRA = 1400, Allowance = 700 }
    };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
                Data = mockServiceResponse,
            };
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };

            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthalyProfTaxReport(year, month) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void MonthalyProfTaxReport_InValidServiceResponse_ReturnsEmptyView()
        {
            // Arrange
            int year = 2023;
            int month = 12;
            var mockServiceResponse = new List<SalaryHeadTotal>
            { };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = false,
                Data = mockServiceResponse,
            };

            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthalyProfTaxReport(year, month) as ViewResult;


            // Assert
            Assert.NotNull(result);
            Assert.Equal("MonthalyProfTaxReport", result.ViewName); // Ensure it returns the correct view
        }

        [Fact]
        public void YearlySalaryReport_ReturnsViewWithDefaultModelAndYearList()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.YearlySalaryReport() as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(DateTime.Now.Year, model.Year); // Ensure default year is set to current year
        }
        [Fact]
        public void YearlySalaryReport_ReturnsViewWithCorrectYearRange()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.YearlySalaryReport() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void YearlySalaryReport_InvalidModelState_ReturnsViewWithError()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            controller.ModelState.AddModelError("Year", "Year is required."); // Simulate invalid ModelState

            // Act
            var result = controller.YearlySalaryReport() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void YearlySalaryReport_Returns_ViewResult_With_Model()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response data
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 2024;
            int month = 7;

            // Act
            var result = controller.YearlySalaryReport(year) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("YearlySalaryReport", result.ViewName); // Ensure correct view name
        }
        [Fact]
        public void YearlySalaryReport_Returns_Error_View_When_Api_Fails()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response with unsuccessful status code
            var mockErrorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockErrorResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 2024;
            int month = 7;

            // Act
            var result = controller.YearlySalaryReport(year) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("YearlySalaryReport", result.ActionName); // Ensure redirect to the same action
            Assert.Equal("No data available for this year.", controller.TempData["ErrorMessage"]);
            if (controller.TempData != null) // Ensure TempData is not null
            {
                Assert.Equal("No data available for this year.", controller.TempData["ErrorMessage"]);
            }
            else
            {
                // Handle case where TempData is unexpectedly null
                Assert.False(true, "TempData is null");
            }
        }
        [Fact]
        public void YearlySalaryReport_ValidServiceResponse_ReturnsViewWithCalculatedTotals()
        {
            // Arrange
            int year = 2023;
            var mockServiceResponse = new List<SalaryHeadTotal>
    {
        new SalaryHeadTotal { BasicSalary = 5000, HRA = 1000, Allowance = 500 },
        new SalaryHeadTotal { BasicSalary = 6000, HRA = 1200, Allowance = 600 },
        new SalaryHeadTotal { BasicSalary = 7000, HRA = 1400, Allowance = 700 }
    };

            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();

            // Mock the expected HTTP response
            var mockResponseContent = new JObject(new JProperty("data", JArray.FromObject(mockServiceResponse)));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };

            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.YearlySalaryReport(year) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result); // Check if a view result is returned

        }
        [Fact]
        public void MonthlyEachEmpSalaryReport_YearIsCurrent_MonthIsFuture()
        {
            // Arrange
            int month = 12;
            int year = 2024;
            var mockServiceResponse = new List<SalaryHeadTotal>
    {
        new SalaryHeadTotal { BasicSalary = 5000, HRA = 1000, Allowance = 500 },
        new SalaryHeadTotal { BasicSalary = 6000, HRA = 1200, Allowance = 600 },
        new SalaryHeadTotal { BasicSalary = 7000, HRA = 1400, Allowance = 700 }
    };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
                Data = mockServiceResponse,
            };
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };

            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthlyEachEmpSalaryReport(month, year, 1) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void YearlySalaryReport_InValidServiceResponse_ReturnsEmptyView()
        {
            // Arrange
            int year = 2023;
            var mockServiceResponse = new List<SalaryHeadTotal>
            { };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = false,
                Data = mockServiceResponse,
            };
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.YearlySalaryReport(year) as ViewResult;


            // Assert
            Assert.NotNull(result);
            Assert.Equal("YearlySalaryReport", result.ViewName); // Ensure it returns the correct view
        }

        [Fact]
        public void YearlyEachEmpSalaryReport_ReturnsViewWithDefaultModelAndViewBagValues()
        {
            // Arrange
            int id = 1; // Example employee id
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.YearlyEachEmpSalaryReport(id) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(DateTime.Now.Year, model.Year); // Verify default year value
        }
        [Fact]
        public void YearlyEachEmpSalaryReport_ReturnsViewWithCorrectEmpId()
        {
            // Arrange
            int empId = 1;
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.YearlyEachEmpSalaryReport(empId) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void YearlyEachEmpSalaryReport_InvalidModelState_ReturnsViewWithError()
        {
            // Arrange
            int id = 1; // Example employee id
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };
            controller.ModelState.AddModelError("Year", "Year is required."); // Simulate invalid ModelState

            // Act
            var result = controller.YearlyEachEmpSalaryReport(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void YearlyEachEmpSalaryReport_Returns_ViewResult_With_Model()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response data
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 2024;
            int id = 7;

            // Act
            var result = controller.YearlyEachEmpSalaryReport(year, id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("YearlyEachEmpSalaryReport", result.ViewName); // Ensure correct view name
        }
        [Fact]
        public void YearlyEachEmpSalaryReport_Returns_Error_View_When_Api_Fails()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response with unsuccessful status code
            var mockErrorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockErrorResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 2024;
            int id = 7;

            // Act
            var result = controller.YearlyEachEmpSalaryReport(year, id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("YearlyEachEmpSalaryReport", result.ActionName); // Ensure redirect to the same action
            Assert.Equal("No data available for this year and employee.", controller.TempData["ErrorMessage"]);
            if (controller.TempData != null) // Ensure TempData is not null
            {
                Assert.Equal("No data available for this year and employee.", controller.TempData["ErrorMessage"]);
            }
            else
            {
                // Handle case where TempData is unexpectedly null
                Assert.False(true, "TempData is null");
            }
        }


        [Fact]
        public void YearlyEachEmpSalaryReport_ValidServiceResponse_ReturnsViewWithCalculatedTotals()
        {
            // Arrange
            int year = 2023;
            int id = 11;
            var mockServiceResponse = new List<SalaryHeadTotal>
    {
        new SalaryHeadTotal { BasicSalary = 5000, HRA = 1000, Allowance = 500 },
        new SalaryHeadTotal { BasicSalary = 6000, HRA = 1200, Allowance = 600 },
        new SalaryHeadTotal { BasicSalary = 7000, HRA = 1400, Allowance = 700 }
    };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
                Data = mockServiceResponse,
            };
            var mockResponseContent = new JObject(new JProperty("data", JArray.FromObject(mockServiceResponse)));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.YearlyEachEmpSalaryReport(year, id) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("YearlyEachEmpSalaryReport", result.ViewName); // Ensure it returns the correct view


        }

        [Fact]
        public void YearlyEachEmpSalaryReport_InValidServiceResponse_ReturnsEmptyView()
        {
            // Arrange
            int year = 2023;
            int id = 12;
            var mockServiceResponse = new List<SalaryHeadTotal>
            { };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = false,
                Data = mockServiceResponse,
            };
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.YearlyEachEmpSalaryReport(year, id) as ViewResult;


            // Assert
            Assert.NotNull(result);
            Assert.Equal("YearlyEachEmpSalaryReport", result.ViewName); // Ensure it returns the correct view
        }

        [Fact]
        public void MonthlyEachEmpSalaryReport_ReturnsViewWithDefaultModelAndViewBagValues()
        {
            // Arrange
            int id = 1; // Example employee id
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthlyEachEmpSalaryReport(id) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(DateTime.Now.Year, model.Year); // Verify default year value
        }
        [Fact]
        public void MonthlyEachEmpSalaryReport_ReturnsViewWithCorrectEmpId()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };
            int empId = 1;
            // Act
            var result = controller.MonthlyEachEmpSalaryReport(empId) as ViewResult;

            // Assert
            Assert.NotNull(result);

        }
        [Fact]
        public void MonthlyEachEmpSalaryReport_InvalidModelState_ReturnsViewWithError()
        {
            // Arrange
            int id = 1; // Example employee id
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var controller = new ReportController(null, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };
            controller.ModelState.AddModelError("Year", "Year is required."); // Simulate invalid ModelState

            // Act
            var result = controller.MonthlyEachEmpSalaryReport(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void MonthlyEachEmpSalaryReport_Returns_ViewResult_With_Model()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response data
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 2024;
            int month = 7;
            int id = 1;
            // Act
            var result = controller.MonthlyEachEmpSalaryReport(month, year, id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("MonthlyEachEmpSalaryReport", result.ViewName); // Ensure correct view name
        }
        [Fact]
        public void MonthlyEachEmpSalaryReport_Returns_Error_View_When_Api_Fails()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response with unsuccessful status code
            var mockErrorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockErrorResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 2024;
            int month = 7;
            int id = 1;
            // Act
            var result = controller.MonthlyEachEmpSalaryReport(month, year, id) as RedirectToActionResult;


        }
        [Fact]
        public void MonthlyEachEmpSalaryReport_Returns_YearIsLessThan2000()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext(); // Mock HttpContext if needed

            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            // Mock response with unsuccessful status code
            var mockErrorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockErrorResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            // Create the controller instance
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            int year = 1999;
            int month = -7;
            int id = 1;
            // Act
            var result = controller.MonthlyEachEmpSalaryReport(month, year, id) as RedirectToActionResult;


        }
        [Fact]
        public void MonthlyEachEmpSalaryReport_InValidMonthYear()
        {
            // Arrange
            int year = 1999;
            int month = 11;
            int id = 1;
            var mockServiceResponse = new List<SalaryHeadTotal>
    {
        new SalaryHeadTotal { BasicSalary = 5000, HRA = 1000, Allowance = 500 },
        new SalaryHeadTotal { BasicSalary = 6000, HRA = 1200, Allowance = 600 },
        new SalaryHeadTotal { BasicSalary = 7000, HRA = 1400, Allowance = 700 }
    };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
                Data = mockServiceResponse,
            }; var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthlyEachEmpSalaryReport(month, year, id) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);

        }

        [Fact]
        public void MonthlyEachEmpSalaryReport_ValidServiceResponse_ReturnsViewWithCalculatedTotals()
        {
            // Arrange
            int year = 2023;
            int month = 11;
            int id = 1;
            var mockServiceResponse = new List<SalaryHeadTotal>
    {
        new SalaryHeadTotal { BasicSalary = 5000, HRA = 1000, Allowance = 500 },
        new SalaryHeadTotal { BasicSalary = 6000, HRA = 1200, Allowance = 600 },
        new SalaryHeadTotal { BasicSalary = 7000, HRA = 1400, Allowance = 700 }
    };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = true,
                Data = mockServiceResponse,
            };
            var mockResponseContent = new JObject(new JProperty("data", JArray.FromObject(mockServiceResponse)));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);


            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthlyEachEmpSalaryReport(month, year, id) as ViewResult;
            var model = result.Model as SalaryHeadTotal;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("MonthlyEachEmpSalaryReport", result.ViewName); // Ensure it returns the correct view


        }

        [Fact]
        public void MonthlyEachEmpSalaryReport_InValidServiceResponse_ReturnsEmptyView()
        {
            // Arrange
            int year = 2023;
            int month = 12;
            int id = 1;
            var mockServiceResponse = new List<SalaryHeadTotal>
            { };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            // Mock configuration values
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("mocked_endpoint_value");

            var mockHttpClientService = new Mock<IHttpClientService>();
            var expectedServiceResponse = new ServiceResponse<IEnumerable<SalaryHeadTotal>>
            {
                Success = false,
                Data = mockServiceResponse,
            };
            var mockResponseContent = new JObject(new JProperty("data", new JArray()));
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockResponseContent.ToString())
            };
            mockHttpClientService.Setup(s => s.GetHttpResponseMessage<SalaryHeadTotal>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.MonthlyEachEmpSalaryReport(month, year, id) as ViewResult;


            // Assert
            Assert.NotNull(result);
            Assert.Equal("MonthlyEachEmpSalaryReport", result.ViewName); // Ensure it returns the correct view
        }

        [Fact]
        public void EachEmployeeGrid_ReturnsViewWithEmployeeData()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var mockHttpClientService = new Mock<IHttpClientService>();

            var expectedEmployees = new List<EmployeeViewModel>
    {
        new EmployeeViewModel { Id = 1, FirstName = "John Doe" },
        new EmployeeViewModel { Id = 2, FirstName = "Jane Smith" }
    };

            var response = new ServiceResponse<IEnumerable<EmployeeViewModel>>
            {
                Success = true,
                Data = expectedEmployees
            };
            //var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new StringContent(JsonConvert.SerializeObject(response))
            //};
            mockHttpClientService.Setup(s => s.ExecuteApiRequest<ServiceResponse<IEnumerable<EmployeeViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(response);
            mockHttpClientService.Setup(s => s.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(new ServiceResponse<int> { Success = true, Data = 2 });
            var controller = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var result = controller.EachEmployeeGrid("searchQuery", 1, 10, "asc") as ViewResult;
            var model = result.Model as IEnumerable<EmployeeViewModel>;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<EmployeeViewModel>>(result.Model);
            Assert.True(result.ViewData.ContainsKey("search")); // Verify ViewBag values
            Assert.True(result.ViewData.ContainsKey("page"));
            Assert.True(result.ViewData.ContainsKey("pageSize"));
            Assert.True(result.ViewData.ContainsKey("sortOrder"));
            Assert.True(result.ViewData.ContainsKey("TotalPages"));

            Assert.NotNull(model); // Ensure model is not null
            Assert.Equal(2, model.Count()); // Ensure the correct number of employees is returned
        }
        [Fact]
        public void Index_SearchQueryLessThan3Characters_ReturnsErrorView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            var target = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };

            string search = "ab"; // less than 3 characters
            int page = 1;
            int pageSize = 4;
            string sortOrder = "asc";

            // Act
            var result = target.EachEmployeeGrid(search, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(result);

            // Assert that the ModelState contains the error
            Assert.False(target.ModelState.IsValid);
            Assert.Equal("Search query must be at least 3 characters long.", target.ModelState[""].Errors[0].ErrorMessage);

            // Assert ViewBag and ViewData
            Assert.Equal(search, result.ViewData["search"]);
            Assert.Equal(page, result.ViewData["page"]);
            Assert.Equal(pageSize, result.ViewData["pageSize"]);
            Assert.Equal(sortOrder, result.ViewData["sortOrder"]);

            // Assert that the view returned is of type List<EmployeeViewModel>
            var model = Assert.IsAssignableFrom<List<EmployeeViewModel>>(result.Model);
            Assert.Empty(model); // Assert that the model is an empty list of EmployeeViewModel
        }
        [Fact]
        public void EachEmployeeGrid_ReturnsError_WhenNoContactExists()
        {
            // Arrange

            var expectedContacts = new List<EmployeeViewModel>();
            var pageSize = 6;

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            var countResponse = new ServiceResponse<int> { Data = expectedContacts.Count };
            var response = new ServiceResponse<IEnumerable<EmployeeViewModel>> { Success = false };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<EmployeeViewModel>>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(countResponse);

            var target = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            // Act
            var actual = target.EachEmployeeGrid(null, pageSize, 1, "default") as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.ViewData.ContainsKey("PageSize"));
            Assert.Equal(expectedContacts, actual.Model as List<EmployeeViewModel>);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.AtLeastOnce);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<EmployeeViewModel>>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Theory]
        [InlineData("eachEmployee", "_EachEmployeeView")]
        [InlineData("allEmployee", "_AllEmployeesView")]
        public void Load_ValidContentType_ReturnsCorrectPartialView(string contentType, string expectedPartialView)
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var target = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };


            // Act
            var result = target.Load(contentType) as PartialViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPartialView, result.ViewName);
        }

        [Fact]
        public void Load_InvalidContentType_ReturnsNotFound()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var target = new ReportController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            string invalidContentType = "invalidContentType";

            // Act
            var result = target.Load(invalidContentType);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
