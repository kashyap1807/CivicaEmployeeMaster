using CivicaEmployeeMaster.Models;

namespace CivicaEmployeeMaster.Data.Contract
{
    public interface IAuthRepository
    {
        bool RegisterUser(User user);
        User? ValidateUser(string username);
        bool UserExist(string loginId, string email);
        bool UpdateUser(User user);
    }
}
