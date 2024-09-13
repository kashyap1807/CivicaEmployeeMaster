using CivicaEmployeeMaster.Data.Contract;
using CivicaEmployeeMaster.Dtos;
using CivicaEmployeeMaster.Models;
using CivicaEmployeeMaster.Services.Contract;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Writers;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace CivicaEmployeeMaster.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        public AuthService(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            //_configuration = configuration;
        }
        [ExcludeFromCodeCoverage]
        public ServiceResponse<string> RegisterUserService(RegisterDto register)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (register != null)
            {
                if(register.Salutation != "Mr." && register.Salutation != "Ms." && register.Salutation != "Miss." && register.Salutation != "Mrs.")
                {
                    response.Success = false;
                    response.Message = "Invalid salutation.";
                    return response;
                }
                if (register.Age == null && register.DateOfBirth == null)
                {
                    response.Success = false;
                    response.Message = "Either age or date of birth is required.";
                    return response;
                }
                
                if (register.Age != null && register.DateOfBirth !=null)
                {
                    var today = DateTime.Today;
                    var calculatedAge = today.Year - register.DateOfBirth.Value.Year;
                    if (register.DateOfBirth.Value.Date > today.AddYears(-calculatedAge)) calculatedAge--;
                    if (register.Age != calculatedAge)
                    {
                        response.Success = false;
                        response.Message = "Age and DateOfBirth should match if both value are passed";
                        return response;
                    }
                }
                if (register.PasswordHintId < 1 || register.PasswordHintId > 3)
                {
                    response.Success = false;
                    response.Message = "Please select a valid passwordHint id.";
                    return response;
                }
                if (register.Gender.ToLower() == "m" || register.Gender.ToLower() == "male")
                {
                    register.Gender = "Male";
                }
                else if (register.Gender.ToLower() == "f" || register.Gender.ToLower() == "female")
                {
                    register.Gender = "Female";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Please enter a valid gender.";
                    return response;
                }
                message = CheckPasswordStrength(register.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }
                else if (_authRepository.UserExist(register.LoginId, register.Email))
                {
                    response.Success = false;
                    response.Message = "User already exists.";
                    return response;
                }
                else
                {
                    //Save User
                    User user = new User()
                    {
                        //UserId = register.userId,
                        LoginId = register.LoginId,
                        Salutation = register.Salutation,
                        Name = register.Name,
                        Age = register.Age,
                        DOB = register.DateOfBirth,
                        Gender = register.Gender,
                        Email = register.Email,
                        Phone = register.Phone,
                        PasswordHintId = register.PasswordHintId,
                        PasswordHintAnswer = register.PasswordHintAnswer,

                    };

                    _tokenService.CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    var result = _authRepository.RegisterUser(user);
                    response.Success = result;
                    response.Message = result ? string.Empty : "Something went wrong, please try after sometimes.";
                }
            }
            return response;
        }
        public ServiceResponse<string> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var response = new ServiceResponse<string>();
            if (changePasswordDto != null)
            {
                var user = _authRepository.ValidateUser(changePasswordDto.LoginId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after some time.";
                    return response;
                }
                if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
                {
                    response.Success = false;
                    response.Message = "New password cannot be same as old password";
                    return response;
                }
                if (!_tokenService.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Old password is incorrect";
                    return response;
                }
                CheckPasswordStrength(changePasswordDto.NewPassword);
                CreatePasswordHash(changePasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                var result = _authRepository.UpdateUser(user);
                response.Success = result;
                response.Message = result ? "Success" : "Something went wrong, please try after some time.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong.";
            }
            return response;
        }
        public ServiceResponse<string> LoginUserService(LoginDto login)
        {
            var response = new ServiceResponse<string>();
            if (login != null)
            {
                var user = _authRepository.ValidateUser(login.UserName);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }
                else if (!_tokenService.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }

                string token = _tokenService.CreateToken(user);
                response.Data = token;
                return response;
            }
            response.Message = "Something went wrong,please try after sometime.";
            return response;
        }
        public ServiceResponse<string> VerifySecurityQuestion(string loginId, int passwordHintid, string securityAnswer)
        {
            var response = new ServiceResponse<string>();

            var user = _authRepository.ValidateUser(loginId);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            if (user.PasswordHintId != passwordHintid || user.PasswordHintAnswer != securityAnswer)
            {
                response.Success = false;
                response.Message = "Invalid security question or answer.";
                return response;
            }

            response.Success = true;

            return response;
        }
        private string CheckPasswordStrength(string password)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (password.Length < 8)
            {
                stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            }
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,*,(,),_,]"))
            {
                stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            }

            return stringBuilder.ToString();
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public ServiceResponse<string> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var response = new ServiceResponse<string>();

            if (forgotPasswordDto != null)
            {
                var user = _authRepository.ValidateUser(forgotPasswordDto.LoginId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after some time.";
                    return response;
                }
                var securityQuestionResponse = VerifySecurityQuestion(user.LoginId, forgotPasswordDto.PasswordHintId, forgotPasswordDto.PasswordHintAnswer);

                if (!securityQuestionResponse.Success)
                {
                    // If security question and answer verification fails, return the response
                    response.Success = false;
                    response.Message = securityQuestionResponse.Message;
                    return response;
                }
                if (forgotPasswordDto.NewPassword != forgotPasswordDto.ConfirmNewPassword)
                {
                    response.Success = false;
                    response.Message = "NewPassword & ConfirmNewPassword must be same";
                    return response;
                }
                CheckPasswordStrength(forgotPasswordDto.NewPassword);
                CreatePasswordHash(forgotPasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                var result = _authRepository.UpdateUser(user);
                response.Success = result;
                response.Message = result ? "Success" : "Something went wrong, please try after some time.";

            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong.";
            }


            return response;
        }
    }
}
