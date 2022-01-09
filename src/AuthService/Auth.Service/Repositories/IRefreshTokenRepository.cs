namespace Auth.Service.Repositories
{
    public interface IRefreshTokenRepository
    { 
        Task Insert(string email, string refreshToken);
        Task Delete(string email);
        Task<string> GetRefreshToken(string email);
    }

}
