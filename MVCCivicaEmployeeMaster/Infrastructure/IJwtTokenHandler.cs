using System.IdentityModel.Tokens.Jwt;

namespace MVCCivicaEmployeeMaster.Infrastructure
{
    public interface IJwtTokenHandler
    {
        JwtSecurityToken ReadJwtToken(string token);
    }
}
