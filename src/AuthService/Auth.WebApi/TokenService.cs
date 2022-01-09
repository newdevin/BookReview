using Auth.Service;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.WebApi
{
    public class TokenService : ITokenService
    {
        private readonly JwtInfo _jwtInfo;

        public TokenService(IOptions<JwtInfo> jwtInfoOptions)
        {
            _jwtInfo = jwtInfoOptions.Value;
        }
        public string GenerateToken(UserInfo userInfo)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(ClaimTypes.GivenName, userInfo.FirstName),
                new Claim(ClaimTypes.Surname, userInfo.LastName)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtInfo.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(_jwtInfo.Issuer, _jwtInfo.Audience, claims, expires: DateTime.Now.AddDays(_jwtInfo.ExpiryInDays), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
