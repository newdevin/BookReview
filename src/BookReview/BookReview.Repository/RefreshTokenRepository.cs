using BookReview.Service;
using BookReview.Service.Repositories;
using Dapper;
using System.Data.SqlClient;

namespace BookReview.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly string _connectionString;
        public RefreshTokenRepository(IAuthConfiguration authConfiguration)
        {
            _connectionString = authConfiguration.GetConnectionString();
        }
        public async Task Delete(string email)
        {
            var query = "DECLARE @UserId INT;" +
                "SELECT @UserId = Id FROM dbo.[User] WHERE Email = @Email;" +
                "DELETE FROM dbo.RefreshToken WHERE UserId = @UserId ;";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(query, new { Email = email });
        }

        public async Task<string> GetRefreshToken(string email)
        {
            var query = "SELECT rt.Token FROM dbo.RefreshToken rt JOIN dbo.[User] u ON u.Id = rt.UserId WHERE u.Email = @Email";

            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<string>(query, new { Email = email });
        }

        public async Task Insert(string email, string refreshToken)
        {
            var command = "DECLARE @UserId INT;" +
                "SELECT @UserId = Id FROM dbo.[User] WHERE Email = @Email;" +
                "DELETE FROM dbo.RefreshToken WHERE UserId = @UserId ;" +
                "INSERT INTO dbo.RefreshToken (UserId, Token) VALUES (@UserId, @Token)";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(command, new { Email = email, Token = refreshToken });
        }
    }

}