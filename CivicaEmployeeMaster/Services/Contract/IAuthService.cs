using CivicaEmployeeMaster.Dtos;

namespace CivicaEmployeeMaster.Services.Contract
{
    public interface IAuthService
    {
        ServiceResponse<string> ChangePassword(ChangePasswordDto changePasswordDto);
        ServiceResponse<string> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        ServiceResponse<string> RegisterUserService(RegisterDto register);
        ServiceResponse<string> LoginUserService(LoginDto login);
    }
}
