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

        public async Task<User> Get(string email)
        {
            var query = "SELECT Email, FirstName, LastName, EmailVerified, PasswordHash, PasswordSalt, CreatedDateTimeUtc, LastModifiedDateTimeUtc FROM [dbo].[User] WHERE Email = @Email";
            using var connection = new SqlConnection(_connectionString);
            var userEntity = await connection.QueryFirstOrDefaultAsync<UserEntity>(query, new { Email = email });

            return userEntity is null ? default : new User(userEntity.Email, userEntity.FirstName, userEntity.LastName, userEntity.EmailVerified, userEntity.PasswordSalt, userEntity.PasswordHash, userEntity.CreatedDateTimeUtc, userEntity.LastModifiedDateTimeUtc);
        }

        public async Task<User> Insert(User user)
        {
            var command = "INSERT INTO [dbo].[User] (Email, FirstName, LastName, EmailVerified, PasswordSalt, PasswordHash, CreatedDateTimeUtc, LastModifiedDateTimeUtc)" +
                " VALUES(@Email, @FirstName, @LastName, @EmailVerified ,@PasswordSalt, @PasswordHash, @CreatedDateTimeUtc, @LastModifiedDateTimeUtc)";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(command, user);

            return user;
        }

        public async Task Update(User user)
        {
            var command = "UPDATE [dbo].[User] SET FirstName = @FirstName, LastName=@LastName, EmailVerified = @EmailVerified, PasswordSalt = @PasswordSalt, PasswordHash = @PasswordHash, CreatedDateTimeUtc = @CreatedDateTimeUtc, LastModifiedDateTimeUtc= @LastModifiedDateTimeUtc" +
                " WHERE Email = @Email";

            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(command, user);
        }
    }

}