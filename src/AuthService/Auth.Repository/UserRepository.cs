using Auth.Domain;
using Auth.Service;
using Auth.Service.Repositories;
using System.Data.SqlClient;
using Dapper;
using Auth.Repository.Entities;

namespace Auth.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IAuthConfiguration authConfiguration)
        {
            _connectionString = authConfiguration.GetConnectionString();
        }

        public async Task<User?> Get(string email)
        {
            var query = "SELECT Email, PasswordHash, PasswordSalt, CreatedDateTime, LastModifiedDateTime FROM [dbo].[User] WHERE Email = @Email";
            using var connection = new SqlConnection(_connectionString);
            var userEntity = await connection.QueryFirstOrDefaultAsync<UserEntity>(query, new {Email = email});

            return userEntity is null  ? default : new User(userEntity.Email, userEntity.PasswordSalt, userEntity.PasswordHash, userEntity.CreatedDateTime, userEntity.LastModifiedDateTime);
        }

        public async Task<User> InsertUser(User user)
        {
            var command = "INSERT INTO [dbo].[User] (Email, PasswordSalt, PasswordHash, CreatedDateTime, LastModifiedDateTime)" +
                " VALUES(@Email, @PasswordSalt, @PasswordHash, @CreatedDateTimeUtc, @LastModifiedDateTimeUtc)";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(command, user);

            return user;
        }

        public async Task Update(User user)
        {
            var command = "UPDATE [dbo].[User] SET PasswordSalt = @PasswordSalt, PasswordHash = @PasswordHash, CreatedDateTime = @CreatedDateTimeUtc, LastModifiedDateTime= @LastModifiedDateTimeUtc" +
                " WHERE Email = @Email";

            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(command,user);
        }
    }
}