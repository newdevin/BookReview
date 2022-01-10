using Auth.Service;
using Auth.Service.Repositories;
using Dapper;
using System.Data.SqlClient;

namespace Auth.Repository
{
    public class EmailVerificationRepository : IEmailVerificationRepository
    {
        private readonly string _connectionString;
        public EmailVerificationRepository(IAuthConfiguration authConfiguration)
        {
            _connectionString = authConfiguration.GetConnectionString();
        }
        public async Task AddCode(string email, string code)
        {
            var command = "DECLARE @UserId INT;" +
                "SELECT @UserId = Id FROM dbo.[User] WHERE Email = @Email; " +
                "INSERT INTO dbo.EmailVerification (UserId, Code) VALUES (@UserId, @Code)";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(command, new { Email = email, Code = code });
        }

        public async Task DeleteCode(string email)
        {
            var command = "DELETE FROM dbo.[EmailVerification] ev JOIN dbo[User] u on u.Id = ev.UserId WHERE u.Email = @Email";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(command, email);
        }

        public async Task<string> GetCode(string email)
        {
            var query = "SELEECT ev.Code FROM dbo.[EmailVerification] ev JOIN dbo[User] u on u.Id = ev.UserId WHERE u.Email = @Email";
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<string>(query, email);
        }
    }

}