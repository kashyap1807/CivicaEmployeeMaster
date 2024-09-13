using AutoFixture;
using CivicaEmployeeMaster.Data.Contract;
using CivicaEmployeeMaster.Dtos;
using CivicaEmployeeMaster.Models;
using CivicaEmployeeMaster.Services.Contract;
using CivicaEmployeeMaster.Services.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicaEmployeeMasterTests.Services
{
    public class AuthServiceTests
    {
        

        [Fact]
        public void LoginUserService_ReturnsSomethingWentWrong_WhenLoginDtoIsNull()
        {
            //Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);


            // Act
            var result = target.LoginUserService(null);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Something went wrong,please try after sometime.", result.Message);

        }
        [Fact]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenUserIsNull()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "username"
            };
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.UserName)).Returns<User>(null);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.UserName), Times.Once);


        }
        [Fact]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenPasswordIsWrong()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "username",
                Password = "password"
            };
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.UserName)).Returns(user);
            mockTokenService.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.UserName), Times.Once);
            mockTokenService.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt), Times.Once);


        }

        [Fact]
        public void LoginUserService_ReturnsResponse_WhenLoginIsSuccessful()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "username",
                Password = "password"
            };
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.UserName)).Returns(user);
            mockTokenService.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(true);
            mockTokenService.Setup(repo => repo.CreateToken(user)).Returns("");

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.UserName), Times.Once);
            mockTokenService.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt), Times.Once);
            mockTokenService.Verify(repo => repo.CreateToken(user), Times.Once);


        }
        [Fact]
        public void ChangePassword_NullChangePasswordDto_ReturnsErrorResponse()
        {
            // Arrange
            ChangePasswordDto changePasswordDto = null;
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong."
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
        }

        [Fact]
        public void ChangePassword_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "loginId",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword"
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong, please try after some time."
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns((User)null);

            var target = new AuthService(mockAuthRepository.Object,mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
        }
        [Fact]
        public void ChangePassword_OldPasswordSameAsNewPassword_ReturnsErrorResponse()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "loginId",
                OldPassword = "Password@123",
                NewPassword = "Password@123"
            };
            var user = new User
            {
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { }
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "New password cannot be same as old password"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();
            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
        }
        [Fact]
        public void ChangePassword_OldPasswordIncorrect_ReturnsErrorResponse()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "loginId",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword"
            };
            var user = new User
            {
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { }
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Old password is incorrect"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt)).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockTokenService.Verify(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt), Times.Once);
        }
        [Fact]
        public void ChangePassword_Successful_ReturnsSuccessResponse()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                LoginId = "loginId",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword"
            };
            var user = new User
            {
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { }
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = "Success"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            mockAuthRepository.Setup(o => o.ValidateUser(changePasswordDto.LoginId)).Returns(user);
            mockTokenService.Setup(o => o.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt)).Returns(true);
            mockAuthRepository.Setup(o => o.UpdateUser(user)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object,mockTokenService.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(changePasswordDto.LoginId), Times.Once);
            mockAuthRepository.Verify(o => o.UpdateUser(user), Times.Once);
        }
        [Fact]
        public void ForgotPassword_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                LoginId = "loginId",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong, please try after some time."
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null);

            mockAuthRepository.Setup(o => o.ValidateUser(forgotPasswordDto.LoginId)).Returns((User)null);

            // Act
            var actual = authService.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(forgotPasswordDto.LoginId), Times.Once);
        }

        [Fact]
        public void ForgotPassword_InvalidSecurityQuestion_ReturnsErrorResponse()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                LoginId = "validUser",
                PasswordHintId = 1,
                PasswordHintAnswer = "IncorrectAnswer",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Invalid security question or answer."
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null);

            var mockUser = new User
            {
                LoginId = "validUser",
                PasswordHintId = 1,
                PasswordHintAnswer = "CorrectAnswer"
            };

            mockAuthRepository.Setup(o => o.ValidateUser(forgotPasswordDto.LoginId)).Returns(mockUser);

            // Act
            var actual = authService.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(forgotPasswordDto.LoginId), Times.Exactly(2));
        }

        [Fact]
        public void ForgotPassword_PasswordMismatch_ReturnsErrorResponse()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                LoginId = "validUser",
                PasswordHintId = 1,
                PasswordHintAnswer = "CorrectAnswer",
                NewPassword = "NewPassword1",
                ConfirmNewPassword = "NewPassword2"
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "NewPassword & ConfirmNewPassword must be same"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null);

            var mockUser = new User
            {
                LoginId = "validUser",
                PasswordHintId = 1,
                PasswordHintAnswer = "CorrectAnswer"
            };

            mockAuthRepository.Setup(o => o.ValidateUser(forgotPasswordDto.LoginId)).Returns(mockUser);

            // Act
            var actual = authService.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(forgotPasswordDto.LoginId), Times.Exactly(2));
        }

        [Fact]
        public void ForgotPassword_WeakPassword_ReturnsErrorResponse()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                LoginId = "validUser",
                PasswordHintId = 1,
                PasswordHintAnswer = "CorrectAnswer",
                NewPassword = "weak",
                ConfirmNewPassword = "weak"
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong, please try after some time."
                //"Password should be alphanumeric" + Environment.NewLine +
                //"Password should contain special characters" + Environment.NewLine

            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null);

            var mockUser = new User
            {
                LoginId = "validUser",
                PasswordHintId = 1,
                PasswordHintAnswer = "CorrectAnswer"
            };

            mockAuthRepository.Setup(o => o.ValidateUser(forgotPasswordDto.LoginId)).Returns(mockUser);

            // Act
            var actual = authService.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(forgotPasswordDto.LoginId), Times.Exactly(2));
        }

        [Fact]
        public void ForgotPassword_SuccessfulReset_ReturnsSuccessResponse()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto
            {
                LoginId = "validUser",
                PasswordHintId = 1,
                PasswordHintAnswer = "CorrectAnswer",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };
            var expectedResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = "Success"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null);

            var mockUser = new User
            {
                LoginId = "validUser",
                PasswordHintId = 1,
                PasswordHintAnswer = "CorrectAnswer"
            };

            mockAuthRepository.Setup(o => o.ValidateUser(forgotPasswordDto.LoginId)).Returns(mockUser);
            mockAuthRepository.Setup(o => o.UpdateUser(mockUser)).Returns(true); // Assuming UpdateUser always succeeds for this test

            // Act
            var actual = authService.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(forgotPasswordDto.LoginId), Times.Exactly(2));
            mockAuthRepository.Verify(o => o.UpdateUser(mockUser), Times.Once);
        }
        [Fact]
        public void ForgotPassword_NullForgotPasswordDto_ReturnsErrorResponse()
        {
            // Arrange
            ForgotPasswordDto forgotPasswordDto = null;
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Something went wrong."
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null);

            // Act
            var actual = authService.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
        }
        [Fact]
        public void VerifySecurityQuestion_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var loginId = "nonexistentUser";
            var passwordHintId = 1;
            var securityAnswer = "someAnswer";
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "User not found."
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var authService = new AuthService(mockAuthRepository.Object, null);

            mockAuthRepository.Setup(o => o.ValidateUser(loginId)).Returns((User)null);

            // Act
            var actual = authService.VerifySecurityQuestion(loginId, passwordHintId, securityAnswer);

            // Assert
            Assert.Equal(expectedResponse.Success, actual.Success);
            Assert.Equal(expectedResponse.Message, actual.Message);
            mockAuthRepository.Verify(o => o.ValidateUser(loginId), Times.Once);
        }
        [Fact]
        public void RegisterUserService_WithValidRegisterDto_Returns_SuccessResponse()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockTokenService = new Mock<ITokenService>();

            var registerDto = new RegisterDto
            {
                Salutation = "Mr.",
                Name = "John Doe",
                Age = 34,
                DateOfBirth = new DateTime(1990, 1, 1),
                PasswordHintId = 1,
                Gender = "Male",
                LoginId = "johndoe",
                Email = "johndoe@example.com",
                Phone = "1234567890",
                Password = "StrongPassword@123",
                PasswordHintAnswer = "Secret"
            };

            mockAuthRepository.Setup(repo => repo.UserExist(registerDto.LoginId, registerDto.Email)).Returns(false);

            byte[] passwordHash, passwordSalt;
            mockTokenService.Setup(service => service.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt));

            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(true);

            var userService = new AuthService(mockAuthRepository.Object, mockTokenService.Object);

            // Act
                var response = userService.RegisterUserService(registerDto);

            // Assert
            Assert.True(response.Success);
            Assert.Empty(response.Message);
            // Add more assertions as needed
        }

        

        
    }
}
