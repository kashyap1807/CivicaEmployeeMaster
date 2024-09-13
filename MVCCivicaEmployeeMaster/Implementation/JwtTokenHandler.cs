using MVCCivicaEmployeeMaster.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;

namespace MVCCivicaEmployeeMaster.Implementation
{
    [ExcludeFromCodeCoverage]
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly JwtSecurityTokenHandler _handler;

        public JwtTokenHandler()
        {
            _handler = new JwtSecurityTokenHandler();
        }

        public JwtSecurityToken ReadJwtToken(string token)
        {
            return _handler.ReadJwtToken(token);
        }
    }
}
