using CivicaEmployeeMaster.Controllers;
using CivicaEmployeeMaster.Dtos;
using CivicaEmployeeMaster.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CivicaEmployeeMasterTests.Controllers
{
    public class AuthControllerTests
    {
        [Theory]
        [InlineData("User already exists.")]
        [InlineData("Something went wrong, please try after sometime.")]
        [InlineData("Mininum password length should be 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        [InlineData("Please select a valid passwordHint id.")]
        [InlineData("Please enter a valid gender.")]
        [InlineData("Either age or date of birth is required.")]


        public void Register_ReturnsBadRequest_WhenRegistrationFails(string message)
        {
            // Arrange
            var registerDto = new RegisterDto();
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            mockAuthService.Setup(service => service.RegisterUserService(registerDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Register(registerDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.RegisterUserService(registerDto), Times.Once);
        }

        [Fact]
        public void Register_ReturnsOk_WhenRegistrationSucceeds()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                //userId = 0,
                Salutation = "Mr",
                Name = "firstname",
                Age = 18,
                LoginId = "loginid",
                Email = "email@email.com",
                Password = "password",
                ConfirmPassword = "password",
                Phone = "1234567890",
                PasswordHintId = 1,
                PasswordHintAnswer = "passwordHint"
            };
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            mockAuthService.Setup(service => service.RegisterUserService(registerDto))
                           .Returns(expectedServiceResponse);

            var controller = new AuthController(mockAuthService.Object);

            // Act
            var actual = controller.Register(registerDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.RegisterUserService(registerDto), Times.Once);
        }

        [Theory]
        [InlineData("Invalid username or password!")]
        [InlineData("Something went wrong, please try after sometime.")]
        public void Login_ReturnsBadRequest_WhenLoginFails(string message)
        {
            // Arrange
            var loginDto = new LoginDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }

        [Fact]
        public void Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            var loginDto = new LoginDto { UserName = "username", Password = "password" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }
        [Fact]
        public void ChangePassword_ReturnsOk_WhenChangePasswordSucceeds()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "username",
                OldPassword = "oldPassword",
                NewPassword = "newPassword"
            };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = "Success"
            };

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ChangePassword(changePasswordDto))
                           .Returns(expectedServiceResponse);

            var authController = new AuthController(mockAuthService.Object);

            // Act
            var result = authController.ChangePassword(changePasswordDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            mockAuthService.Verify(service => service.ChangePassword(changePasswordDto), Times.Once);
        }

        [Fact]
        public void ChangePassword_ReturnsBadRequest_WhenChangePasswordFails()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "username",
                OldPassword = "oldPassword",
                NewPassword = "newPassword"
            };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Old password is incorrect"
            };

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ChangePassword(changePasswordDto))
                           .Returns(expectedServiceResponse);

            var authController = new AuthController(mockAuthService.Object);

            // Act
            var result = authController.ChangePassword(changePasswordDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(result.Value);
            mockAuthService.Verify(service => service.ChangePassword(changePasswordDto), Times.Once);
        }
        [Fact]
        public void ForgotPassword_ReturnsOk_WhenForgotPasswordSucceeds()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                LoginId = "username",
                NewPassword = "newPassword",
                ConfirmNewPassword = "newPassword"
            };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = "Success"
            };

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ForgotPassword(forgotPasswordDto))
                           .Returns(expectedServiceResponse);

            var authController = new AuthController(mockAuthService.Object);

            // Act
            var actual = authController.ForgotPassword(forgotPasswordDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            mockAuthService.Verify(service => service.ForgotPassword(forgotPasswordDto), Times.Once);
        }
        [Fact]
        public void ForgotPassword_ReturnsBadRequest_WhenForgotPasswordFails()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                LoginId = "username",
                NewPassword = "newPassword",
                ConfirmNewPassword = "newPassword"
            };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "User not found"
            };

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ForgotPassword(forgotPasswordDto))
                           .Returns(expectedServiceResponse);

            var authController = new AuthController(mockAuthService.Object);

            // Act
            var actual = authController.ForgotPassword(forgotPasswordDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            mockAuthService.Verify(service => service.ForgotPassword(forgotPasswordDto), Times.Once);
        }
    }
}
