using Auth.Domain;

namespace Auth.Service
{
    public interface ITokenService
    {
        string GenerateToken(UserInfo user);
    }


}
