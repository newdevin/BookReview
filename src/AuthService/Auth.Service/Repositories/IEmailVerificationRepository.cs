namespace Auth.Service.Repositories
{
    public interface IEmailVerificationRepository
    {
        Task AddCode(string email, string code);
        Task<string> GetCode(string email);
        Task DeleteCode(string email);
    }

}
