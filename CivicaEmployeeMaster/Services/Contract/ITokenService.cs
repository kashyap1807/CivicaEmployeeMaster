using CivicaEmployeeMaster.Models;

namespace CivicaEmployeeMaster.Services.Contract
{
    public interface ITokenService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
    }
}
